using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M1_UITimer : MonoBehaviour
{
    [SerializeField] GameObject uiTimer;
    [SerializeField] TextMeshProUGUI TimeUI;
    [SerializeField] TextMeshProUGUI UIText;

    [SerializeField] float timer = 3;
    float currentTime;
    

    [SerializeField] bool isHit;

    private void Start()
    {
        currentTime = timer;
    }

    private void Update()
    {
        if (isHit)
        {
            float secondsCalculation = Mathf.FloorToInt(currentTime % 60);
            currentTime -= Time.deltaTime;
            TimeUI.text = secondsCalculation.ToString();

            if (currentTime <= 0)
            {
                DeactivateUITimer();
            }
        }   
       
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Speed")
        {
            ActivateUITimer("Speed Up!");
        }

        if (col.tag == "Stop")
        {
            ActivateUITimer("Stop!");
        }

        if (col.tag == "Obstacle")
        {
            ActivateUITimer("Slowed Down!");
        }
    }

    void ActivateUITimer(string description)
    {
        uiTimer.SetActive(true);
        isHit = true;
        UIText.text = description;
    }

    void DeactivateUITimer()
    {
        uiTimer.SetActive(false);
        isHit = false;
        currentTime = timer;
    }
}
