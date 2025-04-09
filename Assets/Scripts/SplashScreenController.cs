using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] private AudioClip splashTheme;
    public float waitTime = 10f;
    private float startedAt;

    void Start()
    {
        if (splashTheme != null) GeneralManager.Instance.audioManager.PlayMusic(splashTheme);
        startedAt = Time.time;
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButton(0) || Input.GetMouseButton(1)
            || Time.time - startedAt > waitTime) {
                Done();
            }
    }

    private void Done() {
        GeneralManager.Instance.GoToMainMenu();
        Destroy(gameObject);    // Para evitar que se ejecute mas de una vez, si se presionan varios botones uno detras del otro, por ejemplo.
    }
}
