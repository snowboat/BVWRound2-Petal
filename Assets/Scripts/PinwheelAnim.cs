using UnityEngine;
using System.Collections;
using BASE;

namespace INTERACT {
    public class PinwheelAnim : MonoBehaviour {
        public Animator anim;

        private void Start() {
            GameFlowManager.Instance.Register(GameState.DOG, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    anim.SetTrigger("Start");
                    GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
        }
    }
}
