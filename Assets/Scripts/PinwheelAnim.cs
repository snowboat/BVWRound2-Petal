using UnityEngine;
using System.Collections;
using BASE;

namespace INTERACT {
    public class PinwheelAnim : MonoBehaviour {
        public GameObject wheel;
        private float rotateSpeed;
        public float maxRotateSpeed;
        public float rotateFactor;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.DOG, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    StartCoroutine(Rotate());
                    GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
        }

        private IEnumerator Rotate() {
            while (true) {
                if (rotateSpeed < maxRotateSpeed) {
                    rotateSpeed += rotateFactor * Time.deltaTime;
                }
                wheel.transform.Rotate(0, 0, rotateSpeed);
                yield return null;
            }
        }
    }
}
