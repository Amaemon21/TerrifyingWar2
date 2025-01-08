using UnityEngine;
using Zenject;

public class CameraTilt : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;

    [Space]
    [SerializeField] private float _tiltSpeed;
    [SerializeField] private float _tiltAmount;

    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        Quaternion rotation = Tilt();
        _transform.localRotation = Quaternion.Lerp(_transform.localRotation, rotation, Time.deltaTime * _tiltSpeed);
    }

    private Quaternion Tilt()
    {
        if (_inputService.MoveDirection == Vector2.zero)
            return Quaternion.Euler(Vector3.zero);

        float x = _inputService.MoveDirection.x * _tiltAmount;

        Vector3 vector = new Vector3(0, 0, -x).normalized * _tiltAmount;

        return Quaternion.Euler(vector);
    }
}