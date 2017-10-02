using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using INTERACT;

namespace BASE {
    public enum ObjectState {
        INTERATABLE,
        INTERATING,
        INTERATED
    }

    public enum GameState {
        CALIBRATE,
        IDLE,
        COCOON,
        FRUIT,
        BIRD,
        DOG,
        PINWHEEL,
        ENDING
    }

    public class GameFlowManager : MonoBehaviour {
        public GameState currState;

        public static GameFlowManager Instance;
        private Dictionary<GameState, Action> transitions = new Dictionary<GameState, Action>();

        private GameObject newFlower;
        

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public void Register(GameState state, Action action) {
            if (transitions.ContainsKey(state)) {
                transitions[state] += action;
            }
            else {
                transitions.Add(state, action);
            }
        }

        public void NextState() {
            currState += 1;
            Debug.Log(string.Format("Leaving {0}, Heading to {1} <- {2}", currState - 1, currState, this.name));
            if (transitions.ContainsKey(currState - 1)) {
                transitions[currState - 1].Invoke();
            }
        }

        private void Start() {
            Instance.Register((GameState)0, () => {
                var obj = GameModel.Instance.spawnings[0].Spawn();
                ClickManager.Instance.InputDownEvent += () => {
                    obj.GetComponentInChildren<Canvas>().enabled = false;
                    obj.GetComponent<SphereCollider>().enabled = true;
                    obj.GetComponentInChildren<ParticleSystem>().Play();
                    ClickManager.Instance.InputDownEvent = null;
                };
            });
            //Instance.Register((GameState)0, () => GameModel.Instance.spawnings[0].Spawn());
            Instance.Register((GameState)1, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[2].Spawn());
            Instance.Register((GameState)2, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[3].Spawn());
            Instance.Register((GameState)3, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[4].Spawn());
            Instance.Register((GameState)4, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[5].Spawn());
            Instance.Register(GameState.CALIBRATE, () => {
                Instantiate(GameModel.Instance.grassDistribution);
            });
            Instance.Register(GameState.PINWHEEL, () => {
                newFlower = Instantiate(GameModel.Instance.flowerPrefab, GameModel.Instance.flowerPosition + GameModel.Instance.heightOffset, Quaternion.identity);
                StartCoroutine(FlowerGrow());
            });
        }

        private IEnumerator FlowerGrow()
        {
            float maxScale = 0.25f;
            float t = 0.0f;
            while (t <= 1.0f)
            {
                float scale = Mathf.Lerp(0.01f, maxScale, t);
                Vector3 scaleVector = new Vector3(scale, scale, scale);
                newFlower.transform.localScale = scaleVector;
                yield return null;
                t += 0.5f * Time.deltaTime;
            }
        }

        private IEnumerator WaitPetal() {
            yield return new WaitForSeconds(18f);
            Instance.NextState();
        }

    }
}
