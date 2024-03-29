using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevelPhysics;

public class RagdolSwitch : MonoBehaviour
{
    [SerializeField] private npcAudio audio;
    [SerializeField] private OnTriggerForwarder launchTrigger;
    [SerializeField] private Rigidbody[] rbs;
    //[SerializeField] private CharacterJoint[] joints;
    [SerializeField] private bool updateLists = false;
    [SerializeField] private float launchUpAmount = 1;
    [SerializeField] private float launchVelocity = 2;
    [SerializeField] private float minVelForLaunch = 3;
    

    [SerializeField] private Behaviour[] componentsToDisable;
    private void OnValidate()
    {
        if (updateLists)
        {
            audio = GetComponent<npcAudio>();
            launchTrigger = GetComponentInChildren<OnTriggerForwarder>();
            rbs = GetComponentsInChildren<Rigidbody>();
            //joints = GetComponentsInChildren<CharacterJoint>();

            foreach (var rb in rbs)
            {
                rb.isKinematic = true;
            }
        }
    }

    private void OnEnable()
    {
        launchTrigger.Enter += DoLaunch;
    }

    private void OnDisable()
    {
        launchTrigger.Enter -= DoLaunch;
    }

    private void DoRagdoll(bool state)
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = !state;
        }

        foreach (var c in componentsToDisable)
        {
            c.enabled = !state;
        }
    }

    private void DoLaunch(Collider other)
    {
        if (!componentsToDisable[0].enabled || other.isTrigger)
        {
            return;
        }
        
        //Debug.Log("Trigger");
        var gb = other.GetComponentInParent<GiraffeBehaviour>();
        var oRb = other.GetComponentInChildren<Rigidbody>();
        
        if ((gb != null || (other.CompareTag("PickUp") && other.gameObject.layer == 7) || other.CompareTag("Bullet")) && oRb.velocity.sqrMagnitude > minVelForLaunch * minVelForLaunch)
        {
            if (gb == null)
            {
                //ik it's bad, it's last minute don't @ me
                gb = FindFirstObjectByType<GiraffeBehaviour>();
            }
            
            audio?.PlayLaunchSound();
            gb.IsBad?.Invoke();

            var dir = transform.position - other.transform.position;
            dir.y = launchUpAmount;
            dir.Normalize();
            
            DoRagdoll(true);
            foreach (var rb in rbs)
            {
                rb.velocity = dir * launchVelocity;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
