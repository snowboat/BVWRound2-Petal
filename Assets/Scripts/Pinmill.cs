using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using BASE;

namespace INTERACT {
    public enum WHEELSTATE {
        IDLE,
        INTERACTABLE,
        FLYBACK,
        END
    }

    public class Pinmill : MonoBehaviour {
        public WHEELSTATE currState;
        public ParticleSystem particle;
        private GazeObject gaze;
        private Dictionary<WHEELSTATE, Action> transitions = new Dictionary<WHEELSTATE, Action>();

        public GameObject wheel;
        private float rotateSpeed;
        public float rotateFactor;
        private Coroutine rotateCoroutine;

        public void Register(WHEELSTATE state, Action action) {
            if (transitions.ContainsKey(state)) {
                transitions[state] += action;
            }
            else {
                transitions.Add(state, action);
            }
        }
        private void Rotate(float x) {
            var emission = particle.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
            rotateSpeed = x * rotateFactor;
            if (x == 1) {
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

        private void Start() {
            gaze = GetComponent<GazeObject>();
            Register(WHEELSTATE.IDLE, () => {
                gaze.GazeEvent += Rotate;
                rotateCoroutine = StartCoroutine(Rotate());
                particle.Play();
            });
            Register(WHEELSTATE.INTERACTABLE, () => {
                gaze.GazeEvent -= Rotate;
                gaze.focusTime = 0;
                particle.Stop();
            });
            //GameFlowManager.Instance.Register(GameState.PETALFLY, () => NextState());
        }

        private IEnumerator Rotate() {
            while (true) {
                wheel.transform.Rotate(0, 0, rotateSpeed);
                yield return null;
            }
        }

    }
}
