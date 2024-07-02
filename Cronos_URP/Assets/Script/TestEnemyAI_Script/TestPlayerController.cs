using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.InputSystem.XR;

public class TestPlayerController : MonoBehaviour, IMessageReceiver
{
    protected Damageable m_Damageable;

    readonly int m_HashHurt = Animator.StringToHash("Damaged");

    protected Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);

        // TEST
        //GetComponent<MeleeWeapon>().SetOwner(gameObject);
        //GetComponent<MeleeWeapon>().BeginAttack(false);
    }

    void OnDisable()
    {
        // TEST
        //GetComponent<MeleeWeapon>().EndAttack();

        m_Damageable.onDamageMessageReceivers.Remove(this);
    }
    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        switch (type)
        {
            case MessageType.DAMAGED:
                {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Damaged(damageData);
                }
                break;
            case MessageType.DEAD:
                {
                    Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
                    Death(damageData);
                }
                break;
        }
    }

    void Damaged(Damageable.DamageMessage damageMessage)
    {
        m_Animator.SetTrigger(m_HashHurt);
    }

    public void Death(Damageable.DamageMessage msg)
    {
        var replacer = GetComponent<ReplaceWithRagdoll>();

        if (replacer != null)
        {
            replacer.Replace();
        }

        //We unparent the hit source, as it would destroy it with the gameobject when it get replaced by the ragdol otherwise
    }
}
