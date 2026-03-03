using System;
using System.Drawing;

namespace FlappyBird
{
    /// <summary>
    /// Oyuncu tarafından kontrol edilen kuş varlığı.
    /// Yerçekimi, kanat çırpma fiziği ve animasyon mantığını kapsüller.
    /// Dış dünya yalnızca Flap() ile etkileşir — iç fizik gizlidir.
    /// </summary>
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

        /// <summary>
        /// Kullanıcı girdisine yanıt: kanat çırpma.
        /// Tek genel davranış metodu — iç fizik tamamen kapsüllenmiştir.
        /// </summary>
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

        /// <summary>
        /// Başlangıç ekranında kuşun hafifçe yukarı-aşağı sallanması.
        /// </summary>
        public void UpdateIdle(float baseY, int tick)
        {
            Y = baseY + (float)Math.Sin(tick * 0.12) * 8;
            _rotation = 0;
            CycleAnimation();
        }

        /// <summary>
        /// Tavana çarpma kısıtlaması. Fizik sınırı entity içinde yönetilir.
        /// </summary>
        public void ClampToTop()
        {
            if (Y - Height / 2f < 0)
            {
                Y = Height / 2f;
                _velocity = 0;
            }
        }

        /// <summary>
        /// Zemine temas kontrolü. Temas varsa konum düzeltilir.
        /// </summary>
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

        /// <summary>
        /// Merkez bazlı hitbox. Adil oynanış için kenarlardan küçültülmüştür.
        /// </summary>
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
