using UnityEngine;

public class PlayerProvider : MonoBehaviour
{
    [field: SerializeField] public PlayerMover PlayerMover { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
}