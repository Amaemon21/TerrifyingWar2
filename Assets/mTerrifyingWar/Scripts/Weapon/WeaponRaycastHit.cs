using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class WeaponRaycastHit : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    [Inject] private readonly WeaponProvider _weaponProvider;
    
    [SerializeField, BoxGroup("Main")] private LayerMask _hitScanMask;
    [SerializeField, BoxGroup("Main")] private int _damage;
    
    [SerializeField, BoxGroup("Spread"), HorizontalLine] private bool _applySpread = true;
    [SerializeField, BoxGroup("Spread")] private Vector3 _spreadVariance = new(1.0f, 1.0f, 1.0f);

    private Weapon weapon;
    private WeaponEffects _weaponEffects;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        _weaponEffects = GetComponent<WeaponEffects>();
    }

    private void OnEnable()
    {
        weapon.OnShootChanged += HandleRaycast;
    }

    private void OnDisable()
    {
        weapon.OnShootChanged -= HandleRaycast;
    }

    private void HandleRaycast()
    {
        Vector3 direction;
            
        if (_weaponProvider.RecoilAnimation.isAiming)
        {
            direction = transform.forward;
        }
        else
        {
            direction = WeaponUtilities.GetDirection(transform.forward, _applySpread, _spreadVariance);
        }
        
        Ray ray = _playerProvider.MainCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        ray.direction = direction;
        
        if (Physics.Raycast(ray, out RaycastHit hit, 500, _hitScanMask))
        {
            HandleHit(hit, ray);
        }
        else
        {
            _weaponEffects.CreateTrail(ray.origin + ray.direction * 500);
        }
    }
        
    private void HandleHit(RaycastHit hit, Ray ray)
    {
        _weaponEffects.CreateTrail(hit.point);
        
        Collider hitCollider = hit.collider;
    
        if (hitCollider.TryGetComponent(out Impact impact))
        {
            GameObject spawnedObject = Instantiate(impact.ImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal * 0.1f), hitCollider.transform);
        
            if (spawnedObject.TryGetComponent(out ImpactSound impactSound))
                impactSound.PlaySound();
            
            Destroy(spawnedObject, 10f);
        
            if (hitCollider.TryGetComponent(out BodyPart bodyPart))
            {
                bodyPart.Hit(ray.direction * 150, hit.point);
                bodyPart.TakeDamage(_damage);
            }
        }
    }
}
