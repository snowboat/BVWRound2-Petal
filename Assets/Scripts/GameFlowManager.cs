using System;
using System.Collections.Generic;
using UnityEngine;

namespace BASE {
    public enum GameState {
        CALIBRATE,
        FLOWIDLE,
        GRASSANIM,
        PETALANIM,
        DOG
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
            Debug.Log(string.Format("Leaving {0}", currState));
            if (transitions.ContainsKey(currState)) {
                transitions[currState].Invoke();
            }
            currState += 1;
        }

        private void Start() {
            Instance.Register(GameState.CALIBRATE, () => {
                Instantiate(GameModel.Instance.flowerPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
            });
            Instance.Register(GameState.GRASSANIM, () => {
                GameObject curve = Instantiate(GameModel.Instance.petalCurvePrefab, GameModel.Instance.heightOffset + GameModel.Instance.heightOfFlower + GameModel.Instance.flowerPosition, Quaternion.identity);
                GameObject petal = Instantiate(GameModel.Instance.petalPrefab, GameModel.Instance.heightOffset + GameModel.Instance.flowerPosition, Quaternion.identity);
                petal.GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
                Instantiate(GameModel.Instance.dogPrefab, GameModel.Instance.heightOffset + GameModel.Instance.dogPosition, Quaternion.identity);
            });
            //Instance.Register(GameState.PETALANIM, () => {
            //});
            Instance.Register(GameState.FLOWIDLE, () => Instantiate(GameModel.Instance.grassDistribution));
        }
    }
}
