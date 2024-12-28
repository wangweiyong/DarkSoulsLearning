using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class WeaponPickup : Interactabble
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //PICK UP THE ITEM AND ADD IT TO THE PLAYER INVENTORY
            PickupItem(playerManager);

        }

        private void PickupItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
            
            playerLocomotion.rigidbody.velocity = Vector3.zero; // stop players moving whilst pick up item
            animatorHandler.PlayTargetAnimation("PickupItem", true);
            playerInventory.weaponsInventory.Add(weapon);
            Destroy(gameObject);
        }
    }
}
