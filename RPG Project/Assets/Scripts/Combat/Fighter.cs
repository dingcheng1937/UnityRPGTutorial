using RPG.Core;
using RPG.Movement;
using UnityEngine;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f;   
        [SerializeField] float timeBetweenAttacks = 1f;
        private bool isAbleToAttack = true;
        Transform target;
        CombatTarget curCombatTarget;
        float timeSinceLastAttack = 0;
        private void Update()
        {       

            if (target != null)
            {
                bool isInRange = Vector3.Distance(transform.position, target.position) < attackRange;
                if (!isInRange)
                {
                    GetComponent<Mover>().MoveTo(target.position);
                }
                else
                {
                    GetComponent<Mover>().Cancel();

                    AttackBehaviour();

                    
                }
            }
        }

        private void AttackBehaviour()
        {
            if (Time.time - timeSinceLastAttack >= timeBetweenAttacks)
            {
                isAbleToAttack = true;
            }
            if (isAbleToAttack)
            {
                GetComponent<Animator>().SetTrigger("AttackTrigger");
                isAbleToAttack = false;
                timeSinceLastAttack = Time.time;
                curCombatTarget.GetComponent<Health>().TakeDamage(30);
            }
            
        }

        public void Attack(CombatTarget combatTarget)
        {
            print("Take that!! "+combatTarget);
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
            curCombatTarget = combatTarget;
            
        }    

        public void Cancel()
        {
            target = null;
        }
    }
}
