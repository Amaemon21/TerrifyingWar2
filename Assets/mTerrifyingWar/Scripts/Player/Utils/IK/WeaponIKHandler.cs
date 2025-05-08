using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;
using Zenject;

public class WeaponIKHandler : MonoBehaviour
{
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [SerializeField] private FPSPlayerSettings _playerSettings;

    private WeaponContainer _weaponContainer;
    
    private KTwoBoneIkData _rightHandIk;
    private KTwoBoneIkData _leftHandIk;

    private float _ikMotionPlayBack;
    private IKMotion _activeMotion;
    private KTransform _ikMotion = KTransform.Identity;
    private KTransform _cachedIkMotion = KTransform.Identity;

    private void Awake()
    {
        _weaponContainer = GetComponent<WeaponContainer>();
    }

    public void PlayIkMotion(IKMotion newMotion)
    {
        _ikMotionPlayBack = 0f;
        _cachedIkMotion = _ikMotion;
        _activeMotion = newMotion;
    }

    private void LateUpdate()
    {
        if ( _weaponContainer.WeaponHolder.GetCurrentWeaponSlot() == null) 
            return;
        
        KAnimationMath.RotateInSpace(transform.root, _weaponContainer.TransformsContainer.RightHand.tip,
            _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.rightHandSprintOffset,
            _weaponContainer.HandAnimator.GetFloat(AnimationsConstrains.TAC_SPRINT_WEIGHT));

        KTransform weaponTransform = GetWeaponPose();

        weaponTransform.rotation =
            KAnimationMath.RotateInSpace(weaponTransform, weaponTransform, AnimationsConstrains.ANIMATED_OFFSET, 1f);

        KTransform rightHandTarget =
            weaponTransform.GetRelativeTransform(new KTransform(_weaponContainer.TransformsContainer.RightHand.tip), false);
        
        KTransform leftHandTarget =
            weaponTransform.GetRelativeTransform(new KTransform(_weaponContainer.TransformsContainer.LeftHand.tip), false);

        ProcessOffsets(ref weaponTransform);
        ProcessAds(ref weaponTransform);
        ProcessAdditives(ref weaponTransform);
        ProcessIkMotion(ref weaponTransform);
        ProcessRecoil(ref weaponTransform);

        _weaponContainer.TransformsContainer.WeaponBone.position = weaponTransform.position;
        _weaponContainer.TransformsContainer.WeaponBone.rotation = weaponTransform.rotation;

        rightHandTarget = weaponTransform.GetWorldTransform(rightHandTarget, false);
        leftHandTarget = weaponTransform.GetWorldTransform(leftHandTarget, false);

        SetupIkData(ref _rightHandIk, rightHandTarget, _weaponContainer.TransformsContainer.RightHand, _playerSettings.ikWeight);
        SetupIkData(ref _leftHandIk, leftHandTarget, _weaponContainer.TransformsContainer.LeftHand, _playerSettings.ikWeight);

        KTwoBoneIK.Solve(ref _rightHandIk);
        KTwoBoneIK.Solve(ref _leftHandIk);

        ApplyIkData(_rightHandIk, _weaponContainer.TransformsContainer.RightHand);
        ApplyIkData(_leftHandIk, _weaponContainer.TransformsContainer.LeftHand);
    }

    private void SetupIkData(ref KTwoBoneIkData ikData, in KTransform target, in IKTransforms transforms, float weight = 1f)
    {
        ikData.target = target;

        ikData.tip = new KTransform(transforms.tip);
        ikData.mid = ikData.hint = new KTransform(transforms.mid);
        ikData.root = new KTransform(transforms.root);

        ikData.hintWeight = weight;
        ikData.posWeight = weight;
        ikData.rotWeight = weight;
    }

    private void ApplyIkData(in KTwoBoneIkData ikData, in IKTransforms transforms)
    {
        transforms.root.rotation = ikData.root.rotation;
        transforms.mid.rotation = ikData.mid.rotation;
        transforms.tip.rotation = ikData.tip.rotation;
    }

    private void ProcessOffsets(ref KTransform weaponT)
    {
        var root = transform.root;
        KTransform rootT = new KTransform(root);
        var weaponOffset = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.ikOffset;

        float mask = 1f - _weaponContainer.HandAnimator.GetFloat(AnimationsConstrains.TAC_SPRINT_WEIGHT);
        weaponT.position = KAnimationMath.MoveInSpace(rootT, weaponT, weaponOffset, mask);

        var settings = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings;
        KAnimationMath.MoveInSpace(root, _weaponContainer.TransformsContainer.RightHand.root, settings.rightClavicleOffset, mask);
        KAnimationMath.MoveInSpace(root, _weaponContainer.TransformsContainer.LeftHand.root, settings.leftClavicleOffset, mask);
    }

