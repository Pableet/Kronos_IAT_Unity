using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class AbilityUnlockSystem : MonoBehaviour, IObserver<AbilityIncreaseButton>
{
    [SerializeField] public Button rootAbilityNode;
    [SerializeField] public AbilityAmountLimit abilityAmounts;

    private AbilityAmountLimit _abilityAmountLimit;
    private List<IObservable<AbilityIncreaseButton>> _obserables;
    private List<IDisposable> _unsubscribers;
    private bool _initialized;

    // IObserver /////////////////////////////////////////////////////////////

    public virtual void Subscribe(IObservable<AbilityIncreaseButton> provider)
    {
        if (provider != null)
            _unsubscribers.Add(provider.Subscribe(this));
    }

    public virtual void OnCompleted()
    {
        this.Unsubscribe();
    }

    public virtual void OnError(Exception e)
    {
    }

    public virtual void OnNext(AbilityIncreaseButton value)
    {
        if (abilityAmounts.CanSpend() == true)
        {
            if(abilityAmounts.UpdateSpent(value.abilityLevel.pointNeeded) == true)
            {
                value.Increment();
            }
        }
    }

    public virtual void Unsubscribe()
    {
        foreach (var unsubscriber in _unsubscribers) 
        {
            unsubscriber.Dispose();
            _unsubscribers.Remove(unsubscriber);
        }
    }

    // MonoBehaviour /////////////////////////////////////////////////////////////

    void Awake()
    {
        // 구독자 구독
        _obserables = GetComponentsInChildren<IObservable<AbilityIncreaseButton>>().ToList();
        foreach (var obserable in _obserables)
        {
            obserable.Subscribe(this);
        }

        rootAbilityNode.interactable = false;
    }

    public void OnGUI()
    {
        if (_initialized) return;
        _initialized = true;

        TriggerRootNodeChange();
    }

    private void SetupSkillTree()
    {
        _abilityAmountLimit = abilityAmounts;
        //_observers = gameObject.GetComponentsInChildren<AbilityIncreaseButton>().ToList();
    }

    private void TriggerRootNodeChange()
    {
        rootAbilityNode.interactable = true;
    }

    private void UpdateSkillAmounts(int spent)
    {
        _abilityAmountLimit.UpdateSpent(spent);
    }

    
}