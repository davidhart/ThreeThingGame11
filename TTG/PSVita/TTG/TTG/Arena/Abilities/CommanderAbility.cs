using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class CommanderAbility
    {
        protected UnitTeam _team;
        public UnitTeam Team
        {
            get
            {
                return _team;
            }
        }

        protected Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }
        //similar to units attack damage
        //however the commander abilities maybe able to heal/buff
        protected short _effect;
        public short Effect
        {
            get
            {
                return _effect;
            }
        }

        protected short _areaOfEffect;
        public short AreaOfEffect
        {
            get
            {
                return _areaOfEffect;
            }
        }

        protected short _cost;
        public short Cost
        {
            get
            {
                return _cost;
            }
        }

        protected float _coolDown;
        public float Colldown
        {
            get
            {
                return _coolDown;
            }
        }

        protected List<Unit> _targets = new List<Unit>();
        public List<Unit> Targets
        {
            get
            {
                return _targets;
            }
        }

        private AnimationPlayer _animationPlayer;

        protected Arena _arena;
        protected Animation _animationUse;

        public CommanderAbility(
            Vector2 pos,
            UnitTeam team,
            Arena arena,
            Animation animationUse)
        {
            _position = pos;
            _team = team;
            _arena = arena;
            _animationUse = animationUse;
        }
    }
}
