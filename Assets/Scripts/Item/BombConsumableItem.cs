using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item/Consumable/Bomb Item")]
    public class BombConsumableItem : ConsumableItem
    {
        [Header("Velocity")]
        public int upwardVelocity = 50;
        public int forwardVelocity = 50;
        public int bombMass = 1;

        [Header("Active Model")]
        public GameObject liveBombModel;

        [Header("Base Damage")]
        public int baseDamage = 200;
        public int explosiveDamage = 75;

        public override void AttempToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if(currentItemAmount > 0)
            {
                weaponSlotManager.rightHandSlot.UnloadWeapon();
                playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true);
                GameObject bombModel = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform.position, Quaternion.identity);

                playerEffectsManager.instantiatedFXModel = bombModel;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }
    }
}
