using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    public float jumpSpeed;
    public float rightingStrength;

    private Rigidbody body;
    private Material material;
    private GameObject attackZone;
    private Vector2 _movedir;
    private bool onGround;
    private float damageTime=0.0f;
    private float attackTime=0.0f;

    // Start is called before the first frame update
    void Start()
    {
        body=GetComponent<Rigidbody>();
        material=GetComponent<MeshRenderer>().material;
        attackZone=transform.Find("AttackZone").gameObject;
    }

    void Update(){

    }

    void FixedUpdate(){
        float forceFactor=moveSpeed;
        if(!onGround){
            forceFactor/=2;
        }
        body.AddForce(new Vector3(_movedir.x*forceFactor*Time.fixedDeltaTime,0,0),ForceMode.Impulse);
        
        

        if(onGround){
            float angle=transform.eulerAngles.z;
            if(angle>180){
                angle-=360;
            }
            float righting=-(angle+body.angularVelocity.z);
            righting*=Time.fixedDeltaTime * rightingStrength;

            body.AddTorque(Vector3.forward*righting,ForceMode.Impulse);
            body.angularDrag=5;
        }else{
            body.angularDrag=0;
        }

        onGround=false;


        if(damageTime>0){
            damageTime-=Time.fixedDeltaTime;
            material.color=Color.red;
        }else{
            material.color=Color.white;
        }

        if(attackTime>0){
            attackTime-=Time.fixedDeltaTime;
            attackZone.SetActive(true);
        }else{
            attackZone.SetActive(false);
        }
    }

    void OnMove(InputValue inputValue){
        _movedir=inputValue.Get<Vector2>();
    }

    void OnJump(InputValue inputValue){
        if(onGround){
            body.AddForce(new Vector3(0,jumpSpeed,0),ForceMode.Impulse);
        }
    }

    void OnAttack(){
        if(attackTime>0){
            return;
        }
        attackTime=Mathf.Max(attackTime,0.25f);
        Mouse mouse=InputSystem.GetDevice<Mouse>();
        Vector3 clickPos=screenToWorld(mouse.position.value);
        Vector3 dir=(clickPos-transform.position).normalized;
        attackZone.transform.position=transform.position+dir;
    }

    void OnCollisionStay(Collision collision){
        Vector3 normal=collision.GetContact(0).normal;
        if(Vector3.Dot(normal,Vector3.up)>0.5){
            onGround=true;
        }


        Enemy enemy=collision.gameObject.GetComponent<Enemy>();
        if(enemy!=null && damageTime<=0 && !enemy.IsStunned()){
            damageTime=Mathf.Max(damageTime,0.1f);
            ContactPoint contact=collision.GetContact(0);
            Vector3 dir=(transform.position-enemy.transform.position).normalized;
            body.AddForceAtPosition(dir*enemy.Knockback,contact.point,ForceMode.Impulse);
        }

        
    }

    private void debugDrawPoint(Vector3 point,Color color,float size=0.5f,float duration=0.0f){
        Debug.DrawLine(point+Vector3.back*size,point+Vector3.forward*size,color,duration);
        Debug.DrawLine(point+Vector3.down*size,point+Vector3.up*size,color,duration);
        Debug.DrawLine(point+Vector3.left*size,point+Vector3.right*size,color,duration);
    }

    private Vector3 screenToWorld(Vector2 screen){
        Ray ray=Camera.main.ScreenPointToRay(new Vector3(screen.x,screen.y,0));
        return ray.origin + ray.direction * 
            Mathf.Abs(ray.origin.z / Vector3.Dot(ray.direction,Vector3.forward));
        
    }
}
