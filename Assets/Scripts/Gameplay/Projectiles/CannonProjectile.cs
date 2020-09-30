using UnityEngine;

namespace Gameplay.Projectile
{
    public class CannonProjectile : AbstractProjectile
    {
        [SerializeField] private Rigidbody rigidbody;

        private Rigidbody Rigidbody
        {
            get
            {
                if (rigidbody == null)
                    rigidbody = this.GetComponent<Rigidbody>();
                return rigidbody;
            }
        }

        public void Setup(Monster _target, Vector3 velocity)
        {
            base.Setup(_target);
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = velocity;
        }

        public void Setup(Monster _target, Vector3 force, ForceMode forceMode)
        {
            base.Setup(_target);
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.AddForce(force,forceMode);
        }


        protected override void TargetDead(Monster targ)
        {
            Collapse();
        }
    }
}