    private void ProcessAdditives(ref KTransform weaponT)
    {
        KTransform rootT = new KTransform(_weaponContainer.TransformsContainer.SkeletonRoot);
        
        KTransform additive =
            rootT.GetRelativeTransform(new KTransform(_weaponContainer.TransformsContainer.WeaponBoneAdditive), false);

        float weight = Mathf.Lerp(1f, 0.3f, _playerProvider.PlayerController.AdsWeight) * (1f - _weaponContainer.HandAnimator.GetFloat(AnimationsConstrains.GRENADE_WEIGHT));

        weaponT.position = KAnimationMath.MoveInSpace(rootT, weaponT, additive.position, weight);
        weaponT.rotation = KAnimationMath.RotateInSpace(rootT, weaponT, additive.rotation, weight);
    }

    private void ProcessRecoil(ref KTransform weaponT)
    {
        KTransform recoil = new KTransform
        {
            rotation = _weaponContainer.RecoilAnimation.OutRot,
            position = _weaponContainer.RecoilAnimation.OutLoc,
        };

        KTransform root = new KTransform(transform);
        weaponT.position = KAnimationMath.MoveInSpace(root, weaponT, recoil.position, 1f);
        weaponT.rotation = KAnimationMath.RotateInSpace(root, weaponT, recoil.rotation, 1f);
    }

    private void ProcessAds(ref KTransform weaponT)
    {
        var weaponOffset = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.ikOffset;
        var adsPose = weaponT;

        KTransform aimPoint = KTransform.Identity;

        aimPoint.position =
            -_weaponContainer.TransformsContainer.WeaponBone.InverseTransformPoint(_weaponContainer.WeaponHolder.GetCurrentWeaponSlot()
                .AimPoint.position);
        
        aimPoint.position -= _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.aimPointOffset;
        
        aimPoint.rotation = Quaternion.Inverse(_weaponContainer.TransformsContainer.WeaponBone.rotation) *
                            _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().AimPoint.rotation;

        var root = new KTransform(_weaponContainer.TransformsContainer.CameraPoint);
        adsPose.position = KAnimationMath.MoveInSpace(root, adsPose,
            _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().adsPose.position - weaponOffset, 1f);
        
        adsPose.rotation = KAnimationMath.RotateInSpace(root, adsPose,
            _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().adsPose.rotation, 1f);

        float adsBlendWeight = _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().WeaponSettings.adsBlend;
        adsPose.position = Vector3.Lerp(_weaponContainer.TransformsContainer.CameraPoint.position, adsPose.position, adsBlendWeight);
        
        adsPose.rotation =
            Quaternion.Slerp(_weaponContainer.TransformsContainer.CameraPoint.rotation, adsPose.rotation, adsBlendWeight);

        adsPose.position = KAnimationMath.MoveInSpace(root, adsPose, aimPoint.rotation * aimPoint.position, 1f);
        adsPose.rotation = KAnimationMath.RotateInSpace(root, adsPose, aimPoint.rotation, 1f);

        float weight = KCurves.EaseSine(0f, 1f, _playerProvider.PlayerController.AdsWeight);

        weaponT.position = Vector3.Lerp(weaponT.position, adsPose.position, weight);
        weaponT.rotation = Quaternion.Slerp(weaponT.rotation, adsPose.rotation, weight);
    }

    private KTransform GetWeaponPose()
    {
        KTransform defaultWorldPose =
            new KTransform(_weaponContainer.TransformsContainer.RightHand.tip).GetWorldTransform(
                _weaponContainer.WeaponHolder.GetCurrentWeaponSlot().rightHandPose, false);
        
        float weight = _weaponContainer.HandAnimator.GetFloat(AnimationsConstrains.RIGHT_HAND_WEIGHT);

        return KTransform.Lerp(new KTransform(_weaponContainer.TransformsContainer.WeaponBone), defaultWorldPose, weight);
    }

    private void ProcessIkMotion(ref KTransform weaponT)
    {
        if (_activeMotion == null) return;

        _ikMotionPlayBack = Mathf.Clamp(_ikMotionPlayBack + _activeMotion.playRate * Time.deltaTime, 0f,
            _activeMotion.GetLength());

        Vector3 positionTarget = _activeMotion.translationCurves.GetValue(_ikMotionPlayBack);
        positionTarget.x *= _activeMotion.translationScale.x;
        positionTarget.y *= _activeMotion.translationScale.y;
        positionTarget.z *= _activeMotion.translationScale.z;

        Vector3 rotationTarget = _activeMotion.rotationCurves.GetValue(_ikMotionPlayBack);
        rotationTarget.x *= _activeMotion.rotationScale.x;
        rotationTarget.y *= _activeMotion.rotationScale.y;
        rotationTarget.z *= _activeMotion.rotationScale.z;

        _ikMotion.position = positionTarget;
        _ikMotion.rotation = Quaternion.Euler(rotationTarget);

        if (!Mathf.Approximately(_activeMotion.blendTime, 0f))
        {
            _ikMotion = KTransform.Lerp(_cachedIkMotion, _ikMotion, _ikMotionPlayBack / _activeMotion.blendTime);
        }

        var root = new KTransform(_weaponContainer.TransformsContainer.CameraPoint);
        weaponT.position = KAnimationMath.MoveInSpace(root, weaponT, _ikMotion.position, 1f);
        weaponT.rotation = KAnimationMath.RotateInSpace(root, weaponT, _ikMotion.rotation, 1f);
    }
}