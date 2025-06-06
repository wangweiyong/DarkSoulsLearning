using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace wwy
{
    public class PoisonBuildupBar : MonoBehaviour
    {
        public Slider slider;
        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }

        public void SetPoisonBuildUpAmount(int currentPoisonBuildUp)
        {
            slider.value = currentPoisonBuildUp;
        }
    }
}
