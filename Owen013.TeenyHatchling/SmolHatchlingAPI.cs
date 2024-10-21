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
        if (PlayerScaleController.Instance == null)
        {
            ModMain.Instance.Print($"Cannot get player scale; player has not spawned or doesn't have scale controller", OWML.Common.MessageType.Error);
            return 1f;
        }
        return PlayerScaleController.Instance.Scale;
    }

    /// <summary>
    /// Returns the final scale that the player is easing towards.
    /// </summary>
    public float GetPlayerTargetScale()
    {
        if (PlayerScaleController.Instance == null)
        {
            ModMain.Instance.Print($"Cannot get player target scale; player has not spawned or doesn't have scale controller", OWML.Common.MessageType.Error);
            return 1f;
        }
        return PlayerScaleController.Instance.TargetScale;
    }

    /// <summary>
    /// Instantly resizes the player.
    /// </summary>
    /// <param name="scale">The scale to resize the player to.</param>
    /// <param name="remainGrounded">If true, moves the player up/down so that they will still be on the ground after the resizing.</param>
    public void SetPlayerScale(float scale, bool remainGrounded = false)
    {
        if (PlayerScaleController.Instance == null)
        {
            ModMain.Instance.Print($"Cannot set player scale; player has not spawned or doesn't have scale controller", OWML.Common.MessageType.Error);
            return;
        }
        PlayerScaleController.Instance.Scale = scale;
    }

    /// <summary>
    /// Smoothly resizes the player over a few seconds.
    /// </summary>
    /// <param name="scale">The scale to resize the player to.</param>
    public void EasePlayerToScale(float scale)
    {
        if (PlayerScaleController.Instance == null)
        {
            ModMain.Instance.Print($"Cannot set player target scale; player has not spawned or doesn't have scale controller", OWML.Common.MessageType.Error);
            return;
        }
        PlayerScaleController.Instance.SetTargetScale(scale);
    }

    /// <summary>
    /// Returns true if Smol Hatchling is scaling the player's speed, jump height, and damage thresholds to match their size.
    /// </summary>
    public bool IsScalingPlayerAttributes()
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
    /// Sets the scale the player will be when they start.
    /// </summary>
    /// <param name="scale">The scale the player should be when they start.</param>
    public void SetPlayerStartingScale(float scale)
    {
        PlayerScaleController.StartingScale = scale;
    }

    /// <summary>
    /// Sets the scale anglerfish will be when they start.
    /// </summary>
    /// <param name="scale">The scale anglerfish will be when they start.</param>
    public void SetAnglerfishStartingScale(float scale)
    {
        AnglerfishScaleController.StartingScale = scale;
    }

    /// <summary>
    /// Sets the scale jellyfish will be when they start.
    /// </summary>
    /// <param name="scale">The scale jellyfish will be when they start.</param>
    public void SetJellyfishStartingScale(float scale)
    {
        JellyfishScaleController.StartingScale = scale;
    }

    /// <summary>
    /// Sets the scale inhabitants will be when they start.
    /// </summary>
    /// <param name="scale">The scale inhabitants will be when they start.</param>
    public void SetInhabitantStartingScale(float scale)
    {
        GhostScaleController.StartingScale = scale;
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

    [Obsolete("UseScaledPlayerAttributes() is deprecated. Use IsScalingPlayerAttributes() instead.")]
    public bool UseScaledPlayerAttributes()
    {
        ModMain.Instance.Print("UseScaledPlayerAttributes() is deprecated. Use IsScalingPlayerAttributes() instead.", OWML.Common.MessageType.Debug);
        return ModMain.Instance.UseScaledPlayerAttributes;
    }

    [Obsolete("SetPlayerDefaultScale() is deprecated. Use SetPlayerStartingScale() instead.")]
    public void SetPlayerDefaultScale(float scale)
    {
        ModMain.Instance.Print("SetPlayerDefaultScale() is deprecated. Use SetPlayerStartingScale() instead.", OWML.Common.MessageType.Debug);
        PlayerScaleController.StartingScale = scale;
    }

    [Obsolete("SetAnglerfishDefaultScale() is deprecated. Use SetAnglerfishStartingScale() instead.")]
    public void SetAnglerfishDefaultScale(float scale)
    {
        ModMain.Instance.Print("SetAnglerfishDefaultScale() is deprecated. Use SetAnglerfishStartingScale() instead.", OWML.Common.MessageType.Debug);
        AnglerfishScaleController.StartingScale = scale;
    }

    [Obsolete("SetJellyfishDefaultScale() is deprecated. Use SetJellyfishStartingScale() instead.")]
    public void SetJellyfishDefaultScale(float scale)
    {
        ModMain.Instance.Print("SetJellyfishDefaultScale() is deprecated. Use SetJellyfishStartingScale() instead.", OWML.Common.MessageType.Debug);
        JellyfishScaleController.StartingScale = scale;
    }

    [Obsolete("SetInhabitantDefaultScale() is deprecated. Use SetInhabitantStartingScale() instead.")]
    public void SetInhabitantDefaultScale(float scale)
    {
        ModMain.Instance.Print("SetInhabitantDefaultScale() is deprecated. Use SetInhabitantStartingScale() instead.", OWML.Common.MessageType.Debug);
        GhostScaleController.StartingScale = scale;
    }
}