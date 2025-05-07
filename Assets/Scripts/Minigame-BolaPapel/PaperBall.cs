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

private Vector2[] forceSteps = new Vector2[]
{
    new Vector2(4, 4),
    new Vector2(5, 5),
    new Vector2(6, 6),
    new Vector2(5, 5),
    new Vector2(4, 4)
};

private int currentForceIndex = 0;

    void Start() {

        //ballPaper= FindObjectOfType<GameObject>();
        rb= GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.enabled = false;
        rb.simulated = false;

         StartCoroutine(UpdateForceRoutine());

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

    private IEnumerator UpdateForceRoutine()
    {
        while (true)
        {
            force = forceSteps[currentForceIndex];
            currentForceIndex = (currentForceIndex + 1) % forceSteps.Length;
            yield return new WaitForSeconds(0.2f); // ajusta esto si cada transición dura más/menos
        }
    }
}
