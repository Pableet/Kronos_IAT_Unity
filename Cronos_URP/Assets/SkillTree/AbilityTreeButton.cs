using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTreeButton : MonoBehaviour
{
    [SerializeField] public int maxPoint;
    [SerializeField] public int currentPoint;

    public string abilityName;
    public string descriptionText;
    public int cpNeeded;

    [SerializeField] public TMP_Text name;
    [SerializeField] public TMP_Text description;
    [SerializeField] public TMP_Text subdescription;

    private Button button;
    public Button[] childButton;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    private void Start()
    {
        description.text = descriptionText;
        subdescription.text = $"CP {cpNeeded} 필요";
    }

    private void Update()
    {
        // 텍스트 업데이트
        UpdateText();

        // 자식 노드 활성화
        //if (currentPoint > 0)
        if (currentPoint == maxPoint)
        {
            foreach(var child in childButton)
            {
                child.interactable = true;
            }
        }
    }

    private void OnClickButton()
    {
        if (currentPoint < maxPoint)
        {
            Increment();
        }
        else
        {
            currentPoint = maxPoint;
        }
    }

    private void UpdateText()
    {
        name.text = $"{abilityName} ({currentPoint}/{maxPoint})";
    }

    private void ResetPoiunt()
    {
        currentPoint = 0;
    }

    private void Increment() => currentPoint += 1;
    private void Decrement() { }
}
