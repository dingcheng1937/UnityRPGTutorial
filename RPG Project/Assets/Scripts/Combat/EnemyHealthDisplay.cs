using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
                       
        } 

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            GameObject target = fighter.GetTarget();
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}, {2:0}%", target.GetComponent<Health>().GetHealthPoints(), target.GetComponent<Health>().GetMaxHealthPoints(), target.GetComponent<Health>().GetPercentage());
        }
    }
}
