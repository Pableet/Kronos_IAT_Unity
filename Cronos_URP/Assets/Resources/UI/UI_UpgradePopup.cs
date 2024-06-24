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
    GameObject powerUp;
    UI_Upgrades upgrades;
    UI_Scanner scanner;

    UI_UpgradeButton upgradeB;
    UI_UpgradeButton upgradeS;
    UI_UpgradeButton upgradeG;

    Vector3 selectScale;
    Vector3 originScale;
    Vector3 shrinkScale;

    public RawImage bHide;
    public RawImage sHide;
    public RawImage gHide;

    Color oldCol = new Color(1, 1, 1, 0);
    Color transCol = new Color(1, 1, 1, 0.2f);

    private void Awake()
    {
        originScale = new Vector3(1, 1, 1);
        selectScale = new Vector3(1.1f, 1.1f, 1);
        shrinkScale = new Vector3(0.8f, 0.8f, 1);
        upgradeB = bronze.GetComponent<UI_UpgradeButton>();
        upgradeS = silver.GetComponent<UI_UpgradeButton>();
        upgradeG = gold.GetComponent<UI_UpgradeButton>();
        confirm = GameObject.Find("Confirm");
        powerUp = GameObject.Find("DemoPowerUp");
        scanner = powerUp.GetComponentInChildren<UI_Scanner>();
    }

    void Start()
    {
        bronze.onClick.AddListener(() => OnButtonClick(bronze));
        silver.onClick.AddListener(() => OnButtonClick(silver));
        gold.onClick.AddListener(() => OnButtonClick(gold));
        confirm.SetActive(false);
        upgrades = gameObject.GetComponent<UI_Upgrades>();
        if (upgrades == null)
        {
            Debug.Log("업그레이드가 널");
        }
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
            bHide.color = oldCol;
            sHide.color = transCol;
            gHide.color = transCol;
        }

        if (clickedButton == silver)
        {
            silver.transform.localScale = selectScale;
            bronze.transform.localScale = shrinkScale;
            gold.transform.localScale = shrinkScale;
            bHide.color = transCol;
            sHide.color = oldCol;
            gHide.color = transCol;
        }

        if (clickedButton == gold)
        {
            gold.transform.localScale = selectScale;
            bronze.transform.localScale = shrinkScale;
            silver.transform.localScale = shrinkScale;
            bHide.color = transCol;
            sHide.color = transCol;
            gHide.color = oldCol;
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
            bHide.color = oldCol;
            sHide.color = oldCol;
            gHide.color = oldCol;
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
        if (upgradeB.isSelected)
        {
            upgrades.UpgradeB();
            upgradeB.isSelected = false;
        }

        if (upgradeS.isSelected)
        {
            upgrades.UpgradeS();
            upgradeS.isSelected = false;
        }

        if (upgradeG.isSelected)
        {
            upgrades.UpgradeG();
            upgradeG.isSelected = false;
        }

        // 끌 거 다 끄고
        bronze.transform.localScale = originScale;
        silver.transform.localScale = originScale;
        gold.transform.localScale = originScale;
        confirm.SetActive(false);
        scanner.ExitInteracting();

        // ui 닫기
        UI_PowerUp.StopPowerUp();

        // 강화 오브젝트 디졸브하기
        powerUp.GetComponent<Dissolve>().DoVanish();
    }
}
