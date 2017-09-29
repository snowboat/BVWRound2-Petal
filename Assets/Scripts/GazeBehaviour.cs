using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace BASE {
    public class GazeBehaviour : MonoBehaviour, IFocusable {
        public bool isFocusing;
        public float focusTime = 0;
        public float maxFocusTime;

        public Action<float> GazeEvent;
        public Action GazeEnterEvent;
        public Action GazeExitEvent;

        public void OnFocusEnter() {
            isFocusing = true;
            if (GazeEnterEvent != null) {
                GazeEnterEvent.Invoke();
            }
        }

        public void OnFocusExit() {
            isFocusing = false;
            if (GazeExitEvent != null) {
                GazeExitEvent.Invoke();
            }
        }

        private void Update() {
            if (isFocusing) {
                if (focusTime < maxFocusTime) {
                    focusTime += Time.deltaTime;
                }
                else {
                    focusTime = maxFocusTime;
                }
            }
            else {
                if (focusTime > 0) {
                    focusTime -= Time.deltaTime;
                }
                else {
                    focusTime = 0;
                }
            }
            var ratio = Mathf.Lerp(0, 1f, focusTime / maxFocusTime);
            if (GazeEvent != null) {
                GazeEvent.Invoke(ratio);
            }
        }


    }
}
