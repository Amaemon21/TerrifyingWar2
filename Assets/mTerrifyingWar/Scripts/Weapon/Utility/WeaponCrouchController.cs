using UnityEngine;

public class WeaponCrouchController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [Space]
    [SerializeField] private float _tiltAmount = 15.0f;
    [SerializeField] private float _tiltSpeed = 5.0f;

    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        if (_playerController.IsCrouching)
        {
            rotation = Quaternion.Lerp(_transform.localRotation, Quaternion.Euler(new Vector3(0, 0, _tiltAmount)), Time.deltaTime * _tiltSpeed);
        }
        else
        {
            rotation = Quaternion.Lerp(_transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * _tiltSpeed);
        }

        _transform.localRotation = rotation;
    }
}