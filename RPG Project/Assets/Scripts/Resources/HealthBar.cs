using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace RPG.Resources
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] RectTransform foreground;
        [SerializeField] Canvas rootCanvas = null;
        public void ReduceHealth(float damage)
        {
            
            if (Mathf.Approximately(health.GetFraction(), 0)
            ||  Mathf.Approximately(health.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(health.GetFraction(), 1, 1);
        }
    }
}
