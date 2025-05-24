using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName ="Item/Ammo")]
    public class RangedAmmoItem : Item
    {
        [Header("Ammo Type")]
        public AmmoType ammoType;

        [Header("Ammot Velocity")]
        public float forwardVelocity = 550;
        public float upwardVelocity = 0;
        public float ammoMass = 0;
        public bool useGravity = false;

        [Header("Ammo Capacity")]
        public int carryLimit = 99;
        public int currentAmount = 99;

        [Header("Ammo Base Damage")]
        public int physicalDamage = 50;
        //magic damage
        //fire damage
        //dark damage
        //lighting damage

        [Header("Item Models")]
        public GameObject loadedItemModel;// the model that is displayed while drawing the bow back
        public GameObject liveAmmoMode;//the live model that can damage characters
        public GameObject penetratedMode;//the model that instantiates into a collider on contact


    }
}
