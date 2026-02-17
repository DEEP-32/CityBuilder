using System.Collections.Generic;
using System.Linq;
using BasicRTS.Units.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils;

namespace BasicRTS.Units {
    public class UnitSelectionManager : Singleton<UnitSelectionManager> {
        [SerializeField] InputActionReference selectAction;
        [SerializeField] InputActionReference multiSelectAction;
        [SerializeField] InputActionReference moveAction;

        [SerializeField] float dragThreshold = 5;
        
        [SerializeField] List<Unit> allUnits = new List<Unit>();
        [SerializeField] List<Unit> selectedUnits = new List<Unit>();
        
        [SerializeField] LayerMask clickableLayerMask;
        [SerializeField] LayerMask groundLayerMask;
        [SerializeField] LayerMask attackLayerMask;
        [SerializeField] GameObject groundMarker;
        [SerializeField] Camera cam;
        
        public List<Unit> AllUnits => allUnits;
        public List<Unit> SelectedUnits => selectedUnits;

        public bool AttackCursorVisible {
            get;
            private set;
        }
        
        
        Vector2 pressedPosition;
        Vector2 releasedPosition;

        void Start() {
            cam = Camera.main;
        }

        void Update() {

            bool isShiftHeld = multiSelectAction.action.IsPressed();

            if (selectAction.action.WasPressedThisFrame()) {
                pressedPosition = Mouse.current.position.ReadValue();
            }
            
            if (selectAction.action.WasReleasedThisFrame()) {
                
                releasedPosition = Mouse.current.position.ReadValue();

                float distance = Vector2.Distance(pressedPosition, releasedPosition);

                if (distance > dragThreshold) {
                    Debug.Log($"Not doing the anything in selection manager as the input was considered drag");
                    return;
                }
                
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayerMask)) {
                    if (isShiftHeld) {
                        MultiSelect(hit.collider.gameObject);
                    }
                    else {
                        SelectByClicking(hit.collider.gameObject);
                    }
                }
                else if(!isShiftHeld){
                    DeselectAll();
                }
            }

            if (moveAction.action.WasPressedThisFrame() && selectedUnits.Count > 0)  {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask)) {
                    groundMarker.transform.position = hit.point.Add(y:.1f);
                    //Debug.Log("Ground marker : " + groundMarker.transform.position);
                    groundMarker.SetActive(false);
                    groundMarker.SetActive(true);
                }
            }

            //Attack Target
            if (selectedUnits.Count > 0 && AtLeastOneOffensiveUnit()) {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackLayerMask)) {
                    Debug.Log("Enemy Hovered with mouse");
                    AttackCursorVisible = true;
                    if (moveAction.action.WasCompletedThisFrame()) {
                        Transform target = hit.transform;
                        foreach (var selectedUnit in selectedUnits) {
                            if (selectedUnit.TryGetComponent(out AttackController attackController)) {
                                attackController.TargetToAttack = target;
                            }
                        }
                    }
                }
                else {
                    AttackCursorVisible = false;
                }
            }
        }

        public void MultiSelect(GameObject colliderGameObject) {
            var unit = colliderGameObject.GetComponent<Unit>();

            if (!selectedUnits.Contains(unit)) {
                AddSelectedUnit(unit);
                ToggleUnitMovement(unit,true);
            }
            else {
                ToggleUnitMovement(unit,false);
                RemoveSelectedUnit(unit);
            }
        }

        public void DeselectAll() {
            Debug.Log("DeselectAll");
            foreach (var selectedUnit in selectedUnits) {
                selectedUnit.SelectionIndicator.SetActive(false);
                ToggleUnitMovement(selectedUnit,false);
            }
            groundMarker.SetActive(false);
            selectedUnits.Clear();
        }

        public void SelectByClicking(GameObject colliderGameObject) {
            DeselectAll();
            var unit = colliderGameObject.GetComponent<Unit>();
            AddSelectedUnit(unit);
            ToggleUnitMovement(unit,true);
        }

        public void DragSelect(Unit unit) {

            if (!selectedUnits.Contains(unit)) {
                AddSelectedUnit(unit);
                ToggleUnitMovement(unit,true);
                
            }
            
        }

        void ToggleUnitMovement(Unit unit, bool toggle) {
            unit.GetComponent<UnitMovement>().enabled = toggle;
        }


        public void AddUnit(Unit unit) {
            allUnits.Add(unit);
        }

        public void RemoveUnit(Unit unit) {
            allUnits.Remove(unit);
        }

        public void AddSelectedUnit(Unit unit) {
            unit.SelectionIndicator.SetActive(true);
            selectedUnits.Add(unit);
        }

        public void RemoveSelectedUnit(Unit unit) {
            unit.SelectionIndicator.SetActive(false);
            selectedUnits.Remove(unit);
        }
        
        public bool AtLeastOneOffensiveUnit() {
            return selectedUnits.Any(unit => unit.GetComponent<AttackController>() != null);
        }
        
    }
}