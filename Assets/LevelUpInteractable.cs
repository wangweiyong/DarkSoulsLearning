using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class LevelUpInteractable : Interactabble
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.uIManager.levelUpWindow.SetActive(true);
        }
    }
}
