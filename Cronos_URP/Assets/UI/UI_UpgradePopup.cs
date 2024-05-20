using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UI_UpgradePopup : MonoBehaviour, IPointerDownHandler
{
    public Button bronze;
    public Button silver;
    public Button gold;

    GameObject confirm;

    UI_UpgradeButton upgradeB;
    UI_UpgradeButton upgradeS;
    UI_UpgradeButton upgradeG;

    Vector3 selectScale;
    Vector3 originScale;
    Vector3 shrinkScale;

    private void Awake()
    {
        originScale = new Vector3(1, 1, 1);
        selectScale = new Vector3(1.1f, 1.1f, 1);
        shrinkScale = new Vector3(0.8f, 0.8f, 1);
        upgradeB = bronze.GetComponent<UI_UpgradeButton>();
        upgradeS = silver.GetComponent<UI_UpgradeButton>();
        upgradeG = gold.GetComponent<UI_UpgradeButton>();
        confirm = GameObject.Find("Confirm");
    }

    void Start()
    {
        bronze.onClick.AddListener(() => OnButtonClick(bronze));
        silver.onClick.AddListener(() => OnButtonClick(silver));
        gold.onClick.AddListener(() => OnButtonClick(gold));
        confirm.SetActive(false);
    }

    public void OnButtonClick(Button clickedButton)
    {
        SetSelect(clickedButton);
        confirm.SetActive(true);

        if (clickedButton == bronze)
        {
            bronze.transform.localScale = selectScale;
            silver.transform.localScale = shrinkScale;
            gold.transform.localScale = shrinkScale;
        }

        if (clickedButton == silver)
        {
            silver.transform.localScale = selectScale;
            bronze.transform.localScale = shrinkScale;
            gold.transform.localScale = shrinkScale;
        }

        if (clickedButton == gold)
        {
            gold.transform.localScale = selectScale;
            bronze.transform.localScale = shrinkScale;
            silver.transform.localScale = shrinkScale;
        }
    }

    // 패널을 클릭했다면 모든 선택 취소
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Click {gameObject.name}");

        if (gameObject.name == "UI_PowerUp")
        {
            bronze.transform.localScale = originScale;
            silver.transform.localScale = originScale;
            gold.transform.localScale = originScale;
            upgradeB.isSelected = false;
            upgradeS.isSelected = false;
            upgradeG.isSelected = false;
        }

        confirm.SetActive(false);
    }

    void SetSelect(Button clickedButton)
    {
        if (clickedButton == bronze)
        {
            upgradeB.isSelected = true;
            upgradeS.isSelected = false;
            upgradeG.isSelected = false;
        }

        if (clickedButton == silver)
        {
            upgradeB.isSelected = false;
            upgradeS.isSelected = true;
            upgradeG.isSelected = false;
        }

        if (clickedButton == gold)
        {
            upgradeB.isSelected = false;
            upgradeS.isSelected = false;
            upgradeG.isSelected = true;
        }
    }

    public void DoUpgrade()
    {
        // isSelected에 따른 업데이트 실행 후

        // ui 닫기
        UI_PowerUp.StopPowerUp();
    }
}
