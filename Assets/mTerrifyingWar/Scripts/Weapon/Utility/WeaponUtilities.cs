using System.Collections;
using NTC.Pool;
using UnityEngine;

public static class WeaponUtilities
{
    public static Vector3 GetDirection(Vector3 defaultDirection, bool applySpread, Vector3 spreadVariance)
    {
        if (applySpread)
        {
            var x = Random.Range(-spreadVariance.x, spreadVariance.x);
            var y = Random.Range(-spreadVariance.y, spreadVariance.y);
            var z = Random.Range(-spreadVariance.z, spreadVariance.z);

            defaultDirection += new Vector3(x, y, z);

            defaultDirection.Normalize();
        }

        return defaultDirection;
    }

    public static TrailRenderer CreateTrail(TrailRenderer bulletTrail, Transform barrelTransform, AnimationCurve widthCurve, float duration, float minVertexDistance, Gradient trailColor, Material material)
    {
        var trail = GameObject.Instantiate(bulletTrail, barrelTransform.position, Quaternion.identity);

        trail.widthCurve = widthCurve;
        trail.time = duration;
        trail.minVertexDistance = minVertexDistance;
        trail.colorGradient = trailColor;
        trail.material = material;

        return trail;
    }
    
    public static void CreateMuzzleFlash(bool enableMuzzle, MuzzleFlash[] muzzlePrefabs, Transform barrelTransform, float scaleFactor, float destroyTime)
    {
        if (enableMuzzle)
        {
            var currentMuzzle = muzzlePrefabs[Random.Range(0, muzzlePrefabs.Length)];
            var spawnedMuzzle = NightPool.Spawn(currentMuzzle, barrelTransform.position, barrelTransform.rotation, barrelTransform);

            spawnedMuzzle.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            NightPool.Despawn(spawnedMuzzle.gameObject, destroyTime);
        }
    }

    public static IEnumerator CreateMag(int currentAmmo, float magSpawnDelay, float magEmptySpawnDelay, float magDropForce, GameObject magPrefab, GameObject magEmptyPrefab, Transform magTransform)
    {
        if (currentAmmo > 0)
        {
            yield return new WaitForSeconds(magSpawnDelay);

            var mag = Object.Instantiate(magPrefab, magTransform.position, magTransform.rotation);
            var magRigidbody = mag.GetComponent<Rigidbody>();
            magRigidbody.AddForce(Vector3.forward * magDropForce);
            magRigidbody.AddTorque(Vector3.forward * magDropForce);

            Object.Destroy(mag.gameObject, 10.0f);
        }
        else if (currentAmmo == 0)
        {
            yield return new WaitForSeconds(magEmptySpawnDelay);

            var mag = Object.Instantiate(magEmptyPrefab, magTransform.position, magTransform.rotation);
            var magRigidbody = mag.GetComponent<Rigidbody>();
            magRigidbody.AddForce(Vector3.forward * magDropForce);
            magRigidbody.AddTorque(Vector3.forward * magDropForce);

            Object.Destroy(mag.gameObject, 10.0f);
        }
    }
}