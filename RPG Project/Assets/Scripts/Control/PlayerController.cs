using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.Rendering;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        // Fighter fighter;
        // GameObject combatTarget;
        Health health;
        private void Start()
        {
            // fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            // print("do nothing.");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            foreach (RaycastHit hit in hits)
            {
                CombatTarget combatTarget = hit.transform.GetComponent<CombatTarget>();  
                // print("combatTarget: " + combatTarget);
                if (combatTarget == null) continue;
                if (!GetComponent<Fighter>().CanAttack(combatTarget.gameObject)) continue;
                if (Input.GetMouseButtonDown(0))
                {
                    print("combat now!");
                    GetComponent<Fighter>().Attack(combatTarget.gameObject);                  
                }        
                return true;  
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);          
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                    // GetComponent<Fighter>().Cancel();   
                    
                }
                return true;
            }
            return false;
        }


        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

