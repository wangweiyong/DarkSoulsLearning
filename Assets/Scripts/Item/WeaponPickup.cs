using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            PlayerInventoryManager playerInventory;
            PlayerLocomotionManager playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();
            
            playerLocomotion.rigidbody.velocity = Vector3.zero; // stop players moving whilst pick up item
            animatorHandler.PlayTargetAnimation("PickupItem", true);
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;

            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
