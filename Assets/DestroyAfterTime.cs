using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float timeUntileDestroyed = 3;
        private void Awake()
        {
            Destroy(gameObject, timeUntileDestroyed);
        }
    }
}
