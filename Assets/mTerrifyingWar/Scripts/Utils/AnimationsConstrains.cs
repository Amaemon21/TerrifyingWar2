using UnityEngine;

public static class AnimationsConstrains
{
    public static int RIGHT_HAND_WEIGHT = Animator.StringToHash("RightHandWeight");
    public static int TAC_SPRINT_WEIGHT = Animator.StringToHash("TacSprintWeight");
    public static int GRENADE_WEIGHT = Animator.StringToHash("GrenadeWeight");
    public static int THROW_GRENADE = Animator.StringToHash("ThrowGrenade");
    public static int GAIT = Animator.StringToHash("Gait");
    public static int IS_IN_AIR = Animator.StringToHash("IsInAir");
    
    public static int RELOAD_EMPTY = Animator.StringToHash("Reload_Empty");
    public static int RELOAD_TAC = Animator.StringToHash("Reload_Tac");
    public static int FIRE = Animator.StringToHash("Fire");
    public static int FIREOUT = Animator.StringToHash("FireOut");

    public static int EQUIP = Animator.StringToHash("Equip");
    public static int EQUIP_OVERRIDE = Animator.StringToHash("Equip_Override");
    public static int UNEQUIP = Animator.StringToHash("UnEquip");
    public static int IDLE = Animator.StringToHash("Idle");
    
    public static Quaternion ANIMATED_OFFSET = Quaternion.Euler(90f, 0f, 0f);
}
