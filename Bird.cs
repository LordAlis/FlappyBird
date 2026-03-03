using System;
using System.Drawing;

namespace FlappyBird
{
    public class Bird : Entity
    {
        private float _velocity;
        private float _rotation;
        private int _animTimer;
        private int _animIndex;
        private readonly int[] _animSequence = { 0, 1, 2, 1 };
        private readonly Image[] _frames;

        public int AnimFrame => _animSequence[_animIndex];
        public float Rotation => _rotation;

        private const float Gravity = 0.5f;
        private const float FlapImpulse = -8f;
        private const float TerminalVelocity = 12f;
        private const int AnimInterval = 6;

        public Bird(float x, float y, Image[] frames)
            : base(x, y, 34, 24)
        {
            _frames = frames;
            _velocity = 0;
            _rotation = 0;
        }

        public void Flap()
        {
            _velocity = FlapImpulse;
        }

        public override void Update()
        {
            _velocity = Math.Min(_velocity + Gravity, TerminalVelocity);
            Y += _velocity;
            UpdateRotation();
            CycleAnimation();
        }

        // baslangic ekraninda asagi yukari sallanma
        public void UpdateIdle(float baseY, int tick)
        {
            Y = baseY + (float)Math.Sin(tick * 0.12) * 8;
            _rotation = 0;
            CycleAnimation();
        }

        public void ClampToTop()
        {
            if (Y - Height / 2f < 0)
            {
                Y = Height / 2f;
                _velocity = 0;
            }
        }

        public bool HitsGround(float groundY)
        {
            if (Y + Height / 2f >= groundY)
            {
                Y = groundY - Height / 2f;
                return true;
            }
            return false;
        }

        public override void Draw(Graphics g)
        {
            Image frame = _frames[AnimFrame];
            var saved = g.Save();
            g.TranslateTransform(X, Y);
            g.RotateTransform(_rotation);
            g.DrawImage(frame, -Width / 2f, -Height / 2f, Width, Height);
            g.Restore(saved);
        }

        // hitbox biraz kucultulmus, yoksa cok zor oluyor
        public override RectangleF GetBounds()
        {
            const float margin = 3f;
            return new RectangleF(
                X - Width / 2f + margin,
                Y - Height / 2f + margin,
                Width - margin * 2,
                Height - margin * 2);
        }

        private void UpdateRotation()
        {
            if (_velocity < -2) _rotation = -25;
            else if (_velocity < 0) _rotation = -15;
            else if (_velocity < 3) _rotation = 0;
            else _rotation = Math.Min(_velocity * 15, 90);
        }

        private void CycleAnimation()
        {
            if (++_animTimer >= AnimInterval)
            {
                _animTimer = 0;
                _animIndex = (_animIndex + 1) % _animSequence.Length;
            }
        }
    }
}
