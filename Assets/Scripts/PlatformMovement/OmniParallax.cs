using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniParallax : MonoBehaviour
{
    private Vector2 size, startPosition;
    private Transform cam;
    public Vector2 parallaxFraction;
    void Start() {
        startPosition = transform.position;
        size = GetComponent<SpriteRenderer>().bounds.size;
        cam = Camera.main.transform;
    }
    void LateUpdate() {
        Vector2 offset = (cam.position * parallaxFraction);
        Vector2 temp = new Vector2(cam.position.x, cam.position.y) - offset;
        
        if (temp.x > startPosition.x + size.x) startPosition.x += size.x;
        else if (temp.x < startPosition.x - size.x) startPosition.x -= size.x;
        if (parallaxFraction.y > 0f && temp.y > startPosition.y + size.y)
            startPosition.y += size.y;
        else if (parallaxFraction.y > 0f && temp.y < startPosition.y - size.y) startPosition.y -= size.y;
        transform.position = new Vector3(startPosition.x + offset.x, startPosition.y + offset.y,
            transform.position.z);
    }
}
