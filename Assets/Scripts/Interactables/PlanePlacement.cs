using BASE;
using UnityEngine;

namespace INTERACT {
    public class PlanePlacement : MonoBehaviour {
        public Transform anchor;

        private void Start() {
            ClickManager.Instance.InputDownEvent += () => {
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = new Vector3(0, anchor.position.y, 0);
            };
        }

        private void Update() {
            anchor.rotation = Quaternion.identity;
        }

    }
}

