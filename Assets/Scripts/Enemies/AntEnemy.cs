using Enemies.Core;
using UnityEngine;

namespace Enemies
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