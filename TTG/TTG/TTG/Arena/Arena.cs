using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
        private Random rand;

        int _p1Energy, _p2Energy, _maxEnergy;
        public int P1Energy
        {
            get
            {
                return _p1Energy;
            }
            set
            {
                _p1Energy = value;
            }
        }
        public int P2Energy
        {
            get
            {
                return _p2Energy;
            }
            set
            {
                _p2Energy = value;
            }
        }
        public int MaxEnergy
        {
            get
            {
                return _maxEnergy;
            }
            set
            {
                _maxEnergy = value;
            }
        }

        MarineShotBatch _marineShotBatch;

        public Arena()
        {
            P1Energy = 200;
            P2Energy = 200;
            _units = new List<Unit>();

            rand = new Random();

            _animationsAttack = new Animation[1];
            _animationsMove = new Animation[1];
        }

        Music _bgm;

        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            Texture2D marineTexture = content.Load<Texture2D>("marine");
            _animationsAttack[(int)UnitEnum.Marine] = new Animation(marineTexture, 3, 1, 0, 3, 0.1f, true);
            _animationsMove[(int)UnitEnum.Marine] = new Animation(marineTexture, 3, 1, 0, 1, 0.1f, true);
            _bgm = new Music(content.Load<SoundEffect>("Ropocalypse 2"),true);
            _marineShotBatch = new MarineShotBatch(device);
        }

        public void Update(GameTime gameTime)
        {
            _bgm.Play();
            foreach (Unit unit in _units)
            {
                unit.Update(gameTime);

                if (unit.Position.X > 1280 + 100)
                {
                    unit.Kill();
                }

                if (unit.Position.X < -100)
                {
                    unit.Kill();
                }
            }

            for (int i = 0; i < _units.Count; )
            {
                if (_units[i].IsDead())
                {
                    _units.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }

            _marineShotBatch.Update(gameTime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Unit unit in _units)
            {
                unit.Draw(spritebatch);
            }

            _marineShotBatch.Draw();
        }

        public Vector2 GetSpawnPosition(UnitTeam team)
        {
            float y = rand.Next(400);

            if (team == UnitTeam.Player1)
            {
                return new Vector2(0, y);
            }
            else
            {
                return new Vector2(800, y);
            }
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

        public void AddMarineShot(Unit attacker, Unit target)
        {
            _marineShotBatch.AddShot(attacker, target);
        }
    }
}
