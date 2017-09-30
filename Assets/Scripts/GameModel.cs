using UnityEngine;
using HoloToolkit.Unity;

namespace BASE {
    [System.Serializable]
    public struct SpawnObject {
        public GameObject obj;
        public Vector3 pos;
        public Vector3 rotation;

        public GameObject Spawn() {
            GameObject ret = GameObject.Instantiate(obj, pos + GameModel.Instance.heightOffset, Quaternion.Euler(rotation));
            return ret;
        }
    }

    public class GameModel : Singleton<GameModel> {
        public Vector3 heightOffset;
        public GameObject flowerPrefab;
        public Vector3 flowerPosition;
        public GameObject petalPrefab;
        public GameObject[] petalCurvePrefab;
        public float[] flyingDuration;
        public GameObject dogPrefab;
        public Vector3 dogPosition;
        public GameObject grassDistribution;
        public GameObject flowerDistribution;
        public Vector3 heightOfFlower;
        public GameObject tree1Prefab;
        public Vector3 tree1Pos;
        public GameObject tree2Prefab;
        public Vector3 tree2Pos;
        

        [Header("New")]
        public SpawnObject[] spawnings;
        public GameObject petal;
        //public Vector3 dyingFlowerPos;
        //public GameObject dyingFlower;
        //public Vector3 cocoonPos;
        //public GameObject cocoon;
        //public Vector3 fruitTreePos;
        //public GameObject fruitTree;
        //public Vector3 birdTreePos;
        //public GameObject birdTree;
        //public Vector3 dogPos;
        //public GameObject dogObject;
        //public Vector3 pinwheelPos;
        //public GameObject pinwheel;
    }
}
