using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace BASE {
    public class GazeBehaviour : MonoBehaviour, IFocusable {
        private bool isFocusing;
        public float focusTime = 0;
        public float maxFocusTime;

        public Action<float> GazeEvent;

        public void OnFocusEnter() {
            isFocusing = true;
        }

        public void OnFocusExit() {
            isFocusing = false;
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
