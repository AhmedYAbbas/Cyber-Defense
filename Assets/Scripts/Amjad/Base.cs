using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public Slider Slider;

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            player.CustomProperties[CustomKeys.Base_HEALTH] = 100;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (Slider.value <= 0 && (MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
        {
            Slider.value = 100;
            MatchManager.Instance._destroyedDefenderBase = true;
            MatchManager.Instance.BaseDestroyedRaiseEvent();
        }
        else
        {
            int health = (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.Base_HEALTH];
            health -= dmg;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.Base_HEALTH] = health;
            Slider.value = health;
        }
    }
}
