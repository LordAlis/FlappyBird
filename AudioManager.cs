using System.IO;
using System.Media;

namespace FlappyBird
{
    /// <summary>
    /// Oyun ses efektlerini yükler ve yönetir.
    /// GameEngine tarafından composition (has-a) ilişkisiyle kullanılır.
    /// Kalıtım yerine bileşim tercih edilmiştir — AudioManager bir Entity değildir.
    /// </summary>
    public class AudioManager
    {
        private readonly SoundPlayer _wing;
        private readonly SoundPlayer _point;
        private readonly SoundPlayer _hit;
        private readonly SoundPlayer _die;
        private readonly SoundPlayer _swoosh;

        public AudioManager(string audioFolder)
        {
            _wing = LoadSound(audioFolder, "wing.wav");
            _point = LoadSound(audioFolder, "point.wav");
            _hit = LoadSound(audioFolder, "hit.wav");
            _die = LoadSound(audioFolder, "die.wav");
            _swoosh = LoadSound(audioFolder, "swoosh.wav");
        }

        public void PlayFlap() => SafePlay(_wing);
        public void PlayScore() => SafePlay(_point);
        public void PlayHit() => SafePlay(_hit);
        public void PlayDie() => SafePlay(_die);
        public void PlaySwoosh() => SafePlay(_swoosh);

        private static SoundPlayer LoadSound(string folder, string file)
        {
            string path = Path.Combine(folder, file);
            return File.Exists(path) ? new SoundPlayer(path) : null;
        }

        private static void SafePlay(SoundPlayer player)
        {
            try { player?.Play(); } catch { }
        }
    }
}
