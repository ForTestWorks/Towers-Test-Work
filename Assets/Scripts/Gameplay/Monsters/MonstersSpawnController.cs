using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class MonstersSpawnController : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private Transform spawner;
        [SerializeField] private Transform target;
        [SerializeField] private Monster monsterPrefab;

        private static readonly List<Monster> aliveMonsters = new List<Monster>();
        private static readonly List<Monster> deadMonsters = new List<Monster>();

        void Start()
        {
            StartCoroutine(Spawning());
        }

        IEnumerator Spawning()
        {
            while (true)
            {
                SpawnMonster();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        public static List<Monster> GetTargets()
        {
            return aliveMonsters;
        }

        public static List<Monster> GetTargets(Vector3 source, float attackRange)
        {
            return aliveMonsters.Where(t => Vector3.Distance(source, t.transform.position) < attackRange).ToList();
        }

        public static Monster GetNearestTargets(Vector3 source, float attackRange)
        {
            return GetTargets(source,attackRange).OrderBy(t => Vector3.Distance(source, t.transform.position)).FirstOrDefault();
        }

        public static Monster GetNearestTargets(Vector3 source)
        {
            var targets = GetTargets();
            return targets.Count <= 0 ? null : targets.OrderBy(t => Vector3.Distance(source, t.transform.position)).FirstOrDefault();
        }

        private void SpawnMonster()
        {
            Monster monster;
            if (deadMonsters.Count > 0)
            {
                monster = deadMonsters[0];
                deadMonsters.RemoveAt(0);
                monster.gameObject.transform.position = spawner.transform.position;
            }
            else
            {
                monster = Instantiate(monsterPrefab, spawner.transform.position, Quaternion.identity, spawner.transform);
                monster.Dead += delegate(Monster monst)
                {
                    aliveMonsters.Remove(monst);
                    deadMonsters.Add(monst);
                };
                monster.name = "Monster";
            }

            monster.gameObject.SetActive(true);
            monster.SetupMonster(target);
            aliveMonsters.Add(monster);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(spawner.transform.position, 2);
            if (target != null)
            {
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                Gizmos.DrawSphere(target.transform.position, 2);
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.DrawLine(target.transform.position, spawner.transform.position);
            }
        }
#endif
    }
}