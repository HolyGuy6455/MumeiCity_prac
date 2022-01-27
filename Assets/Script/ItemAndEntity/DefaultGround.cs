using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DefaultGround : MonoBehaviour{
    [SerializeField] Mesh mesh;
    [SerializeField] Vector3[] vertices;
    [SerializeField] int[] triangles;
    [SerializeField] float[] height;
    public float[] _height{get{return height;}}

    private void Start() {
        this.mesh = new Mesh();
        triangles = new int[]{
            0,2,1,
            1,2,3,
            4,0,5,
            5,0,1,
            6,7,0,
            0,7,2
        };
        height = new float[8];
    }

    public void ResetMesh(){
        int x = (int)this.transform.position.x;
        int z = (int)this.transform.position.z;
        vertices = new Vector3[8];
        vertices[0] = new Vector3(x + 0, height[0], z + 0);
        vertices[1] = new Vector3(x + 1, height[1], z + 0);
        vertices[2] = new Vector3(x + 0, height[2], z + 1);
        vertices[3] = new Vector3(x + 1, height[3], z + 1);
        vertices[4] = new Vector3(x + 0, height[4], z + 0);
        vertices[5] = new Vector3(x + 1, height[5], z + 0);
        vertices[6] = new Vector3(x + 0, height[6], z + 0);
        vertices[7] = new Vector3(x + 0, height[7], z + 1);

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = this.mesh;
        GetComponent<MeshCollider>().sharedMesh = this.mesh;
    }
}