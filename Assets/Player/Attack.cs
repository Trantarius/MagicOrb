using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Force;
    public float Stun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        Enemy enemy=collider.GetComponent<Enemy>();
        if(enemy!=null){
            Vector3 dir=(enemy.transform.position-transform.position).normalized;
            Rigidbody body=enemy.GetComponent<Rigidbody>();
            body.AddForce(dir*Force,ForceMode.Impulse);
            enemy.Stun(Stun);
        }
    }
}
