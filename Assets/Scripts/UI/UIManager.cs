using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace wwy
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        EquipmentWindowUI equipmentWindowUI;
        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;

        [Header("Weapon Inventory")]
        public Transform weaponInventorySlotsParent;
        public GameObject weaponInventorySlotPrefab;
        WeaponInventorySlot[] weaponInventorySlots; // 生成的slots


        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
        }
        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for(int i = 0; i < weaponInventorySlots.Length; ++i)
            {
                if(i < playerInventory.weaponsInventory.Count)
                {
                    //ui格子少于玩家的库存武器数量
                    if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            #endregion
        }


        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }
        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            weaponInventoryWindow.SetActive(false);
        }
    }
}
