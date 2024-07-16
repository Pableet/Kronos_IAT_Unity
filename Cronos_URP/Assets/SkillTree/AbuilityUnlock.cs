using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbuilityUnlock : MonoBehaviour
{
    [SerializeField] public Toggle rootNode;
    private bool _initialized;

    void Awake()
    {
        
    }

    public void OnGUI()
    {
        if (_initialized) return;
        _initialized = true;

        TriggerRootNodeChange();
    }
    private void TriggerRootNodeChange()
    {
        rootNode.interactable = true;
        rootNode.isOn = true;
    }
}
