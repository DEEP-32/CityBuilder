using System;
using UnityEngine;

namespace BasicRTS.Units.Controllers {
    
    public class AttackController : MonoBehaviour {

        [SerializeField] Transform targetToAttack;
        public Material idleStateMaterial;
        public Material followStateMaterial;
        public Material attackStateMaterial;

        public Transform TargetToAttack {
            get => targetToAttack;

            set => targetToAttack = value;
        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Enemy") && targetToAttack == null) {
                targetToAttack =  other.transform;
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.CompareTag("Enemy") && targetToAttack != null) {
                targetToAttack = null;
            }
        }

        public void SetIdleMaterial() {
            GetComponent<Renderer>().material = idleStateMaterial;
        }
        public void SetFollowMaterial() {
            GetComponent<Renderer>().material = followStateMaterial;
        }
        public void SetAttackMaterial() {
            GetComponent<Renderer>().material = attackStateMaterial;
        }

        void OnDrawGizmosSelected() {
            //Follow
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 10f * transform.lossyScale.x);
            
            //Attack
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
            
            //Stop Attack
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1.2f);
        }
    }
}