using UnityEngine;
using BASE;
using System.Collections;

namespace INTERACT {
    public class DogAnim : MonoBehaviour {
        public Animator anim;
        public AudioSource audio;
        public float speed;
        public GameObject trick;

        private Coroutine follow;

        public Vector3 yuanfang;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.BIRD, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    anim.SetTrigger("WakeUp");
                    //GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
            GameFlowManager.Instance.Register(GameState.DOG, () => {
                if (follow != null) {
                    StopCoroutine(follow);
                }
                StartCoroutine(GoAway());
            });
        }

        public void Bark() {
            audio.Play();
        }

        public void Walk() {
            trick.transform.rotation = Quaternion.identity;
            follow = StartCoroutine(Follow());
        }

        private IEnumerator Follow() {
            var position = GameModel.Instance.heightOffset;
            while (GameFlowManager.Instance.currState != GameState.ENDING) {
                position.x = GameModel.Instance.petal.transform.position.x;
                position.z = GameModel.Instance.petal.transform.position.z;
                var rotation = Quaternion.LookRotation(new Vector3(position.x - transform.position.x, 0, position.z - transform.position.z));
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
                transform.position += speed * Time.deltaTime * (transform.rotation * Vector3.forward);
                yield return null;
            }
        }

        private IEnumerator GoAway() {
            var position = GameModel.Instance.heightOffset;
            while (true) {
                position.x = yuanfang.x;
                position.z = yuanfang.z;
                var rotation = Quaternion.LookRotation(new Vector3(position.x - transform.position.x, 0, position.z - transform.position.z));
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
                transform.position += speed * Time.deltaTime * (transform.rotation * Vector3.forward);
                yield return null;
            }
        }
    }
}
