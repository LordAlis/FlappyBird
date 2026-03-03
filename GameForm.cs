using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace FlappyBird
{
    /// <summary>
    /// Oyun penceresi. Girdi yönetimi, zamanlayıcı ve görsel çizimden sorumludur.
    /// Oyun mantığını GameEngine'e devreder — Separation of Concerns.
    /// </summary>
    public class GameForm : Form
    {
        private new const float Scale = 2.0f;

        private readonly GameEngine _engine;
        private readonly Image _bgDay;
        private readonly Image _bgNight;
        private readonly Image[] _digitImages;
        private readonly Image _messageImage;
        private readonly Image _gameOverImage;
        private readonly Timer _timer;

        public GameForm()
        {
            Text = "Flappy Bird";
            ClientSize = new Size(
                (int)(GameEngine.GameWidth * Scale),
                (int)(GameEngine.GameHeight * Scale));
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            KeyPreview = true;

            string spritesPath = Path.Combine(Application.StartupPath, "Assets", "sprites");
            string audioPath = Path.Combine(Application.StartupPath, "Assets", "audio");

            _bgDay = LoadImage(spritesPath, "background-day.png");
            _bgNight = LoadImage(spritesPath, "background-night.png");
            _messageImage = LoadImage(spritesPath, "message.png");
            _gameOverImage = LoadImage(spritesPath, "gameover.png");

            _digitImages = new Image[10];
            for (int i = 0; i < 10; i++)
                _digitImages[i] = LoadImage(spritesPath, $"{i}.png");

            Image[] birdFrames =
            {
                LoadImage(spritesPath, "yellowbird-downflap.png"),
                LoadImage(spritesPath, "yellowbird-midflap.png"),
                LoadImage(spritesPath, "yellowbird-upflap.png")
            };

            Image pipeBottom = LoadImage(spritesPath, "pipe-green.png");
            Image pipeTop = (Image)pipeBottom.Clone();
            pipeTop.RotateFlip(RotateFlipType.RotateNoneFlipY);

            Image groundImage = LoadImage(spritesPath, "base.png");

            var audio = new AudioManager(audioPath);
            _engine = new GameEngine(birdFrames, groundImage, pipeBottom, pipeTop, audio);

            _timer = new Timer { Interval = 16 };
            _timer.Tick += OnGameTick;
            _timer.Start();

            KeyDown += OnKeyDown;
            MouseClick += OnMouseClick;
        }

        private void OnGameTick(object sender, EventArgs e)
        {
            _engine.Tick();
            Invalidate();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
                _engine.HandleInput();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            _engine.HandleInput();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.SmoothingMode = SmoothingMode.None;
            g.ScaleTransform(Scale, Scale);

            Image bg = _engine.IsNight ? _bgNight : _bgDay;
            g.DrawImage(bg, 0, 0, GameEngine.GameWidth, GameEngine.GameHeight);

            // Polimorfik Draw çağrıları — her varlık kendini çizer
            foreach (var pipe in _engine.Pipes)
                pipe.Draw(g);

            _engine.Ground.Draw(g);
            _engine.Bird.Draw(g);

            DrawOverlay(g);
        }

        private void DrawOverlay(Graphics g)
        {
            switch (_engine.State)
            {
                case GameState.Ready:
                    g.DrawImage(_messageImage,
                        (GameEngine.GameWidth - _messageImage.Width) / 2f,
                        (GameEngine.GameHeight - _messageImage.Height) / 2f - 50);
                    break;

                case GameState.Playing:
                    DrawDigitScore(g, _engine.Score, GameEngine.GameWidth / 2, 30);
                    break;

                case GameState.GameOver:
                    g.DrawImage(_gameOverImage,
                        (GameEngine.GameWidth - _gameOverImage.Width) / 2f,
                        GameEngine.GameHeight / 2f - 120);

                    DrawDigitScore(g, _engine.Score, GameEngine.GameWidth / 2, GameEngine.GameHeight / 2 - 60);

                    using (var font = new Font("Arial", 8, FontStyle.Bold))
                    using (var white = new SolidBrush(Color.White))
                    using (var shadow = new SolidBrush(Color.FromArgb(128, 0, 0, 0)))
                    using (var fmt = new StringFormat { Alignment = StringAlignment.Center })
                    {
                        float cx = GameEngine.GameWidth / 2f;

                        g.DrawString($"En Yuksek: {_engine.HighScore}", font, shadow, cx + 1, GameEngine.GameHeight / 2f - 21, fmt);
                        g.DrawString($"En Yuksek: {_engine.HighScore}", font, white, cx, GameEngine.GameHeight / 2f - 22, fmt);

                        g.DrawString("Tekrar oynamak icin tikla", font, shadow, cx + 1, GameEngine.GameHeight / 2f + 9, fmt);
                        g.DrawString("Tekrar oynamak icin tikla", font, white, cx, GameEngine.GameHeight / 2f + 8, fmt);
                    }
                    break;
            }
        }

        private void DrawDigitScore(Graphics g, int value, int centerX, int y)
        {
            string text = value.ToString();
            int totalWidth = 0;
            foreach (char c in text)
                totalWidth += _digitImages[c - '0'].Width + 2;
            totalWidth -= 2;

            float startX = centerX - totalWidth / 2f;
            foreach (char c in text)
            {
                int digit = c - '0';
                g.DrawImage(_digitImages[digit], startX, y);
                startX += _digitImages[digit].Width + 2;
            }
        }

        private static Image LoadImage(string folder, string file)
        {
            string path = Path.Combine(folder, file);
            if (File.Exists(path))
                return Image.FromFile(path);

            var placeholder = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(placeholder))
                g.Clear(Color.Magenta);
            return placeholder;
        }
    }
}
