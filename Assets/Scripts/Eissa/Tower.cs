using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tower : MonoBehaviourPun 
{
    private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private Transform towerIconPosition;
    private GameObject _currentTowerIcon;
    private GameObject _towerIcon;
    [SerializeField] private Transform towerHead;
    [SerializeField] private TowerModifications baseTowerSo;
    [HideInInspector] public TowerModifications currentModifications;
    [SerializeField] List<TowerModifications> _modificationsList;
    public event EventHandler TowerGotModified;
    [SerializeField] private Slider healthBarSlider;

    private void Start()
    {
        ModifyTower(baseTowerSo);
        _currentHealth = _maxHealth;
        healthBarSlider.value = 1;
    }
    public void ModifyTower(TowerModifications towerModifications)
    {
        for (int i = 0; i < _modificationsList.Count; i++)
        {
            if (_modificationsList[i] == towerModifications)
            {
                photonView.RPC("UpdateStatsAndSwitch", RpcTarget.All, i);
            }
        }
    }
    private void SwitchTowerIcon()
    {
        if (_currentTowerIcon != null)
        {
            DestroyImmediate(_currentTowerIcon);
        }
        _currentTowerIcon = Instantiate(_towerIcon, towerIconPosition.transform.position, quaternion.identity, towerHead);
    }
    public void DamageTower(int dmg)
    {
        _currentHealth -= dmg;
        UpdateHealthBar();
        CheckTowerHealth();
        photonView.RPC("SyncTowerHealth", RpcTarget.Others, _currentHealth);
    }

    void CheckTowerHealth()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private void UpdateHealthBar()
    {
        float healthPercentage  = (_currentHealth / _maxHealth) * 100;
        healthBarSlider.value = healthPercentage / 100;
    }

    #region RPCs

    [PunRPC]
    private void SyncTowerHealth(float health)
    {
        _currentHealth = health;
        UpdateHealthBar();
        CheckTowerHealth();
    }
    [PunRPC]
    private void UpdateStatsAndSwitch(int i)
    {
        this.name = _modificationsList[i].ModificationName;
        _maxHealth = _modificationsList[i].maxHealth;
        _towerIcon = _modificationsList[i].towerIcon;
        currentModifications = _modificationsList[i];
        TowerGotModified?.Invoke(this, EventArgs.Empty);
        SwitchTowerIcon();
    }

    #endregion
    
    
}
