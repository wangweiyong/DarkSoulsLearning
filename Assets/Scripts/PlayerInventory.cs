using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        public WeaponItem rightWeapon;
        public WeaponItem lefttWeapon;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }
        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(lefttWeapon, true);
        }
    }
}
