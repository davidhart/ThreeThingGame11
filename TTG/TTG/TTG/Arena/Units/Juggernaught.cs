
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

namespace TTG.Units
{
    public class Juggernaught : Unit
    {
       SoundEffect _spawnSE; 
       public Juggernaught(Vector2 position, Animation animationMove, Animation animationAttack, UnitTeam team, Arena arena) : 
            base(position, team, arena, animationMove, animationAttack)
        {
            MaxHP = 150;
            _moveSpeed = 20;
            _attackSpeed = 0.3f;
            _attackDamage = 50;
            _attackRange = 50;
            _followRange = 500;
            _spawnSE = arena.JugRiderSpawnSE;
        }
       public override void OnDeath(DeathEmitter de)
       {
           _spawnSE.Play();
           _arena.AddUnit(UnitEnum.JugRider, UnitTeam.Player1);
           base.OnDeath(de);
       }
    }
}
