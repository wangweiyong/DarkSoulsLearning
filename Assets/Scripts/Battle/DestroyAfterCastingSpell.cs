using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class DestroyAfterCastingSpell : MonoBehaviour
    {
        CharacterManager characterCastingSpell;

        private void Awake()
        {
            characterCastingSpell = transform.root.GetComponent<CharacterManager>();
        }

        private void Update()
        {
            if(characterCastingSpell.isFiringSpell)
            {
                Destroy(gameObject);
            }
        }
    }
}