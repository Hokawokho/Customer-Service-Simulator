using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBall : MonoBehaviour
{
public Vector2 force;

GameObject ballPaper;

Rigidbody2D rigidbody2D;

static int counter = 0;
    void Start() {

        ballPaper= FindObjectOfType<GameObject>();
        rigidbody2D= FindObjectOfType<Rigidbody2D>();

    }
    void Update() {

        if(Input.GetButtonDown("Jump")){
        //rigidbody2D.velocity = speed;

        rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        
        
        }


    }
}
