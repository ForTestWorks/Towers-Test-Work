using System.Linq;
using Gameplay.Projectile;
using UnityEngine;

namespace Gameplay.Tower
{
    public class CannonTower : AbstractTower<CannonProjectile>
    {
        [SerializeField] private float WeaponSpeedRotation = 2;
        [SerializeField] private Transform WeaponRotateY;
        [SerializeField] private Transform WeaponRotateX;

        private Monster target;
        private Vector3 point;
        private Vector3 shootVelocity;

        protected override Monster GetMonster()
        {
            if (target==null || target.IsDead || Vector3.Distance(shootPoint.transform.position, target.transform.position) > attackRange)
                return MonstersSpawnController.GetTargets(this.transform.position, attackRange).LastOrDefault();
            return target;
        }

        void Update()
        {
            if(!CanShoot)
                return;
            target = GetMonster();
            if (target != null && PrepareTower(target))
                Shoot(target);
        }

        private bool PrepareTower(Monster monster)
        {
            point = monster.PositionAfterSeconds(1);

            point.y += 0.4f;

            shootVelocity = CalculateTrajectoryVelocity(shootPoint.position, point, 1);
            var targetForLook = shootPoint.position + shootVelocity.normalized * 2;

            var targetRotation = Quaternion.LookRotation(targetForLook - shootPoint.position);

            var targetRotationX = targetRotation;
            targetRotationX.y = WeaponRotateX.rotation.y;
            targetRotationX.z = WeaponRotateX.rotation.z;

            var targetRotationY = targetRotation;
            targetRotationY.x = WeaponRotateY.rotation.x;
            targetRotationY.z = WeaponRotateY.rotation.z;

            WeaponRotateX.rotation = Quaternion.Slerp(WeaponRotateX.rotation, targetRotationX, WeaponSpeedRotation * Time.deltaTime);
            WeaponRotateY.rotation = Quaternion.Slerp(WeaponRotateY.rotation, targetRotationY, WeaponSpeedRotation * Time.deltaTime);

            return Quaternion.Angle(shootPoint.transform.rotation, targetRotation) < 5;
        }

        Vector3 CalculateTrajectoryVelocity(Vector3 source, Vector3 targ, float t)
        {
            Vector3 speed = Vector3.zero;
            speed.x = (targ.x - source.x) / t;
            speed.z = (targ.z - source.z) / t;
            speed.y = (targ.y - source.y - 0.5f * Physics.gravity.y * t * t) / t;
            return speed;
        }

        protected override void Shoot(Monster monster)
        {
            GetProjectile().Setup(monster, shootVelocity, ForceMode.Impulse);
            UpdateShootingTime();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 1, 0.8f);
            Gizmos.DrawSphere(point, 0.5f);
            Gizmos.DrawLine(shootPoint.transform.position, point);
        }
#endif
    }
}