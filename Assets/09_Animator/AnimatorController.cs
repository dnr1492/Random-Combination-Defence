using UnityEngine;

public class AnimatorController
{
    private Animator animator;
    private AnimatorState curState;

    public AnimatorController(Animator animator)
    {
        this.animator = animator;
    }

    public void ChangeState(AnimatorState newState)
    {
        curState = newState;
        switch (curState)
        {
            case AnimatorState state when
            state == AnimatorState.Idle ||
            state == AnimatorState.Idle_LongSpear:
                animator.SetBool("Move", false);
                animator.SetBool("Move_LongSpear", false);
                DebugLogger.Log(animator.gameObject.name + ": Idle / Idle_LongSpear");
                break;

            case AnimatorState.Move:
                animator.SetBool("Move", true);
                DebugLogger.Log(animator.gameObject.name + ": Move");
                break;

            case AnimatorState.Move_LongSpear:
                animator.SetBool("Move_LongSpear", true);
                DebugLogger.Log(animator.gameObject.name + ": Move_LongSpear");
                break;

            case AnimatorState.Attack_Normal:
                animator.SetTrigger("Attack_Normal");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Normal");
                break;

            case AnimatorState.Attack_Bow:
                animator.SetTrigger("Attack_Bow");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Bow");
                break;

            case AnimatorState.Attack_Magic:
                animator.SetTrigger("Attack_Magic");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Magic");
                break;

            case AnimatorState.Attack_Axe:
                animator.SetTrigger("Attack_Axe");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Axe");
                break;

            case AnimatorState.Attack_ShotSword:
                animator.SetTrigger("Attack_ShotSword");
                DebugLogger.Log(animator.gameObject.name + ": Attack_ShotSword");
                break;

            case AnimatorState.Concentrate:
                animator.SetTrigger("Concentrate");
                DebugLogger.Log(animator.gameObject.name + ": Concentrate");
                break;

            case AnimatorState.Buff:
                animator.SetTrigger("Buff");
                DebugLogger.Log(animator.gameObject.name + ": Buff");
                break;

            case AnimatorState.Debuff:
                animator.SetTrigger("Debuff");
                DebugLogger.Log(animator.gameObject.name + ": Debuff");
                break;

            case AnimatorState.Skill_Normal:
                animator.SetTrigger("Skill_Normal");
                DebugLogger.Log(animator.gameObject.name + ": Skill_Normal");
                break;

            case AnimatorState.Skill_Bow:
                animator.SetTrigger("Skill_Bow");
                DebugLogger.Log(animator.gameObject.name + ": Skill_Bow");
                break;

            case AnimatorState.Skill_Magic:
                animator.SetTrigger("Skill_Magic");
                DebugLogger.Log(animator.gameObject.name + ": Skill_Magic");
                break;

            case AnimatorState.Hit:
                animator.SetTrigger("Hit");
                DebugLogger.Log(animator.gameObject.name + ": Hit");
                break;

            case AnimatorState.Death:
                animator.SetTrigger("Death");
                DebugLogger.Log(animator.gameObject.name + ": Death");
                break;

            case AnimatorState.Other:
                animator.SetTrigger("Other");
                DebugLogger.Log(animator.gameObject.name + ": Other");
                break;
        }
    }

    public float GetClipLength(string animationName)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in controller.animationClips) {
            if (clip.name == animationName) {
                return clip.length;
            }
        }
        return 0;
    }

    public AnimatorState ConvertCharacterAttackTypeToAnimatorStateAttackType(CharacterAttackType attackType)
    {
        switch (attackType)
        {
            case CharacterAttackType.Normal:
                return AnimatorState.Attack_Normal;

            case CharacterAttackType.Bow:
                return AnimatorState.Attack_Bow;

            case CharacterAttackType.Magic:
                return AnimatorState.Attack_Magic;

            case CharacterAttackType.LongSpear:
                return AnimatorState.Attack_LongSpear;

            case CharacterAttackType.Axe:
                return AnimatorState.Attack_Axe;

            case CharacterAttackType.ShotSword:
                return AnimatorState.Attack_ShotSword;

            default:
                return AnimatorState.Idle;
        }
    }
}
