using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class EventColliderBeginBossFight : MonoBehaviour
    {
        WorldEventManager worldEventManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();  
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Character")
            {
                worldEventManager.ActivateBossFight();
            }
        }
    }
}