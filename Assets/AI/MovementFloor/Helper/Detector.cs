using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.AI.MovementFloor.Helper
{
    class Detector
    {

        /// <summary>
        /// 扇形检测  返回是否在范围内
        /// </summary>
        /// <param name="angle">检测角度</param>
        public static bool Sector(float angle,GameObject target,GameObject character,int segments,float Radius)
        {
            if(angle>360&&angle<0)
            {
                Debug.Log("角度太大，范围为0-360");
                return false;
            }
            //角色到目标的向量
            Vector3 toTarget = target.transform.position - character.transform.position;
            //角色前方向量和角色到目标向量的点积
            float direction = Vector3.Dot(target.transform.forward, toTarget.normalized);
            float halfangel = angle / 2;
            if (Mathf.Cos(halfangel) < direction)
                return true;
            else
                return false;
        }

        //得到一个扇形mesh
        public static Mesh CreateMesh(float radius, float innerradius, float angledegree, int segments)
        {
            //vertices(顶点):
            int vertices_count = segments * 2 + 2;              //因为vertices(顶点)的个数与triangles（索引三角形顶点数）必须匹配
            Vector3[] vertices = new Vector3[vertices_count];
            float angleRad = Mathf.Deg2Rad * angledegree;
            float angleCur = angleRad;
            float angledelta = angleRad / segments;
            for (int i = 0; i < vertices_count; i += 2)
            {
                float cosA = Mathf.Cos(angleCur);
                float sinA = Mathf.Sin(angleCur);
                vertices[i] = new Vector3(radius * cosA, 0, radius * sinA);
                vertices[i + 1] = new Vector3(innerradius * cosA, 0, innerradius * sinA);
                angleCur -= angledelta;
            }
            //triangles:
            int triangle_count = segments * 6;
            int[] triangles = new int[triangle_count];
            for (int i = 0, vi = 0; i < triangle_count; i += 6, vi += 2)
            {
                triangles[i] = vi;
                triangles[i + 1] = vi + 3;
                triangles[i + 2] = vi + 1;
                triangles[i + 3] = vi + 2;
                triangles[i + 4] = vi + 3;
                triangles[i + 5] = vi;
            }
            //uv:
            Vector2[] uvs = new Vector2[vertices_count];
            for (int i = 0; i < vertices_count; i++)
            {
                uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
            }
            //负载属性与mesh
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateTangents();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh; 
        }
    }
}
