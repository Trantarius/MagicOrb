using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody body;
    private Detector detector;
    private Material material;
    public float Knockback;
    public float Speed;

    private float stunTime=0.0f;
    // Start is called before the first frame update
    void Start()
    {
        body=GetComponent<Rigidbody>();
        detector=transform.Find("Detector").GetComponent<Detector>();
        material=GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(stunTime>0){
            stunTime-=Time.fixedDeltaTime;
            material.color=new Color(0.5f,0.5f,0.5f);
            return;
        }else{
            material.color=new Color(1.0f,0.5f,0.5f);
        }


        GameObject[] detected=detector.GetDetected();
        List<Enemy> enemies = new List<Enemy>();
        Player player=null;
        foreach(GameObject obj in detected){

            player=obj.GetComponent<Player>();
            if(player!=null){
                break;
            }
        }

        if(player==null){
            return;
        }

        Vector3 dir=(player.transform.position-transform.position).normalized;
        Vector3 axis=-Vector3.Cross(dir,Vector3.up).normalized;
        body.AddTorque(axis*Speed*Time.fixedDeltaTime,ForceMode.Impulse);
    }

    public bool IsStunned(){
        return stunTime>0;
    }

    public void Stun(float duration){
        stunTime=Mathf.Max(duration,stunTime);
    }
}
