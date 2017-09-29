using UnityEngine;
using BASE;
using System.Collections;

namespace INTERACT {
    public class TreeAnim : MonoBehaviour {
        public Vector3[] positions;
        public GameObject applePrefab;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.COCOON, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    StartCoroutine(GrowApple());
                    GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
        }

        private IEnumerator GrowApple() {
            GameObject apple = null;
            for (int i = 0; i < 3; i++) {
                var tmp = Instantiate(applePrefab, positions[i] + transform.position, Quaternion.identity);
                if (i == 0) {
                    apple = tmp;
                }
                yield return new WaitForSeconds(1f);
            }
            if (apple != null) {
                apple.GetComponent<Apple>().Drop();
            }
        }
    }
}
