using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
    [SerializeField, BoxGroup("Main"), HorizontalLine] private Weapon _weapon;
    
    [SerializeField, BoxGroup("Bullet Trail"), HorizontalLine] private AnimationCurve _widthCurve;
    [SerializeField, BoxGroup("Bullet Trail")] private float _duration = 0.1f;
    [SerializeField, BoxGroup("Bullet Trail")] private float _minVertexDistance = 0.1f;
    [SerializeField, BoxGroup("Bullet Trail")] private Gradient _trailColor;
    [SerializeField, BoxGroup("Bullet Trail")] private Material _material;
    [SerializeField, BoxGroup("Bullet Trail")] private TrailRenderer _bulletTrail;
    [SerializeField, BoxGroup("Bullet Trail")] private float _bulletSpeed = 100;
    
    public void CreateTrail(Vector3 hitPoint)
    {
        StartCoroutine(SpawnTrail(hitPoint));
    }

    private IEnumerator SpawnTrail(Vector3 hitPoint)
    {
        TrailRenderer trail = WeaponUtilities.CreateTrail(_bulletTrail, _weapon.BarrelPoint, _widthCurve, _duration, _minVertexDistance, _trailColor, _material);

        yield return null;

        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(startPosition, hitPoint);
        float elapsedTime = 0f;

        while (elapsedTime < distance / _bulletSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime * _bulletSpeed / distance;
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, t);
            yield return null;
        }

        trail.transform.position = hitPoint;
        Destroy(trail.gameObject, trail.time);
    }
}
