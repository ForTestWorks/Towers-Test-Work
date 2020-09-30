using UnityEngine;

namespace Gameplay.Projectile
{
    public class GuidedProjectile : AbstractProjectile
    {
        [SerializeField] private float speed = 5f;
        public float Speed
        {
            get { return speed; }
        }
        protected bool acting { get; private set; }

        protected void Move()
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        protected override void TargetDead(Monster targ)
        {
            Collapse();
        }

        protected override void Collapse()
        {
            acting = false;
            base.Collapse();
        }

        public override void Setup(Monster _target)
        {
            acting = true;
            base.Setup(_target);
        }

        private void Update()
        {
            if (acting)
                Move();
        }

    }
}