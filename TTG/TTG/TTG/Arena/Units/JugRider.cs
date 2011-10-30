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
    public class JugRider : Unit
    {
         public JugRider(Vector2 position, Animation animationMove, Animation animationAttack, UnitTeam team, Arena arena) : 
            base(position, team, arena, animationMove, animationAttack)
        {
            MaxHP = 30;
            _moveSpeed = 50;
            _attackSpeed = 0.3f;
            _attackDamage = 50;
            _attackRange = 50;
            _followRange = 500;
        }

         public override void OnDeath(DeathEmitter de)
         {
             base.OnDeath(de);
         }
    }
}
