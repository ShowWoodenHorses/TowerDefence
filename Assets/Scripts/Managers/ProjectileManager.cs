using Assets.Scripts.Configs;
using Assets.Scripts.Data;
using Assets.Scripts.Pool;
using Assets.Scripts.Visuals;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Managers
{
    public class ProjectileManager : MonoBehaviour
    {
        public ProjectileData[] projectiles;
        public ProjectileVisual[] visuals;

        public int count;

        private EnemyManager enemyManager;
        private ProjectilePool projectilePool;
        private GameEvents gameEvents;

        // радиус попадания для dumb-снаряда
        private const float hitRadius = 0.5f;
        private const float hitRadiusSqr = hitRadius * hitRadius;

        public void Init(
            EnemyManager enemyManager,
            GameEvents events,
            ProjectilePool pool)
        {
            this.enemyManager = enemyManager;
            this.gameEvents = events;
            this.projectilePool = pool;

            projectiles = new ProjectileData[1000];
            visuals = new ProjectileVisual[1000];
        }

        void Update()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < count; i++)
            {
                ref ProjectileData p = ref projectiles[i];

                float move = p.speed * dt;

                if (p.type == ProjectileType.Dumb)
                {
                    if (UpdateDumb(ref p, move, i))
                    {
                        i--;
                    }
                }
                else
                {
                    if (UpdateHoming(ref p, move, i))
                    {
                        i--;
                    }
                }
            }
        }

        // ===============================
        // DUMB PROJECTILE
        // ===============================

        bool UpdateDumb(ref ProjectileData p, float move, int index)
        {
            if (move >= p.remainingDistance)
            {
                TryHitAtPosition(p.targetPosition, p.damage);
                Remove(index);
                return true;
            }

            p.position += p.direction * move;
            p.remainingDistance -= move;

            visuals[index].transform.position = p.position;

            return false;
        }

        void TryHitAtPosition(Vector3 pos, int damage)
        {
            for (int i = 0; i < enemyManager.countEnemies; i++)
            {
                ref EnemyData e = ref enemyManager.enemies[i];

                if ((e.position - pos).sqrMagnitude <= hitRadiusSqr)
                {
                    e.hp -= damage;
                    return;
                }
            }
        }

        // ===============================
        // HOMING PROJECTILE
        // ===============================

        bool UpdateHoming(ref ProjectileData p, float move, int index)
        {
            // индекс вне диапазона
            if (p.targetIndex >= enemyManager.countEnemies)
            {
                Remove(index);
                return true;
            }

            ref EnemyData target = ref enemyManager.enemies[p.targetIndex];

            // ❗ ВЕРСИЯ НЕ СОВПАДАЕТ — цель умерла или слот переиспользован
            if (target.version != p.targetVersion || target.hp <= 0)
            {
                Remove(index);
                return true;
            }

            Vector3 toTarget = target.position - p.position;
            float sqrDist = toTarget.sqrMagnitude;

            if (sqrDist <= move * move)
            {
                target.hp -= p.damage;
                Remove(index);
                return true;
            }

            Vector3 dir = toTarget.normalized;
            p.position += dir * move;

            visuals[index].transform.position = p.position;
            visuals[index].transform.LookAt(target.position);

            return false;
        }

        // ===============================
        // SPAWN METHODS
        // ===============================

        public void Spawn(
            ProjectileType type,
            Vector3 start,
            int enemyIndex,
            float speed,
            int damage)
        {
            switch (type)
            {
                case ProjectileType.Homing:
                    SpawnHoming(ProjectileType.Homing, start, enemyIndex, speed, damage);
                    break;
                case ProjectileType.Dumb:
                    SpawnDumb(ProjectileType.Dumb, start, enemyIndex, speed, damage);
                    break;
            }
        }

        private void SpawnDumb(
            ProjectileType type,
            Vector3 start,
            int enemyIndex,
            float speed,
            int damage)
        {
            if (count >= projectiles.Length)
                return;

            if (enemyIndex >= enemyManager.countEnemies)
                return;

            ref EnemyData target = ref enemyManager.enemies[enemyIndex];

            Vector3 targetPos = target.position;
            Vector3 dir = (targetPos - start).normalized;
            float dist = Vector3.Distance(start, targetPos);

            projectiles[count] = new ProjectileData
            {
                position = start,
                direction = dir,
                targetPosition = targetPos,
                remainingDistance = dist,
                speed = speed,
                damage = damage,
                type = ProjectileType.Dumb
            };

            visuals[count] = projectilePool.Spawn(type, start);

            count++;
        }

        private void SpawnHoming(
            ProjectileType type,
            Vector3 start,
            int enemyIndex,
            float speed,
            int damage)
        {
            if (count >= projectiles.Length)
                return;

            if (enemyIndex >= enemyManager.countEnemies)
                return;

            ref EnemyData target = ref enemyManager.enemies[enemyIndex];

            projectiles[count] = new ProjectileData
            {
                position = start,
                speed = speed,
                damage = damage,
                targetIndex = enemyIndex,
                targetVersion = target.version,
                type = ProjectileType.Homing
            };

            visuals[count] = projectilePool.Spawn(type, start);

            count++;
        }

        // ===============================
        // REMOVE (SWAP BACK)
        // ===============================

        void Remove(int index)
        {
            int last = count - 1;

            gameEvents.OnProjectileUsed.OnRaised?.Invoke(visuals[index]);

            if (index != last)
            {
                projectiles[index] = projectiles[last];
                visuals[index] = visuals[last];
            }

            count--;
        }
    }
}