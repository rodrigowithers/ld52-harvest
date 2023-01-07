using UnityEngine;
using Code.Weapons;
using System.Collections.Generic;
using deJect;

namespace Code.Main
{
    public class SproutController : MonoBehaviour
    {
        [SerializeField] private List<WeaponSprout> _possibleSprouts;
        
        private List<WeaponSprout> _sprouts;

        public void AddSprout(int type, Vector2 position)
        {
            var sprout = _possibleSprouts[type];

            var instance = Instantiate(sprout, position, Quaternion.identity, null);
            _sprouts.Add(instance);
        }
        
        public void Initialize()
        {
            Container.Bind<SproutController>().To(this);
            
            _sprouts = new List<WeaponSprout>();
        }

        public void Tick()
        {
            for (var i = 0; i < _sprouts.Count; i++)
            {
                _sprouts[i].Tick();
            }
        }

        public void RemoveSprout(WeaponSprout sprout)
        {
            _sprouts.Remove(sprout);
        }
    }
}