using UnityEngine;

public class TransformsContainer : MonoBehaviour
{
    [Header("Skeleton")]
    [field: SerializeField] public Transform SkeletonRoot { get; private set; }

    [field: SerializeField] public Transform WeaponBoneAdditive { get; private set; }
    [field: SerializeField] public Transform CameraPoint { get; private set; }
    [field: SerializeField] public Transform WeaponBone { get; private set; }

    [field: SerializeField] public IKTransforms RightHand { get; private set; }
    [field: SerializeField] public IKTransforms LeftHand { get; private set; }
}