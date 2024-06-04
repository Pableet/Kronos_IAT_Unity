using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MeleeTriggerEnterDamager : MonoBehaviour
{
    public float damageAmount = 1;
    public bool stopCamera = false;

    public LayerMask targetLayers;

    protected GameObject m_owner;
    protected bool m_inAttack = false;

    public void SetOwner(GameObject owner)
    {
        m_owner = owner;
    }

    public void BeginAttack(bool thowingAttack)
    {
        m_inAttack = true;
    }

    public void EndAttack()
    {
        Debug.Log("°ø°Ý ³¡");
        m_inAttack = false;
    }

    private void Reset()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_inAttack)
        {
            return;
        }

        var d = other.GetComponent<Damageable>();

        if (d == null)
        {
            return;
        }

        if (d.gameObject == m_owner)
        {
            return;
        }

        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        var msg = new Damageable.DamageMessage()
        {
            amount = damageAmount,
            damager = this,
            direction = Vector3.up,
            stopCamera = stopCamera
        };

        d.ApplyDamage(msg);
    }
}