using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class SpellItem : MonoBehaviour
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttempToCastSepll()
        {
            Debug.Log("you attempt to cast a spell");
        }
        public virtual void SuccessfullyCastSpell()
        {
            Debug.Log("you cast a spell successfully");
        }
    }
}
