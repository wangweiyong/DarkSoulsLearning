using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class EnemyBossManager : MonoBehaviour
    {
        //handle switching phase
        //handle switch attack patterns
        public string bossName;
        UIBossHealthBar bossHealthBar;
        EnemyStats enemyStats;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyStats = GetComponent<EnemyStats>();
        }
        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }
    }
}
