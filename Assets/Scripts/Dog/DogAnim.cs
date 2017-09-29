using UnityEngine;
using BASE;
using System.Collections;

namespace INTERACT {
    public class DogAnim : MonoBehaviour {
        public Animator anim;
        public AudioSource audio;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.BIRD, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    anim.SetTrigger("WakeUp");
                    GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
        }

        public void Bark() {
            audio.Play();
        }

        public void Walk() {
            StartCoroutine(Follow());
        }

        private IEnumerator Follow() {
            var position = GameModel.Instance.heightOffset;
            while (GameFlowManager.Instance.currState != GameState.ENDING) {
                position.x = GameModel.Instance.petal.transform.position.x;
                position.z = GameModel.Instance.petal.transform.position.z;
                transform.position = position;
                yield return null;
            }
        }
    }
}
