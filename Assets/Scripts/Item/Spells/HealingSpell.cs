using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Spells/Headling Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttempToCastSepll(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.AttempToCastSepll (animatorHandler, playerStats);
            if (spellWarmUpFX != null)
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            }
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell (animatorHandler, playerStats);
            if(spellCastFX != null)
            {
                GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            }
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spell cast successful");
        }
    }
}
