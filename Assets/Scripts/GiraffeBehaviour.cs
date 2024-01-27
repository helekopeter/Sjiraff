using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiraffeBehaviour : MonoBehaviour
{
    public UnityAction<bool> isBad;

    [SerializeField] private bool bad;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            isBad?.Invoke(bad);
        }
    }
}
