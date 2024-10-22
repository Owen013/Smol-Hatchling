using UnityEngine;

namespace SmolHatchling.Components;

public class ScaleController : MonoBehaviour
{
    public float Scale
    {
        get
        {
            return transform.localScale.x;
        }
    }

    public float TargetScale { get; private set; }

    private Vector3 _scaleVelocity;

    public virtual void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
        SetTargetScale(scale);
    }

    public virtual void SetTargetScale(float scale)
    {
        TargetScale = scale;
    }

    protected virtual void Awake()
    {
        TargetScale = Scale;
    }

    protected virtual void FixedUpdate()
    {
        if (Scale != TargetScale)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one * TargetScale, ref _scaleVelocity, 0.25f, Scale);
        }
        else
        {
            _scaleVelocity = Vector3.zero;
        }
    }
}