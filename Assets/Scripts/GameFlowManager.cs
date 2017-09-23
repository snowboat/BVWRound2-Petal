using System;
using System.Collections.Generic;
using UnityEngine;

namespace BASE {
    public enum GameState {
        CALIBRATE,
        FLOWIDLE,
        GRASSANIM,
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
                Instantiate(GameModel.Instance.flowerPrefab, new Vector3(0, GameModel.Instance.heightOffset, 2), Quaternion.identity);
            });
            Instance.Register(GameState.GRASSANIM, () => {
                GameObject curve = Instantiate(GameModel.Instance.petalCurvePrefab, new Vector3(0, GameModel.Instance.heightOffset, 2), Quaternion.identity);
                GameObject petal = Instantiate(GameModel.Instance.petalPrefab, new Vector3(0, GameModel.Instance.heightOffset, 2), Quaternion.identity);
                petal.GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
                Instantiate(GameModel.Instance.dogPrefab, new Vector3(0, GameModel.Instance.heightOffset, -2), Quaternion.identity);
            });
        }
    }
}
