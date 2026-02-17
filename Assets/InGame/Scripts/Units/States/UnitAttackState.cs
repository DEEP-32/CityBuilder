using BasicRTS.Units.Controllers;
using UnityEngine;
using UnityEngine.AI;

namespace BasicRTS.Units.States {
    public class UnitAttackState : StateMachineBehaviour {

        NavMeshAgent agent;
        AttackController attackController;

        public float stopAttackingDistance = 1.2f;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            agent = animator.GetComponent<NavMeshAgent>();
            attackController = animator.GetComponent<AttackController>();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (attackController.TargetToAttack != null && attackController.GetComponent<UnitMovement>().isCommandToMove == false) {
                LookAtTarget();
                //keep moving towards enemy
                agent.SetDestination(attackController.TargetToAttack.position);
                //Should unit still attack 
                
                float distFromTarget = Vector3.Distance(
                    attackController.TargetToAttack.position, 
                    animator.transform.position
                );

                if (distFromTarget > stopAttackingDistance || attackController.TargetToAttack == null) {
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true);
                    Debug.Log("Units attacking");
                }
            }
        }

        

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
        
        void LookAtTarget() {
            Vector3 dir = attackController.TargetToAttack.position - agent.transform.position;
            agent.transform.rotation = Quaternion.LookRotation(dir);

            var yRotation = agent.transform.eulerAngles.y;
            agent.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        }
    }
}