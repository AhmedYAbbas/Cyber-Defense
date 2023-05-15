using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MiningAbility : MonoBehaviourPunCallbacks, IAbility
{
    private const int COST = 50;


    public void Use()
    {
        int defenderEnergy = (int)PhotonNetwork.PlayerListOthers[0].CustomProperties[CustomKeys.ENERGY];
        int energyToAdd = (int)(defenderEnergy * 0.2);
        EnergyManager.Instance.DecreaseEnergyEvent(-energyToAdd);
    }

}
