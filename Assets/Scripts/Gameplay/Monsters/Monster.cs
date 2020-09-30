using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private float attackRange = 0.3f;
        [SerializeField] private float speed = 0.1f;
        [SerializeField] private int maxHp = 30;

        private bool acting;
        private Transform target;
        private int currentHp;

        public event Action<Monster> Dead = delegate(Monster monster) {  };

        #region API

        public bool IsDead
        {
            get { return currentHp <= 0; }
        }

        public float Speed
        {
            get { return speed; }
        }

        public Vector3 TargetPosition
        {
            get { return target.position; }
        }

        public Vector3 MovingVector
        {
            get { return (target.position - this.transform.position).normalized; }
        }

        public Vector3 PositionAfterSeconds(float seconds)
        {
            Vector3 newMonsterPosition = transform.position;
            newMonsterPosition += MovingVector * Speed * seconds;
            return newMonsterPosition;
        }

        public void SetupMonster(Transform _target)
        {
            target = _target;
            currentHp = maxHp;
            acting = true;
        }

        public void ReciveDamage(int damage)
        {
            if(IsDead)
                return;
            currentHp -= damage;
            if (currentHp <= 0)
                Die();
        }


        #endregion

        #region Acting

        void Update()
        {
            if (!acting)
                return;

            if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
                Die();
            else
                Move();
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        private void Die()
        {
            target = null;
            acting = false;
            currentHp = 0;
            Dead.Invoke(this);
            this.gameObject.SetActive(false);
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.6f);
            Gizmos.DrawSphere(this.transform.position, attackRange);
        }
#endif
    }
}