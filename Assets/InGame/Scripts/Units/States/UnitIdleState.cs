using BasicRTS.Units.Controllers;
using UnityEngine;

namespace BasicRTS.Units.States {
    public class UnitIdleState : StateMachineBehaviour {

        AttackController attackController;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            attackController = animator.GetComponent<AttackController>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

            if (attackController.TargetToAttack != null) {
                animator.SetBool("isFollowing",true);
            }

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            
        }
    }
}