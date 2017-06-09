using DestroyNobots.Engine.Input;
using Microsoft.Xna.Framework;

namespace DestroyNobots.UI
{
    public class AssemblerEditorScrollbar : IInputElement, IRenderable
    {
        private const int ScrollBarSize = 15;
        private const int BarOutSize = 4;

        private float value;

        public AssemblerEditor Editor { get; private set; }
        public bool IsHorizontal { get; set; }
        public Rectangle Bounds
        {
            get
            {
                if(IsHorizontal)
                    return new Rectangle(Editor.Position + new Point(0, Editor.Size.Y - ScrollBarSize), new Point(Editor.Size.X - ScrollBarSize, ScrollBarSize));
                else
                    return new Rectangle(Editor.Position + new Point(Editor.Size.X - ScrollBarSize, 0), new Point(ScrollBarSize, Editor.Size.Y - ScrollBarSize));
            }
        }

        private Rectangle Bar
        {
            get
            {
                if (IsHorizontal)
                    return new Rectangle(Bounds.Location + new Point(BarOutSize / 2 + (int)value, BarOutSize / 2), 
                                         new Point(
                                                 MathHelper.Clamp((int)((1.8f - Editor.GetTextWidth() / Editor.Size.X) * (Bounds.Width - BarOutSize)), 20, (Bounds.Width - BarOutSize)),
                                                 ScrollBarSize - BarOutSize
                                             )
                                        );

                return Rectangle.Empty;
            }
        }
        public DestroyNobotsGame Game { get { return Editor.Game; } }

        public AssemblerEditorScrollbar(AssemblerEditor editor)
        {
            Editor = editor;
            IsHorizontal = false;
        }

        public void OnKeyDown(KeyboardEventArgs e)
        {
           
        }

        public void OnKeyUp(KeyboardEventArgs e)
        {

        }

        public void OnMouseDown(MouseEventArgs e)
        {

        }

        public void OnMouseMove(MouseEventArgs e)
        {

        }

        public void OnMouseUp(MouseEventArgs e)
        {

        }

        public void Draw(GameTime gt)
        {
            Game.SpriteBatch.Draw(Game.BlankTexture, Bounds, new Color(60, 60, 60, 255));
            Game.SpriteBatch.Draw(Game.BlankTexture, Bar, new Color(90, 90, 90, 255));
        }
    }
}