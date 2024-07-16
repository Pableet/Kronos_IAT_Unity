using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField] public Button rootNode;
    private bool _initialized;

    void Awake()
    {
        rootNode.interactable = false;
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
    }
}
