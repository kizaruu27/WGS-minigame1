using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Value", menuName = "Scriptable Value")]

public class Multiplayer_ScriptableValue : ScriptableObject
{
    [Header("UI Value")]
    public GameObject finishUI;

    [Header("Finish Validation")]
    public bool isFinish;
}
