using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils;

namespace BasicRTS.Units {
    public class UnitSelectionManager : Singleton<UnitSelectionManager> {
        [SerializeField] InputActionReference selectAction;
        [SerializeField] InputActionReference multiSelectAction;
        [SerializeField] InputActionReference moveAction;
        
        [SerializeField] List<Unit> allUnits = new List<Unit>();
        [SerializeField] List<Unit> selectedUnits = new List<Unit>();
        
        [SerializeField] LayerMask clickableLayerMask;
        [SerializeField] LayerMask groundLayerMask;
        [SerializeField] GameObject groundMarker;
        [SerializeField] Camera cam;
        
        

        void Start() {
            cam = Camera.main;
        }

        void Update() {

            bool isShiftHeld = multiSelectAction.action.IsPressed();
            
            if (selectAction.action.WasPressedThisFrame()) {
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
                if (selectAction.action.WasPressedThisFrame()) {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayerMask)) {
                        groundMarker.transform.position = hit.point;
                        
                        groundMarker.SetActive(false);
                        groundMarker.SetActive(true);
                    }
                }
            }
        }

        void MultiSelect(GameObject colliderGameObject) {
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

        void DeselectAll() {
            foreach (var selectedUnit in selectedUnits) {
                selectedUnit.SelectionIndicator.SetActive(false);
                ToggleUnitMovement(selectedUnit,false);
            }
            groundMarker.SetActive(false);
            selectedUnits.Clear();
        }

        void SelectByClicking(GameObject colliderGameObject) {
            DeselectAll();
            var unit = colliderGameObject.GetComponent<Unit>();
            AddSelectedUnit(unit);
            ToggleUnitMovement(unit,true);
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
    }
}