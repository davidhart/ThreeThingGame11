﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public abstract class Target
    {
        private int _hitPoints;

        public int HP
        {
            get
            {
                return _hitPoints;
            }
        }

        private int _maxHP;
        public int MaxHP
        {
            get
            {
                return _maxHP;
            }
            set
            {
                _maxHP = value;
                _hitPoints = _maxHP;
            }
        }

        protected UnitType _type;
        public UnitType Type
        {
            get
            {
                return _type;
            }
        }

        protected UnitTeam _team;
        public UnitTeam Team
        {
            get
            {
                return _team;
            }
        }
        protected Vector2 _position;

        protected float _hitCooldown;
        protected const float _hitDuration = 0.1f;

        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        public Target(Vector2 position, UnitTeam team)
        {
            _position = position;
            _team = team;
            _hitCooldown = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            _hitCooldown = Math.Max(0, _hitCooldown - (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void TakeDamage(int damage)
        {
            _hitPoints = Math.Max(0, _hitPoints - damage);
            _hitCooldown = _hitDuration;
        }

        public abstract Rectangle GetRect();
        public abstract void Draw(SpriteBatch spritebatch);

        public void Kill()
        {
            _hitPoints = 0;
        }

        public bool IsDead()
        {
            return _hitPoints <= 0;
        }

        public Color GetHitColor()
        {
            return Color.Lerp(Color.White, Color.IndianRed, _hitCooldown / _hitDuration);
        }

        public abstract Vector2 GetMidPoint();

        public abstract void OnDeath(PEmitter de);
    }
}
