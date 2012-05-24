﻿
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
    
    public class JetpackMarine : Unit
    {
        public JetpackMarine(Vector2 position, Animation animationMove, Animation animationAttack, UnitTeam team, Arena arena) : 
            base(position, team, arena, animationMove, animationAttack)
        {
            MaxHP = 70;
            _moveSpeed = 50;
            _attackSpeed = 0.3f;
            _attackDamage = 9;
            _attackRange = 150;
            _followRange = 200;

            _type = UnitType.Air;
        }

        protected override void OnAttack(Target target)
        {
            _arena.AddMarineShot(this, target, Color.Blue,Color.White);
            base.OnAttack(target);
        }
    }
    
}