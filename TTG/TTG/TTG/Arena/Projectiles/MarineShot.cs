using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TTG
{
    public class MarineShot
    {
        const float _lifetime = 0.2f;
        private float _elapsed;

        private Vector2 _start;
        private Vector2 _end;
        private Vector2 _direction;
        private Target _target;
        private Unit _attacker;

        Color _color1;
        public Color Color1
        {
            get
            {
                return _color1;
            }
            set
            {
                _color1 = value;
            }

        }


        Color _color2;
        public Color Color2
        {
            get
            {
                return _color2;
            }
            set
            {
                _color2 = value;
            }
        }

        public MarineShot(Unit attacker, Target target, Color color1, Color color2)
        {
            Vector2 offset = new Vector2(0, 8);

            _start = attacker.Position + offset;
            Rectangle r = target.GetRect();
            _end = target.Position + Util.RandVector(r.Width, r.Height); // TODO: random target point

            _direction = _end -  _start;

            _target = target;
            _attacker = attacker;
            _elapsed = 0;
            _color1 = color1;
            _color2 = color2;
        }

        public void Update(GameTime gameTime)
        {
            _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsed > _lifetime)
                _target.TakeDamage(_attacker.AttackDamage);
        }

        public float PercentAlive()
        {
            return _elapsed / _lifetime;
        }

        public Vector2 GetLineStart()
        {
            return _start + _direction * PercentAlive();
        }

        public Vector2 GetLineEnd()
        {
            return _start + _direction * Math.Min(1, PercentAlive() + 0.2f);
        }

        public bool IsDead()
        {
            return _elapsed > _lifetime;
        }
    }


    public class MarineShotBatch
    {
        List<MarineShot> _shots;
        GraphicsDevice _graphics;
        BasicEffect _effect;

        public MarineShotBatch(GraphicsDevice graphics, int displayWidth, int displayHeight)
        {
            _graphics = graphics;
            _shots = new List<MarineShot>();

            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);

            Matrix projectionMatrix = Matrix.CreateOrthographicOffCenter(
                0,
                (float)displayWidth,
                (float)displayHeight,
                0,
                1.0f, 1000.0f);

            _effect = new BasicEffect(_graphics);
            _effect.VertexColorEnabled = true;

            _effect.Projection = projectionMatrix;
            _effect.View = viewMatrix;
        }

        public void AddShot(Unit attacker, Target target, Color color1, Color color2)
        {
            _shots.Add(new MarineShot(attacker, target, color1, color2));
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _shots.Count;)
            {
                _shots[i].Update(gameTime);

                if (_shots[i].IsDead())
                {
                    _shots.RemoveAt(i);
                }
                else
                    ++i;
            }
        }

        public void Draw()
        {
            if (_shots.Count > 0)
            {
                VertexPositionColor[] vertices = new VertexPositionColor[_shots.Count * 2];

                for (int i = 0; i < _shots.Count; ++i)
                {
                    Color c = Color.Lerp(_shots[i].Color1, _shots[i].Color2, _shots[i].PercentAlive());
                    vertices[i * 2].Color = c;
                    vertices[i * 2].Position = new Vector3(_shots[i].GetLineStart(), 0.0f);

                    vertices[i * 2 + 1].Color = c;
                    vertices[i * 2 + 1].Position = new Vector3(_shots[i].GetLineEnd(), 0.0f);
                }

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, _shots.Count);
                }
            }
        }

        public void Clear()
        {
            _shots.Clear();
        }
    }
}
