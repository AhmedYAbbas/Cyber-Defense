using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviourPunCallbacks
{
    public Slider healthBarSlider;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        photonView.RPC("SyncBaseHealth", RpcTarget.All, _currentHealth);
    }

    public void TakeDamage(int dmg)
    {
        _currentHealth -= dmg;
        UpdateHealthBar();
        CheckBaseHealth();
        photonView.RPC("SyncBaseHealth", RpcTarget.All, _currentHealth);
    }

    private void UpdateHealthBar()
    {
        float healthPercentage  = (_currentHealth / _maxHealth) * 100;
        healthBarSlider.value = healthPercentage / 100;
    }

    void CheckBaseHealth()
    {
        if (_currentHealth <= 0)    
        {
            MatchManager.Instance.EndRound(true);
            _currentHealth = _maxHealth;
        }
    }

    [PunRPC]
    private void SyncBaseHealth(float health)
    {
        _currentHealth = health;
        UpdateHealthBar();
    }
}
