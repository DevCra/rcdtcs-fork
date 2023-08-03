using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Assets.NavMeshUtils;

namespace Assets.RcdtcsUnityDemo.Scripts.Editor
{
    public class ExportNavMesh : EditorWindow
    {
        [MenuItem("Export/NavMesh")]
        private static void Export()
        {
            var navMesh = NavMesh.CalculateTriangulation();
            var mesh = new Mesh
            {
                vertices = navMesh.vertices,
                triangles = navMesh.indices
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            var sceneName = SceneManager.GetActiveScene().name;
            var obj = new GameObject("Temp");
            var meshRenderer = obj.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Diffuse"));
            var meshFilter = obj.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var path = $"Assets/NavMesh/{sceneName}";

            if (!AssetDatabase.IsValidFolder("Assets/NavMesh"))
            {
                AssetDatabase.CreateFolder("Assets", "NavMesh");
            }
            // ModelExporter.ExportObject($"{path}.fbx", obj);
            NavMeshObjExporter.SaveMeshToFile(mesh, meshRenderer, $"{sceneName}", $"{path}.obj");

            DestroyImmediate(obj);
        }
       
    }
}
