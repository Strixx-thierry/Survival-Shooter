using SurvivalShooter.Core;
using SurvivalShooter.Pooling;
using UnityEngine;

namespace SurvivalShooter.Enemies
{
    /// <summary>Factory pattern for enemies.</summary>
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] MeleeEnemy meleePrefab;
        [SerializeField] ShooterEnemy shooterPrefab;
        [SerializeField] ProjectilePool enemyBulletPool;

        public EnemyBase Create(EnemyType type, Vector3 position, Transform player, DifficultySettings difficulty)
        {
            EnemyBase enemy;
            switch (type)
            {
                case EnemyType.Shooter:
                    var shooter = Instantiate(shooterPrefab, position, Quaternion.identity);
                    shooter.SetPool(enemyBulletPool);
                    enemy = shooter;
                    break;
                default:
                    enemy = Instantiate(meleePrefab, position, Quaternion.identity);
                    break;
            }

            enemy.Init(player, difficulty);
            return enemy;
        }
    }
}
