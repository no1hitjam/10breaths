using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Environment {
    public class MountainRange : MonoBehaviour {
        [SerializeField] private Material mountainMaterial;
        [SerializeField] private Color color1;
        [SerializeField] private Color color2;
        [SerializeField] private float distance;
        [SerializeField] private float minY;
        [SerializeField] private float valleyY;
        [SerializeField] private float maxY;
        private static readonly int Color1 = Shader.PropertyToID("_Color1");
        private static readonly int Color2 = Shader.PropertyToID("_Color2");

        private (float x, float z) GetPosition(float angle) {
            return (Mathf.Sin(angle) * this.distance, Mathf.Cos(angle) * this.distance);
        }

        private void Awake() {
            // Build mesh
            var vertices = new List<Vector3>();
            var angle = 0f;
            var (startX, startZ) = this.GetPosition(angle);
            while (angle < Mathf.PI * 2) {
                var (x, z) = this.GetPosition(angle);
                vertices.Add(new Vector3(x, this.minY, z));
                var maxX = this.maxY - (Rand.Value * .5f + Mathf.Sin(angle) * .5f) * this.valleyY ;
                vertices.Add(new Vector3(x, maxX, z));
                angle += Rand.Value * .1f + .01f;
            }
            vertices.Add(new Vector3(startX, vertices[0].y, startZ));
            vertices.Add(new Vector3(startX, vertices[1].y, startZ));


            var numPoints = vertices.Count / 2 - 1;
            var triangleCount = numPoints * 6;
            var triangles = new int[triangleCount];
            for (var i = 0; i < triangleCount / 6; i++) {
                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = i * 2 + 1;
                triangles[i * 6 + 2] = i * 2 + 2;
            
                triangles[i * 6 + 3] = i * 2 + 3;
                triangles[i * 6 + 4] = i * 2 + 2;
                triangles[i * 6 + 5] = i * 2 + 1;
            }

            var uvs = new Vector2[vertices.Count];
            for (var i = 0; i < uvs.Length / 4; i++) {
                uvs[i * 4] = vertices[i * 4].normalized;
                uvs[i * 4 + 1] = vertices[i * 4 + 1].normalized;
                uvs[i * 4 + 2] = vertices[i * 4 + 2].normalized;
                uvs[i * 4 + 3] = vertices[i * 4 + 3].normalized;
            
                // uvs[i * 4] = new Vector2(0, 0);
                // uvs[i * 4 + 1] = new Vector2(0, 1);
                // uvs[i * 4 + 2] = new Vector2(1, 1);
                // uvs[i * 4 + 3] = new Vector2(1, 0);
            }
        
            // Vector3[] normals = new Vector3[4]
            // {
            //     -Vector3.forward,
            //     -Vector3.forward,
            //     -Vector3.forward,
            //     -Vector3.forward
            // };


            var meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = this.mountainMaterial;
            meshRenderer.material.SetColor(Color1, this.color1);
            meshRenderer.material.SetColor(Color2, this.color2);

            var meshFilter = this.gameObject.AddComponent<MeshFilter>();
            var mesh = new Mesh {vertices = vertices.ToArray(), triangles = triangles,  uv = uvs};
            meshFilter.mesh = mesh;
        }

        public void SetColors(Color c1, Color c2) {
            this.GetComponent<MeshRenderer>().material.SetColor(Color1, c1);
            this.GetComponent<MeshRenderer>().material.SetColor(Color2, c2);
        }

        public void SetThroughVector(Vector4 throughVector) {
            this.GetComponent<MeshRenderer>().material.SetVector("_ThroughVector", throughVector);
        }
    }
}