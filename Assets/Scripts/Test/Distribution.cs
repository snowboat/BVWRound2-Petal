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
            //CombineMesh();
            //CombineMesh2();
            GameFlowManager.Instance.NextState();
        }

        private void CombineMesh() {
            Mesh finalMesh = new Mesh();
            MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>(false);
            CombineInstance[] combiners = new CombineInstance[meshes.Length - 1];
            for (int i = 1; i < meshes.Length; i++) {
                if (meshes[i].transform == transform) continue;
                //combiners[i].subMeshIndex = 0;
                combiners[i - 1].mesh = meshes[i].sharedMesh;
                combiners[i - 1].transform = meshes[i].transform.localToWorldMatrix;
                meshes[i].gameObject.SetActive(false);
            }
            finalMesh.CombineMeshes(combiners);
            GetComponent<MeshFilter>().sharedMesh = finalMesh;
        }

        private void CombineMesh2() {
            SkinnedMeshRenderer[] smRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            List<Transform> bones = new List<Transform>();
            List<BoneWeight> boneWeights = new List<BoneWeight>();
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            List<Texture2D> textures = new List<Texture2D>();
            int numSubs = 0;

            foreach (SkinnedMeshRenderer smr in smRenderers)
                numSubs += smr.sharedMesh.subMeshCount;

            int[] meshIndex = new int[numSubs];
            int boneOffset = 0;
            for (int s = 0; s < smRenderers.Length; s++) {
                SkinnedMeshRenderer smr = smRenderers[s];

                BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;

                // May want to modify this if the renderer shares bones as unnecessary bones will get added.
                foreach (BoneWeight bw in meshBoneweight) {
                    BoneWeight bWeight = bw;

                    bWeight.boneIndex0 += boneOffset;
                    bWeight.boneIndex1 += boneOffset;
                    bWeight.boneIndex2 += boneOffset;
                    bWeight.boneIndex3 += boneOffset;

                    boneWeights.Add(bWeight);
                }
                boneOffset += smr.bones.Length;

                Transform[] meshBones = smr.bones;
                foreach (Transform bone in meshBones)
                    bones.Add(bone);

                if (smr.material.mainTexture != null)
                    textures.Add(smr.GetComponent<Renderer>().material.mainTexture as Texture2D);

                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                meshIndex[s] = ci.mesh.vertexCount;
                ci.transform = smr.transform.localToWorldMatrix;
                combineInstances.Add(ci);

                Object.Destroy(smr.gameObject);
            }

            List<Matrix4x4> bindposes = new List<Matrix4x4>();

            for (int b = 0; b < bones.Count; b++) {
                bindposes.Add(bones[b].worldToLocalMatrix * transform.worldToLocalMatrix);
            }

            SkinnedMeshRenderer r = gameObject.AddComponent<SkinnedMeshRenderer>();
            r.sharedMesh = new Mesh();
            r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

            Texture2D skinnedMeshAtlas = new Texture2D(128, 128);
            Rect[] packingResult = skinnedMeshAtlas.PackTextures(textures.ToArray(), 0);
            Vector2[] originalUVs = r.sharedMesh.uv;
            Vector2[] atlasUVs = new Vector2[originalUVs.Length];

            int rectIndex = 0;
            int vertTracker = 0;
            for (int i = 0; i < atlasUVs.Length; i++) {
                atlasUVs[i].x = Mathf.Lerp(packingResult[rectIndex].xMin, packingResult[rectIndex].xMax, originalUVs[i].x);
                atlasUVs[i].y = Mathf.Lerp(packingResult[rectIndex].yMin, packingResult[rectIndex].yMax, originalUVs[i].y);

                if (i >= meshIndex[rectIndex] + vertTracker) {
                    vertTracker += meshIndex[rectIndex];
                    rectIndex++;
                }
            }

            Material combinedMat = new Material(Shader.Find("Diffuse"));
            combinedMat.mainTexture = skinnedMeshAtlas;
            r.sharedMesh.uv = atlasUVs;
            r.sharedMaterial = combinedMat;

            r.bones = bones.ToArray();
            r.sharedMesh.boneWeights = boneWeights.ToArray();
            r.sharedMesh.bindposes = bindposes.ToArray();
            r.sharedMesh.RecalculateBounds();
        }
    }
}
