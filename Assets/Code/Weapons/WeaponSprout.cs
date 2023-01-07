using UnityEngine;
using System.Collections.Generic;
using Code.Main;
using deJect;

namespace Code.Weapons
{
    public class WeaponSprout : MonoBehaviour
    {
        [Header("Config")] 
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Weapon _product;
       
        [Space]
        [SerializeField] private List<Sprite> _stages;
        
        private int _currentStage;

        public int CurrentStage => _currentStage;

        public void IncreaseStage()
        {
            _currentStage++;

            if (_stages.Count > _currentStage)
                _renderer.sprite = _stages[_currentStage];
        }

        public void Tick()
        {
            if (_currentStage > _stages.Count)
            {
                Container.Resolve<SproutController>().RemoveSprout(this);
                Destroy(this.gameObject);
                
                _product.transform.parent = null;
                _product.gameObject.SetActive(true);
                _product.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
    }
}