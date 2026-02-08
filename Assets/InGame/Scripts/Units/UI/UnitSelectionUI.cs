using UnityEngine;
using UnityEngine.InputSystem;

namespace BasicRTS.Units.UI {
    public class UnitSelectionUI : MonoBehaviour {
        Camera myCam;

        [SerializeField] RectTransform boxVisual;
        [SerializeField] InputActionReference dragSelectAction;

        Rect selectionBox;

        Vector2 startPosition;
        Vector2 endPosition;

        private void Start() {
            myCam = Camera.main;
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }

        private void Update() {
            // When Clicked
            if (dragSelectAction.action.WasPressedThisFrame()) {
                startPosition = Mouse.current.position.ReadValue();

                // For selection the Units
                selectionBox = new Rect();
            }

            // When Dragging
            if (dragSelectAction.action.IsPressed()) {

                if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0) {
                    UnitSelectionManager.Instance.DeselectAll();
                    SelectUnits();
                }
                
                
                endPosition = Mouse.current.position.ReadValue();
                DrawVisual();
                DrawSelection();
            }

            // When Releasing
            if (dragSelectAction.action.WasReleasedThisFrame()) {
                SelectUnits();

                startPosition = Vector2.zero;
                endPosition = Vector2.zero;
                DrawVisual();
            }
        }

        void DrawVisual() {
            // Calculate the starting and ending positions of the selection box.
            Vector2 boxStart = startPosition;
            Vector2 boxEnd = endPosition;

            // Calculate the center of the selection box.
            Vector2 boxCenter = (boxStart + boxEnd) / 2;

            // Set the position of the visual selection box based on its center.
            boxVisual.position = boxCenter;

            // Calculate the size of the selection box in both width and height.
            Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

            // Set the size of the visual selection box based on its calculated size.
            boxVisual.sizeDelta = boxSize;
        }

        void DrawSelection() {
            if (Mouse.current.position.ReadValue().x < startPosition.x) {
                selectionBox.xMin = Mouse.current.position.ReadValue().x;
                selectionBox.xMax = startPosition.x;
            }
            else {
                selectionBox.xMin = startPosition.x;
                selectionBox.xMax = Mouse.current.position.ReadValue().x;
            }


            if (Mouse.current.position.ReadValue().y < startPosition.y) {
                selectionBox.yMin = Mouse.current.position.ReadValue().y;
                selectionBox.yMax = startPosition.y;
            }
            else {
                selectionBox.yMin = startPosition.y;
                selectionBox.yMax = Mouse.current.position.ReadValue().y;
            }
        }

        void SelectUnits() {
            foreach (var unit in UnitSelectionManager.Instance.AllUnits) {
                if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position))) {
                    UnitSelectionManager.Instance.DragSelect(unit);
                }
            }
        }
    }
}