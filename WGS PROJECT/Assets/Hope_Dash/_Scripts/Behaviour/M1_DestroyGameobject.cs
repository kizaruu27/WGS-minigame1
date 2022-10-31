using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_DestroyGameobject : MonoBehaviour
{
    [SerializeField] float destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

}
