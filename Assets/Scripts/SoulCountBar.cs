using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace wwy
{
    public class SoulCountBar : MonoBehaviour
    {
        public Text soulCountText;

        public void SetSoulCountText(int souldCountNumber)
        {
            soulCountText.text = souldCountNumber.ToString();
        }
    }
}
