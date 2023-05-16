using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;
    [SerializeField] private Transform towerIconPosition;
    private GameObject _currentTowerIcon;
    private GameObject _towerIcon;
    [SerializeField] private Transform towerHead;
    [SerializeField] private TowerModifications baseTowerSo;
    [HideInInspector] public TowerModifications currentModifications;
    public event EventHandler TowerGotModified;


    private void Start()
    {
        ModifyTower(baseTowerSo);
        _currentHealth = _maxHealth;
    }
    public void ModifyTower(TowerModifications towerModifications)
    {
        _maxHealth = towerModifications.maxHealth;
        _towerIcon = towerModifications.towerIcon;
        currentModifications = towerModifications;
        TowerGotModified?.Invoke(this, EventArgs.Empty);
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
        if (_currentHealth <= 0)
        {
            DestroyImmediate(gameObject);
        }
    }
}
