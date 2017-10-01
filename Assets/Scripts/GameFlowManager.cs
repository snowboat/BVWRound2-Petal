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

        /*
        private void InitializePetal(int numOfPetal)
        {
            Vector3 offsetOfPetal = new Vector3(offsetOfPetal_x[numOfPetal], 0, 0);
            GameObject curve = Instantiate(GameModel.Instance.petalCurvePrefab[numOfPetal], GameModel.Instance.heightOffset + GameModel.Instance.heightOfFlower + GameModel.Instance.flowerPosition + offsetOfPetal, Quaternion.identity);
            curve.transform.Rotate(0, directionOfPetal[numOfPetal], 0);
            petal[numOfPetal] = Instantiate(GameModel.Instance.petalPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
            petal[numOfPetal].GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
            petal[numOfPetal].GetComponent<SplineWalker>().SetMove(true);
        }
        */

        private void Start() {
            //for (int i = 0; i < GameModel.Instance.spawnings.Length; i++) {
            //    Instance.Register((GameState)i, () => GameModel.Instance.spawnings[i].Spawn());
            //}
            Instance.Register((GameState)0, () => GameModel.Instance.spawnings[0].Spawn());
            Instance.Register((GameState)1, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[2].Spawn());
            Instance.Register((GameState)2, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[3].Spawn());
            Instance.Register((GameState)3, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[4].Spawn());
            Instance.Register((GameState)4, () => GameModel.Instance.petal.GetComponent<PetalFly>().FlyEvent += () => GameModel.Instance.spawnings[5].Spawn());
            Instance.Register(GameState.CALIBRATE, () => {
                //Instantiate(GameModel.Instance.flowerPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
                //foreach (var spawn in GameModel.Instance.spawnings) {
                //    spawn.Spawn();
                //    //Instantiate(spawn.obj, spawn.pos, Quaternion.Euler(spawn.rotation));
                //}
                Instantiate(GameModel.Instance.grassDistribution);
            });
            Instance.Register(GameState.PINWHEEL, () => {
                newFlower = Instantiate(GameModel.Instance.flowerPrefab, GameModel.Instance.flowerPosition + GameModel.Instance.heightOffset, Quaternion.identity);
                StartCoroutine(FlowerGrow());
            });
            //Instance.Register(GameState.PETALIDLE, () => {
            //    for (int i=0; i<totalNumOfPetal; i++)
            //    {
            //        InitializePetal(i);
            //    }
            //    Instantiate(GameModel.Instance.dogPrefab, GameModel.Instance.heightOffset + GameModel.Instance.dogPosition, Quaternion.identity);
            //    Instantiate(GameModel.Instance.tree2Prefab, GameModel.Instance.heightOffset + GameModel.Instance.tree2Pos, Quaternion.identity);
            //    Instantiate(GameModel.Instance.tree1Prefab, GameModel.Instance.heightOffset + GameModel.Instance.tree1Pos, Quaternion.identity);
            //    StartCoroutine(WaitPetal());
            //});
            //Instance.Register(GameState.PETALANIM, () => {
            //});
            //Instance.Register(GameState.FLOWIDLE, () => Instantiate(GameModel.Instance.grassDistribution));
            //Instance.Register(GameState.FINISHFLY, () => {
            //    Instantiate(GameModel.Instance.flowerDistribution);
            //});
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
                yield return new WaitForSeconds(Time.deltaTime);
                t += 0.5f * Time.deltaTime;
            }
        }

        private IEnumerator WaitPetal() {
            yield return new WaitForSeconds(18f);
            Instance.NextState();
        }

        /*
        public GameObject GetPetal(int numOfPetal)
        {
            return petal[numOfPetal];
        }
        */
    }
}
