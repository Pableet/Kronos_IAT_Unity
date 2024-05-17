using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    public bool isCLear /*false*/;

    public GameObject target;

    public TestCombatZoneEnemyBehavior[] enemyList;

    private bool isTargetIn;
    public GameObject Detect(Transform detector, bool useHeightDifference = true)
    {
        if (isTargetIn)
        {
            return target;
        }

        return null;
    }

    private void Start()
    {
        foreach (var enemy in enemyList)
        {
            enemy.combatZone = this;
        }
    }

    private void OnEnable()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

    }

    private void LateUpdate()
    {
        CheckClear();
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

    private void CheckClear()
    {
        if (isCLear)
        {
            return;
        }

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] != null)
            {
                return;
            }
            isCLear = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (isCLear)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
        }

        Gizmos.DrawCube(transform.position, gameObject.transform.localScale);
    }
}
