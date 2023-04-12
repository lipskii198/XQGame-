using _game.Scripts.Enemies.Core;

namespace _game.Scripts.Enemies
{
    public class AntEnemy : EnemyBase
    {
        protected override void Awake()
        {
            base.Awake();
            isFollowingPlayer = true;
        }
    }
}