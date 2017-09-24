using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribution : MonoBehaviour {
    public GameObject grass;
    public int MaxNumber;
    public float timeInterval;
    public float maxTimeInterval;
    public float minTimeInterval;

    private static System.Random rnd = new System.Random();
    private List<float> distance = new List<float>();
    private List<Vector2> positions = new List<Vector2>();

	// Use this for initialization
	private void Start () {
		for (int i = 0; i < MaxNumber; i++) {
            float radius = Mathf.Sqrt((float)rnd.NextDouble()) * 5;
            distance.Add(radius);
            float degree = (float)rnd.NextDouble() * 360 * Mathf.Deg2Rad;
            positions.Add(new Vector2(Mathf.Sin(degree), Mathf.Cos(degree)));
        }
        distance.Sort();
        StartCoroutine(GrowGrass());
        //for (int i = 0; i < MaxNumber; i++) {
        //    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    cube.transform.position = new Vector3(positions[i].x * distance[i], 0, positions[i].y * distance[i]);
        //}
    }

    private IEnumerator GrowGrass() {
        int grassNumber = 0;
        int step = 1;
        Vector3 position = new Vector3();
        Quaternion rotation = Quaternion.identity;
        while (true) {
            for (int i = grassNumber; i < step + grassNumber && i < MaxNumber; i++) {
                position.x = positions[i].x * distance[i];
                position.z = positions[i].y * distance[i];
                rotation = Quaternion.Euler(0, (float)rnd.NextDouble() * 360, 0);
                //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.transform.position = new Vector3(positions[i].x * distance[i], 0, positions[i].y * distance[i]);
                var grassObject = Instantiate(grass, position, rotation);
                grassObject.transform.localScale = (float)(rnd.NextDouble() * 0.8 + 0.2) * Vector3.one;
            }
            grassNumber += step;
            step++;
            yield return new WaitForSeconds(Mathf.Lerp(maxTimeInterval, minTimeInterval, (float)grassNumber / MaxNumber));
            //yield return null;
            if (grassNumber >= MaxNumber) {
                break;
            }
        }
    }
}
