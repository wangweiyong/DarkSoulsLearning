using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerDamage : MonoBehaviour
    {
        private int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStatsManager playerStats=other.GetComponent<PlayerStatsManager>();
            if(playerStats != null )
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}
