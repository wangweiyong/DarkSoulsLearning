using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        [Header("Equipment Model Changers")]
        //chest, leg equipment, hand equipment
        HelmetModelChanger helmetModelChanger;
        TorsoModelChanger torsoModelChanger;
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        RightArmModelChanger rightArmModelChanger;
        LeftArmModelChanger leftArmModelChanger;
        [Header("Deafult Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;
        public string nakedRightLegModel;
        public string nakedLeftLegModel;
        public string nakedLeftArmModel;
        public string nakedRightArmModel;

        public BlockingCollider blockingCollider;
        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            rightArmModelChanger = GetComponentInChildren<RightArmModelChanger>();
            leftArmModelChanger = GetComponentInChildren<LeftArmModelChanger>();
        }
        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }
        private void EquipAllEquipmentModelsOnStart()
        {
            //helmet equipment
            helmetModelChanger.UnEquipAllHelmetModels();

            if (playerInventory.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            }
            else
            {
                //Equip default equipment
                //helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);
                nakedHeadModel.SetActive(true);
            }
            //torso equipment
            torsoModelChanger.UnEquipAllTorsoModels();
            leftArmModelChanger.UnEquipAllArmModels();
            rightArmModelChanger.UnEquipAllArmModels();
            if (playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                rightArmModelChanger.EquipArmModelByName(playerInventory.currentTorsoEquipment.rightArmModelName);
                leftArmModelChanger.EquipArmModelByName(playerInventory.currentTorsoEquipment.leftArmModelName);
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                rightArmModelChanger.EquipArmModelByName(nakedRightArmModel);
                leftArmModelChanger.EquipArmModelByName(nakedLeftArmModel);
            }

            //leg equipment
            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();
            if (playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegModelName);
            
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLegModel);
                rightLegModelChanger.EquipLegModelByName(nakedRightLegModel);
            }
        }
        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);

            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);

            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
