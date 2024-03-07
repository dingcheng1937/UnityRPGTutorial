using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float giveupDistance = 10f;
        GameObject player;
        Fighter fighter;
        Health health;
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health =  GetComponent<Health>();
        }
        private void Update()
        {
            
            if (health.IsDead()){return;}
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= chaseDistance)
                {
                    // print (gameObject.name + " should chase !!! " + player);
                    fighter.Attack(player);
                }
                if (distance >= giveupDistance)
                {
                    fighter.Cancel();
                }
                
            }
        }

        private bool InteractWithCombat()
        {               
            GameObject combatTarget = GameObject.FindWithTag("Player");
            if (combatTarget == null) {return false;}
            if (!GetComponent<Fighter>().CanAttack(combatTarget)) 
            {
                return false;
            }
            else
            {
                GetComponent<Fighter>().Attack(combatTarget);
                return true;  
            }
        }

        private bool InteractWithMovement()
        {
            CombatTarget combatTarget = player.GetComponent<CombatTarget>();        
            if (combatTarget!=null)
            {

                GetComponent<Mover>().StartMoveAction(combatTarget.transform.position);
                    // GetComponent<Fighter>().Cancel();   
                    

                return true;
            }
            return false;
        }
    }
}
