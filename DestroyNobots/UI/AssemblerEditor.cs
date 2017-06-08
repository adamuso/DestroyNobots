using System;
using DestroyNobots.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyNobots.UI
{
    public class AssemblerEditor : IRenderable, IInputElement
    {
        private Texture2D texture;
        private StringBuilder text;
        private bool visible;
        private int selectionAnchor;
        private int position;
        private string clipboard;

        public int SelectionStart { get { return position > selectionAnchor ? selectionAnchor : position; } }
        public int SelectionEnd { get { return position > selectionAnchor ? position : selectionAnchor; } }

        public GUI GUI { get; set; }
        public DestroyNobotsGame Game { get { return GUI.Game; } }

        public AssemblerEditor()
        {
            text = new StringBuilder("");
            selectionAnchor = -1;
            clipboard = null;
        }

        public void Show()
        {
            visible = true;
            Focus();
        }

        public void Hide()
        {
            visible = false;
        }

        public void Focus()
        {
            GUI.Game.InputManager.FocusedElement = this;
        }

        void IInputElement.OnKeyDown(KeyboardEventArgs e)
        {
            if (!e.Control)
            {
                if(e.KeyCode >= (int)Keys.D0 && e.KeyCode <= (int)Keys.D9)
                {
                    if (e.Shift)
                    {

                    }
                    else
                    {
                        text.Insert(position, (char)(e.KeyCode));
                        position++;
                    }
                }

                if (e.KeyCode >= (int)Keys.A && e.KeyCode <= (int)Keys.Z)
                {
                    if (e.Shift || e.CapsLock)
                    {
                        text.Insert(position, (char)e.KeyCode);
                        position++;
                    }
                    else
                    {
                        text.Insert(position, (char)(e.KeyCode + 32));
                        position++;
                    }
                }

                if (e.Key == Keys.Space)
                {
                    text.Insert(position, " ");
                    position++;
                }

                if (e.Key == Keys.Enter)
                {
                    text.Insert(position, "\n");
                    position++;
                }

                if (e.Key == Keys.Back && (position > 0 || selectionAnchor != -1))
                {
                    if (selectionAnchor == -1)
                    {
                        text.Remove(position - 1, 1);
                        position--;
                    }
                    else
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        position = SelectionStart;
                        selectionAnchor = -1;
                    }
                }


                if (e.Key == Keys.Left || e.Key == Keys.Right || e.Key == Keys.Up || e.Key == Keys.Down)
                {
                    if (!e.Shift)
                        selectionAnchor = -1;
                    else if (selectionAnchor == -1)
                        selectionAnchor = position;

                    if (e.Key == Keys.Left && position > 0)
                        position--;

                    if (e.Key == Keys.Right && position < text.Length)
                        position++;

                    if (e.Key == Keys.Up)
                    {
                        MoveUpLine();
                    }

                    if (e.Key == Keys.Down)
                    {
                        MoveDownLine();
                    }
                }
            }

            if(e.Key == Keys.OemComma)
            {
                text.Insert(position, ",");
                position++;
            }

            if (e.Key == Keys.OemSemicolon)
            {
                if (e.Shift)
                {
                    text.Insert(position, ":");
                    position++;
                }
                else
                {
                    text.Insert(position, ";");
                    position++;
                }
            }

            if (e.Key == Keys.Tab)
            {
                text.Insert(position, "    ");
                position += 4;
            }

            if(e.Key == Keys.Delete && position < text.Length)
            {
                if(selectionAnchor == -1)
                {
                    text.Remove(position, 1);
                }
                else
                {
                    text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                    position = SelectionStart;
                    selectionAnchor = -1;
                }
            }

            if (e.Control)
            {
                if (e.Key == Keys.C && selectionAnchor != -1)
                {
                    clipboard = text.ToString(SelectionStart, SelectionEnd - SelectionStart);
                }

                if (e.Key == Keys.X && selectionAnchor != -1)
                {
                    clipboard = text.ToString(SelectionStart, SelectionEnd - SelectionStart);
                    text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                    position = SelectionStart;
                    selectionAnchor = -1;
                }

                if (e.Key == Keys.V && clipboard != null)
                {
                    if (selectionAnchor != -1)
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        position = SelectionStart;
                        selectionAnchor = -1;
                    }

                    text.Insert(position, clipboard);
                    position += clipboard.Length;
                }

                if(e.Key == Keys.B)
                {
                    byte[] code = Game.b.Computer.Processor.GetAssociatedCompiler().Compile(text.ToString());
                    var memory = new Assembler.SafeMemory(code.Length, Assembler.BinaryMultiplier.B);
                    memory.Write(0, code);
                    Game.b.Computer.PowerDown();
                    Game.b.Computer.SwitchROM(memory);
                    Game.b.Computer.PowerUp();
                }

                if (e.Key == Keys.Back && (position > 0 || selectionAnchor != -1))
                {
                    if (selectionAnchor == -1)
                    {
                        int removed = 0;

                        while (position > 0 && ((!char.IsWhiteSpace(text[position - 1]) && text[position - 1] != ',') || removed == 0))
                        {
                            text.Remove(position - 1, 1);
                            position--;
                            removed++;
                        }

                        if(position > 0)
                        {
                            text.Remove(position - 1, 1);
                            position--;
                            removed++;
                        }
                    }
                    else
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        position = SelectionStart;
                        selectionAnchor = -1;
                    }
                }


                if (e.Key == Keys.Left || e.Key == Keys.Right || e.Key == Keys.Up || e.Key == Keys.Down)
                {
                    if (!e.Shift)
                        selectionAnchor = -1;
                    else if (selectionAnchor == -1)
                        selectionAnchor = position;

                    if (e.Key == Keys.Left && position > 0)
                    {
                        int skipped = 0;

                        while (position > 0 && ((!char.IsWhiteSpace(text[position - 1]) && text[position - 1] != ',') || skipped == 0))
                        {
                            position--;
                            skipped++;
                        }
                    }

                    if (e.Key == Keys.Right && position < text.Length)
                    {
                        int skipped = 0;

                        while (position < text.Length && ((!char.IsWhiteSpace(text[position]) && text[position] != ',') || skipped == 0))
                        {
                            position++;
                            skipped++;
                        }
                    }
                }
            }
        }

        void IInputElement.OnKeyUp(KeyboardEventArgs e)
        {

        }

        private void SetLineNumberAndPosition(int line, int pos)
        {
            string[] lines = text.ToString().Split('\n');

            if (line < 0 || line >= lines.Length)
                return; 

            position = 0;

            if (line >= lines.Length - 1)
                line = lines.Length - 1;

            for (int i = 0; i < line; i++)
            {
                position += lines[i].Length + 1;
            }

            if (pos > lines[line].Length)
                pos = lines[line].Length;

            position += pos;
        }

        private void GetLineNumberAndPositionFromIndex(int index, out int line, out int pos)
        {
            string[] lines = text.ToString().Split('\n');
            int caret = index;

            for (int i = 0; i < lines.Length; i++)
            {
                if (caret - (lines[i].Length + 1) < 0)
                {
                    line = i;
                    pos = caret;
                    return;
                }
                else if (caret - (lines[i].Length + 1) == 0)
                {
                    line = i + 1;
                    pos = 0;
                    return;
                }
                else
                    caret -= lines[i].Length + 1;
            }

            line = -1;
            pos = -1;
        }

        private void MoveUpLine()
        {
            int line, pos;
            GetLineNumberAndPositionFromIndex(position, out line, out pos);
            line--;
            SetLineNumberAndPosition(line, pos);
        }

        private void MoveDownLine()
        {
            int line, pos;
            GetLineNumberAndPositionFromIndex(position, out line, out pos);
            line++;
            SetLineNumberAndPosition(line, pos);
        }

        public void Draw(GameTime gt)
        {
            if (texture == null)
            {
                texture = new Texture2D(Game.GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.White });
            }

            if (visible)
            {
                string[] lines = text.ToString().Split('\n');
                int line, pos;
                GetLineNumberAndPositionFromIndex(position, out line, out pos);

                for (int i = 0; i < lines.Length; i++)
                {
                    Game.SpriteBatch.DrawString(Game.EditorFont, lines[i], new Vector2(20, 20 + i * 15), Color.Black);
                }

                Vector2 size = Game.EditorFont.MeasureString(lines[line].Substring(0, pos));
                Game.SpriteBatch.Draw(texture, new Rectangle(20 + (int)size.X, 20 + line * 15, 1, 15), Color.White);

                if (selectionAnchor != -1)
                {
                    int startLine, startPos;

                    GetLineNumberAndPositionFromIndex(SelectionStart, out startLine, out startPos);
                    GetLineNumberAndPositionFromIndex(SelectionEnd, out line, out pos);

                    for(int i = startLine; i <= line; i++)
                    {
                        Vector2 offset = Vector2.Zero;

                        if (startLine == line)
                        {
                            offset = Game.EditorFont.MeasureString(lines[i].Substring(0, startPos));
                            size = Game.EditorFont.MeasureString(lines[i].Substring(startPos, pos - startPos));
                        }
                        else if (i == startLine)
                        {
                            offset = Game.EditorFont.MeasureString(lines[i].Substring(0, startPos));
                            size = Game.EditorFont.MeasureString(lines[i].Substring(startPos));
                        }
                        else if (i == line)
                            size = Game.EditorFont.MeasureString(lines[i].Substring(0, pos));
                        else
                            size = Game.EditorFont.MeasureString(lines[i]);

                        Game.SpriteBatch.Draw(texture, new Rectangle(20 + (int)offset.X, 20 + i * 15, (int)size.X, 15), new Color(128, 128, 128, 100));
                    }
                }
            }
        }
    }
}
