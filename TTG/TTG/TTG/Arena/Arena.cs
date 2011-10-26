using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public enum UnitEnum
    {
        Marine = 0,
    };

    public enum UnitTeam
    {
        Player1,
        Player2,
    };

    public enum UnitType
    {
        Ground,
        Air
    };

    public enum TargetUnitType
    {
        Any,
        GroundOnly,
        AirOnly,
    };

    public class Arena
    {
        private List<Unit> _units;
        private Animation[] _animationsAttack;
        private Animation[] _animationsMove;

        public Arena()
        {
            _units = new List<Unit>();

            _animationsAttack = new Animation[1];
            _animationsMove = new Animation[1];
        }

        public void LoadContent(ContentManager content)
        {
            Texture2D marineTexture = content.Load<Texture2D>("marine");
            _animationsAttack[(int)UnitEnum.Marine] = new Animation(marineTexture, 3, 1, 0, 3, 0.1f, true);
            _animationsMove[(int)UnitEnum.Marine] = new Animation(marineTexture, 3, 1, 0, 1, 0.1f, true);

        }

        public void Update(GameTime gameTime)
        {
            foreach (Unit unit in _units)
            {
                unit.Update(gameTime);
            }

            for (int i = 0; i < _units.Count; )
            {
                if (_units[i].IsDead())
                    _units.RemoveAt(i);
                else
                    ++i;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Unit unit in _units)
            {
                unit.Draw(spritebatch);
            }
        }

        public Vector2 GetSpawnPosition(UnitTeam team)
        {
            if (team == UnitTeam.Player1)
                return Vector2.Zero;
            else
                return new Vector2(800, 60);
        }

        public void AddUnit(UnitEnum unit, UnitTeam team)
        {
            if (unit == UnitEnum.Marine)
            {
                _units.Add(new Marine(GetSpawnPosition(team), _animationsMove[(int)UnitEnum.Marine], _animationsAttack[(int)UnitEnum.Marine], team, this));
            }
        }

        public Unit AcquireTarget(Unit attacker)
        {
            Unit target = null;
            float closest = -1;

            foreach (Unit unit in _units)
            {
                if (attacker.CanTarget(unit))
                {
                    Vector2 direction = unit.Position - attacker.Position;
                    float distance = direction.Length();

                    if (distance < attacker.FollowRange)
                    {
                        if (closest < 0 || distance < closest)
                        {
                            closest = distance;
                            target = unit;
                        }
                    }
                }
            }

            return target;
        }
    }
}
