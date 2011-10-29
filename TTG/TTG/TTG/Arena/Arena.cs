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
        private List<Target> _units;
        private Animation[] _animationsAttack;
        private Animation[] _animationsMove;
        private Base _p1Base;
        private Base _p2Base;

        private int _displayHeight;
        public int DisplayHeight
        {
            get
            {
                return _displayHeight;
            }
        }

        private int _displayWidth;
        public int DisplayWidth
        {
            get
            {
                return _displayWidth;
            }
        }

        private RenderTarget2D _renderTarget;

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
        GraphicsDevice _graphics;

        public Arena(int displayWidth, int displayHeight)
        {
            P1Energy = 200;
            P2Energy = 200;
            _units = new List<Target>();

            _displayWidth = displayWidth;
            _displayHeight = displayHeight;

            _animationsAttack = new Animation[1];
            _animationsMove = new Animation[1];
        }

        Music _bgm;

        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            _graphics = device;
            _renderTarget = new RenderTarget2D(device, _displayWidth, _displayHeight, false, device.PresentationParameters.BackBufferFormat,
                device.PresentationParameters.DepthStencilFormat);

            Texture2D marineTexture = content.Load<Texture2D>("marine");
            _animationsAttack[(int)UnitEnum.Marine] = new Animation(content.Load<Texture2D>("marine"), 3, 1, 0, 3, 0.1f, false);
            _animationsMove[(int)UnitEnum.Marine] = new Animation(content.Load<Texture2D>("marineWalk"), 4, 1, 0, 4, 0.15f, true);
            _bgm = new Music(content.Load<SoundEffect>("Ropocalypse 2"),true);
            _marineShotBatch = new MarineShotBatch(device, _renderTarget.Width, _renderTarget.Height);

            Texture2D baseTexture = content.Load<Texture2D>("marine");
            _p1Base = new Base(new Vector2(0, _displayHeight / 2 - baseTexture.Height / 2), UnitTeam.Player1, baseTexture);
            _p2Base = new Base(new Vector2(_displayWidth - baseTexture.Width, _displayHeight / 2 - baseTexture.Height / 2), UnitTeam.Player2, content.Load<Texture2D>("marine"));

            _units.Add(_p1Base);
            _units.Add(_p2Base);
        }

        public void Update(GameTime gameTime)
        {
            //_bgm.Play();
            foreach (Target target in _units)
            {
                target.Update(gameTime);

                if (target.Position.X > 1280 + 100)
                {
                    target.Kill();
                }

                if (target.Position.X < -100)
                {
                    target.Kill();
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
            _graphics.SetRenderTarget(_renderTarget);
            _graphics.Clear(Color.Black);

            spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            foreach (Target target in _units)
            {
                target.Draw(spritebatch);
            }

            spritebatch.End();

            _marineShotBatch.Draw();

            _graphics.SetRenderTarget(null);
        }

        public void DrawOntoScreen(SpriteBatch spritebatch, Vector2 position)
        {
            spritebatch.Begin();

            spritebatch.Draw(_renderTarget, position, Color.White);

            spritebatch.End();
        }

        public Vector2 GetSpawnPosition(UnitTeam team)
        {
            float y = 40 + Util.Rand(_displayHeight - 80);

            if (team == UnitTeam.Player1)
            {
                return new Vector2(40, y);
            }
            else
            {
                return new Vector2(_displayWidth-40, y);
            }
        }

        public void AddUnit(UnitEnum unit, UnitTeam team)
        {
            if (unit == UnitEnum.Marine)
            {
                _units.Add(new Marine(GetSpawnPosition(team), _animationsMove[(int)UnitEnum.Marine], _animationsAttack[(int)UnitEnum.Marine], team, this));
            }
        }

        public Target AcquireTarget(Unit attacker)
        {
            Target bestTarget = null;
            float closest = -1;

            foreach (Target target in _units)
            {
                if (attacker.CanTarget(target))
                {
                    Vector2 direction = target.Position - attacker.Position;
                    float distance = direction.Length();

                    if (distance < attacker.FollowRange)
                    {
                        if (closest < 0 || distance < closest)
                        {
                            closest = distance;
                            bestTarget = target;
                        }
                    }
                }
            }

            return bestTarget;
        }

        public void AddMarineShot(Unit attacker, Target target)
        {
            _marineShotBatch.AddShot(attacker, target);
        }
    }
}
