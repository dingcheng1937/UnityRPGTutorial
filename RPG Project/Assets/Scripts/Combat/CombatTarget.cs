using UnityEngine;
namespace RPG.Combat
{
    public class CombatTarget : MonoBehaviour 
    {
        public bool CanAttack()
        {
            if (GetComponent<Health>().IsDead())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
