using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class OpenChest : Interactabble
    {
        Animator animator;
        public Transform playerStandingPosition;
        OpenChest openChest;
        public GameObject itemSpawner;

        public WeaponItem itemInChest;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }
        public override void Interact(PlayerManager playerManager)
        {
            //Rotate our player towards the chest
            //lock his transform to a certain point in front of the chest
            //open the chest lid, and animate the player
            //spawn an item inside the chest the player can pick up
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            playerManager.OpenChestInteraction(playerStandingPosition);

            animator.Play("Chest Open");
            StartCoroutine(SwapnItemInChest());
            WeaponPickup weaponPickUp = itemSpawner.GetComponent<WeaponPickup>();
            if(weaponPickUp != null)
            {
                weaponPickUp.weapon = itemInChest;
            }

        }

        private IEnumerator SwapnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}
