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
    public event EventHandler TowerGotModified;
    [SerializeField] private Slider healthBarSlider;

    private void Start()
    {
        
            ModifyTower(baseTowerSo);
            _currentHealth = _maxHealth;
            healthBarSlider.value = 1;
            print("Is Mine");
        
    }
    public void ModifyTower(TowerModifications towerModifications)
    {
        this.name = towerModifications.ModificationName;
        _maxHealth = towerModifications.maxHealth;
        _towerIcon = towerModifications.towerIcon;
        currentModifications = towerModifications;
        TowerGotModified?.Invoke(this, EventArgs.Empty);
        print("tower modified");
        SwitchTowerIcon();
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
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    [PunRPC]
    private void SyncTowerHealth(float health)
    {
        _currentHealth = health;
        UpdateHealthBar();
        CheckTowerHealth();
    }

    private void OnDisable()
    {
        Destroy(gameObject,5);
    }

    private void UpdateHealthBar()
    {
        float healthPercentage  = (_currentHealth / _maxHealth) * 100;
        healthBarSlider.value = healthPercentage / 100;
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref _currentHealth);
        if (stream.IsWriting)
        {
            stream.SendNext(_currentHealth);
        }
        else
        {
            _currentHealth = (float)stream.ReceiveNext();
        }
        UpdateHealthBar();
    }*/
}
