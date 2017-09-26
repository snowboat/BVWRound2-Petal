using System.Collections.Generic;
using System;
using UnityEngine;
using BASE;

namespace INTERACT {
    public enum TREESTATE {
        IDLE,
        INTERACTABLE,
        PETALFLY,
        END
    }

    public class TreeState : MonoBehaviour {
        public AudioSource audioSource;
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
            if (gaze.isFocusing && !audioSource.isPlaying) {
                audioSource.Play();
            }
            if (!gaze.isFocusing && audioSource.isPlaying) {
                audioSource.Stop();
            }
            //animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            if (x == 1) {
                emission.rateOverTime = 0.5f;
                audioSource.Stop();
                NextState();
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
            gaze = GetComponent<GazeObject>();
            Register(TREESTATE.IDLE, () => {
                gaze.GazeEvent += Shaking;
                particle.Play();
            });
            Register(TREESTATE.INTERACTABLE, () => {
                gaze.GazeEvent -= Shaking;
                gaze.focusTime = 0;
                particle.Stop();
                // TODO: Petals should start to fly back;
                NextState();
            });
            Register(TREESTATE.PETALFLY, () => {
                GameProgress.Instance.InteractedCount++;
            });
            GameFlowManager.Instance.Register(GameState.PETALFLY, () => NextState());
        }
    }
}
