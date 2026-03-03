using System;
using System.Collections.Generic;
using System.Drawing;

namespace FlappyBird
{
    public enum GameState
    {
        Ready,
        Playing,
        GameOver
    }

    // oyun mantigi burada, form'dan bagimsiz calisir
    public class GameEngine
    {
        public Bird Bird { get; private set; }
        public List<PipePair> Pipes { get; private set; }
        public ScrollingGround Ground { get; private set; }
        public GameState State { get; private set; }
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public bool IsNight { get; private set; }

        private readonly AudioManager _audio;
        private readonly Image _pipeBottom;
        private readonly Image _pipeTop;
        private readonly Image[] _birdFrames;
        private readonly Image _groundImage;
        private readonly Random _rng = new Random();
        private int _tick;

        public const int GameWidth = 288;
        public const int GameHeight = 512;
        public const float GroundY = 400f;
        private const float PipeSpawnDistance = 180f;
        private const float BirdStartX = 60f;

        public GameEngine(Image[] birdFrames, Image groundImage,
                          Image pipeBottom, Image pipeTop, AudioManager audio)
        {
            _birdFrames = birdFrames;
            _groundImage = groundImage;
            _pipeBottom = pipeBottom;
            _pipeTop = pipeTop;
            _audio = audio;
            Reset();
        }

        public void Reset()
        {
            Bird = new Bird(BirdStartX, GameHeight / 2f - 20, _birdFrames);
            Pipes = new List<PipePair>();
            Ground = new ScrollingGround(GroundY, GameWidth, _groundImage);
            State = GameState.Ready;
            Score = 0;
            _tick = 0;
            IsNight = _rng.Next(2) == 1;
        }

        public void Tick()
        {
            _tick++;
            switch (State)
            {
                case GameState.Ready:
                    Bird.UpdateIdle(GameHeight / 2f - 20, _tick);
                    Ground.Update();
                    break;

                case GameState.Playing:
                    UpdatePlaying();
                    Ground.Update();
                    break;
            }
        }

        public void HandleInput()
        {
            switch (State)
            {
                case GameState.Ready:
                    State = GameState.Playing;
                    Bird.Flap();
                    _audio.PlayFlap();
                    break;

                case GameState.Playing:
                    Bird.Flap();
                    _audio.PlayFlap();
                    break;

                case GameState.GameOver:
                    _audio.PlaySwoosh();
                    Reset();
                    break;
            }
        }

        private void UpdatePlaying()
        {
            Bird.Update();
            Bird.ClampToTop();

            foreach (var pipe in Pipes)
            {
                pipe.Update();
                if (!pipe.Scored && pipe.X + PipePair.PipeWidth / 2f < Bird.X)
                {
                    pipe.Scored = true;
                    Score++;
                    _audio.PlayScore();
                }
            }

            Pipes.RemoveAll(p => p.IsOffScreen());

            if (Pipes.Count == 0 || Pipes[Pipes.Count - 1].X < GameWidth - PipeSpawnDistance)
                SpawnPipe();

            if (DetectCollision())
            {
                _audio.PlayHit();
                _audio.PlayDie();
                State = GameState.GameOver;
                if (Score > HighScore)
                    HighScore = Score;
            }
        }

        private void SpawnPipe()
        {
            float minY = 80;
            float maxY = GroundY - 80;
            float gapY = (float)(_rng.NextDouble() * (maxY - minY) + minY);
            Pipes.Add(new PipePair(GameWidth + 10, gapY, _pipeBottom, _pipeTop));
        }

        private bool DetectCollision()
        {
            if (Bird.HitsGround(GroundY))
                return true;

            RectangleF birdBounds = Bird.GetBounds();
            foreach (var pipe in Pipes)
            {
                if (birdBounds.IntersectsWith(pipe.GetTopBounds()) ||
                    birdBounds.IntersectsWith(pipe.GetBottomBounds(GroundY)))
                    return true;
            }
            return false;
        }
    }
}
