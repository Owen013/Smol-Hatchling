using HarmonyLib;

namespace SmolHatchling.Components;

[HarmonyPatch]
public class AnglerfishScaleController : ScaleController
{
    public static float StartingScale = 1;

    private AnglerfishController _anglerfishController;

    protected override void Awake()
    {
        base.Awake();
        _anglerfishController = GetComponent<AnglerfishController>();
    }

    protected override void FixedUpdate()
    {
        if (ModMain.Instance.UseOtherCustomScales && TargetScale != ModMain.Instance.CustomAnglerfishScale)
        {
            SetTargetScale(ModMain.Instance.CustomAnglerfishScale);
        }

        base.FixedUpdate();
        _anglerfishController._acceleration = 40 * Scale;
        _anglerfishController._chaseSpeed = 75 * Scale;
        _anglerfishController._investigateSpeed = 20 * Scale;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AnglerfishController), nameof(AnglerfishController.Start))]
    private static void AddScaleControllerToAnglerfish(AnglerfishController __instance)
    {
        ScaleController scaleController = __instance.gameObject.AddComponent<AnglerfishScaleController>();
        // fire on the next update to avoid breaking things
        ModMain.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
        {
            if (ModMain.Instance.UseOtherCustomScales)
            {
                scaleController.Scale = ModMain.Instance.CustomAnglerfishScale;
            }
            else
            {
                scaleController.Scale = StartingScale;
            }
        });
    }
}