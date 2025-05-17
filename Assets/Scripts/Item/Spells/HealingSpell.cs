using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Spells/Headling Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttempToCastSepll(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.AttempToCastSepll (animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
            if (spellWarmUpFX != null)
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            }
            animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.SuccessfullyCastSpell (animatorHandler, playerStats, cameraHandler, weaponSlotManager, isLeftHanded);
            if(spellCastFX != null)
            {
                GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            }
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spell cast successful");
        }
    }
}
