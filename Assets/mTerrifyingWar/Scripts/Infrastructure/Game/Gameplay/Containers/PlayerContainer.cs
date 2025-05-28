using UnityEngine;

public class PlayerContainer : MonoBehaviour
{ 
    [field: SerializeField] public PlayerMover PlayerMover { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public GameObject CinematicCamera { get; private set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public WeaponContainer WeaponContainer { get; private set; }
    [field: SerializeField] public UIBluer UIBluer { get; private set; }
}