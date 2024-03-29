using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        // [SerializeField] float maxHealth = 100f;
        public bool IsDead()
        {
            if (health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print("health: " + health);
            Die();
        }

        private void Die()
        {
            if (health <= 0)
            {
                GetComponent<Animator>().SetTrigger("isDead");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            Die();
        }

    }
}
