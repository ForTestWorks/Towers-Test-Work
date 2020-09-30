using System;
using UnityEngine;

namespace Gameplay.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        [Range(1,50)]
        [SerializeField] private int damage = 5;
        [Range(12, 20)]
        
        private bool idAlreadyCollapsed = false;
        protected Monster target { get; private set; }

        public event Action<AbstractProjectile> Collapsed = delegate(AbstractProjectile projectile) {  };

        protected abstract void TargetDead(Monster targ);

        protected virtual void Collapse()
        {
            if (target != null)
            {
                target.Dead -= TargetDead;
                target = null;
            }
            Collapsed(this);
            this.gameObject.SetActive(false);
        }

        public virtual void Setup(Monster _target)
        {
            target = _target;
            target.Dead += TargetDead;
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag.Equals("Monster"))
            {
                other.GetComponent<Monster>().ReciveDamage(damage);
            }
            Collapse();
        }
    }
}