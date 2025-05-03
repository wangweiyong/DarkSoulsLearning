using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        public GameObject currentParticleFX;
        // the particles that will play of the current effect that is effecting player
        public int amountToHeal;
        public GameObject instantiatedFXModel;
        protected override void Awake()
        {
            base.Awake();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();  
        }
        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToHeal);
            if(currentParticleFX != null)
            {
                GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            }
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadBothWeaponsOnSlot();
        }
    }
}
