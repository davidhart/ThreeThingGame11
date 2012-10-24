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
        JetpackMarine = 4,
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
        Any,        // Can shoot anything
        GroundOnly, // Can shoot only ground targets
        AirOnly,    // Can shoot only air targets
        None,       // Can't shoot anything
    };

    public class Arena
    {
        private List<Unit> _units;
        public bool isTimeAtk = false;
        private UnitProperties[] _unitProperties;

        private UnitProperties _p1BaseProperties;
        private UnitProperties _p2BaseProperties;

        private Unit _p1Base;
        private Unit _p2Base;

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
        Texture2D _unitSpriteSheet;
        Texture2D _battleBG;
        Rectangle _bgRect;

        SoundEffect _jugRiderSpawn;

        //Particles
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
            _units = new List<Unit>();

            _displayWidth = displayWidth;
            _displayHeight = displayHeight;

            _unitProperties = new UnitProperties[10];

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

            _unitSpriteSheet = content.Load<Texture2D>("UnitsSpriteSheet");

            // Configure p1 base
            _p1BaseProperties = new UnitProperties();
            _p1BaseProperties._attack = new Animation(_unitSpriteSheet, 8, 8, 2, 1, 1, true);
            _p1BaseProperties._move = _p1BaseProperties._attack;
            _p1BaseProperties._maxHp = 10000;
            _p1BaseProperties._moveSpeed = 0;
            _p1BaseProperties._attackSpeed = 0.0f;
            _p1BaseProperties._attackDamage = 0;
            _p1BaseProperties._attackRange = 0;
            _p1BaseProperties._followRange = 0;
            _p1BaseProperties._targetType = TargetUnitType.None;
            _p1BaseProperties._type = UnitType.Ground;

            _p2BaseProperties = new UnitProperties();
            _p2BaseProperties._attack = new Animation(_unitSpriteSheet, 8, 8, 3, 1, 1, true);
            _p2BaseProperties._move = _p2BaseProperties._attack;
            _p2BaseProperties._maxHp = 10000;
            _p2BaseProperties._moveSpeed = 0;
            _p2BaseProperties._attackSpeed = 0.0f;
            _p2BaseProperties._attackDamage = 0;
            _p2BaseProperties._attackRange = 0;
            _p2BaseProperties._followRange = 0;
            _p2BaseProperties._targetType = TargetUnitType.None;
            _p2BaseProperties._type = UnitType.Ground;

            // Configure marines
            UnitProperties marineProperties = new UnitProperties();
            marineProperties._attack = new Animation(_unitSpriteSheet, 32, 32, 0, 3, 0.1f, false);
            marineProperties._move = new Animation(_unitSpriteSheet, 32, 32, 3, 4, 0.15f, true);
            marineProperties._maxHp = 50;
            marineProperties._moveSpeed = 50.0f;
            marineProperties._attackSpeed = 0.3f;
            marineProperties._attackDamage = 3;
            marineProperties._attackRange = 40;
            marineProperties._followRange = 70;
            marineProperties._targetType = TargetUnitType.Any;
            marineProperties._type = UnitType.Ground;
            marineProperties._attackHandler = new BulletAttackHandler(this, Color.White, Color.CornflowerBlue);

            _unitProperties[(int)UnitEnum.Marine] = marineProperties;


            // Configure jetpack marines
            UnitProperties jetpackProperties = new UnitProperties();
            jetpackProperties._attack = new Animation(_unitSpriteSheet, 32, 32, 32 + 0, 3, 0.1f, false);
            jetpackProperties._move = new Animation(_unitSpriteSheet, 32, 32, 32 + 3, 4, 0.15f, true);
            jetpackProperties._maxHp = 70;
            jetpackProperties._moveSpeed = 50.0f;
            jetpackProperties._attackSpeed = 0.3f;
            jetpackProperties._attackDamage = 9;
            jetpackProperties._attackRange = 100;
            jetpackProperties._followRange = 160;
            jetpackProperties._targetType = TargetUnitType.GroundOnly;
            jetpackProperties._type = UnitType.Air;
            jetpackProperties._attackHandler = new BulletAttackHandler(this, Color.White, Color.CornflowerBlue);

            _unitProperties[(int)UnitEnum.JetpackMarine] = jetpackProperties;

            
            // Configure juggernaught
            UnitProperties juggernaughtProperties = new UnitProperties();
            juggernaughtProperties._attack = new Animation(_unitSpriteSheet, 16, 16, 16, 3, 0.1f, false);
            juggernaughtProperties._move = new Animation(_unitSpriteSheet, 16, 16, 16, 1, 0.1f, true);
            juggernaughtProperties._maxHp = 1600;
            juggernaughtProperties._moveSpeed = 20;
            juggernaughtProperties._attackSpeed = 0.1f;
            juggernaughtProperties._attackDamage = 4;
            juggernaughtProperties._attackRange = 50;
            juggernaughtProperties._followRange = 100;
            juggernaughtProperties._targetType = TargetUnitType.GroundOnly;
            juggernaughtProperties._type = UnitType.Ground;
            juggernaughtProperties._attackHandler = new BulletAttackHandler(this, Color.Red, Color.Orange);
            juggernaughtProperties._deathHandler = new JuggernaughtDeathHandler(this);
            juggernaughtProperties._isLarge = true;

            _unitProperties[(int)UnitEnum.Juggernaught] = juggernaughtProperties;


            // Configure ember
            UnitProperties emberProperties = new UnitProperties();
            emberProperties._attack = new Animation(_unitSpriteSheet, 32, 32, 32 * 4, 3, 0.1f, false);
            emberProperties._move = new Animation(_unitSpriteSheet, 32, 32, 32 * 4 + 3, 4, 0.15f, true);
            emberProperties._maxHp = 35;
            emberProperties._moveSpeed = 50;
            emberProperties._attackSpeed = 0.4f;
            emberProperties._attackDamage = 6;
            emberProperties._attackRange = 50;
            emberProperties._followRange = 80;
            emberProperties._targetType = TargetUnitType.Any;
            emberProperties._type = UnitType.Ground;
            emberProperties._attackHandler = new BulletAttackHandler(this, Color.Yellow, Color.Green);

            _unitProperties[(int)UnitEnum.Ember] = emberProperties;


            // Configure uberember
            UnitProperties uberEmberProperties = new UnitProperties();
            uberEmberProperties._attack = new Animation(_unitSpriteSheet, 32, 32, 32 * 5, 3, 0.1f, false);
            uberEmberProperties._move = new Animation(_unitSpriteSheet, 32, 32, 32 * 5 + 3, 4, 0.15f, true);
            uberEmberProperties._maxHp = 100;
            uberEmberProperties._moveSpeed = 100;
            uberEmberProperties._attackSpeed = 5.0f;
            uberEmberProperties._attackDamage = 60;
            uberEmberProperties._attackRange = 50;
            uberEmberProperties._followRange = 80;
            uberEmberProperties._targetType = TargetUnitType.GroundOnly;
            uberEmberProperties._type = UnitType.Ground;
            uberEmberProperties._attackHandler = new ProjectileAttackHandler(this, content.Load<Texture2D>("ember_proj2"));

            _unitProperties[(int)UnitEnum.UberEmber] = uberEmberProperties;


            _battleBG = content.Load<Texture2D>("BattleBG2");

            _bgRect = new Rectangle(0, 30, _battleBG.Width, _battleBG.Height);

            _bgm = new Music(content.Load<SoundEffect>("Ropocalypse 2"),true);
            _marineShotBatch = new MarineShotBatch(device, _displayWidth, _displayHeight, _drawPosition); 
            _hydroShotBatch = new MarineShotBatch(device, _displayWidth, _displayHeight, _drawPosition);
            _projectileBatch = new ProjectileBatch();

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

            Rectangle p1Rect = _p1BaseProperties._attack.GetFrameRect(0);
            _p1Base = new Unit(_p1BaseProperties, new Vector2(0, _displayHeight / 2 - p1Rect.Height / 2), UnitTeam.Player1, this);

            Rectangle p2Rect = _p2BaseProperties._attack.GetFrameRect(0);
            _p2Base = new Unit(_p2BaseProperties, new Vector2(_displayWidth - p2Rect.Width, _displayHeight / 2 - p2Rect.Height / 2),
                UnitTeam.Player2, this);

            _units.Add(_p1Base);
            _units.Add(_p2Base);

            _projectileBatch.Clear();
            _marineShotBatch.Clear();
        }

        public void Update(GameTime gameTime)
        {
            _bgm.Play();
            for (int i = 0; i < _units.Count; ++i)
            {
                // Update each target
                Unit unit = _units[i];
                unit.Update(gameTime);

                // Kill anything that ended up offscreen (shouldn't happen under normal circumstances)
                if (unit.GetPosition().X > _displayWidth + 100 || unit.GetPosition().X < -100)
                {
                    unit.Kill();
                }
            }

            // Remove dead units and emit death effects
            for (int i = 0; i < _units.Count; )
            {
                if (_units[i].IsDead())
                {
                    
                    if (_units[i].GetTeam() == UnitTeam.Player1)
                    {
                        EmitDeathEffect(_deadHumanEmitters, _units[i]);
                    }
                    else
                    {
                        EmitDeathEffect(_deadEnemyEmitters, _units[i]);
                    }

                    _units.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }

            // TODO: extract repeated code to method
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

        // TODO: unify MarineDeathEmitter and EnemyDeathEmitter by configuring base class instead of using derived classes
        private void EmitDeathEffect(List<MarineDeathEmitter> emitters, Unit  unit)
        {
            for (int j = 0; j < emitters.Count - 1; ++j)
            {
                if (!emitters[j].Active)
                {
                    unit.OnDeath(emitters[j]);
                    return;
                }
            }
        }
        private void EmitDeathEffect(List<EnemyDeathEmitter> emitters, Unit unit)
        {
            for (int j = 0; j < emitters.Count - 1; ++j)
            {
                if (!emitters[j].Active)
                {
                    unit.OnDeath(emitters[j]);
                    return;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Matrix offset = Matrix.CreateTranslation(new Vector3(_drawPosition, 0.0f));

            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, null, null, offset);
            spritebatch.Draw(_battleBG, _bgRect, Color.White);
            spritebatch.End();

            // TODO: Draw unit shadow circles
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, RasterizerState.CullNone, null, offset);
            
            for (int i = 0; i < _units.Count; ++i)
            {
                _units[i].DrawShadow(spritebatch, _unitSpriteSheet);
                    
            }
            spritebatch.End();

            spritebatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, RasterizerState.CullNone, null, offset);

            // Draw units
            for (int i = 0; i < _units.Count; ++i)
            {
                _units[i].Draw(spritebatch);
            }

            _projectileBatch.Draw(spritebatch);

            spritebatch.End();

            _marineShotBatch.Draw();
            spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, null, offset);

            // TODO: unify repeated code
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

        public Vector2 GetSpawnPosition(UnitTeam team)
        {
            float y = 100 + Util.Rand(50);

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
            Unit u = new Unit(_unitProperties[(int)unit], GetSpawnPosition(team), team, this);

            _units.Add(u);

            return u;
        }

        public Unit AcquireTarget(Unit attacker)
        {
            Unit bestTarget = null;
            float closest = -1;

            foreach (Unit target in _units)
            {
                if (attacker.CanTarget(target))
                {
                    Vector2 direction = target.GetMidPoint() - attacker.GetMidPoint();
                    float distance = direction.Length();

                    if (distance < attacker.FollowRange())
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

        public void AddMarineShot(Unit attacker, Unit target, Color color1, Color color2)
        {
            _marineShotBatch.AddShot(attacker, target, color1, color2);
        }

        public Unit GetBase1()
        {
            return _p1Base;
        }

        public Unit GetBase2()
        {
            return _p2Base;
        }
        public void AddProjectile(Projectile projectile)
        {
            _projectileBatch.AddProjectile(projectile);
        }
    }
}
