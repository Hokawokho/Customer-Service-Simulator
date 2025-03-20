using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.VisualScripting;

public class FlySpawner : MonoBehaviour
{
    public int flyCount = 0;
    public int flyLimit = 50;
    public int fliesPerFrame = 1;
    public float maxSpeed = 7f;
    public float minSpeed = 1f;
    public float spawnCircleRadiusOffset = 0.7f;
    public float maxRotation = 45f;
    public float rotationSpeed = 300f;
    [Tooltip("Tiempo hasta que la mosca empieze a girar de nuevo tras haberse topado con un borde de la pantalla.")]
    public float nextRotationWaitTime = 0.7f;

    public Fly flyPrefab;

    private float _spawnCircleRadius;

    void Start()
    {
        // Calcular el radio del circulo que spawnea moscas usando la altura y
        // anchura de la cámara.
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = Camera.main.aspect * camHalfHeight;
        Vector3 cornerPoint = new Vector3(camHalfWidth, camHalfHeight, 0f);
        _spawnCircleRadius = Vector3.Distance(Vector3.zero, cornerPoint);
    }

    void Update()
    {
        if (flyCount < flyLimit) {
            for (int i = 0; i < fliesPerFrame; i++) {
                Vector3 pos = GetRandomPos();
                Fly fly = SpawnFly(pos);
            }
        }
    }

    private Vector3 GetRandomPos() {
        return Random.insideUnitCircle.normalized * _spawnCircleRadius;
    }

    private Fly SpawnFly(Vector3 position) {
        flyCount += 1;
        Vector3 direction = Vector3.zero - position;
        Fly fly = Instantiate<Fly>(flyPrefab, position,
            Quaternion.FromToRotation(Vector3.down, Vector3.zero - position));
        fly.SetFly(Random.Range(minSpeed, maxSpeed), this);
        return fly;
    }
}
