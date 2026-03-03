using System.Drawing;

namespace FlappyBird
{
    /// <summary>
    /// Oyundaki tüm varlıklar için soyut temel sınıf.
    /// Konum ve boyut bilgisini tutar; çizim ve güncelleme sözleşmelerini zorunlu kılar.
    /// IRenderable ve IUpdatable arayüzlerini birleştirir.
    /// </summary>
    public abstract class Entity : IRenderable, IUpdatable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        protected Entity(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public abstract void Update();
        public abstract void Draw(Graphics g);

        public virtual RectangleF GetBounds()
        {
            return new RectangleF(X, Y, Width, Height);
        }
    }
}
