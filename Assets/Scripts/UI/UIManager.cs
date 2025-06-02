using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace wwy
{
    public class UIManager : MonoBehaviour
    {
        PlayerManager playerManager;
        public EquipmentWindowUI equipmentWindowUI;
        private QuickSlotsUI quickSlotUI;
        [Header("HUD")]
        public GameObject crossHair;
        public Text soulCount;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentScreenWindow;
        public GameObject levelUpWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [Header("Weapon Inventory")]
        public Transform weaponInventorySlotsParent;
        public GameObject weaponInventorySlotPrefab;
        WeaponInventorySlot[] weaponInventorySlots; // 生成的slots
        private void Awake()
        {
            quickSlotUI = GetComponentInChildren<QuickSlotsUI>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerManager.playerInventoryManager);
            quickSlotUI.UpdateCurrentConsumableIcon(playerManager.playerInventoryManager.currentConsumableItem);
            quickSlotUI.UpdateCurrentSpellIcon(playerManager.playerInventoryManager.currentSpell);
            soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();
        }
        public void UpdateUI()
        {
            #region Weapon Inventory Slots
            for(int i = 0; i < weaponInventorySlots.Length; ++i)
            {
                if(i < playerManager.playerInventoryManager.weaponsInventory.Count)
                {
                    //ui格子少于玩家的库存武器数量
                    if(weaponInventorySlots.Length < playerManager.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(playerManager.playerInventoryManager.weaponsInventory[i]);
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
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
        }
    }
}
