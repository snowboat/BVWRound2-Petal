using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BASE {
    public enum GameState {
        CALIBRATE,
        FLOWIDLE,
        GRASSANIM,
        PETALIDLE,
        PETALFLY,
        MAIN,
        FINISHFLY,
        FINISHBLOOM
    }

    public class GameFlowManager : MonoBehaviour {
        public GameState currState;

        public static GameFlowManager Instance;
        private Dictionary<GameState, Action> transitions = new Dictionary<GameState, Action>();

        // information of petals
        private GameObject[] petal = new GameObject[5];
        private int[] directionOfPetal = new int[] {-90, -80, -100, -85, -95};
        private float[] offsetOfPetal_x = new float[] { 0f, -0.17f, 0.17f, 0f, 0f };
        private int totalNumOfPetal = 3;
        

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

        private void InitializePetal(int numOfPetal)
        {
            Vector3 offsetOfPetal = new Vector3(offsetOfPetal_x[numOfPetal], 0, 0);
            GameObject curve = Instantiate(GameModel.Instance.petalCurvePrefab[numOfPetal], GameModel.Instance.heightOffset + GameModel.Instance.heightOfFlower + GameModel.Instance.flowerPosition + offsetOfPetal, Quaternion.identity);
            curve.transform.Rotate(0, directionOfPetal[numOfPetal], 0);
            petal[numOfPetal] = Instantiate(GameModel.Instance.petalPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
            petal[numOfPetal].GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
            petal[numOfPetal].GetComponent<SplineWalker>().SetMove(true);
        }

        private void Start() {
            Instance.Register(GameState.CALIBRATE, () => {
                Instantiate(GameModel.Instance.flowerPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
            });
            Instance.Register(GameState.PETALIDLE, () => {
                for (int i=0; i<totalNumOfPetal; i++)
                {
                    InitializePetal(i);
                }
                Instantiate(GameModel.Instance.dogPrefab, GameModel.Instance.heightOffset + GameModel.Instance.dogPosition, Quaternion.identity);
                Instantiate(GameModel.Instance.tree2Prefab, GameModel.Instance.heightOffset + GameModel.Instance.tree2Pos, Quaternion.identity);
                Instantiate(GameModel.Instance.tree1Prefab, GameModel.Instance.heightOffset + GameModel.Instance.tree1Pos, Quaternion.identity);
                StartCoroutine(WaitPetal());
            });
            //Instance.Register(GameState.PETALANIM, () => {
            //});
            Instance.Register(GameState.FLOWIDLE, () => Instantiate(GameModel.Instance.grassDistribution));
            Instance.Register(GameState.FINISHFLY, () => {
                Instantiate(GameModel.Instance.flowerDistribution);
            });
        }

        private IEnumerator WaitPetal() {
            yield return new WaitForSeconds(18f);
            Instance.NextState();
        }

        public GameObject GetPetal(int numOfPetal)
        {
            return petal[numOfPetal];
        }
    }
}
