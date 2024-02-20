using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private MMProgressBar mmProgressBar;

    private float _maxHealth = 250f;
    private float _currentHealth;

    public void InitHealthBar(float maxHealth, float currentHeatlth)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHeatlth;

        float percent = Mathf.Clamp01(_currentHealth / _maxHealth);
        mmProgressBar.UpdateBar01(percent);
    }

    public void InitHealthBar(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;

        float percent = Mathf.Clamp01(_currentHealth / _maxHealth);
        mmProgressBar.UpdateBar01(percent);
    }

    public void IncreaseHealhtBar(float currentHealth, float damage)
    {
        _currentHealth = currentHealth;
        float _newCurrentHealth = _currentHealth + damage;
        float percent = Mathf.Clamp01((_newCurrentHealth - currentHealth) / _maxHealth);
        PlusPercentage(percent);
        _currentHealth = _newCurrentHealth;
    }

    public void DecreaseHealthBar(float currentHealth, float damage)
    {
        _currentHealth = currentHealth;
        float _newCurrentHealth = _currentHealth - damage;
        float percent = Mathf.Clamp01((currentHealth - _newCurrentHealth) / _maxHealth);
        MinusPercentage(percent);
        _currentHealth = _newCurrentHealth;
    }

    private void PlusPercentage(float percentage)
    {
        float newProgress = mmProgressBar.BarTarget + percentage;
        newProgress = Mathf.Clamp(newProgress, 0f, 1f);
        mmProgressBar.UpdateBar01(newProgress);
    }

    private void MinusPercentage(float percentage)
    {
        float newProgress = mmProgressBar.BarTarget - percentage;
        newProgress = Mathf.Clamp(newProgress, 0f, 1f);
        mmProgressBar.UpdateBar01(newProgress);
    }
}
