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
            _p1BaseProperties.AtkAnim = new Animation(_unitSpriteSheet, 8, 8, 2, 1, 1, true);
            _p1BaseProperties.MoveAnim = _p1BaseProperties.AtkAnim;
            _p1BaseProperties.MaxHp = 10000;
            _p1BaseProperties.MoveSpeed = 0;
            _p1BaseProperties.AttackSpeed = 0.0f;
            _p1BaseProperties.AttackDamage = 0;
            _p1BaseProperties.AttackRange = 0;
            _p1BaseProperties.FollowRange = 0;
            _p1BaseProperties.TargetType = TargetUnitType.None;
            _p1BaseProperties.Type = UnitType.Ground;

            _p2BaseProperties = new UnitProperties();
            _p2BaseProperties.AtkAnim = new Animation(_unitSpriteSheet, 8, 8, 3, 1, 1, true);
            _p2BaseProperties.MoveAnim = _p2BaseProperties.AtkAnim;
            _p2BaseProperties.MaxHp = 10000;
            _p2BaseProperties.MoveSpeed = 0;
            _p2BaseProperties.AttackSpeed = 0.0f;
            _p2BaseProperties.AttackDamage = 0;
            _p2BaseProperties.AttackRange = 0;
            _p2BaseProperties.FollowRange = 0;
            _p2BaseProperties.TargetType = TargetUnitType.None;
            _p2BaseProperties.Type = UnitType.Ground;

            //This stuff should be loaded from XML
            // Configure marines
            UnitProperties marineProperties = new UnitProperties();
            marineProperties.AtkAnim = new Animation(_unitSpriteSheet, 32, 32, 0, 3, 0.1f, false);
            marineProperties.MoveAnim = new Animation(_unitSpriteSheet, 32, 32, 3, 4, 0.15f, true);
            marineProperties.MaxHp = 50;
            marineProperties.MoveSpeed = 50.0f;
            marineProperties.AttackSpeed = 0.3f;
            marineProperties.AttackDamage = 3;
            marineProperties.AttackRange = 40;
            marineProperties.FollowRange = 70;
            marineProperties.TargetType = TargetUnitType.Any;
            marineProperties.Type = UnitType.Ground;
            marineProperties.AttackHandler = new BulletAttackHandler(this, Color.White, Color.CornflowerBlue);

            _unitProperties[(int)UnitEnum.Marine] = marineProperties;


            // Configure jetpack marines
            UnitProperties jetpackProperties = new UnitProperties();
            jetpackProperties.AtkAnim = new Animation(_unitSpriteSheet, 32, 32, 32 + 0, 3, 0.1f, false);
            jetpackProperties.MoveAnim = new Animation(_unitSpriteSheet, 32, 32, 32 + 3, 4, 0.15f, true);
            jetpackProperties.MaxHp = 70;
            jetpackProperties.MoveSpeed = 50.0f;
            jetpackProperties.AttackSpeed = 0.3f;
            jetpackProperties.AttackDamage = 9;
            jetpackProperties.AttackRange = 100;
            jetpackProperties.FollowRange = 160;
            jetpackProperties.TargetType = TargetUnitType.GroundOnly;
            jetpackProperties.Type = UnitType.Air;
            jetpackProperties.AttackHandler = new BulletAttackHandler(this, Color.White, Color.CornflowerBlue);

            _unitProperties[(int)UnitEnum.JetpackMarine] = jetpackProperties;

            
            // Configure juggernaught
            UnitProperties juggernaughtProperties = new UnitProperties();
            juggernaughtProperties.AtkAnim = new Animation(_unitSpriteSheet, 16, 16, 16, 3, 0.1f, false);
            juggernaughtProperties.MoveAnim = new Animation(_unitSpriteSheet, 16, 16, 16, 1, 0.1f, true);
            juggernaughtProperties.MaxHp = 1600;
            juggernaughtProperties.MoveSpeed = 20;
            juggernaughtProperties.AttackSpeed = 0.1f;
            juggernaughtProperties.AttackDamage = 4;
            juggernaughtProperties.AttackRange = 50;
            juggernaughtProperties.FollowRange = 100;
            juggernaughtProperties.TargetType = TargetUnitType.GroundOnly;
            juggernaughtProperties.Type = UnitType.Ground;
            juggernaughtProperties.AttackHandler = new BulletAttackHandler(this, Color.Red, Color.Orange);
            juggernaughtProperties.DeathHandler = new JuggernaughtDeathHandler(this);
            juggernaughtProperties._isLarge = true;

            _unitProperties[(int)UnitEnum.Juggernaught] = juggernaughtProperties;


            // Configure ember
            UnitProperties emberProperties = new UnitProperties();
            emberProperties.AtkAnim = new Animation(_unitSpriteSheet, 32, 32, 32 * 4, 3, 0.1f, false);
            emberProperties.MoveAnim = new Animation(_unitSpriteSheet, 32, 32, 32 * 4 + 3, 4, 0.15f, true);
            emberProperties.MaxHp = 35;
            emberProperties.MoveSpeed = 50;
            emberProperties.AttackSpeed = 0.4f;
            emberProperties.AttackDamage = 6;
            emberProperties.AttackRange = 50;
            emberProperties.FollowRange = 80;
            emberProperties.TargetType = TargetUnitType.Any;
            emberProperties.Type = UnitType.Ground;
            emberProperties.AttackHandler = new BulletAttackHandler(this, Color.Yellow, Color.Green);

            _unitProperties[(int)UnitEnum.Ember] = emberProperties;


            // Configure uberember
            UnitProperties uberEmberProperties = new UnitProperties();
            uberEmberProperties.AtkAnim = new Animation(_unitSpriteSheet, 32, 32, 32 * 5, 3, 0.1f, false);
            uberEmberProperties.MoveAnim = new Animation(_unitSpriteSheet, 32, 32, 32 * 5 + 3, 4, 0.15f, true);
            uberEmberProperties.MaxHp = 100;
            uberEmberProperties.MoveSpeed = 100;
            uberEmberProperties.AttackSpeed = 5.0f;
            uberEmberProperties.AttackDamage = 60;
            uberEmberProperties.AttackRange = 50;
            uberEmberProperties.FollowRange = 80;
            uberEmberProperties.TargetType = TargetUnitType.GroundOnly;
            uberEmberProperties.Type = UnitType.Ground;
            uberEmberProperties.AttackHandler = new ProjectileAttackHandler(this, content.Load<Texture2D>("ember_proj2"));

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

            Rectangle p1Rect = _p1BaseProperties.AtkAnim.GetFrameRect(0);
            _p1Base = new Unit(_p1BaseProperties, new Vector2(0, _displayHeight / 2 - p1Rect.Height / 2), UnitTeam.Player1, this);

            Rectangle p2Rect = _p2BaseProperties.AtkAnim.GetFrameRect(0);
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
                if (unit.Position.X > _displayWidth + 100 || unit.Position.X < -100)
                {
                    unit.Kill();
                }
            }

            // Remove dead units and emit death effects
            for (int i = 0; i < _units.Count; )
            {
                if (_units[i].IsDead())
                {
                    
                    if (_units[i].Team == UnitTeam.Player1)
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
