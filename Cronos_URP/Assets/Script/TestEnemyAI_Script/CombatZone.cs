using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    public GameObject target;

    public TestCombatZoneEnemyBehavior[] enemyList;

    private bool isTargetIn;

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        foreach (var enemy in enemyList)
        {
            enemy.combatZone = this;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            isTargetIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            isTargetIn = false;
        }
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
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, gameObject.transform.localScale);
    }
}
