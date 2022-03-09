using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenHyperLinks : MonoBehaviour
{
    public void ClickHyperLink()
    {
        Application.OpenURL("https://assetstore.unity.com/publishers/34772");
    }
}
