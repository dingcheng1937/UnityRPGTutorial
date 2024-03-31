using System;
using RPG.Resources;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        // [SerializeField] string defaultWeaponName = "Unarmed";
        private bool isAbleToAttack = true;
        Transform target;
        public GameObject curCombatTarget;
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

        public GameObject GetTarget()
        {
            return curCombatTarget;
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
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, curCombatTarget.GetComponent<Health>(), gameObject, damage);
            }
            else
            {
                curCombatTarget.GetComponent<Health>().TakeDamage(gameObject, damage);
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
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
