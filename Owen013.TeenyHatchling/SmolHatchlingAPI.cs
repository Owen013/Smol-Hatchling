using SmolHatchling.Components;
using System;
using UnityEngine;

namespace SmolHatchling;

public class SmolHatchlingAPI
{
    /// <summary>
    /// Returns the current scale of the player.
    /// </summary>
    public float GetPlayerScale()
    {
        if (PlayerScaleController.Instance == null) return 1;
        return PlayerScaleController.Instance.Scale;
    }

    /// <summary>
    /// Returns the final scale that the player is easing towards.
    /// </summary>
    public float GetPlayerTargetScale()
    {
        if (PlayerScaleController.Instance == null) return 1;
        return PlayerScaleController.Instance.TargetScale;
    }

    /// <summary>
    /// Returns true if Smol Hatchling is scaling the player's speed, jump height, and damage thresholds to match their size.
    /// </summary>
    public bool UseScaledPlayerAttributes()
    {
        return ModMain.Instance.UseScaledPlayerAttributes;
    }

    /// <summary>
    /// Returns the animation speed multiplier.
    /// </summary>
    public float GetPlayerAnimSpeed()
    {
        return PlayerScaleController.Instance.AnimSpeed;
    }

    /// <summary>
    /// The scale the player will be when they start.
    /// </summary>
    /// <param name="scale">The default scale the player should be.</param>
    public void SetPlayerDefaultScale(float scale)
    {
        PlayerScaleController.DefaultScale = scale;
    }

    /// <summary>
    /// The scale anglerfish will be when they start.
    /// </summary>
    /// <param name="scale">The default scale anglerfish should be.</param>
    public void SetAnglerfishDefaultScale(float scale)
    {
        AnglerfishScaleController.DefaultScale = scale;
    }

    /// <summary>
    /// The scale anglerfish will be when they start.
    /// </summary>
    /// <param name="scale">The default scale jellyfish should be.</param>
    public void SetJellyfishDefaultScale(float scale)
    {
        JellyfishScaleController.DefaultScale = scale;
    }

    /// <summary>
    /// The scale inhabitants will be when they start.
    /// </summary>
    /// <param name="scale">The default scale inhabitants should be.</param>
    public void SetInhabitantDefaultScale(float scale)
    {
        GhostScaleController.DefaultScale = scale;
    }

    /// <summary>
    /// Resizes a GameObject using its ScaleController. If the GameObject does not have a ScaleController, one will be created.
    /// </summary>
    /// <param name="gameObject">The GameObject to resize.</param>
    /// <param name="scale">The size you want the GameObject to be.</param>
    public void SetGameObjectScale(GameObject gameObject, float scale)
    {
        ScaleController scaleController = gameObject.GetComponent<ScaleController>();
        if (gameObject.GetComponent<ScaleController>() == null)
        {
            if (gameObject.GetComponent<PlayerCharacterController>())
            {
                scaleController = gameObject.AddComponent<PlayerScaleController>();
            }
            else if (gameObject.GetComponent<AnglerfishController>())
            {
                scaleController = gameObject.AddComponent<AnglerfishScaleController>();
            }
            else if (gameObject.GetComponent<JellyfishController>())
            {
                scaleController = gameObject.AddComponent<JellyfishScaleController>();
            }
            else
            {
                scaleController = gameObject.AddComponent<ScaleController>();
            }
        }

        scaleController.Scale = scale;
    }

    /// <summary>
    /// Smoothly resizes a GameObject using its ScaleController. If the GameObject does not have a ScaleController, one will be created.
    /// </summary>
    /// <param name="gameObject">The GameObject to resize.</param>
    /// <param name="scale">The size you want the GameObject to be.</param>
    public void EaseGameObjectToScale(GameObject gameObject, float scale)
    {
        ScaleController scaleController = gameObject.GetComponent<ScaleController>();
        if (gameObject.GetComponent<ScaleController>() == null)
        {
            if (gameObject.GetComponent<PlayerCharacterController>())
            {
                scaleController = gameObject.AddComponent<PlayerScaleController>();
            }
            else if (gameObject.GetComponent<AnglerfishController>())
            {
                scaleController = gameObject.AddComponent<AnglerfishScaleController>();
            }
            else if (gameObject.GetComponent<JellyfishController>())
            {
                scaleController = gameObject.AddComponent<JellyfishScaleController>();
            }
            else
            {
                scaleController = gameObject.AddComponent<ScaleController>();
            }
        }

        scaleController.SetTargetScale(scale);
    }

    [Obsolete("GetTargetScale() is deprecated. Use GetPlayerScale() instead.")]
    public Vector3 GetTargetScale()
    {
        ModMain.Instance.Print("GetTargetScale() is deprecated. Use GetPlayerScale() instead.", OWML.Common.MessageType.Debug);
        return Vector3.one * PlayerScaleController.Instance.TargetScale;
    }

    [Obsolete("GetCurrentScale() is deprecated. Use GetPlayerScale() instead.")]
    public Vector3 GetCurrentScale()
    {
        ModMain.Instance.Print("GetCurrentScale() is deprecated. Use GetPlayerScale() instead.", OWML.Common.MessageType.Debug);
        return Vector3.one * PlayerScaleController.Instance.Scale;
    }

    [Obsolete("GetAnimSpeed() is deprecated. Use GetPlayerAnimSpeed() instead.")]
    public float GetAnimSpeed()
    {
        ModMain.Instance.Print("GetAnimSpeed() is deprecated. Use GetPlayerAnimSpeed() instead.", OWML.Common.MessageType.Debug);
        return PlayerScaleController.Instance.AnimSpeed;
    }
}