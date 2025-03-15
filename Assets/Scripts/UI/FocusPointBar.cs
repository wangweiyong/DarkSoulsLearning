using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace wwy
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider slider;
        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponent<Slider>();
        }
        public void SetMaxFocusPoint(float maxFocusPoint)
        {
            slider.maxValue = maxFocusPoint;
            slider.value= maxFocusPoint;
        }

        public void SetCurrentFocusPoint(float currentFocusPoint)
        {
            slider.value = currentFocusPoint;
        }
    }
}
