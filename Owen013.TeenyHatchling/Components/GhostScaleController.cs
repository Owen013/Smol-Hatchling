using HarmonyLib;
using UnityEngine;

namespace SmolHatchling.Components;

[HarmonyPatch]
public class GhostScaleController : ScaleController
{
    public static float StartingScale = 1f;

    protected override void FixedUpdate()
    {
        if (ModMain.Instance.UseOtherCustomScales && TargetScale != ModMain.Instance.CustomInhabitantScale)
        {
            SetTargetScale(ModMain.Instance.CustomInhabitantScale);
        }

        base.FixedUpdate();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GhostController), nameof(GhostController.UpdatePositionFromVelocity))]
    private static bool GhostController_MoveToLocalPosition(GhostController __instance)
    {
        ScaleController scaleController = __instance.GetComponent<ScaleController>();
        if (scaleController == null || scaleController.Scale == 1f) return true;

        Vector3 newPosition = __instance.transform.localPosition + __instance._velocity * scaleController.Scale * Time.fixedDeltaTime;
        if (__instance._ghostCollider != null && __instance._ghostCollider.enabled && __instance._playerCollider != null && !__instance._grabController.enabled)
        {
            Vector3 positionB = __instance.WorldToLocalPosition(__instance._playerCollider.transform.position);
            Quaternion rotationB = __instance._nodeRoot.InverseTransformRotation(__instance._playerCollider.transform.rotation);
            if (Physics.ComputePenetration(__instance._ghostCollider, newPosition, __instance.transform.localRotation, __instance._playerCollider, positionB, rotationB, out Vector3 direction, out float distance))
            {
                newPosition += direction * distance;
            }
        }

        __instance.transform.localPosition = newPosition;

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GhostBrain), nameof(GhostBrain.Start))]
    private static void AddScaleControllerToGhost(GhostBrain __instance)
    {
        ScaleController scaleController = __instance.gameObject.AddComponent<GhostScaleController>();
        // fire on the next update to avoid breaking things
        ModMain.Instance.ModHelper.Events.Unity.FireOnNextUpdate(() =>
        {
            if (ModMain.Instance.UseOtherCustomScales)
            {
                scaleController.Scale = ModMain.Instance.CustomInhabitantScale;
            }
            else
            {
                scaleController.Scale = StartingScale;
            }

            __instance.transform.position += __instance.transform.up * (scaleController.Scale - 1f);
        });
    }
}