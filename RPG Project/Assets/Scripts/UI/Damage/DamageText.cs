using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Damage
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;
        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float amount)
        {
            if (damageText == null) return;
            damageText.text = String.Format("{0:0}", amount);
        }
    }
}
