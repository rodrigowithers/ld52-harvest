using System;
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

        [SerializeField] private PlayerController _playerController;
        [SerializeField] private SproutController _sproutController;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _playerController.Initialize();
            _sproutController.Initialize();
            
            _sproutController.AddSprout(0, new Vector2(0, -2f));
        }

        private void Update()
        {
            _playerController.Tick();
            _sproutController.Tick();
        }
    }
}
