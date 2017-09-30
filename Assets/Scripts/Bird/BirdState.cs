using System;
using System.Collections;
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
        public GameObject bird;
        public GameObject standWing;
        public GameObject flyWing;
        public GameObject birdCurve;

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
        /*
        private void StartFly() {
            standWing.SetActive(false);
            flyWing.SetActive(true);
            animator.SetTrigger("Fly");
            StartCoroutine(FlyCoroutine());
        }
        */

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
                // StartFly();
                // TODO: Petals should start to fly back;
             
            });
            Register(BIRDSTATE.FLYBACK, () => {
                GameProgress.Instance.InteractedCount++;
            });
            //GameFlowManager.Instance.Register(GameState.PETALFLY, () => NextState());
        }
        /*
        private IEnumerator FlyCoroutine()
        {
            // Fly to the petal
            Vector3 offset = new Vector3(-0.9f, 0f, 0f);
            GameObject curve = Instantiate(birdCurve, bird.transform.position + offset, Quaternion.identity);
            curve.transform.parent = transform;
            // curve.transform.Rotate(0, directionOfPetal[numOfPetal], 0);
            bird.GetComponent<SplineWalker>().spline = curve.GetComponent<BezierSpline>();
            bird.GetComponent<SplineWalker>().rotationAdjustment = new Vector3(0, -90, 0);
            bird.GetComponent<SplineWalker>().SetMove(true);
            yield return new WaitForSeconds(6.0f);
            // Pick the petal #TODO
            // GameFlowManager.Instance.GetPetal(0).transform.position = new Vector3(0f, 0f, 0f); 
            // GameFlowManager.Instance.GetPetal(0).transform.parent = bird.transform;
            // Fly to the flower #TODO
            GameFlowManager.Instance.GetPetal(0).GetComponent<SplineWalker>().SetGoingForward(false);
            yield return new WaitForSeconds(18.0f);
            GameFlowManager.Instance.GetPetal(0).SetActive(false);
            NextState();
            // Fly away #TODO
        }
        */
    }
}
