using RPG.Core;
using RPG.Movement;
using UnityEngine;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f;   
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 20f;
        private bool isAbleToAttack = true;
        Transform target;
        GameObject curCombatTarget;
        float timeSinceLastAttack = 0;
        private void Update()
        {       

            if (target != null )
            {
                if (!curCombatTarget.GetComponent<Health>().IsDead())
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
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target);
            if (Time.time - timeSinceLastAttack >= timeBetweenAttacks)
            {
                isAbleToAttack = true;
            }
            if (isAbleToAttack)
            {
                TriggerAttack();
                isAbleToAttack = false;
                timeSinceLastAttack = Time.time;
                curCombatTarget.GetComponent<Health>().TakeDamage(weaponDamage);
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("CancelAttack");
            GetComponent<Animator>().SetTrigger("AttackTrigger");
        }

        public void Attack(GameObject combatTarget)
        {
            print("Take that!! "+combatTarget);
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
            curCombatTarget = combatTarget;
            
        }    

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("AttackTrigger");
            GetComponent<Animator>().SetTrigger("CancelAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null){return false;}
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
    }
}
