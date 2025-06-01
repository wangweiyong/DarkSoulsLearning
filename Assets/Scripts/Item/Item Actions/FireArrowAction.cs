using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item Actions/ Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            //create live arrow at specific location
            //give ammo velocity
            //destory previous loaded arrow fx
            //set live arrow damage
            //animate the bow firing the arrow
            ArrowInstantiationLocotion arrowInstantiationLocotion;
            arrowInstantiationLocotion = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocotion>();

            Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("BOW_TH_FIRE_01");
            Destroy(player.playerEffectsManager.currentRangedFX); // destroy the loaded arrow model

            //reset the player holding arrow flag
            player.playerAnimatorManager.PlayTargetAnimation("BOW_TH_FIRE_01", true);
            player.playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

            GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoMode, arrowInstantiationLocotion.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if (player.isAiming)
            {
                Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;
                if (Physics.Raycast(ray, out hitPoint, 100.0f))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                    Debug.Log(hitPoint.transform.name);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                if (player.cameraHandler.currentLockOnTarget != null)
                {
                    //since while locked we are always facing our target we can copy our facing direction to our arrow facing when fired
                    Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }
            rigidBody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity);
            rigidBody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity);
            rigidBody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;
            rigidBody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = player;
            damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
        }
    }
}
