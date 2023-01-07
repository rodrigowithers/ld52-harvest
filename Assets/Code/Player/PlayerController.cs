using deJect;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D Rigidbody;
        
        public void Initialize()
        {
            Container.Bind<PlayerController>().To(this);
        }

        public void Tick()
        {
            var keyboard = Keyboard.current;

            Move(keyboard);
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
            Rigidbody.velocity = movement;
        }
    }
}
