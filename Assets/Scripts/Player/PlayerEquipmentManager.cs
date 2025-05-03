using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;

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
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            rightArmModelChanger = GetComponentInChildren<RightArmModelChanger>();
            leftArmModelChanger = GetComponentInChildren<LeftArmModelChanger>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }
        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }
        private void EquipAllEquipmentModelsOnStart()
        {
            //helmet equipment
            helmetModelChanger.UnEquipAllHelmetModels();

            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                //Equip default equipment
                //helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);
                nakedHeadModel.SetActive(true);
                playerStatsManager.physicalDamageAbsorptionHead = 0;
            }
            //torso equipment
            torsoModelChanger.UnEquipAllTorsoModels();
            leftArmModelChanger.UnEquipAllArmModels();
            rightArmModelChanger.UnEquipAllArmModels();
            if (playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
                rightArmModelChanger.EquipArmModelByName(playerInventoryManager.currentTorsoEquipment.rightArmModelName);
                leftArmModelChanger.EquipArmModelByName(playerInventoryManager.currentTorsoEquipment.leftArmModelName);
                playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                rightArmModelChanger.EquipArmModelByName(nakedRightArmModel);
                leftArmModelChanger.EquipArmModelByName(nakedLeftArmModel);
                playerStatsManager.physicalDamageAbsorptionBody = 0;
            }

            //leg equipment
            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();
            if (playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.rightLegModelName);
                playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLegModel);
                rightLegModelChanger.EquipLegModelByName(nakedRightLegModel);
                playerStatsManager.physicalDamageAbsorptionLegs = 0;
            }
        }
        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);

            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);

            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
