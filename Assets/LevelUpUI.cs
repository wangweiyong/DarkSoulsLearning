using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace wwy
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Button confimLevelUpButton;

        [Header("Player Level")]
        public int currentPlayerLevel;
        public int projectedPlayerLevel;// the possible level we will be if we accept leveling up
        public Text currentPlayerLevelText;
        public Text projectedPlayerLevelText;

        [Header("Souls")]
        public Text currentSoulsText;
        public Text soulsRequiredToLevelUpText;
        public int soulsRequireToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("Health")]
        public Slider healthSlider;
        public Text currentHealthLevelText;
        public Text projectedHealthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public Text currentStaminaLevelText;
        public Text projectedStaminaLevelText;

        [Header("Focus")]
        public Slider focusSlider;
        public Text currentFocusLevelText;
        public Text projectedFocusLevelText;

        [Header("Strength")]
        public Slider strengthSlider;
        public Text currentStrengthLevelText;
        public Text projectedStrengthLevelText;


        [Header("Poise")]
        public Slider poiseSlider;
        public Text currentPoiseLevelText;
        public Text projectedPoiseLevelText;

        [Header("Dexterity")]
        public Slider dexteritySlider;
        public Text currentDexterityLevelText;
        public Text projectedDexterityLevelText;

        [Header("Faith")]
        public Slider faithSlider;
        public Text currentFaithLevelText;
        public Text projectedFaithLevelText;

        [Header("Intelligence")]
        public Slider intelligenceSlider;
        public Text currentIntelligenceLevelText;
        public Text projectedIntelligenceLevelText;
        private void OnEnable()
        {
            currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();
            projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStatsManager.healthLevel;
            currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
            healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;

            staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
            currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
            staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;

            focusSlider.value = playerManager.playerStatsManager.focusLevel;
            currentFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
            focusSlider.minValue = playerManager.playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;

            poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
            currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
            poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;

            strengthSlider.value = playerManager.playerStatsManager.strengthLevel;
            currentStaminaLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();
            projectedPoiseLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();
            poiseSlider.minValue = playerManager.playerStatsManager.strengthLevel;
            poiseSlider.maxValue = 99;

            dexteritySlider.value = playerManager.playerStatsManager.dexterityLevel;
            currentDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
            dexteritySlider.minValue = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;

            intelligenceSlider.value = playerManager.playerStatsManager.intelligenceLevel;
            currentIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
            projectedDexterityLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
            intelligenceSlider.minValue = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;

            faithSlider.value = playerManager.playerStatsManager.faithLevel;
            currentFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
            faithSlider.minValue = playerManager.playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;
            currentSoulsText.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            UpdateProjectedPlayerLevel();
        }
    
        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
            playerManager.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
            playerManager.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
            playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
            playerManager.playerStatsManager.strengthLevel = Mathf.RoundToInt(strengthSlider.value);
            playerManager.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);
            playerManager.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);

            playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
            playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStatsManager.maxFocusPoints = playerManager.playerStatsManager.SetMaxFocusPointsFromFocusLevel();
            playerManager.playerStatsManager.currentSoulCount -= soulsRequireToLevelUp;
            playerManager.uIManager.soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();
            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for(int i = 0; i < projectedPlayerLevel; ++i)
            {
                soulsRequireToLevelUp = soulsRequireToLevelUp + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
            }
        }
        //update the projected player's total level, by adding up all the projected level up stats
        private void UpdateProjectedPlayerLevel()
        {
            soulsRequireToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel += Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
            projectedPlayerLevel += Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
            projectedPlayerLevel += Mathf.RoundToInt(focusSlider.value) - playerManager.playerStatsManager.focusLevel;
            projectedPlayerLevel += Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
            projectedPlayerLevel += Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStatsManager.strengthLevel;
            projectedPlayerLevel += Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStatsManager.dexterityLevel;
            projectedPlayerLevel += Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStatsManager.intelligenceLevel;
            projectedPlayerLevel += Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;

            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();
            CalculateSoulCostToLevelUp();
            soulsRequiredToLevelUpText.text = soulsRequireToLevelUp.ToString();

            if (playerManager.playerStatsManager.currentSoulCount < soulsRequireToLevelUp)
            {
                confimLevelUpButton.interactable = false;
            }
            else
            {
                confimLevelUpButton.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = focusSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = strengthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = dexteritySlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }
}
