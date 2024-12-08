using UnityEngine;

public class WeaponCrouchController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [Space]
    [SerializeField] private float _tiltAmount = 15.0f;
    [SerializeField] private float _tiltSpeed = 5.0f;
    
    private void Update()
    {
        var rotation = new Quaternion(0, 0, 0, 0);
        
        if (_playerController.IsCrouching)
        {
            rotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0, 0, _tiltAmount)), Time.deltaTime * _tiltSpeed);
        }
        else
        {
            rotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * _tiltSpeed);
        }

        transform.localRotation = rotation;
    }
}