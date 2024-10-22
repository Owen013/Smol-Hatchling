using System;
using UnityEngine;

namespace SmolHatchling;

public interface ISmolHatchling
{
    /// <summary>
    /// Returns the current scale of the player.
    /// </summary>
    public float GetPlayerScale();

    /// <summary>
    /// Returns the final scale that the player is easing towards.
    /// </summary>
    public float GetPlayerTargetScale();

    /// <summary>
    /// Instantly resizes the player.
    /// </summary>
    /// <param name="scale">The scale to resize the player to.</param>
    /// <param name="remainGrounded">If true, moves the player up/down so that the bottom of their collider will be in the same postion after being resized.</param>
    public void SetPlayerScale(float scale, bool remainGrounded = false);

    /// <summary>
    /// Smoothly resizes the player over a few seconds.
    /// </summary>
    /// <param name="scale">The scale to resize the player to.</param>
    public void EasePlayerToScale(float scale);

    /// <summary>
    /// Returns true if Smol Hatchling is scaling the player's speed, jump height, and damage thresholds to match their size.
    /// </summary>
    public bool UsingScaledPlayerAttributes();

    /// <summary>
    /// Returns the animation speed multiplier.
    /// </summary>
    public float GetPlayerAnimSpeed();

    /// <summary>
    /// Sets the scale the player will be when they start.
    /// </summary>
    /// <param name="scale">The scale the player should be when they start.</param>
    public void SetPlayerStartingScale(float scale);

    /// <summary>
    /// Sets the scale anglerfish will be when they start.
    /// </summary>
    /// <param name="scale">The scale anglerfish will be when they start.</param>
    public void SetAnglerfishStartingScale(float scale);

    /// <summary>
    /// Sets the scale jellyfish will be when they start.
    /// </summary>
    /// <param name="scale">The scale jellyfish will be when they start.</param>
    public void SetJellyfishStartingScale(float scale);

    /// <summary>
    /// Sets the scale inhabitants will be when they start.
    /// </summary>
    /// <param name="scale">The scale inhabitants will be when they start.</param>
    public void SetInhabitantStartingScale(float scale);

    /// <summary>
    /// Resizes a GameObject using its ScaleController. If the GameObject does not have a ScaleController, one will be created.
    /// </summary>
    /// <param name="gameObject">The GameObject to resize.</param>
    /// <param name="scale">The size you want the GameObject to be.</param>
    public void SetGameObjectScale(GameObject gameObject, float scale);

    /// <summary>
    /// Smoothly resizes a GameObject using its ScaleController. If the GameObject does not have a ScaleController, one will be created.
    /// </summary>
    /// <param name="gameObject">The GameObject to resize.</param>
    /// <param name="scale">The size you want the GameObject to be.</param>
    public void EaseGameObjectToScale(GameObject gameObject, float scale);

    [Obsolete("GetTargetScale() is deprecated. Use GetPlayerScale() instead.")]
    public Vector3 GetTargetScale();

    [Obsolete("GetCurrentScale() is deprecated. Use GetPlayerScale() instead.")]
    public Vector3 GetCurrentScale();

    [Obsolete("GetAnimSpeed() is deprecated. Use GetPlayerAnimSpeed() instead.")]
    public float GetAnimSpeed();

    [Obsolete("UseScaledPlayerAttributes() is deprecated. Use IsScalingPlayerAttributes() instead.")]
    public bool UseScaledPlayerAttributes();

    [Obsolete("SetPlayerDefaultScale() is deprecated. Use SetPlayerStartingScale() instead.")]
    public void SetPlayerDefaultScale(float scale);

    [Obsolete("SetAnglerfishDefaultScale() is deprecated. Use SetAnglerfishStartingScale() instead.")]
    public void SetAnglerfishDefaultScale(float scale);

    [Obsolete("SetJellyfishDefaultScale() is deprecated. Use SetJellyfishStartingScale() instead.")]
    public void SetJellyfishDefaultScale(float scale);

    [Obsolete("SetInhabitantDefaultScale() is deprecated. Use SetInhabitantStartingScale() instead.")]
    public void SetInhabitantDefaultScale(float scale);
}