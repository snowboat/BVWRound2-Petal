using UnityEngine;
using HoloToolkit.Unity;

public class GameModel : Singleton<GameModel> {
    public Vector3 heightOffset;
    public GameObject flowerPrefab;
    public Vector3 flowerPosition;
    public GameObject petalPrefab;
    public GameObject[] petalCurvePrefab;
    public GameObject dogPrefab;
    public Vector3 dogPosition;
    public GameObject grassDistribution;
    public Vector3 heightOfFlower;
    public GameObject tree1Prefab;
    public Vector3 tree1Pos;
    public GameObject tree2Prefab;
    public Vector3 tree2Pos;
}
