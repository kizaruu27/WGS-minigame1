using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishChecker : MonoBehaviour
{
    public static FinishChecker finishChecker;
    public bool isFinish;

    private void Start()
    {
        finishChecker = this;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isFinish = true;
        }
    }
}
