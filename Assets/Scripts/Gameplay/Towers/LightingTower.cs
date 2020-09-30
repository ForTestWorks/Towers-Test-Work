using Gameplay.Projectile;

namespace Gameplay.Tower
{
    public class LightingTower : AbstractTower<GuidedProjectile>
    {
        protected override Monster GetMonster()
        {
            foreach (var monster in MonstersSpawnController.GetTargets(this.transform.position, attackRange))
                return monster;
            return null;
        }
    }
}