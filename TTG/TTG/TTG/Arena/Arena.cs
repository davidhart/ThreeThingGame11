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
        Ember = 1,
        Juggernaught = 2,
        Hydro = 4,
        Launcher = 5,
        UberEmber = 6,
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
        private Texture2D _emberProjectile;
        private Texture2D _emberProjectile2;
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

        private Vector2 _drawPosition;

        //private RenderTarget2D _renderTarget;

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
                if (_p1Energy >= _maxEnergy)
                {
                    _p1Energy = _maxEnergy;
                }
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

        MarineShotBatch _marineShotBatch, _hydroShotBatch;
        ProjectileBatch _projectileBatch;
        GraphicsDevice _graphics;
        Texture2D _battleBG;
        Rectangle _bgRect;

        //HealthBar _p1HealthBar;
        //HealthBar _p2HealthBar;

        Texture2D _base1Texture;
        Texture2D _base2Texture;

        SoundEffect _jugRiderSpawn;

        //Particle
        List<MarineDeathEmitter> _deadHumanEmitters;
        List<EnemyDeathEmitter> _deadEnemyEmitters;
        Random rand;

        public SoundEffect JugRiderSpawnSE
        {
            get
            {
                return _jugRiderSpawn;
            }
            set
            {
                _jugRiderSpawn = value;
            }
        }

        public Arena(int displayWidth, int displayHeight, Vector2 drawPosition)
        {
            _drawPosition = drawPosition;

            _maxEnergy = 2000;
            P1Energy = 200;
            P2Energy = 200;
            _units = new List<Target>();

            _displayWidth = displayWidth;
            _displayHeight = displayHeight;

            _animationsAttack = new Animation[10];
            _animationsMove = new Animation[10];

            _deadHumanEmitters = new List<MarineDeathEmitter>(20);
            _deadEnemyEmitters = new List<EnemyDeathEmitter>(20);
            rand = new Random();
        }

        Music _bgm;
        //BloomPostProcess _bloomEffect;

        public void LoadContent(ContentManager content, GraphicsDevice device)
        {
            _jugRiderSpawn = content.Load<SoundEffect>("JuggerWalker");
            _graphics = device;
            //_renderTarget = new RenderTarget2D(device, _displayWidth, _displayHeight, false, device.PresentationParameters.BackBufferFormat,
                //device.PresentationParameters.DepthStencilFormat);

            _animationsAttack[(int)UnitEnum.Marine] = new Animation(content.Load<Texture2D>("UnitsSpriteSheet"), 32, 32, 0, 3, 0.1f, false);
            _animationsMove[(int)UnitEnum.Marine] = new Animation(content.Load<Texture2D>("UnitsSpriteSheet"), 32, 32, 3, 4, 0.15f, true);

            _animationsAttack[(int)UnitEnum.Hydro] = new Animation(content.Load<Texture2D>("UnitsSpriteSheet"), 32, 32, 32+0, 3, 0.1f, false);
            _animationsMove[(int)UnitEnum.Hydro] = new Animation(content.Load<Texture2D>("UnitsSpriteSheet"), 32, 32, 32+3, 4, 0.15f, true);

            _animationsAttack[(int)UnitEnum.Juggernaught] = new Animation(content.Load<Texture2D>("UnitsSpriteSheet"), 16, 16, 16, 3, 0.1f, false);
            _animationsMove[(int)UnitEnum.Juggernaught] = new Animation(content.Load<Texture2D>("UnitsSpriteSheet"), 16, 16, 16, 1, 0.1f, true);

            _animationsAttack[(int)UnitEnum.Ember] = new Animation(content.Load<Texture2D>("alien"), 8, 1, 0, 4, 0.1f, false);
            _animationsMove[(int)UnitEnum.Ember] = new Animation(content.Load<Texture2D>("alien"), 8, 1, 4, 4, 0.15f, true);

            _animationsAttack[(int)UnitEnum.UberEmber] = new Animation(content.Load<Texture2D>("Ember2"), 6, 1, 3, 3, 0.1f, false);
            _animationsMove[(int)UnitEnum.UberEmber] = new Animation(content.Load<Texture2D>("Ember2"), 6, 1, 0, 3, 0.15f, true);

            _emberProjectile = content.Load<Texture2D>("ember_proj");
            _emberProjectile2 = content.Load <Texture2D>("ember_proj2");

            _battleBG = content.Load<Texture2D>("BattleBG");

            _bgRect = new Rectangle(0, 30, _battleBG.Width, _battleBG.Height);

            _bgm = new Music(content.Load<SoundEffect>("Ropocalypse 2"),true);
            _marineShotBatch = new MarineShotBatch(device, _displayWidth, _displayHeight, _drawPosition); 
            _hydroShotBatch = new MarineShotBatch(device, _displayWidth, _displayHeight, _drawPosition);
            _projectileBatch = new ProjectileBatch();

            _base1Texture = content.Load<Texture2D>("base");
            _base2Texture = content.Load<Texture2D>("VolcanoBase");

            Reset();

            //Particles
            List<string> textures = new List<string>();
            textures.Add("Particles/MarineArmour");
            textures.Add("Particles/smoke");

            for (int i = 0; i < 20; ++i)
            {
                MarineDeathEmitter temp = new MarineDeathEmitter(rand);
                temp.Initialize(content, textures[0]);
                _deadHumanEmitters.Add(temp);
            }

            for (int i = 0; i < 20; ++i)
            {
                EnemyDeathEmitter temp = new EnemyDeathEmitter(rand);
                temp.Initialize(content, textures[1]);
                _deadEnemyEmitters.Add(temp);
            }
        }

        public void Reset()
        {
            _units.Clear();

            _p1Base = new Base(new Vector2(0, _displayHeight / 2 - _base1Texture.Height / 2), UnitTeam.Player1, _base1Texture);
            _p2Base = new Base(new Vector2(_displayWidth - _base2Texture.Width, _displayHeight / 2 - _base2Texture.Height / 2), UnitTeam.Player2, _base2Texture);

            _units.Add(_p1Base);
            _units.Add(_p2Base);

            _projectileBatch.Clear();
            _marineShotBatch.Clear();
        }

        public void Update(GameTime gameTime)
        {
            _bgm.Play();
            foreach (Target target in _units)
            {
                target.Update(gameTime);

                if (target.Position.X > _displayWidth + 100)
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
                    if (_units[i].Team == UnitTeam.Player1)
                    {
                        for (int j = 0; j < _deadHumanEmitters.Count - 1; ++j)
                        {
                            if (!_deadHumanEmitters[j].Active)
                            {
                                _units[i].OnDeath(_deadHumanEmitters[j]);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < _deadEnemyEmitters.Count - 1; ++j)
                        {
                            if (!_deadEnemyEmitters[j].Active)
                            {
                                _units[i].OnDeath(_deadEnemyEmitters[j]);
                                break;
                            }
                        }

                    }
                    _units.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }

            for (int i = 0; i < _deadHumanEmitters.Count; ++i)
            {
                if(_deadHumanEmitters[i].Active)
                    _deadHumanEmitters[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            for (int i = 0; i < _deadEnemyEmitters.Count; ++i)
            {
                if (_deadEnemyEmitters[i].Active)
                    _deadEnemyEmitters[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            _marineShotBatch.Update(gameTime);
            _projectileBatch.Update(gameTime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Matrix offset = Matrix.CreateTranslation(new Vector3(_drawPosition, 0.0f));

            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, null, null, offset);
            spritebatch.Draw(_battleBG, _bgRect, Color.White);
            spritebatch.End();

            spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, RasterizerState.CullNone, null, offset);

            for (int i = 0; i < _units.Count; ++i)
            {
                _units[i].Draw(spritebatch);
            }

            _projectileBatch.Draw(spritebatch);

            spritebatch.End();

            _marineShotBatch.Draw();
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, null, offset);

            for (int i = 0; i < _deadHumanEmitters.Count; ++i)
            {
                if (_deadHumanEmitters[i].Active)
                    _deadHumanEmitters[i].Draw(spritebatch);
            }

            for (int i = 0; i < _deadEnemyEmitters.Count; ++i)
            {
                if (_deadEnemyEmitters[i].Active)
                    _deadEnemyEmitters[i].Draw(spritebatch);
            }
            spritebatch.End();
            _graphics.SetRenderTarget(null);
        }

        public Vector2 GetSpawnPosition(UnitTeam team, UnitEnum unit)
        {
            float y = 100 + Util.Rand(50);

            if (team == UnitTeam.Player1 && unit == UnitEnum.Hydro)
            {
                y = 40;
                return new Vector2(40, y);
            }

            if (team == UnitTeam.Player1)
            {
                return new Vector2(40, y);
            }
            else
            {
                return new Vector2(_displayWidth-40, y);
            }
        }

        public Unit AddUnit(UnitEnum unit, UnitTeam team)
        {
            Unit u = null;
            if (unit == UnitEnum.Marine)
            {
                u = new Marine(GetSpawnPosition(team, unit), _animationsMove[(int)UnitEnum.Marine], _animationsAttack[(int)UnitEnum.Marine], team, this);
            }
            else if (unit == UnitEnum.Juggernaught)
            {
                u = new Juggernaught(GetSpawnPosition(team, unit), _animationsMove[(int)UnitEnum.Juggernaught], _animationsAttack[(int)UnitEnum.Juggernaught], team, this);
            }
            else if (unit == UnitEnum.Hydro)
            {
                u = new Hydro(GetSpawnPosition(team, unit), _animationsMove[(int)UnitEnum.Hydro], _animationsAttack[(int)UnitEnum.Hydro], team, this);
            }
            else if (unit == UnitEnum.Launcher)
            {
            }
            else if (unit == UnitEnum.Ember)
            {
                u = new Ember(GetSpawnPosition(team, unit), _animationsMove[(int)UnitEnum.Ember], _animationsAttack[(int)UnitEnum.Ember], team, this, _emberProjectile);
            }
            else if (unit == UnitEnum.UberEmber)
            {
                u = new UberEmber(GetSpawnPosition(team, unit), _animationsMove[(int)UnitEnum.UberEmber], _animationsAttack[(int)UnitEnum.UberEmber], team, this, _emberProjectile2);
            }

            _units.Add(u);
            return u;
        }

        public Target AcquireTarget(Unit attacker)
        {
            Target bestTarget = null;
            float closest = -1;

            foreach (Target target in _units)
            {
                if (attacker.CanTarget(target))
                {
                    Vector2 direction = target.GetMidPoint() - attacker.GetMidPoint();
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

        public void AddMarineShot(Unit attacker, Target target, Color color1, Color color2)
        {
            _marineShotBatch.AddShot(attacker, target, color1, color2);
        }

        public Base GetBase1()
        {
            return _p1Base;
        }

        public Base GetBase2()
        {
            return _p2Base;
        }
        public void AddProjectile(Projectile projectile)
        {
            _projectileBatch.AddProjectile(projectile);
        }
    }
}
