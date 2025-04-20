using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public Material illusionaryMaterial;
        public float alpha;
        public float fadeTimer = 2.5f;

        public BoxCollider wallCollider;

        public AudioSource audioSource;
        public AudioClip illusionarywallSound;


        private void Update()
        {
            if(wallHasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                wallHasBeenHit = true;
            }
        }
        public void FadeIllusionaryWall()
        {
            alpha = illusionaryMaterial.color.a;
            alpha = alpha - Time.deltaTime / fadeTimer;

            Color fadeWallColor = new Color(1, 1, 1, alpha);
            illusionaryMaterial.color = fadeWallColor;

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(illusionarywallSound);
            }

            if(alpha <= 0)
            {
                Destroy(this);
            }
        }
    }
}
