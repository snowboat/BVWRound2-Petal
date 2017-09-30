using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using BASE;

namespace INTERACT {
    public class Distribution1 : MonoBehaviour {
        public AudioSource grassAudio;
        public int MaxNumber;
        public float maxTimeInterval;
        public float minTimeInterval;
        public float maxRadius;

        public Material mat;

        private List<Vector3> verts = new List<Vector3>();

        private static System.Random rnd = new System.Random();
        private List<float> distance = new List<float>();
        private List<Vector2> positions = new List<Vector2>();

        private void Start() {
            grassAudio.Play();
            for (int i = 0; i < MaxNumber; i++) {
                float radius = Mathf.Sqrt((float)rnd.NextDouble()) * maxRadius;
                distance.Add(radius);
                float degree = (float)rnd.NextDouble() * 360 * Mathf.Deg2Rad;
                positions.Add(new Vector2(Mathf.Sin(degree), Mathf.Cos(degree)));
            }
            distance.Sort();
            Generate();
        }

        private void Generate() {
            int grassNumber = 0;
            int step = 1;
            List<int> indices = new List<int>();
            for (int i = 0; i < MaxNumber; i++) {
                indices.Add(i);
            }
            for (int i = 0; i < MaxNumber; i++) {
                verts.Add(new Vector3(positions[i].x * distance[i], 0, positions[i].y * distance[i]) + GameModel.Instance.heightOffset);
            }
            GameObject grassLayer;
            MeshFilter mf;
            MeshRenderer renderer;
            Mesh m;
            m = new Mesh();
            grassLayer = new GameObject("grassLayer");
            mf = grassLayer.AddComponent<MeshFilter>();
            renderer = grassLayer.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = mat;
            m.vertices = verts.GetRange(0, MaxNumber).ToArray();
            m.SetIndices(indices.GetRange(0, MaxNumber).ToArray(), MeshTopology.Points, 0);
            mf.mesh = m;
            //while (grassNumber < MaxNumber) {
            //    grassNumber += step;
            //    step++;
            //    yield return null;
            //}
            //GameFlowManager.Instance.NextState();
        }
    }
}
