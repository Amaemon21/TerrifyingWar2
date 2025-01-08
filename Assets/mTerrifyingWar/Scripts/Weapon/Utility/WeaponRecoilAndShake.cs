using Code.Global.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponRecoilAndShake : MonoBehaviour
{
    private Vector3 _targetRotation;
    private Vector3 _currentRotation;

    public void Recoil(float returnSpeed, float snappiness)
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void AddRecoil(float recoilX, float recoilY, float recoilZ)
    {
        _targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
    
    public void AddRecoil(RecoilPreset recoilPreset)
    {
        _targetRotation += new Vector3(-recoilPreset.RecoilX, Random.Range(-recoilPreset.RecoilY, recoilPreset.RecoilY), Random.Range(-recoilPreset.RecoilZ, recoilPreset.RecoilZ));
    }
    
    public void PlayShakeAnimation(ShakePreset shakePreset)
    {
        AnimationShortCuts.ShakeRotationAnimation(transform, shakePreset.ShakeSettings.RotationShake);
        AnimationShortCuts.ShakePositionAnimation(transform, shakePreset.ShakeSettings.PositionShake);
    }
}