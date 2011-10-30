using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    class Ember : Unit
    {
        Texture2D _projectile;

        public Ember(Vector2 position, Animation animationMove, Animation animationAttack, UnitTeam team, Arena arena, Texture2D projectile, Game1 game, Random rand) :
            base(position, team, arena, animationMove, animationAttack, game, rand, "particle_03")
        {
            MaxHP = 50;
            _moveSpeed = 200;
            _attackSpeed = 5.0f;
            _attackDamage = 200;
            _attackRange = 300;
            _followRange = 500;
            _projectile = projectile;
        }

        protected override void OnAttack(Target target)
        {
            _arena.AddProjectile(new Projectile(this, target, _projectile));
            base.OnAttack(target);
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }
    }
}
