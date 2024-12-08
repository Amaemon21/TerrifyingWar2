using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");
    private static readonly int IsMoving = Animator.StringToHash("IsWalk");
    private static readonly int IsRun = Animator.StringToHash("IsRun");

    private void OnValidate()
    {
        _animator ??= GetComponent<Animator>();
    }

    public void Idle(bool flag)
    {
        _animator.SetBool(IsIdle, flag);
    }
    
    public void Move(bool flag)
    {
        _animator.SetBool(IsMoving, flag);
    }
    
    public void Run(bool flag)
    {
        _animator.SetBool(IsRun, flag);
    }
    
    public void PlayAttack()
    {
        _animator.SetTrigger(Attack);
    }
}