using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class WeaponVisualEffects : MonoBehaviour
{
    [SerializeField, BoxGroup("Muzzle Flash"), HorizontalLine] private bool _enableMuzzle = true;
    [SerializeField][Range(0.0f, 2.0f), BoxGroup("Muzzle Flash")] private float _scaleFactor = 1.0f;
    [SerializeField][Range(0.0f, 5.0f), BoxGroup("Muzzle Flash")] private float _destroyTime = 2.0f;
    
    [Space]
    [SerializeField][Range(0.0f, 2.0f), BoxGroup("Muzzle Flash")] private GameObject[] _muzzlePrefabs;

    [SerializeField, BoxGroup("Bullet Trail"), HorizontalLine] private AnimationCurve _widthCurve;
    [SerializeField, BoxGroup("Bullet Trail")] private float _duration = 0.1f;
    [SerializeField, BoxGroup("Bullet Trail")] private float _minVertexDistance = 0.1f;
    [SerializeField, BoxGroup("Bullet Trail")] private Gradient _trailColor;
    [SerializeField, BoxGroup("Bullet Trail")] private Material _material;
    [SerializeField, BoxGroup("Bullet Trail")] private TrailRenderer _bulletTrail;
    [SerializeField, BoxGroup("Bullet Trail")] private float _bulletSpeed = 100;
    
    [SerializeField, BoxGroup("Mag"), HorizontalLine] private GameObject _magPrefab;
    [SerializeField, BoxGroup("Mag")] private GameObject _magEmptyPrefab;
    [SerializeField, BoxGroup("Mag")] private float _magSpawnDelay = 1.0f;
    [SerializeField, BoxGroup("Mag")] private float _magEmptySpawnDelay = 1.2f;
    [SerializeField, BoxGroup("Mag")] private float _magDropForce = 10.0f;
    
    [SerializeField, BoxGroup("Shell"), HorizontalLine] private Rigidbody _shellPrefab;
    [SerializeField, BoxGroup("Shell")] private ForceMode _shellForceMode;
    [SerializeField, BoxGroup("Shell")] private Vector2 _rotationOffset;
    [SerializeField, BoxGroup("Shell")] private float _force;
    [SerializeField, BoxGroup("Shell")] private Vector3 _defaultShellRotation;
    
    [SerializeField, BoxGroup("Transform"), HorizontalLine] private Transform _barrelTransform;
    [SerializeField, BoxGroup("Transform")] private Transform _shellTransform;
    [SerializeField, BoxGroup("Transform")] private Transform _magTransform;

    public Transform BarrelTransform => _barrelTransform;
    
    public void CreateMuzzleFlash()
    {
        WeaponUtilities.CreateMuzzleFlash(_enableMuzzle, _muzzlePrefabs, _barrelTransform, _scaleFactor, _destroyTime);
    }

    public void CreateTrail(Vector3 hitPoint)
    {
        StartCoroutine(SpawnTrail(hitPoint));
    }
    
    private IEnumerator SpawnTrail(Vector3 hitPoint)
    {
        TrailRenderer trail = WeaponUtilities.CreateTrail(_bulletTrail, _barrelTransform, _widthCurve, _duration, _minVertexDistance, _trailColor, _material);

        Vector3 StartPosition = trail.transform.position;
        float Distance = Vector3.Distance(trail.transform.position, hitPoint);
        float RemainingDistance = Distance;

        while (RemainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(StartPosition, hitPoint, 1 - (RemainingDistance / Distance));

            RemainingDistance -= _bulletSpeed * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hitPoint;

        Destroy(trail.gameObject, trail.time);
    }
    
    public void CreateShell()
    {
        Vector3 finalRotation = _defaultShellRotation + new Vector3(Random.Range(_rotationOffset.x, _rotationOffset.y), 0, 0);
        _shellTransform.localRotation = Quaternion.Euler(finalRotation);

        Rigidbody shell = Instantiate(_shellPrefab, _shellTransform);
        shell.transform.SetParent(null);
        shell.AddForce(_shellTransform.forward * _force);
        shell.AddTorque(_shellTransform.forward * _force * 10);
        Destroy(shell.gameObject, 2.5f);
    }

    public void CreateMag(WeaponInventoryItemConfig weaponInventoryItemConfig)
    {
        StartCoroutine(WeaponUtilities.CreateMag(weaponInventoryItemConfig.CurrentAmmo, _magSpawnDelay, _magEmptySpawnDelay, _magDropForce, _magPrefab, _magEmptyPrefab, _magTransform));
    }
}
