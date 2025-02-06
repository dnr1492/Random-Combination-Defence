using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    private CharacterAnimationState curState = CharacterAnimationState.Idle;

    private void Update()
    {
        switch (curState)
        {
            case CharacterAnimationState.Idle:
                animator.SetFloat("Speed", 0);
                break;

            case CharacterAnimationState.Running:
                animator.SetFloat("Speed", 1);
                break;

            case CharacterAnimationState.Jumping:
                animator.SetTrigger("JumpTrigger");
                break;

            case CharacterAnimationState.Attacking:
                animator.SetTrigger("AttackTrigger");
                break;
        }
    }

    public void ChangeState(CharacterAnimationState newState)
    {
        curState = newState;
    }
}
