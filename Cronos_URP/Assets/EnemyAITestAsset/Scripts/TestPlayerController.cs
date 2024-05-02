using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    protected Damageable m_Damageable;


    // Start is called before the first frame update
    void OnEnable()
    {
        m_Damageable = GetComponent<Damageable>();
        m_Damageable.onDamageMessageReceivers.Add(this);
    }

    void OnDisable()
    {
        m_Damageable.onDamageMessageReceivers.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
