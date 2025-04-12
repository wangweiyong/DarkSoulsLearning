using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Spells/Headling Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttempToCastSepll(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            base.AttempToCastSepll (animatorHandler, playerStats, weaponSlotManager);
            if (spellWarmUpFX != null)
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            }
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell (animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            if(spellCastFX != null)
            {
                GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            }
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spell cast successful");
        }
    }
}
