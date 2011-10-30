﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public abstract class PEmitter
    {
        #region Vars
        private Vector2 Pos;
        public Vector2 pos
        {
            set
            {
                Pos = value;
                foreach (Particle p in aliveParticles)
                {
                    p.Position = value;
                }
            }
        }

        private List<Particle> aliveParticles;
        public int ParticleCount
        {
            get { return aliveParticles.Count; }
        }
        private Queue<Particle> deadParticles;
        public Texture2D texture;
        private Vector2 origin;
        private Random rand;
        private float curTime = 0.0f;

        private bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        #endregion

        #region Abstract Vars
        protected int maxNumParticles;

        protected float timeBetweenRelease;

        protected string textureFilename;

        protected float minInitSpeed;
        protected float maxInitSpeed;

        protected Vector2 minAccel;
        protected Vector2 maxAccel;

        protected float minRotSpeed;
        protected float maxRotSpeed;

        protected float minLife;
        protected float maxLife;

        protected float minSize;
        protected float maxSize;

        protected BlendState blend;

        protected bool cycleOnce;
        #endregion

        protected PEmitter(Random rand)
        {
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
            float life = Particle.RandomBetween(minLife, maxLife, rand);
            float scale = Particle.RandomBetween(minSize, maxSize, rand);
            float rotSpeed = Particle.RandomBetween(minRotSpeed, maxRotSpeed, rand);

            Vector2 accel = Vector2.Zero;

            accel.X = Particle.RandomBetween(minAccel.X, maxAccel.X, rand);
            accel.Y = Particle.RandomBetween(minAccel.Y, maxAccel.Y, rand);

            p.Initialize(this.Pos,
                vel * direction,
                accel,
                life,
                scale,
                rotSpeed,
                rand);
        }

        public void Initialize(ContentManager cm, string texture)
        {
            aliveParticles = new List<Particle>();
            deadParticles = new Queue<Particle>();

            InitializeConstants(cm, texture);

            if (!cycleOnce)
            {
                for (int i = 0; i < maxNumParticles; ++i)
                {
                    Particle p = new Particle();
                    RecycleParticle(p);
                    deadParticles.Enqueue(p);
                }
            }
            else
            {
                for (int i = 0; i < maxNumParticles; ++i)
                {
                    Particle p = new Particle();
                    RecycleParticle(p);
                    aliveParticles.Add(p);
                }
            }
        }

        public void LoadContent(ContentManager content, string filename)
        {
            texture = content.Load<Texture2D>(filename);

            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
        }

        protected abstract void InitializeConstants(ContentManager cm, string content);

        public void Update(float dt)
        {
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
            if ((curTime >= timeBetweenRelease) && (!cycleOnce))
            {
                if (deadParticles.Count != 0)
                {
                    aliveParticles.Add(deadParticles.Dequeue());
                    RecycleParticle(aliveParticles[aliveParticles.Count - 1]);
                }
                // Reset timer
                curTime = 0.0f;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, blend);

            foreach (Particle p in aliveParticles)
            {
                float normalizedLifeTime = p.TimeSinceStart / p.LifeTime;
                float alpha = 4 * normalizedLifeTime * (1 - normalizedLifeTime);
                Color colour = Color.White * alpha;

                float scale = p.Scale * (0.75f + 0.25f * normalizedLifeTime);

                sb.Draw(texture, p.Position, null, colour,
                    p.Rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

            sb.End();
        }
    }
}
