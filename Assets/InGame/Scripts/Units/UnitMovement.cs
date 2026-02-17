using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace BasicRTS.Units {
    public class UnitMovement : MonoBehaviour {
        Camera cam;
        NavMeshAgent agent;
        
        public InputActionReference actionRef;
        public LayerMask groundLayerMask;
        public bool isCommandToMove;

        void Start() {
            cam = Camera.main;
            agent = GetComponent<NavMeshAgent>();
        }

        void Update() {
            if (actionRef.action.WasPressedThisFrame()) {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask)) {
                    isCommandToMove = true;
                    agent.SetDestination(hit.point);
                }
            }

            if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance) {
                isCommandToMove = false;
            }
        }
    }
}