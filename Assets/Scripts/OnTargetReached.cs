using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReached : MonoBehaviour
{
    private float _threshold = 0.02f;
    [SerializeField] private Transform _target;
    public UnityEvent onReached;
    private bool wasReached;

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, _target.position);

        if(distance < _threshold && !wasReached)
        {
            onReached.Invoke();
            wasReached = true;
        }
        else if(distance > _threshold)
        {
            wasReached = false;
        }
    }
}
