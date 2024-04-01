using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        // [SerializeField] float giveupDistance = 10f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        int curWaypointIndex = 0;
        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        LazyValue<Vector3> guardLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedWaypoint = Mathf.Infinity;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] float DwellingTime = 2f;
        
        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health =  GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardLocation = new LazyValue<Vector3>(GetGuardPosition);
        }
        private void Start()
        {
            guardLocation.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
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
                    
                    AttackBehaviour();

                }
                else if (timeSinceLastSawPlayer < suspicionTime)
                {
                    SuspicionAction();
                }
                else
                {
                    // fighter.Cancel();
                    PatrolBehaviour();
                }
                UpdateTimer();

            }
        }

        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardLocation.value);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation.value;
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedWaypoint = 0f;
                    CycleWaypoint();
                    
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedWaypoint > DwellingTime)
            {
                mover.StartMoveAction(nextPosition);
            }
        }

        private void CycleWaypoint()
        {
            curWaypointIndex = patrolPath.GetNextIndex(curWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint <= waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(curWaypointIndex);
        }

        private void SuspicionAction()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
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

        // private void OnDrawGizmos() {

        // }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
