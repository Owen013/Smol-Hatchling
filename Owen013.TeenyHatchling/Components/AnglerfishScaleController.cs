using HarmonyLib;
using OWML.Common;

namespace SmolHatchling.Components;

[HarmonyPatch]
public class AnglerfishScaleController : ScaleController
{
    public static float StartingScale = 1f;

    private AnglerfishController _anglerfishController;

    protected override void Awake()
    {
        base.Awake();
        _anglerfishController = GetComponent<AnglerfishController>();
    }

    protected override void FixedUpdate()
    {
        if (ModMain.Instance.IsUsingOtherCustomScales && TargetScale != ModMain.Instance.CustomAnglerfishScale)
        {
            SetTargetScale(ModMain.Instance.CustomAnglerfishScale);
        }

        base.FixedUpdate();
        _anglerfishController._acceleration = 40f * Scale;
        _anglerfishController._chaseSpeed = 75f * Scale;
        _anglerfishController._investigateSpeed = 20f * Scale;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AnglerfishController), nameof(AnglerfishController.Start))]
    private static void AddScaleControllerToAnglerfish(AnglerfishController __instance)
    {
        ScaleController scaleController = __instance.gameObject.AddComponent<AnglerfishScaleController>();
        // fire on the next update to avoid breaking things
        ModMain.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
        {
            if (ModMain.Instance.IsUsingOtherCustomScales)
            {
                scaleController.SetScale(ModMain.Instance.CustomAnglerfishScale);
            }
            else
            {
                scaleController.SetScale(StartingScale);
            }
        });
    }
}