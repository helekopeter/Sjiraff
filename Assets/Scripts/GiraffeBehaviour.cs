using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiraffeBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask taserLayer;
    
    public UnityAction IsBad;
    public UnityAction Tased;

    [SerializeField] private bool bad;

    private void OnValidate()
    {
        
        if (Application.isPlaying && bad)
        {
            IsBad?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.parent.name + " " + other.name);
        
        if (((1 << other.gameObject.layer) & taserLayer.value) != 0)
        {
            Tased?.Invoke();
        }
    }
}
