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


        [Header("Deafult Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;

        public BlockingCollider blockingCollider;
        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();  
        }
        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }
        private void EquipAllEquipmentModelsOnStart()
        {
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

            torsoModelChanger.UnEquipAllTorsoModels();
            if(playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
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
