using System.Drawing;
using TMPro;
using UnityEngine;

public class AbilityAmountLimit : MonoBehaviour
{
    [SerializeField] public TMP_Text availableText;

    public Player player;

    public int available;
    public int totalSpent;

    public void Awake()
    {
        UpdatePlayerTimePoint();

        Render();
    }

    public int CanSpend(int point)
    {
        if (available <= 0)
            return -1;

        int result = available - point;

        if (result < 0)
        {
            return -1;
        }

        return result;
    }

    public int GetAvailable() => available;

    private void Render()
    {
        availableText.text = available.ToString();
    }

    public void UpdatePlayerTimePoint()
    {
        if (player != null)
        {
            available = (int)player.TP;
        }

        Render();
    }

    public void UpdateSpent(int point)
    {
        int canSpend = CanSpend(point);

        if (canSpend != -1)
        {
            available = canSpend;
            totalSpent += point;
        }

        if (player != null)
        {
            player.TP = (float)available;
        }

        Render();
    }
}
