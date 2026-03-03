using System.Drawing;

namespace FlappyBird
{
    /// <summary>
    /// Ekrana çizilebilen nesneler için sözleşme.
    /// </summary>
    public interface IRenderable
    {
        void Draw(Graphics g);
    }
}
