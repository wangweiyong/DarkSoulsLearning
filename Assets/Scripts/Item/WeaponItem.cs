using UnityEngine;

namespace wwy
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        public string offHandIdleAnimation = "Left Arm Idle_01";

        [Header("Damage")]
        public int physicalDamage = 25;
        public int fireDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorption")]
        public float physicalDamageAbsorption;

        [Header("Weapon Type")]
        public WeaponType weaponType;

        [Header("Weapon Stamina")]
        public float baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Item Actions")]
        public ItemAction hold_RB_Action;
        public ItemAction tap_RB_Action;
        public ItemAction tap_LB_Action;
        public ItemAction hold_LB_Action;
        public ItemAction tap_RT_Action;
        public ItemAction hold_RT_Action;
        public ItemAction tap_LT_Action;
        public ItemAction hold_LT_Action;
    }
}
