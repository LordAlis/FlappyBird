using System.Drawing;

namespace FlappyBird
{
    // ust ve alt boruyu tek class olarak yonetir
    public class PipePair : Entity
    {
        public float GapCenterY { get; private set; }
        public bool Scored { get; set; }

        private readonly Image _pipeBottom;
        private readonly Image _pipeTop;

        public const int PipeWidth = 52;
        public const int GapSize = 100;
        public const float Speed = 2.5f;

        public PipePair(float x, float gapCenterY, Image pipeBottom, Image pipeTop)
            : base(x, 0, PipeWidth, 0)
        {
            GapCenterY = gapCenterY;
            _pipeBottom = pipeBottom;
            _pipeTop = pipeTop;
            Scored = false;
        }

        public override void Update()
        {
            X -= Speed;
        }

        public override void Draw(Graphics g)
        {
            float gapTop = GapCenterY - GapSize / 2f;
            float gapBottom = GapCenterY + GapSize / 2f;

            // ust boru (ters cevirilmis hali)
            g.DrawImage(_pipeTop,
                X, gapTop - _pipeTop.Height,
                PipeWidth, _pipeTop.Height);

            g.DrawImage(_pipeBottom,
                X, gapBottom,
                PipeWidth, _pipeBottom.Height);
        }

        public RectangleF GetTopBounds()
        {
            return new RectangleF(X, 0, PipeWidth, GapCenterY - GapSize / 2f);
        }

        public RectangleF GetBottomBounds(float groundY)
        {
            float top = GapCenterY + GapSize / 2f;
            return new RectangleF(X, top, PipeWidth, groundY - top);
        }

        public bool IsOffScreen()
        {
            return X + PipeWidth < 0;
        }
    }
}
