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
            Instance.Register(GameState.CALIBRATE, () => {
                Instantiate(GameModel.Instance.flowerPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
            });
            Instance.Register(GameState.PETALIDLE, () => {
                GameObject curve = Instantiate(GameModel.Instance.petalCurvePrefab, GameModel.Instance.heightOffset + GameModel.Instance.heightOfFlower + GameModel.Instance.flowerPosition, Quaternion.identity);
                GameObject petal = Instantiate(GameModel.Instance.petalPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
                petal.GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
                Instantiate(GameModel.Instance.dogPrefab, GameModel.Instance.heightOffset + GameModel.Instance.dogPosition, Quaternion.identity);
                Instantiate(GameModel.Instance.tree2Prefab, GameModel.Instance.heightOffset + GameModel.Instance.tree2Pos, Quaternion.identity);
                Instantiate(GameModel.Instance.tree1Prefab, GameModel.Instance.heightOffset + GameModel.Instance.tree1Pos, Quaternion.identity);
                StartCoroutine(WaitPetal());
            });
            //Instance.Register(GameState.PETALANIM, () => {
            //});
            Instance.Register(GameState.FLOWIDLE, () => Instantiate(GameModel.Instance.grassDistribution));
        }

        private IEnumerator WaitPetal() {
            yield return new WaitForSeconds(10f);
            Instance.NextState();
        }
    }
}
