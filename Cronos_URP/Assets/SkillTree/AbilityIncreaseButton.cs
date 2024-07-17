using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIncreaseButton : MonoBehaviour, IObservable<AbilityIncreaseButton>
{
    public AbilityLevel abilityLevel = new AbilityLevel();

    [SerializeField] public TMP_Text abilityName;
    [SerializeField] public TMP_Text description;
    [SerializeField] public TMP_Text subdescription;

    private Button button;
    public Button[] childButton;

    private IObserver<AbilityIncreaseButton> _observer;

    // IObservable /////////////////////////////////////////////////////////////

    public IDisposable Subscribe(IObserver<AbilityIncreaseButton> observer)
    {
        if (_observer != null)
        {
            throw new InvalidOperationException("관찰자(observer)는 한 명만 있어야 합니다.");
        }
        _observer = observer;
        return new Unsubscriber(this);
    }

    private class Unsubscriber : IDisposable
    {
        private AbilityIncreaseButton _observable;

        public Unsubscriber(AbilityIncreaseButton observable)
        {
            _observable = observable;
        }

        public void Dispose()
        {
            _observable._observer = null;
        }
    }

    // MonoBehaviour /////////////////////////////////////////////////////////////

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        description.text = abilityLevel.descriptionText;
        subdescription.text = $"CP {abilityLevel.pointNeeded} 필요";
    }

    private void Update()
    {
        // 텍스트 업데이트
        Render();

        // 자식 노드 활성화
        //if (currentPoint > 0)
        if (abilityLevel.currentPoint == abilityLevel.nextNodeUnlockCondition)
        {
            foreach (var child in childButton)
            {
                child.interactable = true;
            }
        }
    }

    private void OnClickButton()
    {
        if (abilityLevel.currentPoint < abilityLevel.maxPoint)
        {
            _observer.OnNext(this);
        }
        else
        {
            abilityLevel.currentPoint = abilityLevel.maxPoint;
        }
    }

    private void Render()
    {
        abilityName.text = $"{abilityLevel.abilityName} ({abilityLevel.currentPoint}/{abilityLevel.maxPoint})";
    }

    public void Increment() => abilityLevel.currentPoint += 1;
}
