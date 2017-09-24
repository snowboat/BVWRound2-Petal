using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using BASE;

namespace INTERACT {
    public class Distribution : MonoBehaviour {
        public GameObject grass;
        public int MaxNumber;
        public float maxTimeInterval;
        public float minTimeInterval;
        public float maxRadius;
        public float scaleFactor;

        private static System.Random rnd = new System.Random();
        private List<float> distance = new List<float>();
        private List<Vector2> positions = new List<Vector2>();

        private void Start() {
            for (int i = 0; i < MaxNumber; i++) {
                float radius = Mathf.Sqrt((float)rnd.NextDouble()) * maxRadius;
                distance.Add(radius);
                float degree = (float)rnd.NextDouble() * 360 * Mathf.Deg2Rad;
                positions.Add(new Vector2(Mathf.Sin(degree), Mathf.Cos(degree)));
            }
            distance.Sort();
            StartCoroutine(GrowGrass());
        }

        private IEnumerator GrowGrass() {
            int grassNumber = 0;
            int step = 1;
            Vector3 position = GameModel.Instance.heightOffset;
            Quaternion rotation = Quaternion.identity;
            while (true) {
                for (int i = grassNumber; i < step + grassNumber && i < MaxNumber; i++) {
                    position.x = positions[i].x * distance[i];
                    position.z = positions[i].y * distance[i];
                    rotation = Quaternion.Euler(0, (float)rnd.NextDouble() * 360, 0);
                    var grassObject = Instantiate(grass, position + GameModel.Instance.flowerPosition, rotation);
                    grassObject.transform.localScale = (float)(rnd.NextDouble() * 0.8 + 0.2) * Vector3.one * scaleFactor;
                    grassObject.transform.parent = transform;
                    //grassObject.isStatic = true;
                }
                grassNumber += step;
                step++;
                yield return new WaitForSeconds(Mathf.Lerp(maxTimeInterval, minTimeInterval, (float)grassNumber / MaxNumber));
                if (grassNumber >= MaxNumber) {
                    break;
                }
            }
            //StaticBatchingUtility.Combine(gameObject);
            CombineMesh();
            GameFlowManager.Instance.NextState();
        }

        private void CombineMesh() {
            Mesh finalMesh = new Mesh();
            MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>(false);
            CombineInstance[] combiners = new CombineInstance[meshes.Length - 1];
            for (int i = 0; i < meshes.Length - 1; i++) {
                if (meshes[i].transform == transform) continue;
                //combiners[i].subMeshIndex = 0;
                combiners[i].mesh = meshes[i].mesh;
                combiners[i].transform = meshes[i].transform.localToWorldMatrix;
                meshes[i].gameObject.SetActive(false);
            }
            finalMesh.CombineMeshes(combiners, false);
            GetComponent<MeshFilter>().sharedMesh = finalMesh;
        }
    }
}
