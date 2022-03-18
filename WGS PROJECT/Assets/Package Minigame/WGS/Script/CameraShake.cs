using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float power = 0.7f;
    public float duration = 1f;
    public Transform cam;
    public float slowDownAmmount = 1f;
    public bool Shake = false;

    Vector3 starPos;
    float initialDuration;

    private void Start()
    {
        cam = Camera.main.transform;
        starPos = cam.localPosition;
        initialDuration = duration;
    }

    private void Update()
    {
        if (Shake)
        {
            if (duration > 0)
            {
                cam.localPosition = starPos + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmmount;
            }
            else 
            {
                Shake = false;
                duration = initialDuration;
                cam.localPosition = starPos;
            }
        }
    }
}