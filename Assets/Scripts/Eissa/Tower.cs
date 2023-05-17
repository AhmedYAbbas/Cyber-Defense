using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    [SerializeField] private Slider healthBarSlider;


    private void Start()
    {
        ModifyTower(baseTowerSo);
        _currentHealth = _maxHealth;
        healthBarSlider.value = 1;
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
        if (_currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
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
}
