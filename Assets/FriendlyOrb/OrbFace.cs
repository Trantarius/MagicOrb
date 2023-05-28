using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFace : MonoBehaviour
{
    public Mesh[] meshes;

    private MeshFilter meshFilter;

    public void Start(){
        meshFilter=GetComponent<MeshFilter>();
        if(meshes.Length>0){
            meshFilter.mesh=meshes[0];
        }
    }

    public void Update(){
        transform.eulerAngles=Vector3.zero;
    }
    
    public void set_mesh_idx(int to){
        if(to<0||to>=meshes.Length){
            throw new System.Exception("Invalid mesh index");
        }
        meshFilter.mesh=meshes[to];
    }
}
