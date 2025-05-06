using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBall : MonoBehaviour
{
public Vector2 force;

//private GameObject ballPaper;

private Rigidbody2D rb;

private SpriteRenderer sprite;

private bool hasLaunched = false;

    void Start() {

        //ballPaper= FindObjectOfType<GameObject>();
        rb= GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.enabled = false;
        rb.simulated = false;

    }
    void Update() {

        if(Input.GetButtonDown("Jump") && !hasLaunched){
        //rigidbody2D.velocity = speed;

        hasLaunched = true;

        transform.localPosition = new Vector3(-6.25f,1.16f,0);


        sprite.enabled = true;
        rb.simulated = true;

        rb.velocity = Vector2.zero;

        rb.AddForce(force, ForceMode2D.Impulse);
        
        
        }

        if(Input.GetKeyDown(KeyCode.R)){

            hasLaunched = false;
        }


    }
}
