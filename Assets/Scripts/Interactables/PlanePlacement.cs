using BASE;
using UnityEngine;

namespace INTERACT {
    public class PlanePlacement : MonoBehaviour {
        public Transform anchor;

        private void OnPlacement() {
            GameModel.Instance.heightOffset.y = anchor.position.y;
            GameFlowManager.Instance.NextState();
            Destroy(gameObject);
        }

        private void Start() {
            ClickManager.Instance.InputDownEvent += OnPlacement;
        }

        private void Update() {
            anchor.rotation = Quaternion.identity;
        }

        private void OnDestroy() {
            ClickManager.Instance.InputDownEvent -= OnPlacement;
        }
    }
}

