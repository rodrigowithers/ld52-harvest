using deJect;
using UnityEngine;
using Code.Weapons;
using UnityEngine.InputSystem;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _hand;
        [SerializeField] private float _speed;

        [SerializeField] private LayerMask _sproutsMask;
        [SerializeField] private LayerMask _weaponMask;

        [Header("Runtime")] 
        [SerializeField] private Weapon _currentWeapon;

        private bool HasWeapon => _currentWeapon != null;

        private float _sproutTick;

        public void Initialize()
        {
            Container.Bind<PlayerController>().To(this);
        }

        public void Tick()
        {
            var keyboard = Keyboard.current;

            Move(keyboard);
            HandleAttack(keyboard);
            HandleSprouts();
        }

        private void HandleSprouts()
        {
            if (HasWeapon) return;

            _sproutTick += Time.deltaTime;
            if (_sproutTick >= 0.5f)
            {
                _sproutTick = 0.0f;

                var hits = Physics2D.OverlapCircleAll(transform.position, 1.0f, _sproutsMask);
                foreach (var hit in hits)
                {
                    hit.GetComponent<WeaponSprout>().IncreaseStage();
                }
            }
        }

        private void HandleAttack(Keyboard keyboard)
        {
            if (keyboard[Key.Z].wasPressedThisFrame)
            {
                if (!HasWeapon) return;

                if (!_currentWeapon.CanAttack) return;
                if (_currentWeapon.IsAttacking) return;

                _currentWeapon.Attack();
            }

            if (keyboard[Key.X].wasPressedThisFrame)
            {
                if (HasWeapon) return;

                var hit = Physics2D.OverlapCircle(transform.position, 1.0f, _weaponMask);
                if (hit != null)
                {
                    var weapon = hit.GetComponent<Weapon>();

                    weapon.Grab(_hand);
                }
            }
        }

        private void Move(Keyboard keyboard)
        {
            Vector2 movement = Vector2.zero;

            if (keyboard[Key.LeftArrow].isPressed)
            {
                movement.x = -1;
            }
            else if (keyboard[Key.RightArrow].isPressed)
            {
                movement.x = 1;
            }

            if (keyboard[Key.UpArrow].isPressed)
            {
                movement.y = 1;
            }
            else if (keyboard[Key.DownArrow].isPressed)
            {
                movement.y = -1;
            }

            movement.Normalize();
            _rigidbody.velocity = movement * (_speed * Time.deltaTime);
        }
    }
}