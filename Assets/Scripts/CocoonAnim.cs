using UnityEngine;
using System.Collections;
using BASE;

namespace INTERACT {
    public class CocoonAnim : MonoBehaviour {
        public Animator cocoon;
        public GameObject cocoonObject;
        public GameObject butterfly;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.IDLE, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    StartCoroutine(Butterfly());
                    GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
        }

        private IEnumerator Butterfly() {
            cocoon.SetTrigger("Start");
            yield return new WaitForSeconds(2f);
            cocoonObject.SetActive(false);
            butterfly.SetActive(true);
            butterfly.GetComponent<SplineWalker>().SetMove(true);
            if (butterfly.GetComponent<SplineWalker>().spline!= null)
            {
                Debug.Log("has spline");
            }
        }

    }
}

