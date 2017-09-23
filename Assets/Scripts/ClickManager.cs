using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using HoloToolkit.Unity;

namespace BASE {
    public class ClickManager : Singleton<ClickManager>, IInputHandler {
        public IInputSource inputSource { private set; get; } 
        public uint inputSourceID { private set; get; }

        public Vector3 position { get { return pos; } }
        private Vector3 pos;
        public Quaternion rotation { get { return rot; } }
        private Quaternion rot;

        public bool isClicked { private set; get; }

        public Action InputDownEvent;
        public Action InputUpEvent;

        public void OnInputDown(InputEventData eventData) {
            Debug.Log("Trigger down");
            inputSource = eventData.InputSource;
            inputSourceID = eventData.SourceId;
            GetPositionRotation();

            isClicked = true;
            if (InputDownEvent != null) {
                InputDownEvent.Invoke();
            }
        }

        public void OnInputUp(InputEventData eventData) {
            Debug.Log("Trigger up");
            isClicked = false;
            if (InputUpEvent != null) {
                InputUpEvent.Invoke();
            }
        }

        private void Start() {
            InputManager.Instance.AddGlobalListener(gameObject);
        }

        private void Update() {
            if (isClicked) {
                GetPositionRotation();
            }
        }

        private void GetPositionRotation() {
            inputSource.TryGetPosition(inputSourceID, out pos);
            inputSource.TryGetOrientation(inputSourceID, out rot);
        }
    }
}
