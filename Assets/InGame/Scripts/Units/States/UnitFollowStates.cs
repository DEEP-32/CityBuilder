using BasicRTS.Units.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace BasicRTS.Units.States {
    public class UnitFollowStates : StateMachineBehaviour {

        public float attackDistance = 1f;
        
        AttackController attackController;
        NavMeshAgent navMeshAgent;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            attackController = animator.GetComponent<AttackController>();
            navMeshAgent = animator.GetComponent<NavMeshAgent>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            //Should transition to idle

            if (attackController.TargetToAttack == null) {
                animator.SetBool("isFollowing", false);
            }

            else {
                if (animator.GetComponent<UnitMovement>().isCommandToMove == false) {
                    navMeshAgent.SetDestination(attackController.TargetToAttack.position);
                    animator.transform.LookAt(attackController.TargetToAttack);
                }
                
                //Should transition to attack

                float distFromTarget = Vector3.Distance(
                    attackController.TargetToAttack.position, 
                    animator.transform.position
                );

                if (distFromTarget < attackDistance) {
                    //animator.SetBool("isAttacking", true);
                    //navMeshAgent.SetDestination(animator.transform.position);
                    Debug.Log("Units attacking");
                }
            }
            
            //Move unit
            

           
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        }

        
    }
}