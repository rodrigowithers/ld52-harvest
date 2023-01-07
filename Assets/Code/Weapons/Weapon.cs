using System.Collections;
using UnityEngine;

namespace Code.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float _attackRate = 1.0f;
        [SerializeField] private float _swingSpeed = 2.0f;
        
        public bool IsAttacking { get; set; }
        public bool CanAttack { get; private set; }

        public void Grab(Transform hand)
        {
            transform.parent = hand;
            transform.localPosition = Vector3.zero;
        }

        public void Throw(Vector2 direction)
        {
            Release();
        }
        
        public void Attack()
        {
            IsAttacking = true;
            CanAttack = false;

            StartCoroutine(AttackRoutine());
        }

        private void Release()
        {
            
        }
        
        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(_attackRate);
            CanAttack = true;
        }

        private IEnumerator AttackRoutine()
        {
            // Get Start Angle
            // Rotate to Final Angle, damaging things in the path

            IsAttacking = false;
            StartCoroutine(Cooldown());

            yield return null;
        }
    }
}