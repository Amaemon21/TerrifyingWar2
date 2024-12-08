using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField, BoxGroup("Animation"), HorizontalLine] private string _shootName = "Shoot";
    [SerializeField, BoxGroup("Animation")] private string[] _aimShootsName;
    [SerializeField, BoxGroup("Animation")] private string _reloadBoolName = "Reload";
    [SerializeField, BoxGroup("Animation")] private string _reloadFullBoolName = "Reload Full";
    [SerializeField, BoxGroup("Animation")] private string _aimBoolName = "Aim";
    [SerializeField, BoxGroup("Animation")] private string _hideWaponName = "Hide";

    [SerializeField, BoxGroup("Animator"), HorizontalLine] private Animator _animator;

    private void Awake()
    {
        _animator.applyRootMotion = false;
        _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

    public void PlayShootAnimation(bool isAiming)
    {
        if (isAiming)
        {
            _animator.Play(_aimShootsName[Random.Range(0, _aimShootsName.Length)]);
        }
        else
        {
            _animator.Play(_shootName);
        }
    }

    public void SetAimState(bool isAiming)
    {
        _animator.SetBool(_aimBoolName, isAiming);
    }

    public void PlayReloadAnimation(bool isFullReload)
    {
        string reloadName = isFullReload ? _reloadFullBoolName : _reloadBoolName;
        _animator.SetBool(reloadName, true);
    }

    public void StopReloadAnimation(bool isFullReload)
    {
        string reloadName = isFullReload ? _reloadFullBoolName : _reloadBoolName;
        _animator.SetBool(reloadName, false);
    }

    public void SetMovementState(bool isWalking, bool isRunning)
    {
        _animator.SetBool("Walk", isWalking);
        _animator.SetBool("Run", isRunning);
    }

    public void PlayHideAnimation()
    {
        _animator.Play(_hideWaponName);
    }
}