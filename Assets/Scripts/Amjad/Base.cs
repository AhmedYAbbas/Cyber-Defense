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
        if (_currentHealth <= 0 && (MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Defender
            && MatchManager.Instance.canRaiseBaseDestroyedEvent)    
        {
            _currentHealth = _maxHealth;
            MatchManager.Instance.canRaiseBaseDestroyedEvent = false;
            MatchManager.Instance.BaseDestroyedRaiseEvent();
        }
    }

    [PunRPC]
    private void SyncBaseHealth(float health)
    {
        _currentHealth = health;
        UpdateHealthBar();
    }

    //public void TakeDamage(int dmg)
    //{
    //    if (healthBarSlider.value <= 0 && (MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
    //    {
    //        healthBarSlider.value = 100;
    //        MatchManager.Instance.BaseDestroyedRaiseEvent();
    //    }
    //    else
    //    {
    //        int health = (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.Base_HEALTH];
    //        health -= dmg;
    //        PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.Base_HEALTH] = health;
    //        healthBarSlider.value = health;
    //    }
    //}
}
