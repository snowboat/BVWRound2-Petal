using System.Collections.Generic;
using System;
using UnityEngine;
using BASE;

namespace INTERACT {
    public enum TREESTATE {
        IDLE,
        INTERACTABLE,
        RUNNING
    }


    public class TreeState : MonoBehaviour {
        public TREESTATE currState;
        public ParticleSystem particle;
        private GazeObject gaze;
        private Dictionary<TREESTATE, Action> transitions = new Dictionary<TREESTATE, Action>();

        public void Register(TREESTATE state, Action action) {
            if (transitions.ContainsKey(state)) {
                transitions[state] += action;
            }
            else {
                transitions.Add(state, action);
            }
        }

        private void Shaking(float x) {
            //animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                NextState();
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
            gaze = GetComponent<GazeObject>();
            Register(TREESTATE.IDLE, () => {
                gaze.GazeEvent += Shaking;
                particle.Play();
            });
            Register(TREESTATE.INTERACTABLE, () => {
                gaze.GazeEvent -= Shaking;
                gaze.focusTime = 0;
                particle.Stop();
            });
            GameFlowManager.Instance.Register(GameState.PETALFLY, () => NextState());
        }
    }
}
