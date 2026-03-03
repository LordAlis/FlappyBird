using System.Drawing;

namespace FlappyBird
{
    /// <summary>
    /// Sürekli kayan zemin varlığı.
    /// Tek bir sprite'ı yatayda tekrarlayarak sonsuz kaydırma efekti oluşturur.
    /// </summary>
    public class ScrollingGround : Entity
    {
        private readonly Image _image;
        private readonly int _gameWidth;
        private float _scrollOffset;

        public const float Speed = 2.5f;
        public float GroundY { get; }

        public ScrollingGround(float groundY, int gameWidth, Image image)
            : base(0, groundY, image.Width, image.Height)
        {
            GroundY = groundY;
            _gameWidth = gameWidth;
            _image = image;
            _scrollOffset = 0;
        }

        public override void Update()
        {
            _scrollOffset -= Speed;
            if (_scrollOffset <= -_image.Width)
                _scrollOffset += _image.Width;
        }

        public override void Draw(Graphics g)
        {
            for (float x = _scrollOffset; x < _gameWidth + _image.Width; x += _image.Width)
                g.DrawImage(_image, x, GroundY, _image.Width, _image.Height);
        }
    }
}
