using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBladeTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Trigger(other.gameObject);
        }
    }


    protected void Trigger(GameObject trigger)
    {
        IDead deadTarget = trigger.GetComponent<IDead>();
        if (deadTarget != null)
        {
            deadTarget.Die();
        }
    }
}
