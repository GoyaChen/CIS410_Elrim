using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent : MonoBehaviour
{
    public Action<Collider> OnColliderTriggerEnter;
    public Action<Collider> OnColliderTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        OnColliderTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnColliderTriggerExit?.Invoke(other);
    }
}
