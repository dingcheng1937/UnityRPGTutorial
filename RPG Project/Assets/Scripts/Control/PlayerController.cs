using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.Rendering;
using System;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("do nothing.");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            foreach (RaycastHit hit in hits)
            {
                CombatTarget combatTarget = hit.collider.GetComponent<CombatTarget>();  
                if (combatTarget == null) 
                {
                    continue;
                }
                // else
                // {
                //     print("combat ready.");
                // }
                if (Input.GetMouseButtonDown(0))
                {
                    print("combat now!");
                    GetComponent<Fighter>().Attack(combatTarget);                  
                }        
                return true;  
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);          
            print("move.");
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

