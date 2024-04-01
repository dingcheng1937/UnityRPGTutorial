using System.Collections;
using System.Collections.Generic;
using System;
using RPG.Combat;
using RPG.Resources;
using RPG.Movement;
using UnityEngine;
using UnityEngine.Rendering;
// using System;


namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        // Fighter fighter;
        // GameObject combatTarget;
        Health health;
        enum CursorType
        {
            None, 
            Movement,
            Combat,
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        private void Awake()
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
            SetCursor(CursorType.None);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
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
                    // print("combat now!");
                    GetComponent<Fighter>().Attack(combatTarget.gameObject);                  
                }        
                SetCursor(CursorType.Combat);
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
                SetCursor(CursorType.Movement);
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

