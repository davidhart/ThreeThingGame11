
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
    public class Juggernaught : Unit
    {
        SoundEffect _spawnSE;
       public Juggernaught(Vector2 position, Animation animationMove, Animation animationAttack, UnitTeam team, Arena arena) : 
            base(position, team, arena, animationMove, animationAttack)
        {
            MaxHP = 1600;
            _moveSpeed = 20;
            _attackSpeed = 0.1f;
            _attackDamage = 4;
            _attackRange = 50;
            _followRange = 100;
          
        }

       protected override void OnAttack(Target target)
       {
           _arena.AddMarineShot(this, target, Color.Red, Color.Yellow);
           base.OnAttack(target);
       }
       public override void OnDeath(PEmitter de)
       {
           _spawnSE = _arena.JugRiderSpawnSE;
           _spawnSE.Play();
           Unit u = _arena.AddUnit(UnitEnum.Marine, UnitTeam.Player1);
           u.Position = _position;
           base.OnDeath(de);
       }
    }
}
