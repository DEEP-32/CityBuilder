using System;
using UnityEngine;

namespace BasicRTS.Units.Controllers {
    
    public class AttackController : MonoBehaviour {

        [SerializeField] Transform targetToAttack;

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
    }
}