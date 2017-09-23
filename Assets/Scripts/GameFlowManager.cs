using System;
using System.Collections.Generic;
using UnityEngine;

namespace BASE {
    public enum GameState {

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
            if (transitions.ContainsKey(currState)) {
                currState += 1;
                transitions[currState - 1].Invoke();
            }
            Debug.Log(string.Format("Leaving {0}", currState));
        }
    }
}
