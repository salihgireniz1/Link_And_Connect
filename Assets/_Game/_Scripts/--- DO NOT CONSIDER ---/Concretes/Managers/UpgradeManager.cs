using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager
{
    private Dictionary<string, Upgrade> _upgrades = new Dictionary<string, Upgrade>();

    public void CreateUpgrade(string upgradeName, int initialCost, float initialLevel, Button button)
    {
        Upgrade newUpgrade = new Upgrade(upgradeName, initialCost, initialLevel, button);
        _upgrades.Add(upgradeName, newUpgrade);
    }


    private void CheckInteractable(ref int currency)
    {
        int upgradeButtonCounts = _upgrades.Count;

        foreach (var upgradeValue in _upgrades.Values)
        {
            int upgradeCost = upgradeValue.Cost;
            upgradeValue.Button.interactable = currency < upgradeCost ? false : true;
        }
    }

    public void MakeUpgrade(string upgradeName, int costIncreaseAmount, ref int currency, ref float level, float levelIncreaseAmount)
    {
        Upgrade upgrade = GetUpgrade(upgradeName);

        if (upgrade != null)
        {
            DecreaseCurrency(upgrade, ref currency);
            upgrade.IncreaseCost(costIncreaseAmount);
            IncreaseLevel(upgrade, ref level, levelIncreaseAmount);
            CheckInteractable(ref currency);
        }
        else
        {
            Debug.Log("Upgrade Null");
        }
    }

    private Upgrade GetUpgrade(string upgradeName)
    {
        Upgrade upgrade = null;

        foreach (var value in _upgrades.Values)
        {
            if (value.Name == upgradeName)
            {
                upgrade = value;
                break;
            }
        }
        return upgrade;
    }

    private int DecreaseCurrency(Upgrade upgrade, ref int currency)
    {
        return currency -= upgrade.Cost;
    }

    private void IncreaseLevel(Upgrade upgrade, ref float level, float levelIncreaseAmount)
    {
        upgrade.Level += levelIncreaseAmount;
        level = upgrade.Level;
    }
}


public class Upgrade
{
    private string _name;
    public string Name { get => _name; private set => _name = value; }

    private int _cost;
    public int Cost { get => _cost; private set => _cost = value; }

    private Button _button;
    public Button Button { get => _button; private set => _button = value; }

    private float _level;
    public float Level { get => _level; set => _level = value; }

    private TextMeshProUGUI _tmPro;

    public Upgrade(string name, int cost, float level ,Button button)
    {
        _name = name;
        _cost = cost;
        _level = level;
        _button = button;
        _tmPro = _button.GetComponentInChildren<TextMeshProUGUI>();
        RefreshText();
    }

    public void IncreaseCost(int increaeAmount)
    {
        _cost += increaeAmount;
        RefreshText();
    }

    public void DecreaseCost(int decreaseAmount)
    {
        _cost -= decreaseAmount;
        RefreshText();
    }

    public void IncreaseLevel(int increaseAmount)
    {
        _level += increaseAmount;
    }

    private void RefreshText()
    {
        _tmPro.text = _cost.ToString();
    }

}
