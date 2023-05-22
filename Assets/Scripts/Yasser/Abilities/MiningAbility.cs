using Photon.Pun;
using UnityEngine;

public class MiningAbility : GenericAbility, IAbility
{
    public void Use()
    {
        // TODO: Make the ability usable after a time period (30 secs) and also make it usable only one time per (Game/Round)?!
        if (Time.time > _nextUseTime)
        {
            EnergyManager.Instance.DecreaseEnergy(cost);
            int defenderEnergy = (int)PhotonNetwork.PlayerListOthers[0].CustomProperties[CustomKeys.ENERGY];
            int energyToAdd = (int)(defenderEnergy * 0.2);
            EnergyManager.Instance.DecreaseEnergyEvent(-energyToAdd);

            _nextUseTime = Time.time + COOLDOWN_TIME;
        }
    }
}