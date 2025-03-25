using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.VisualScripting;

public class FlySpawner : MonoBehaviour
{
    public int amountOfSpawns = 6;
    public float timeBetweenSpawns = 2f;
    public float maxSpeed = 7f;
    public float minSpeed = 1f;
    public float spawnCircleRadiusOffset = 0.7f;
    public float maxRotation = 45f;
    public float rotationSpeed = 300f;
    [Tooltip("Tiempo hasta que la mosca empieze a girar de nuevo tras haberse topado con un borde de la pantalla.")]
    public float nextRotationWaitTime = 0.7f;

    public Fly flyPrefab;

    private float _spawnCircleRadius;
    private int _fliesRequired;
    private int _fliesDestroyed = 0;

    void Start()
    {
        // Calcular el radio del circulo que spawnea moscas usando la altura y
        // anchura de la cámara.
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = Camera.main.aspect * camHalfHeight;
        Vector3 cornerPoint = new Vector3(camHalfWidth, camHalfHeight, 0f);
        _spawnCircleRadius = Vector3.Distance(Vector3.zero, cornerPoint);

        _fliesRequired = amountOfSpawns * (amountOfSpawns + 1) / 2;

        // Empezar a spawnear moscas.
        StartCoroutine(IncreaseDifficulty());
    }

    public void FlyDestroyed() {
        _fliesDestroyed++;
        if (_fliesDestroyed >= _fliesRequired) {
            MinigameWon();
        }
    }

    private Fly SpawnFly(Vector3 position) {
        Vector3 direction = Vector3.zero - position;
        Fly fly = Instantiate<Fly>(flyPrefab, position,
            Quaternion.FromToRotation(Vector3.down, Vector3.zero - position));
        fly.SetFly(Random.Range(minSpeed, maxSpeed), this);
        return fly;
    }

    private void MinigameWon() {
        // TEMPORAL. En el futuro habrá un game manager que se ocupe de cerrar y/o
        // cambiar escena, o de cerrar el juego.
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator IncreaseDifficulty() {
        int round = 0;
        int i = 1;
        while (round < amountOfSpawns) {
            for (int j = 0; j < i; j++) {
                Vector3 pos = GetRandomPos();
                Fly fly = SpawnFly(pos);
            }
            i++;
            round++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    //~ UTILITIES ~//
    private Vector3 GetRandomPos() {
        return Random.insideUnitCircle.normalized * _spawnCircleRadius;
    }
}
