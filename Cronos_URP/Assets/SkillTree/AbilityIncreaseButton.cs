using Cinemachine;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIncreaseButton : MonoBehaviour, IObservable<AbilityIncreaseButton>
{
    public AbilityLevel abilityLevel = new AbilityLevel();

    [SerializeField] public TMP_Text abilityName;
    [SerializeField] public TMP_Text description;
    [SerializeField] public TMP_Text subdescription;

    public Button[] childButton;

    public bool isFocaus;

    private Button _button;
    private FadeEffector _fadeUI;
    private CinemachineVirtualCamera _virtualCam;
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
        _button = GetComponent<Button>();
        _virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _fadeUI = GetComponentInChildren<FadeEffector>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickButton);
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
        if (abilityLevel.currentPoint == abilityLevel.nextNodeUnlockCondition ||
            abilityLevel.currentPoint == abilityLevel.maxPoint)
        {
            foreach (var child in childButton)
            {
                child.interactable = true;
            }
        }

        /// Test
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FocusOut();
        }

    }

    public void FocusIn()
    {
        _virtualCam.Priority = 10;
        StartCoroutine(InFocausAfter(2));
        _fadeUI.StartFadeIn(2);
    }
    public void FocusOut()
    {
        isFocaus = false;
        _virtualCam.Priority = 0;
        _fadeUI.StartFadeOut(2);
    }

    private void OnClickButton()
    {
        _observer.OnNext(this);
        //if (isFocaus == false)
        //{
        //    FocusIn();
        //}
        //else if (isFocaus == true)
        //{
        //    if (abilityLevel.currentPoint < abilityLevel.maxPoint)
        //    {
        //        _observer.OnNext(this);
        //    }
        //    else if (abilityLevel.currentPoint >= abilityLevel.maxPoint)
        //    {
        //        abilityLevel.currentPoint = abilityLevel.maxPoint;
        //    }
        //}
    }

    private IEnumerator InFocausAfter(float time)
    {
        // 지정된 시간(2초) 대기
        yield return new WaitForSeconds(time);

        isFocaus = true;
    }

    private void Render()
    {
        abilityName.text = $"{abilityLevel.abilityName} ({abilityLevel.currentPoint}/{abilityLevel.maxPoint})";
    }

    public bool Increment()
    {
        int addedPoint = abilityLevel.currentPoint + 1;

        bool result = addedPoint <= abilityLevel.maxPoint;

        if (result == true)
        {
            abilityLevel.currentPoint = addedPoint;
        }

        return result;
    }
}
