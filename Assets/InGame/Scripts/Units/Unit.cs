using System;
using UnityEngine;

namespace BasicRTS.Units {
    public class Unit : MonoBehaviour {

        [SerializeField] GameObject selectionIndicator;

        public GameObject SelectionIndicator => selectionIndicator; 

        void Start() {
            UnitSelectionManager.Instance.AddUnit(this);
        }

        void OnDestroy() {
            UnitSelectionManager.Instance.RemoveSelectedUnit(this);
        }
    }
}