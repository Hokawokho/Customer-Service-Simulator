using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Fly : MonoBehaviour
{
    private float speed;

    private Animator _anim;

    private float _horizontalBound;
    private float _verticalBound;
    private Quaternion _targetRotation;
    private bool _setupComplete = false;
    private bool _rotating = false;
    private bool _dead = false;

    public FlySpawner flySpawner {
        get; private set;
    }

    //~ SETUP ~//
    void Start()
    {
        _anim = GetComponent<Animator>();
        _verticalBound = Camera.main.orthographicSize;
        _horizontalBound = Camera.main.aspect * _verticalBound;
    }

    public void SetFly(float speed, FlySpawner spawner) {
        this.speed = speed;
        flySpawner = spawner;
    }

    void Update()
    {
        Move();
        
        // Si la mosca se sale de la cámara, dar media vuelta.
        if (!IsInBounds()) {
            if (_dead) {
                Destroy(gameObject);
            } else {
                transform.rotation = Quaternion.FromToRotation(Vector3.down, Vector3.zero - transform.position);
                StartCoroutine(WaitBeforeNextRotation());
                return;
            }
        }

        // Establecer la primera rotación a realizar.
        if (!_setupComplete) {
            _targetRotation = RandomRotation(transform.rotation, flySpawner.maxRotation);
            _setupComplete = true;
        }

        if (_rotating) {
            if (Quaternion.Angle(transform.rotation, _targetRotation) > 0.1f) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                        _targetRotation,
                                        Time.deltaTime * flySpawner.rotationSpeed);
            } else {
                // Se terminó la rotación. Se elige siguiente rotación...
                if (_dead) {
                    _targetRotation = RandomRotation(Quaternion.LookRotation(Vector3.forward, Vector3.up), 20f);
                } else {
                    _targetRotation = RandomRotation(transform.rotation, flySpawner.maxRotation);
                }
            }
        }
    }

    //~ OTHER FUNCTIONS ~//
    private void Move() {
        transform.position += (-transform.up) * (Time.deltaTime * speed);
    }

    public void Kill() {
        _dead = true;
        _anim.SetBool("dead", true);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        _targetRotation = transform.rotation;
        
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0.5f;
        GetComponent<SpriteRenderer>().color = tmp;
        speed = 7.0f;
    }

    void OnDestroy()
    {
        flySpawner.FlyDestroyed();
    }

    private IEnumerator WaitBeforeNextRotation() {
        if (!_rotating) yield return null;
        _rotating = false;
        yield return new WaitForSeconds(flySpawner.nextRotationWaitTime);
        _rotating = true;
    }

    //~ UTILITIES ~//
    // Se obtiene un nuevo Quaternion indicando la rotación siguiente objectivo de
    // la mosca basado en su dirección actual.
    public Quaternion RandomRotation(Quaternion rotation, float maxRotation) {
        float angle = Random.Range(-maxRotation, maxRotation);
        return rotation * Quaternion.AngleAxis(angle, Vector3.back);
    }

    private bool IsInBounds() {

        return Math.Abs(transform.position.x) < _horizontalBound &&
            Math.Abs(transform.position.y) < _verticalBound;
    }
}
