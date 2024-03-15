using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        // [SerializeField] Transform target;
        private NavMeshAgent navMeshAgent;
        Health health;

        // public Animator animator;
        public float maxMoveSpeed = 20f;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent> ();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            
            // if (Input.GetMouseButton(0))
            // {
            //     MoveToCursor();
                
            // }
            
            // if (target != null && navMeshAgent != null)
            // {
            //     navMeshAgent.SetDestination(target.position);
            //     // Vector3 direction = target.position - transform.position;

            //     // if (direction.magnitude > 0.1f)
            //     // {
            //     //     transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
            //     // }
            // }
            navMeshAgent.enabled = !health.IsDead();
        }

        private void LateUpdate() {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            // float moveSpeed = Mathf.Abs(navMeshAgent.velocity.z);
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            // Debug.Log("speed: " + speed);
            GetComponent<Animator>().SetFloat("MoveSpeed", Mathf.Abs(speed));
        }

        public object CaptureState()
        {
            throw new System.NotImplementedException();
        }

        public void RestoreState(object state)
        {
            throw new System.NotImplementedException();
        }
    }
}
