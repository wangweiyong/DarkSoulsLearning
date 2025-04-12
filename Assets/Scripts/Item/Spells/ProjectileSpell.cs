using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wwy;
namespace wwy
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;
        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectilUpwardVelocity;

        public bool isEffectedByGravity;
        public float projectileMass;
        Rigidbody rigidBody;

        public override void AttempToCastSepll(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            base.AttempToCastSepll(animatorHandler, playerStats, weaponSlotManager);
            //Instantiate the spell in the casting hand of the player;
            //play animation to cast spell
            if(spellWarmUpFX != null)
            {
                GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
                instantiateWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
            }
            
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            GameObject instantiateSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            rigidBody = instantiateSpellFX.GetComponent<Rigidbody>();
            //spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
            
            if(cameraHandler.currentLockOnTarget != null)
            {
                instantiateSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiateSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
            }
            rigidBody.AddForce(instantiateSpellFX.transform.forward * projectileForwardVelocity);
            rigidBody.AddForce(instantiateSpellFX.transform.up * projectilUpwardVelocity);
            rigidBody.useGravity = isEffectedByGravity;
            rigidBody.mass = projectileMass;
            instantiateSpellFX.transform.parent = null;
        }
    }
}