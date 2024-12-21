using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;


        [Header("One Handed Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Light_Attack_3;
        public string OH_Light_Attack_4;
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;
        public string OH_Heavy_Attack_3;
        public string OH_Heavy_Attack_4;

    }
}
