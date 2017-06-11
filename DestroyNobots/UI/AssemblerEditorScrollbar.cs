using DestroyNobots.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DestroyNobots.UI
{
    public class AssemblerEditorScrollbar : IInputElement, IRenderable
    {
        private const int ScrollBarSize = 15;
        private const int BarOutSize = 4;

        private float offset;

        private Point previousPosition;
        private bool moveOffset;

        public bool IsFocusable { get { return false; } }
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

        private int InsideWidth { get { return (IsHorizontal ? Bounds.Width : Bounds.Height) - BarOutSize; } }
        private int BarWidth
        {
            get
            {
                if(IsHorizontal)
                    return (int)(MathHelper.Clamp(Editor.Size.X / Editor.GetTextWidth(), 0.0000001f, 1) * InsideWidth);
                else
                    return (int)(MathHelper.Clamp((float)Editor.Size.Y / (Editor.GetLineCount() * Editor.LineHeight), 0.0000001f, 1) * InsideWidth);
            }
        }

        private Rectangle Bar
        {
            get
            {
                if (IsHorizontal)
                    return new Rectangle(Bounds.Location + new Point(BarOutSize / 2 + (int)offset, BarOutSize / 2), 
                                         new Point(
                                                 MathHelper.Clamp(BarWidth, ScrollBarSize, InsideWidth),
                                                 ScrollBarSize - BarOutSize
                                             )
                                        );
                else
                    return new Rectangle(Bounds.Location + new Point(BarOutSize / 2, BarOutSize / 2 + (int)offset),
                                        new Point(
                                                ScrollBarSize - BarOutSize,
                                                MathHelper.Clamp(BarWidth, ScrollBarSize, InsideWidth)
                                            )
                                       );
            }
        }
        public DestroyNobotsGame Game { get { return Editor.Game; } }

        public AssemblerEditorScrollbar(AssemblerEditor editor)
        {
            Editor = editor;
            IsHorizontal = false;
        }

        public void UpdateOffsets()
        {
            if(IsHorizontal)
            {
                offset = MathHelper.Clamp(Editor.HorizontalScroll, 0, InsideWidth - BarWidth);
            }
            else
            {
                offset = MathHelper.Clamp((Editor.VerticalScroll * Editor.LineHeight) / (float)(Editor.GetLineCount() * Editor.LineHeight) * (InsideWidth - BarWidth), 0, InsideWidth - BarWidth);
            }
        }

        public void OnKeyDown(KeyboardEventArgs e)
        {

        }

        public void OnKeyUp(KeyboardEventArgs e)
        {

        }

        public void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moveOffset = true;
                previousPosition = e.State.Position;
            }
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            if(e.State.LeftButton == ButtonState.Pressed && moveOffset)
            {
                if (IsHorizontal)
                {
                    offset = MathHelper.Clamp(offset + e.State.X - previousPosition.X, 0, InsideWidth - BarWidth);

                    Editor.HorizontalScroll = MathHelper.Clamp((int)(offset / (InsideWidth - BarWidth) * (Editor.GetTextWidth() - Editor.Size.X + 60)), 0, (int)Editor.GetTextWidth() - Editor.Size.X + 60);
                }
                else
                {
                    offset = MathHelper.Clamp(offset + e.State.Y - previousPosition.Y, 0, InsideWidth - BarWidth);

                    Editor.VerticalScroll = MathHelper.Clamp((int)(offset / (InsideWidth - BarWidth) * (Editor.GetLineCount() * Editor.LineHeight)) / Editor.LineHeight, 0, Editor.GetLineCount());
                }

                previousPosition = e.State.Position;
            }
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                moveOffset = false;
        }

        public void Draw(GameTime gt)
        {
            Game.SpriteBatch.Draw(Game.BlankTexture, Bounds, new Color(60, 60, 60, 255));
            Game.SpriteBatch.Draw(Game.BlankTexture, Bar, new Color(90, 90, 90, 255));
        }
    }
}