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
        public Vector3 positionOfFlower;
        public float heightOfFlower;
        public Vector3 positionOfDog;


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
                Instantiate(GameModel.Instance.flowerPrefab, new Vector3(positionOfFlower.x, GameModel.Instance.heightOffset, positionOfFlower.z), Quaternion.identity);
            });
            Instance.Register(GameState.GRASSANIM, () => {
                GameObject curve = Instantiate(GameModel.Instance.petalCurvePrefab, new Vector3(positionOfFlower.x, GameModel.Instance.heightOffset + heightOfFlower, positionOfFlower.z), Quaternion.identity);
                GameObject petal = Instantiate(GameModel.Instance.petalPrefab, new Vector3(positionOfFlower.x, GameModel.Instance.heightOffset + heightOfFlower, positionOfFlower.z), Quaternion.identity);
                petal.GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
                Instantiate(GameModel.Instance.dogPrefab, new Vector3(positionOfDog.x, GameModel.Instance.heightOffset, positionOfDog.z), Quaternion.identity);
            });
        }
    }
}
