using UnityEngine;
using Zenject;

public class WeaponSway : MonoBehaviour
{
    [Inject] private readonly InputService _inputService;
    
    [Header("Common")]
    [SerializeField] private Vector2 _force = Vector2.one;
    [SerializeField, Min(0f)] private float _multiplier = 5f;
    [SerializeField] private bool _inverseX;
    [SerializeField] private bool _inverseY;
    [SerializeField] private bool _inverseZ;
    
    [Header("Clamp")] 
    [SerializeField] private Vector2 _minMaxX;
    [SerializeField] private Vector2 _minMaxY;
    [SerializeField] private Vector2 _minMaxZ;

    [Header("Z Rotation")]
    [SerializeField] private float _zRotationForce = 1f;

    protected float AdditionalX;
    protected float AdditionalY;

    private float _mouseX, _mouseY;
    private float _velocityY;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        PerformTransformSway();
    }

    private void PerformTransformSway()
    {
        var deltaTime = Time.deltaTime;
        var inverseSwayX = _inverseX ? -1f : 1f;
        var inverseSwayY = _inverseY ? -1f : 1f;
        var inverseSwayZ = _inverseZ ? -1f : 1f;

        _mouseX = _inputService.LookDirection.x * inverseSwayX;
        _mouseY = _inputService.LookDirection.y * inverseSwayY;
            
        OnSwayPerforming(deltaTime);

        var currentX = _mouseY * _force.y;
        var currentY = _mouseX * _force.x;
        var currentZ = _mouseX * _zRotationForce * inverseSwayZ;

        var endEulerAngleX = Mathf.Clamp(currentX + AdditionalX, _minMaxX.x, _minMaxX.y);
        var endEulerAngleY = Mathf.Clamp(currentY + AdditionalY, _minMaxY.x, _minMaxY.y);
        var endEulerAngleZ = Mathf.Clamp(currentZ, _minMaxZ.x, _minMaxZ.y);

        var moment = deltaTime * _multiplier;
        var localEulerAngles = _transform.localEulerAngles;
            
        localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, endEulerAngleX, moment);
        localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, endEulerAngleY, moment);
        localEulerAngles.z = Mathf.LerpAngle(localEulerAngles.z, endEulerAngleZ, moment);

        _transform.localEulerAngles = localEulerAngles;
    }
        
    protected virtual void OnSwayPerforming(in float deltaTime) { }
}
