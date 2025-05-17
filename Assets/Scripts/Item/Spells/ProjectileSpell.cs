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

        public override void AttempToCastSepll(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.AttempToCastSepll(animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
            //Instantiate the spell in the casting hand of the player;
            //play animation to cast spell

            if (isLeftHanded)
            {
                if (spellWarmUpFX != null)
                {
                    GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.leftHandSlot.transform);
                    instantiateWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
                }

                animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
            }
            else
            {
                if (spellWarmUpFX != null)
                {
                    GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
                    instantiateWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
                }

                animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);

            }

        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager, isLeftHanded);


            if (isLeftHanded)
            {
                GameObject instantiateSpellFX = Instantiate(spellCastFX, weaponSlotManager.leftHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
                //in the futuer, add an instantiation location on the caster weapon itself//polish
                SpellDamageCollider spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
                rigidBody = instantiateSpellFX.GetComponent<Rigidbody>();
                if (cameraHandler.currentLockOnTarget != null)
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
            else
            {
                GameObject instantiateSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
                SpellDamageCollider spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
                rigidBody = instantiateSpellFX.GetComponent<Rigidbody>();
                if (cameraHandler.currentLockOnTarget != null)
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
            //spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
        }
    }
}