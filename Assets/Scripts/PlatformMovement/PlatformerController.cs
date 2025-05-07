using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{

    public float speed = 4.5f;
    private Rigidbody2D _body;

    private Animator _anim;

    [SerializeField] public float jumpForce;
    
    private BoxCollider2D _box;


    public LayerMask groundLayer;  // Capa que representa el suelo



    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        _anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        _body.velocity = movement;

        if (grounded() && Input.GetButtonDown("Jump"))
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        _anim.SetBool("airborne", !grounded());

    }
    private bool grounded()
    {       
            float extraHeight = 0.1f;
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(_box.bounds.center, _box.bounds.size, 0f,  Vector2.down, extraHeight, groundLayer);

            Color rayColor;
            if(raycastHit2D.collider != null){
                rayColor = Color.green;
            }else{
                rayColor = Color.red;
            }

           //Debug.DrawRay(_box.bounds.center, Vector2.down * (_box.bounds.extents.y + extraHeight));
           // Debug.Log(raycastHit2D.collider);
            return raycastHit2D.collider != null;
;        }
    }

