using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<MovingTarget>().StopMoving();
    }
}
