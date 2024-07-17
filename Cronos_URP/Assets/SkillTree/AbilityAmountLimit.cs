using System.Drawing;
using TMPro;
using UnityEngine;

public class AbilityAmountLimit : MonoBehaviour
{
    [SerializeField] public TMP_Text availableText;

    public int available;
    public int totalSpent;

    public void Awake()
    {
        Render();
    }

    public bool CanSpend() => available > 0;

    public int GetAvailable() => available;

    private void Render()
    {
        availableText.text = available.ToString();
    }

    public bool UpdateSpent(int point)
    {
        int result = available - point;
        bool canSpend = result > 0;

        if (canSpend)
        {
            available = result;
            totalSpent += point;
        }

        Render();

        return canSpend;
    }
}
