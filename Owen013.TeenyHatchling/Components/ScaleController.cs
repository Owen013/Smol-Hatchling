using UnityEngine;

namespace SmolHatchling.Components;

public class ScaleController : MonoBehaviour
{
    public virtual float Scale
    {
        get
        {
            return transform.localScale.x;
        }

        set
        {
            transform.localScale = Vector3.one * value;
            SetTargetScale(value);
        }
    }

    public float TargetScale { get; private set; }

    private Vector3 _scaleVelocity;

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
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one * TargetScale, ref _scaleVelocity, 0.1f, Scale);
        }
        else
        {
            _scaleVelocity = Vector3.zero;
        }
    }
}