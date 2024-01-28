using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerForwarder : MonoBehaviour
{
    public UnityAction<Collider> Enter;
    public UnityAction<Collider> Exit;
    public UnityAction<Collider> Stay;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter");
        Enter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Exit?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Stay?.Invoke(other);
    }
}
