using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.NavMeshUtils
{
    public static class NavMeshObjExporter
    {
        private static string MeshToString(Mesh mesh, MeshRenderer mr, string name)
        {
            var mats = mr.sharedMaterials;
            var sb = new StringBuilder();
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var uv = mesh.uv;

            sb.Append($"# UnityEngine\n");
            sb.Append($"# File Created : {DateTime.Now}\n");
            sb.Append("\n");

            sb.Append($"# {mesh.vertices.Length} Vertices\n");
            sb.Append($"# {mesh.normals.Length} Vertex Normals\n");
            sb.Append($"# {mesh.uv.Length} Texture Coordinates\n");
            sb.Append($"# {mesh.subMeshCount} Submeshes\n");
            sb.Append($"# {mesh.triangles.Length} Polygons\n");
            sb.Append("\n");

            // 1. Name
            sb.Append("g ").Append(name).Append("\n\n");

            // 2. Vertices
            foreach (var v in vertices)
            {
                // 유니티는 좌표계가 달라서 x 반전시켜야 함
                sb.Append($"v {-v.x} {v.y} {v.z}\n");
            }
            sb.Append("\n");

            // 3. Normals
            foreach (var v in normals)
            {
                // x 반전
                sb.Append($"vn {-v.x} {v.y} {v.z}\n");
            }
            sb.Append("\n");

            // 4. UVs
            foreach (var v in uv)
            {
                sb.Append($"vt {v.x} {v.y}\n");
            }

            // 5. Triangles
            for (var material = 0; material < mesh.subMeshCount; material++)
            {
                sb.Append("\n");
                sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                sb.Append("usemap ").Append(mats[material].name).Append("\n");

                var triangles = mesh.GetTriangles(material);
                for (var i = 0; i < triangles.Length; i += 3)
                {
                    // x 반전
                    sb.Append(
                        $"f {triangles[i + 1] + 1}/{triangles[i + 1] + 1}/{triangles[i + 1] + 1} {triangles[i] + 1}/{triangles[i] + 1}/{triangles[i] + 1} {triangles[i + 2] + 1}/{triangles[i + 2] + 1}/{triangles[i + 2] + 1}\n");
                }
            }
            return sb.ToString();
        }

        public static void SaveMeshToFile(Mesh mesh, MeshRenderer mr, string meshName, string path)
        {
            using var sw = new StreamWriter(path);
            sw.Write(MeshToString(mesh, mr, meshName));
        }
    }
}
