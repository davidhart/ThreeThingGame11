using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TTG
{
    public class Marine : Unit
    {
        public Marine(Vector2 position, Animation animationMove, Animation animationAttack, UnitTeam team, Arena arena) : 
            base(position, team, arena, animationMove, animationAttack)
        {
            MaxHP = 50;
            _moveSpeed = 50;
            _attackSpeed = 0.3f;
            _attackDamage = 3;
            _attackRange = 200;
            _followRange = 500;
        }

        protected override void OnAttack(Target target)
        {
            _arena.AddMarineShot(this, target);
            base.OnAttack(target);
        }
    }
}
