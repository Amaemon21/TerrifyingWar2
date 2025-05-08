using KINEMATION.ProceduralRecoilAnimationSystem.Runtime;
using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    [field: SerializeField] public Camera WeaponCamera { get; private set; }
    [field: SerializeField] public RecoilPattern RecoilPattern { get; private set; }
    [field: SerializeField] public RecoilAnimation RecoilAnimation { get; private set; }
    [field: SerializeField] public WeaponHolder WeaponHolder { get; private set; }
    [field: SerializeField] public TransformsContainer TransformsContainer { get; private set; }
    [field: SerializeField] public Animator HandAnimator { get; private set; }
}
