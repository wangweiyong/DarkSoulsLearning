using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class Interactabble : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            // Called when player interacts;
            Debug.Log("You Interacted with an object");
        }
    }
}
