using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private List<GameObject> detected=new List<GameObject>();

    public GameObject[] GetDetected(){
        List<GameObject> det=new List<GameObject>();
        foreach(GameObject obj in detected){
            Vector3 rel=(obj.transform.position-transform.position);
            bool blocked = Physics.Raycast(transform.position,rel.normalized,rel.magnitude);
            if(!blocked){
                det.Add(obj);
            }
        }
        return det.ToArray();
    }

    void OnTriggerEnter(Collider other){
        detected.Add(other.gameObject);
    }
    void OnTriggerExit(Collider other){
        detected.Remove(other.gameObject);
    }
}
