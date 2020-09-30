using System.Collections.Generic;
using System.Linq;
using Gameplay.Projectile;
using UnityEngine;

namespace Gameplay.Tower
{
    public abstract class AbstractTower<T> : MonoBehaviour where T : AbstractProjectile
    {
        [SerializeField] protected float shootInterval = 0.5f;
        [SerializeField] protected float attackRange = 4f;
        [SerializeField] protected T projectilePrefab;
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected Transform projectilesRoot;

        private float lastShotTime;
        private HashSet<AbstractProjectile> aliveProjectiles =  new HashSet<AbstractProjectile>();
        private HashSet<AbstractProjectile> deadProjectiles = new HashSet<AbstractProjectile>();

        protected bool CanShoot
        {
            get { return lastShotTime + shootInterval < Time.time; }
        }

        void Start()
        {
            lastShotTime = -shootInterval;
        }

        void Update()
        {
            if (CanShoot)
            {
                var monster = GetMonster();
                if(monster!=null)
                    Shoot(monster);
            }
        }

        protected abstract Monster GetMonster();

        protected virtual void Shoot(Monster monster)
        {
            if (!CanShoot)
                return;
            GetProjectile().Setup(monster);
            UpdateShootingTime();
        }

        protected void UpdateShootingTime()
        {
            lastShotTime = Time.time;
        }

        protected T GetProjectile()
        {
            T projectile;
            if (deadProjectiles.Count > 0)
            {
                projectile = deadProjectiles.FirstOrDefault() as T;
                deadProjectiles.Remove(projectile);
                projectile.transform.position = shootPoint.transform.position;
                projectile.transform.rotation = Quaternion.identity;
            }
            else
            {
                projectile = Instantiate(projectilePrefab, shootPoint.transform.position, Quaternion.identity, projectilesRoot);
                projectile.Collapsed += delegate(AbstractProjectile proj)
                {
                    aliveProjectiles.Remove(proj);
                    deadProjectiles.Add(proj);
                };
                projectile.name = projectilePrefab.name +  " : " +  projectile.gameObject.GetInstanceID();
            }
            projectile.gameObject.SetActive(true);
            aliveProjectiles.Add(projectile);
            return projectile;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(this.transform.position, attackRange);
        }
#endif
    }
}

