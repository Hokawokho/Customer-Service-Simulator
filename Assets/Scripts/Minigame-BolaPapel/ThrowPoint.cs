using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPoint : MonoBehaviour
{
public float shootingPeriod = 1.0f;
public GameObject PaperBall;
private void Start() {
    InvokeRepeating("Fire", 0.0f, shootingPeriod);
    // llama a funcion "Fire", en el segundo 0.0, cada shootingPeriod


}
void Fire() {
    GameObject obj = Instantiate<GameObject>(PaperBall);
    obj.transform.position = transform.position;
    
    PaperBall proj = obj.GetComponent<PaperBall>();
    proj.force = new Vector2(Random.Range(6, 12), Random.Range(-1, 0));
    }
}