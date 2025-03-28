using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class WeaponHolderSlots : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;
        public WeaponItem currentWeaponItem;

        public GameObject currentWeaponModel;

        //ж������
        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null )
            {
                if(parentOverride != null)
                {
                    model.transform.parent = parentOverride.transform;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one * 0.015f;
            }

            currentWeaponModel = model;
        }
    }
}
