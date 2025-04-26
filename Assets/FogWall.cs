using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class FogWall : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void ActivagteFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }
    }
}
