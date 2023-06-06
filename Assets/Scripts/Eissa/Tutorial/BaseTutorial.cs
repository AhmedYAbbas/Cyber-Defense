using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class BaseTutorial : MonoBehaviour
{
    public Slider healthBarSlider;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int dmg)
    {
        _currentHealth -= dmg;
        UpdateHealthBar();
        CheckBaseHealth();
    }

    private void UpdateHealthBar()
    {
        float healthPercentage  = (_currentHealth / _maxHealth) * 100;
        DOTween.To(() => healthBarSlider.value, x => healthBarSlider.value = x, healthPercentage / 100f, 1f);
    }

    void CheckBaseHealth()
    {
        if (_currentHealth <= 0)
        {
            TutorialManager.Instance._currentIndex++;
            TutorialManager.Instance.SwitchThePopupsOnAndOff();
            _currentHealth = _maxHealth;
            UpdateHealthBar();
        }
    }
}
