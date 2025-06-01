using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item Actions/ Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting) return;
            player.playerAnimatorManager.EraseHandIKForWeapon();
            WeaponItem parryingWeapon = player.playerInventoryManager.rightWeapon;

            //check if parring weapon is a fast parry weapon or a medium parrying weapon
            if(parryingWeapon.weaponType == WeaponType.SmallShield )
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
            else if(parryingWeapon.weaponType == WeaponType.Shield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
        }
    }
}
