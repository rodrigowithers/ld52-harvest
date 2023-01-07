using Code.Player;
using UnityEngine;

namespace Code.Main
{
    public class GameController : MonoBehaviour
    {
        public enum GameState
        {
            Loading,
            Playing,
            Paused,
        }

        public struct GameData
        {
        
        }

        [SerializeField] private PlayerController PlayerController;

        private void Start()
        {
            PlayerController.Initialize();
        }

        private void Update()
        {
            PlayerController.Tick();
        }
    }
}
