using UnityEngine;
namespace wwy
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterWeaponSlotManager characterWeaponSlotManager;
        [Header("Current Item Being Used")]
        public Item currentItemBeingUsed;

        [Header("Current Equipment")]
        public HelmetEquipment currentHelmetEquipment;
        public TorsoEquipment currentTorsoEquipment;
        public LegEquipment currentLegEquipment;

        public ConsumableItem currentConsumableItem;
        [Header("Quick Slot Items")]
        public SpellItem currentSpell;
        public RangedAmmoItem currentAmmo;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        private void Awake()
        {
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();    
        }

        private void Start()
        {
            characterWeaponSlotManager.LoadWeaponsOnBothHands();
        }
    }
}
