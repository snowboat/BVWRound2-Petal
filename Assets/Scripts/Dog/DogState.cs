﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using BASE;

namespace INTERACT {
    public enum DOGSTATE {
        IDLE,
        INTERACTABLE,
        RUNNING,
        END
    }

    public class DogState : MonoBehaviour {
        public DOGSTATE currState;
        public ParticleSystem particle;
        private GazeObject gaze;
        private Dictionary<DOGSTATE, Action> transitions = new Dictionary<DOGSTATE, Action>();

        public void Register(DOGSTATE state, Action action) {
            if (transitions.ContainsKey(state)) {
                transitions[state] += action;
            }
            else {
                transitions.Add(state, action);
            }
        }

        private void Waking(float x) {
            //animator.Play("Blooming", 0, x);
            var emission = particle.emission;
            emission.rateOverTime = (1 + 4 * x) * 0.5f;
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
            Register(DOGSTATE.IDLE, () => {
                gaze.GazeEvent += Waking;
                particle.Play();
            });
            Register(DOGSTATE.INTERACTABLE, () => {
                gaze.GazeEvent -= Waking;
                gaze.focusTime = 0;
                particle.Stop();
                // TODO: Dog should go back
                NextState();
            });
            Register(DOGSTATE.RUNNING, () => {
                GameProgress.Instance.InteractedCount++;
            });
            GameFlowManager.Instance.Register(GameState.PETALFLY, () => NextState());
        }
    }
}