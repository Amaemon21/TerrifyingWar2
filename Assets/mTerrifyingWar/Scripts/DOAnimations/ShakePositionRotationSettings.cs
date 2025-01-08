using System;
using Code.Global.Animations;
using UnityEngine;

[Serializable]
public class ShakePositionRotationSettings
{
    [SerializeField] private ShakeAnimationPreset rotationShake;
    [SerializeField] private ShakeAnimationPreset positionShake;

    public ShakeAnimationPreset RotationShake => rotationShake;
    public ShakeAnimationPreset PositionShake => positionShake;
}