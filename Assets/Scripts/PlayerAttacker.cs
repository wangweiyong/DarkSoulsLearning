using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }
        public void HandleLigthAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);

        }
    }
}
