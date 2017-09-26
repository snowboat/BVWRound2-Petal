using System;
using System.Collections.Generic;
using UnityEngine;
using BASE;

namespace INTERACT {
    public enum BIRDSTATE {
        IDLE,
        INTERACTABLE,
        FLYBACK,
        END
    }

    public class BirdState : MonoBehaviour {
        public AudioSource birdAudio;
        public BIRDSTATE currState;
        public ParticleSystem particle;
        public Animator animator;
        private GazeObject gaze;
        private Dictionary<BIRDSTATE, Action> transitions = new Dictionary<BIRDSTATE, Action>();

        [Header("Component")]
        public GameObject standWing;
        public GameObject flyWing;

        public void Register(BIRDSTATE state, Action action) {
            if (transitions.ContainsKey(state)) {
                transitions[state] += action;
            }
            else {
                transitions.Add(state, action);
            }
        }

        private void Tweeting(float x) {
            if (gaze.isFocusing && !birdAudio.isPlaying) {
                birdAudio.Play();
            }
            if (!gaze.isFocusing && birdAudio.isPlaying) {
                birdAudio.Stop();
            }

            //animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            if (x == 1) {
                birdAudio.Stop();
                emission.rateOverTime = 0.5f;
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

        private void StartFly() {
            standWing.SetActive(false);
            flyWing.SetActive(true);
            animator.SetTrigger("Fly");
        }

        private void Start() {
            gaze = GetComponent<GazeObject>();
            Register(BIRDSTATE.IDLE, () => {
                gaze.GazeEvent += Tweeting;
                particle.Play();
            });
            Register(BIRDSTATE.INTERACTABLE, () => {
                gaze.GazeEvent -= Tweeting;
                gaze.focusTime = 0;
                particle.Stop();
                StartFly();
                // TODO: Petals should start to fly back;
                NextState();
            });
            Register(BIRDSTATE.FLYBACK, () => {
                GameProgress.Instance.InteractedCount++;
            });
            GameFlowManager.Instance.Register(GameState.PETALFLY, () => NextState());
        }
    }
}
