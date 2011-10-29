using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles
{
    public abstract class PEmitter : DrawableGameComponent
    {
        #region Vars
        private Game1 game;
        public Vector2 pos;
        private List<Particle> aliveParticles;
        public int ParticleCount
        {
            get { return aliveParticles.Count; }
        }
        private Queue<Particle> deadParticles;
        private Texture2D texture;
        private Vector2 origin;
        private Random rand;
        private float curTime = 0.0f;
        #endregion

        #region Abstract Vars
        protected int maxNumParticles;

        protected float timeBetweenRelease;

        protected string textureFilename;

        protected float minInitSpeed;
        protected float maxInitSpeed;

        protected float minAccel;
        protected float maxAccel;

        protected float minRotSpeed;
        protected float maxRotSpeed;

        protected float minLife;
        protected float maxLife;

        protected float minSize;
        protected float maxSize;

        protected BlendState blend;
        #endregion

        protected PEmitter(Game1 game, Random rand)
            : base(game)
        {
            this.game = game;
            this.rand = rand;
        }

        protected virtual Vector2 PickRandomDirection()
        {
            float angle = Particle.RandomBetween(0, MathHelper.TwoPi, rand);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        private void RecycleParticle(Particle p)
        {
            Vector2 direction = PickRandomDirection();

            float vel = Particle.RandomBetween(minInitSpeed, maxInitSpeed, rand);
            float accel = Particle.RandomBetween(minAccel, maxAccel, rand);
            float life = Particle.RandomBetween(minLife, maxLife, rand);
            float scale = Particle.RandomBetween(minSize, maxSize, rand);
            float rotSpeed = Particle.RandomBetween(minRotSpeed, maxRotSpeed, rand);

            p.Initialize(this.pos,
                vel * direction,
                accel * direction,
                life,
                scale,
                rotSpeed,
                rand);
        }

        public override void Initialize()
        {
            aliveParticles = new List<Particle>();
            deadParticles = new Queue<Particle>();

            InitializeConstants();

            for (int i = 0; i < maxNumParticles; ++i)
            {
                Particle p = new Particle();
                RecycleParticle(p);
                deadParticles.Enqueue(p);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            texture = game.Content.Load<Texture2D>(textureFilename);

            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            base.LoadContent();
        }

        protected abstract void InitializeConstants();

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            curTime += dt;

            // Update Current Particles
            for (int i = 0; i < aliveParticles.Count - 1; ++i )
            {
                if (aliveParticles[i].Active)
                {
                    aliveParticles[i].Update(dt);
                }
                else
                {
                    deadParticles.Enqueue(aliveParticles[i]);
                    aliveParticles.RemoveAt(i);
                    --i;
                }
            }

            // Release new one if needed
            if (curTime >= timeBetweenRelease)
            {
                if (deadParticles.Count != 0)
                {
                    aliveParticles.Add(deadParticles.Dequeue());
                    RecycleParticle(aliveParticles[aliveParticles.Count - 1]);
                }
                // Reset timer
                curTime = 0.0f;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.spriteBatch.Begin(SpriteSortMode.Deferred, blend);

            foreach (Particle p in aliveParticles)
            {
                float normalizedLifeTime = p.TimeSinceStart / p.LifeTime;
                float alpha = 4 * normalizedLifeTime * (1 - normalizedLifeTime);
                Color colour = Color.White * alpha;

                float scale = p.Scale * (0.75f + 0.25f * normalizedLifeTime);

                game.spriteBatch.Draw(texture, p.Position, null, colour,
                    p.Rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

            game.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
