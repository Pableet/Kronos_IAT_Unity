using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    public GameObject target { private get; set; }

    private bool isTargetIn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
            isTargetIn = true;
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
            isTargetIn = false;
    }


    public GameObject Detect(Transform detector, bool useHeightDifference = true)
    {
        if (isTargetIn)
        {
            return target;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, gameObject.transform.localScale);
    }
}
