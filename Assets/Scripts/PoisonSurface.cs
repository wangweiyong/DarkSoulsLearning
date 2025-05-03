using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class PoisonSurface : MonoBehaviour
    {
        public float poisonBuildUpAmount = 7;

        public List<CharacterEffectsManager> charactersInsidePoisonSurface;


        private void OnTriggerEnter(Collider other)
        {
            CharacterEffectsManager characterEffectsManager = other.GetComponent<CharacterEffectsManager>();
            if(characterEffectsManager != null )
            {
                charactersInsidePoisonSurface.Add(characterEffectsManager);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterEffectsManager characterEffectsManager = other.GetComponent<CharacterEffectsManager>();
            if (characterEffectsManager != null)
            {
                charactersInsidePoisonSurface.Remove(characterEffectsManager);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            foreach(var character in charactersInsidePoisonSurface)
            {
                if (character.isPoisoned) continue;
                character.poisonBuildUp += poisonBuildUpAmount * Time.deltaTime;
            }
        }
    }
}
