using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class PlayerEffectsManager : MonoBehaviour
    {
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        public GameObject currentParticleFX;
        // the particles that will play of the current effect that is effecting player
        public int amountToHeal;
        public GameObject instantiatedFXModel;
        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();  
        }
        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountToHeal);
            if(currentParticleFX != null)
            {
                GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
            }
            Destroy(instantiatedFXModel.gameObject);
            weaponSlotManager.LoadBothWeaponsOnSlot();
        }
    }
}
