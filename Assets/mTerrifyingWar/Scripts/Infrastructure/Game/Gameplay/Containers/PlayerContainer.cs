using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public WeaponRecoilAndShake WeaponRecoilAndShake { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public Camera WeaponCamera { get; private set; }
}