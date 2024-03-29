using System;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        private bool isAbleToAttack = true;
        Transform target;
        GameObject curCombatTarget;
        float timeSinceLastAttack = 0;
        Weapon currentWeapon = null;
        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator aniamtor = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, aniamtor);
        }

        private void Update()
        {       

            if (target != null )
            {
                if (!curCombatTarget.GetComponent<Health>().IsDead())
                {
                    bool isInRange = Vector3.Distance(transform.position, target.position) < currentWeapon.GetRange();
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

        private void  AttackBehaviour()
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
                // curCombatTarget.GetComponent<Health>().TakeDamage(currentWeapon.GetDamage());
            }

        }

        void Hit()
        {
            if (target == null)
            {
                return;
            }
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, curCombatTarget.GetComponent<Health>());
            }
            else
            {
                curCombatTarget.GetComponent<Health>().TakeDamage(currentWeapon.GetDamage());
            }
        }

        void Shoot()
        {
            Hit();
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
            GetComponent<Mover>().Cancel();
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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
