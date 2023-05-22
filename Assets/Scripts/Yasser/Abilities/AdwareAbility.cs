using Photon.Pun;
using UnityEngine;

public class AdwareAbility : GenericAbility, IAbility
{
    public void Use()
    {
        if (Time.time > _nextUseTime)
        {
            EnergyManager.Instance.DecreaseEnergy(cost);
            MatchManager.Instance.AdwareAbilityRaiseEvent();
            _nextUseTime = Time.time + COOLDOWN_TIME;
        }
    }
}