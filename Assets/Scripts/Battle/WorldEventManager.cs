using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogwalls;
        public UIBossHealthBar bossHealthBar;
        public EnemyBossManager boss;

        public bool bossFightIsActive;//Is currently fighting boss
        public bool bossHasBeenAwakened;//awake the boss/watched cutscene but died during fight
        public bool bossHasBeenDefeated;//boss has been defeated

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();
            //active fog walls
            foreach(var fogwall in fogwalls)
            {
                fogwall.ActivagteFogWall() ;
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;
            //deactivate fog walls
            foreach (var fogwall in fogwalls)
            {
                fogwall.DeactivateFogWall();
            }
        }
    }
}
