using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace wwy
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image currentSpellIcon;
        public Image currentConsumableIcon;
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weaponItem)
        {
            if(isLeft == false)
            {
                if (weaponItem.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weaponItem.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if(weaponItem.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weaponItem.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }
        public void UpdateCurrentSpellIcon(SpellItem spell)
        {
            if(spell.itemIcon !=null)
            {
                currentSpellIcon.sprite = spell.itemIcon;
                currentSpellIcon.enabled = true;
            }
            else
            {
                currentSpellIcon.sprite = null;
                currentSpellIcon.enabled = false;
            }
        }
        public void UpdateCurrentConsumableIcon(ConsumableItem consumableItem)
        {
            if (consumableItem.itemIcon != null)
            {
                currentConsumableIcon.sprite = consumableItem.itemIcon;
                currentConsumableIcon.enabled = true;
            }
            else
            {
                currentConsumableIcon.sprite = null;
                currentConsumableIcon.enabled = false;
            }
        }
    }
}
