using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item Actions/ Aim Action")]
    public class AimAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isAiming) return;
            player.inputHandler.uiManager.crossHair.SetActive(true);
            player.isAiming = true;
        }
    }
}
