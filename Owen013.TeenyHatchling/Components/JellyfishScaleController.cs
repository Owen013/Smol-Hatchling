using HarmonyLib;

namespace SmolHatchling.Components;

[HarmonyPatch]
public class JellyfishScaleController : ScaleController
{
    public static float StartingScale = 1f;

    protected override void FixedUpdate()
    {
        if (ModMain.Instance.UseOtherCustomScales && TargetScale != ModMain.Instance.CustomJellyfishScale)
        {
            SetTargetScale(ModMain.Instance.CustomJellyfishScale);
        }

        base.FixedUpdate();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(JellyfishController), nameof(JellyfishController.FixedUpdate))]
    private static bool Jellyfish_FixedUpdate(JellyfishController __instance)
    {
        ScaleController scaleController = __instance.GetComponent<ScaleController>();
        if (scaleController == null || scaleController.Scale == 1f) return true;

        float sqrMagnitude = (__instance._jellyfishBody.GetPosition() - __instance._planetBody.GetPosition()).sqrMagnitude;
        if (__instance._isRising)
        {
            __instance._jellyfishBody.AddAcceleration(__instance.transform.up * __instance._upwardsAcceleration * scaleController.Scale);
            if (sqrMagnitude > __instance._upperLimit * __instance._upperLimit)
            {
                __instance._isRising = false;
                __instance._attractiveFluidVolume.SetVolumeActivation(true);
                return false;
            }
        }
        else
        {
            __instance._jellyfishBody.AddAcceleration(-__instance.transform.up * __instance._downwardsAcceleration * scaleController.Scale);
            if (sqrMagnitude < __instance._lowerLimit * __instance._lowerLimit)
            {
                __instance._isRising = true;
                __instance._attractiveFluidVolume.SetVolumeActivation(false);
            }
        }

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(JellyfishController), nameof(JellyfishController.Start))]
    private static void AddScaleControllerToJellyfish(JellyfishController __instance)
    {
        ScaleController scaleController = __instance.gameObject.AddComponent<JellyfishScaleController>();
        // fire on the next update to avoid breaking things
        ModMain.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
        {
            if (ModMain.Instance.UseOtherCustomScales)
            {
                scaleController.Scale = ModMain.Instance.CustomJellyfishScale;
            }
            else
            {
                scaleController.Scale = StartingScale;
            }
        });
    }
}