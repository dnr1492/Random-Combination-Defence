using System.Threading.Tasks;
using UnityEngine;

public class AnimatorController
{
    private Animator animator;
    private AnimatorState curState;

    public AnimatorController(Animator animator)
    {
        this.animator = animator;
    }

    public void ChangeState(AnimatorState newState, CharacterAttackType attackType = CharacterAttackType.Normal)
    {
        curState = newState;
        switch (curState)
        {
            case AnimatorState state when
            state == AnimatorState.Idle ||
            state == AnimatorState.Idle_LongSpear:
                animator.SetBool("Move", false);
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

            case AnimatorState state when
            state == AnimatorState.Attack_Normal &&
            attackType == CharacterAttackType.Normal:
                animator.SetTrigger("Attack_Normal");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Normal");
                break;

            case AnimatorState state when
            state == AnimatorState.Attack_Bow &&
            attackType == CharacterAttackType.Arrow:
                animator.SetTrigger("Attack_Bow");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Bow");
                break;

            case AnimatorState state when
            state == AnimatorState.Attack_Magic &&
            attackType == CharacterAttackType.Magic:
                animator.SetTrigger("Attack_Magic");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Magic");
                break;

            case AnimatorState state when
            state == AnimatorState.Attack_Axe &&
            attackType == CharacterAttackType.Normal:
                animator.SetTrigger("Attack_Axe");
                DebugLogger.Log(animator.gameObject.name + ": Attack_Axe");
                break;

            case AnimatorState state when
            state == AnimatorState.Attack_ShotSword &&
            attackType == CharacterAttackType.Normal:
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

    //public async Task WaitForAnimationEnd(string animationName)
    //{
    //    while (true)
    //    {
    //        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //        if (stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f) {
    //            break;
    //        }

    //        await Task.Yield();
    //    }
    //}
}
