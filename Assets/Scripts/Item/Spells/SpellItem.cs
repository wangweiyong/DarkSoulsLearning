using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;
        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttempToCastSepll(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager)
        {
            Debug.Log("you attempt to cast a spell");
        }
        public virtual void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager)
        {
            Debug.Log("you cast a spell successfully");
            playerStats.DeductFocusPoints(focusPointCost);
        }
    }
}
