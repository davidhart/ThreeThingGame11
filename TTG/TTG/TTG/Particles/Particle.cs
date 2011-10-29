using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class Particle
    {
        // Vars
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;

        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private float timeSinceStart;
        public float TimeSinceStart
        {
            get { return timeSinceStart; }
            set { timeSinceStart = value; }
        }

        private float lifeTime;
        public float LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private float rotationSpeed;
        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        public bool Active
        {
            get { return TimeSinceStart < LifeTime; }
        }

        // Methods
        public static float RandomBetween(float min, float max, Random rand)
        {
            return min + (float)rand.NextDouble() * (max - min);
        }

        public void Initialize(Vector2 position, Vector2 velocity, Vector2 accel,
            float life, float scale, float rotSpeed, Random rand)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = accel;
            this.LifeTime = life;
            this.Scale = scale;
            this.RotationSpeed = rotSpeed;
            this.TimeSinceStart = 0.0f;

            this.Rotation = RandomBetween(0, MathHelper.TwoPi, rand);
        }

        public void Update(float dt)
        {
            Velocity += Acceleration * dt;
            Position += Velocity * dt;

            Rotation += RotationSpeed * dt;

            TimeSinceStart += dt;
        }
    }
}
