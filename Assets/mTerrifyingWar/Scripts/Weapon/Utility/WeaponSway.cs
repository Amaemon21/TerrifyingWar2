using UnityEngine;
using Zenject;

public class WeaponSway : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    
    [SerializeField] private float _swayAmount = 4.0f;
    [SerializeField] private float _smoothTime = 10.0f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * _swayAmount;
        var mouseY = Input.GetAxisRaw("Mouse Y") * _swayAmount;

        var rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        var rotationY = Quaternion.AngleAxis(mouseX, Vector3.back);

        var targetRotation = rotationX * rotationY;

        _transform.localRotation = Quaternion.Slerp(_transform.localRotation, targetRotation, _smoothTime * Time.deltaTime);
    }
}
