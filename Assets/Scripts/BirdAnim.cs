using UnityEngine;
using System.Collections;
using BASE;

namespace INTERACT
{
    public class BirdAnim : MonoBehaviour
    {
        public Animator bird;
        public GameObject birdObject;
        public GameObject birdWing;
        public GameObject birdLeg;

        private void Start()
        {
            GameFlowManager.Instance.Register(GameState.FRUIT, () => {
                GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => {
                    StartCoroutine(Birdfly());
                    //GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent = null;
                };
            });
        }

        private IEnumerator Birdfly()
        {
            yield return new WaitForSeconds(3.5f);
            birdLeg.SetActive(false);
            birdWing.SetActive(true);
            bird.SetTrigger("Fly");
            birdObject.GetComponent<SplineWalker>().SetMove(true);
        }

    }
}