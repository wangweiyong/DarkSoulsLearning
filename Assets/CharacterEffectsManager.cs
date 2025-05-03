using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;
        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(isLeft == false)
            {
                //play the right weapons trail
                if(rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                //play the left weapons trail
                if(leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }
    }
}
