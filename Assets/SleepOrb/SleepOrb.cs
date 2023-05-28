using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepOrb : MonoBehaviour
{
    public float spinSpeed;

    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body=GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 avel=body.angularVelocity;
        avel.y=spinSpeed;
        body.angularVelocity=avel;
    }
}
