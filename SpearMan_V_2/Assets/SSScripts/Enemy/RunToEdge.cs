using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToEdge : StateMachineBehaviour
{
    public Enemy enemy;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.gameObject.GetComponentInParent<Enemy>();

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        enemy.RunToEdge();
        if (enemy.CanMoveForward() == false)
        {
            animator.SetTrigger("Idle");
        }

        if (enemy.canAttackPlayer() && enemy.relodTime <= 0)
        {
            animator.SetTrigger("Attack");
        }
        else if (enemy.canAttackPlayer() == false || enemy.rememberTime <= 0)
        {
            animator.SetTrigger("Patrol");
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
