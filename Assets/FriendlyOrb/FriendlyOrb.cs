using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyOrb : MonoBehaviour
{

    public float Speed;

    private Rigidbody body;
    private Detector detector;
    private OrbFace face;

    private Vector2 movedir;
    private bool sleeping=false;
    // Start is called before the first frame update
    void Start()
    {
        body=GetComponent<Rigidbody>();
        detector=transform.Find("Detector").GetComponent<Detector>();
        face=transform.Find("Face").GetComponent<OrbFace>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(sleeping){
            return;
        }

        GameObject[] detected=detector.GetDetected();
        List<Enemy> enemies = new List<Enemy>();
        foreach(GameObject obj in detected){

            SleepOrb sleep=obj.GetComponent<SleepOrb>();
            if(sleep!=null){
                Vector3 rel=(sleep.transform.position-transform.position);
                if(rel.magnitude<2.0){
                    sleeping=true;
                    face.set_mesh_idx(2);
                    return;
                }
                
            }

            Enemy enemy=obj.GetComponent<Enemy>();
            if(enemy!=null){
                enemies.Add(enemy);
            }

            
        }

        Vector3 fleeDirection=new Vector3();
        if(enemies.Count>0){
            foreach(Enemy enemy in enemies){
                Vector3 rel=(transform.position-enemy.transform.position);
                fleeDirection += rel/Vector3.Dot(rel,rel);
            }
            fleeDirection = (fleeDirection/enemies.Count).normalized;
            face.set_mesh_idx(1);
        }else{
            face.set_mesh_idx(0);
        }

        //body.velocity=new Vector3(fleeDirection.x*Speed,body.velocity.y,0);
        body.AddForce(new Vector3(fleeDirection.x*Speed,0,0),ForceMode.Impulse);
    }
}
