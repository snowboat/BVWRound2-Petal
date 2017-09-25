using HoloToolkit.Unity;

namespace BASE {
    public class GameProgress : Singleton<GameProgress> {
        private int interactedObject;
        public int maxInteractable;
        public int InteractedCount {
            get { return interactedObject; }
            set {
                interactedObject = value;
                if (value >= maxInteractable) {
                    GameFlowManager.Instance.NextState();
                }
            }
        }
    }
}
