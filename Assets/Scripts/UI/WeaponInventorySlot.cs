using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace wwy
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        PlayerInventory playerInvenory;
        WeaponSlotManager weaponSlotManager;
        UIManager uIManager;
        public Image icon;
        WeaponItem item;

        private void Awake()
        {
            playerInvenory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uIManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            //Add current item to inventory
            //equip this new item
            //remove this item from inventory
            if (uIManager.rightHandSlot01Selected)
            {
                playerInvenory.weaponsInventory.Add(playerInvenory.weaponsInRightHandSlots[0]);
                playerInvenory.weaponsInRightHandSlots[0] = item;
                playerInvenory.weaponsInventory.Remove(item);
            }
            else if (uIManager.rightHandSlot02Selected)
            {
                playerInvenory.weaponsInventory.Add(playerInvenory.weaponsInRightHandSlots[1]);
                playerInvenory.weaponsInRightHandSlots[1] = item;
                playerInvenory.weaponsInventory.Remove(item);

            }
            else if (uIManager.leftHandSlot01Selected)
            {
                playerInvenory.weaponsInventory.Add(playerInvenory.weaponsInLeftHandSlots[0]);
                playerInvenory.weaponsInLeftHandSlots[0] = item;
                playerInvenory.weaponsInventory.Remove(item);

            }
            else if(uIManager.leftHandSlot02Selected)
            {
                playerInvenory.weaponsInventory.Add(playerInvenory.weaponsInLeftHandSlots[1]);
                playerInvenory.weaponsInLeftHandSlots[1] = item;
                playerInvenory.weaponsInventory.Remove(item);

            }
            else
            {
                return;
            }
            playerInvenory.rightWeapon = playerInvenory.weaponsInRightHandSlots[playerInvenory.currentRightWeaponIndex];
            playerInvenory.leftWeapon = playerInvenory.weaponsInLeftHandSlots[playerInvenory.currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(playerInvenory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInvenory.leftWeapon, true);

            uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInvenory);
            uIManager.ResetAllSelectedSlots();
        }
    }
}
