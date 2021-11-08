using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_Destroyer : MonoBehaviour
{
    [SerializeField] private float destructionDelayTime;

    private void Start()
    {
        Destroy(gameObject, destructionDelayTime);
    }
}