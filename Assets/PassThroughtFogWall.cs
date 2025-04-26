using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class PassThroughtFogWall : Interactabble
    {
        WorldEventManager worldEventManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            playerManager.PassThroughForWallInteraction(transform);
            worldEventManager.ActivateBossFight();
        }
    }
}