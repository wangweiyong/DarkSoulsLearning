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
        PoisonBuildupBar poisonBuildupBar;
        PoisonAmountBar poisonAmountBar;
        // the particles that will play of the current effect that is effecting player
        public int amountToHeal;
        public GameObject instantiatedFXModel;
        protected override void Awake()
        {
            base.Awake();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            poisonBuildupBar = FindObjectOfType<PoisonBuildupBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }
        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToHeal);
            if(currentParticleFX != null)
            {
                GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            }
            Destroy(instantiatedFXModel.gameObject);
            playerWeaponSlotManager.LoadWeaponsOnBothHands();
        }
        protected override void HandlePoisonBuildUp()
        {
            if(poisonBuildUp <= 0)
            {
                poisonBuildupBar.gameObject.SetActive(false);
            }
            else
            {
                poisonBuildupBar.gameObject.SetActive(true);
            }
            base.HandlePoisonBuildUp();
            poisonBuildupBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildUp));
        }
        protected override void HandleIsPoisonedEffect()
        {
            if(isPoisoned == false)
            {
                poisonAmountBar.gameObject.SetActive(false);
            }
            else
            {
                poisonAmountBar.gameObject.SetActive(true);
            }
            base.HandleIsPoisonedEffect();
            poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }
    }
}
