using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleDamager : MonoBehaviour
{
    public bool drawGizmos;

    public float damageAmount = 1;
    public bool stopCamera = false;

    public LayerMask targetLayers;

    protected GameObject m_owner;
    protected bool m_inAttack = false;

    public delegate void TriggerEnterAction(Collider other);
    public event TriggerEnterAction OnTriggerEnterEvent;

    SoundManager soundManager;

    private void OnEnable()
    {
        soundManager = SoundManager.Instance;
    }

    public void SetOwner(GameObject owner)
    {
        m_owner = owner;
    }

    public void BeginAttack()
    {
        m_inAttack = true;
    }

    public void EndAttack()
    {
        m_inAttack = false;
    }

    private void Reset()
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (m_inAttack)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);

            Gizmos.matrix = transform.localToWorldMatrix;

            var collider = GetComponent<Collider>();

            if (collider is BoxCollider boxCollider)
            {
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            }
            else if (collider is CapsuleCollider capsuleCollider)
            {
                // 캡슐의 반지름과 높이를 가져옴
                float radius = capsuleCollider.radius;
                float height = capsuleCollider.height / 2.0f - radius;

                // 캡슐의 방향을 설정
                Vector3 direction = Vector3.up;
                if (capsuleCollider.direction == 0) // X-axis
                {
                    direction = Vector3.right;
                }
                else if (capsuleCollider.direction == 2) // Z-axis
                {
                    direction = Vector3.forward;
                }

                // 캡슐의 두 끝점을 계산
                Vector3 offset = direction * height;
                Vector3 topSphereCenter = capsuleCollider.center + offset;
                Vector3 bottomSphereCenter = capsuleCollider.center - offset;

                // 위쪽 반구를 그림
                Gizmos.DrawSphere(topSphereCenter, radius);

                // 아래쪽 반구를 그림
                Gizmos.DrawSphere(bottomSphereCenter, radius);

                // 두 반구 사이에 박스를 그림
                Vector3 boxCenter = (topSphereCenter + bottomSphereCenter) / 2;
                Vector3 boxSize = new Vector3(
                    direction == Vector3.right ? height + radius : radius,
                    direction == Vector3.up ? height + radius : radius,
                    direction == Vector3.forward ? height + radius : radius
                );
                Gizmos.DrawCube(boxCenter, boxSize);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_inAttack)
        {
            return;
        }

        if (this.CompareTag("Player"))
        {
            OnTriggerEnterEvent(other);
        }

        var damageable = other.GetComponent<Damageable>();

        if (damageable == null)
        {
            return;
        }

        if (damageable.gameObject == m_owner)
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

        damageable.ApplyDamage(msg);
    }
}