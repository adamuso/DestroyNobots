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
        #region Drawing variables and properties
        private Texture2D texture;
        private bool visible;
        private float fontScale;

        public Point Position { get; set; }
        public Point Size { get; set; }
        #endregion

        #region Text variables and properties
        private StringBuilder text;
        private int selectionAnchor;
        private int caretPosition;
        private string clipboard;

        public int SelectionStart { get { return caretPosition > selectionAnchor ? selectionAnchor : caretPosition; } }
        public int SelectionEnd { get { return caretPosition > selectionAnchor ? caretPosition : selectionAnchor; } }
        #endregion

        public GUI GUI { get; set; }
        public DestroyNobotsGame Game { get { return GUI.Game; } }

        public AssemblerEditor()
        {
            text = new StringBuilder("");
            selectionAnchor = -1;
            clipboard = null;
            fontScale = 12 / 48.0f;

            Size = new Point(200, 200);
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
            if (!e.Control && (
                    e.KeyCode >= (int)Keys.D0 && e.KeyCode <= (int)Keys.D9 ||
                    e.KeyCode >= (int)Keys.A && e.KeyCode <= (int)Keys.Z ||
                    e.Key == Keys.Space ||
                    e.Key == Keys.Enter ||
                    e.Key == Keys.OemMinus ||
                    e.Key == Keys.OemPlus ||
                    e.Key == Keys.OemOpenBrackets ||
                    e.Key == Keys.OemCloseBrackets ||
                    e.Key == Keys.OemComma ||
                    e.Key == Keys.OemSemicolon ||
                    e.Key == Keys.OemPipe ||
                    e.Key == Keys.OemQuotes ||
                    e.Key == Keys.OemPeriod ||
                    e.Key == Keys.OemQuestion ||
                    e.Key == Keys.OemTilde
                ))
            {
                if (selectionAnchor != -1)
                {
                    text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                    caretPosition = SelectionStart;
                    selectionAnchor = -1;
                }

                // insert keys

                if (e.KeyCode >= (int)Keys.D0 && e.KeyCode <= (int)Keys.D9)
                {
                    if (e.Shift)
                    {
                        switch (e.Key)
                        {
                            case Keys.D0: text.Insert(caretPosition, ")"); break;
                            case Keys.D1: text.Insert(caretPosition, "!"); break;
                            case Keys.D2: text.Insert(caretPosition, "@"); break;
                            case Keys.D3: text.Insert(caretPosition, "#"); break;
                            case Keys.D4: text.Insert(caretPosition, "$"); break;
                            case Keys.D5: text.Insert(caretPosition, "%"); break;
                            case Keys.D6: text.Insert(caretPosition, "^"); break;
                            case Keys.D7: text.Insert(caretPosition, "&"); break;
                            case Keys.D8: text.Insert(caretPosition, "*"); break;
                            case Keys.D9: text.Insert(caretPosition, "("); break;
                        }

                        caretPosition++;
                    }
                    else
                    {
                        text.Insert(caretPosition, (char)(e.KeyCode));
                        caretPosition++;
                    }
                }

                if (e.KeyCode >= (int)Keys.A && e.KeyCode <= (int)Keys.Z)
                {
                    if (e.Shift || e.CapsLock)
                        text.Insert(caretPosition, (char)e.KeyCode);
                    else
                        text.Insert(caretPosition, (char)(e.KeyCode + 32));

                    caretPosition++;
                }

                if (e.Key == Keys.Space)
                {
                    text.Insert(caretPosition, " ");
                    caretPosition++;
                }

                if (e.Key == Keys.Enter)
                {
                    text.Insert(caretPosition, "\n");
                    caretPosition++;
                }

                if (e.Key == Keys.OemMinus)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "_");
                    else
                        text.Insert(caretPosition, "-");

                    caretPosition++;
                }

                if (e.Key == Keys.OemPlus)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "+");
                    else
                        text.Insert(caretPosition, "=");

                    caretPosition++;
                }

                if (e.Key == Keys.OemOpenBrackets)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "{");
                    else
                        text.Insert(caretPosition, "[");

                    caretPosition++;
                }

                if (e.Key == Keys.OemCloseBrackets)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "}");
                    else
                        text.Insert(caretPosition, "]");

                    caretPosition++;
                }

                if (e.Key == Keys.OemComma)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "<");
                    else
                        text.Insert(caretPosition, ",");

                    caretPosition++;
                }

                if (e.Key == Keys.OemSemicolon)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, ":");
                    else
                        text.Insert(caretPosition, ";");

                    caretPosition++;
                }

                if (e.Key == Keys.OemPipe)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "|");
                    else
                        text.Insert(caretPosition, "\\");

                    caretPosition++;
                }

                if (e.Key == Keys.OemQuotes)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "\"");
                    else
                        text.Insert(caretPosition, "'");

                    caretPosition++;
                }

                if (e.Key == Keys.OemPeriod)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, ">");
                    else
                        text.Insert(caretPosition, ".");

                    caretPosition++;
                }

                if (e.Key == Keys.OemQuestion)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "?");
                    else
                        text.Insert(caretPosition, "/");

                    caretPosition++;
                }

                if (e.Key == Keys.OemTilde)
                {
                    if (e.Shift)
                        text.Insert(caretPosition, "~");
                    else
                        text.Insert(caretPosition, "`");

                    caretPosition++;
                }
            }

            if(!e.Control)
            { 
                if (e.Key == Keys.Back && (caretPosition > 0 || selectionAnchor != -1))
                {
                    if (selectionAnchor == -1)
                    {
                        text.Remove(caretPosition - 1, 1);
                        caretPosition--;
                    }
                    else
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        caretPosition = SelectionStart;
                        selectionAnchor = -1;
                    }
                }


                if (e.Key == Keys.Left || e.Key == Keys.Right || e.Key == Keys.Up || e.Key == Keys.Down)
                {
                    if (!e.Shift)
                        selectionAnchor = -1;
                    else if (selectionAnchor == -1)
                        selectionAnchor = caretPosition;

                    if (e.Key == Keys.Left && caretPosition > 0)
                        caretPosition--;

                    if (e.Key == Keys.Right && caretPosition < text.Length)
                        caretPosition++;

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

            if (e.Key == Keys.Tab)
            {
                text.Insert(caretPosition, "    ");
                caretPosition += 4;
            }

            if(e.Key == Keys.Delete && caretPosition < text.Length)
            {
                if(selectionAnchor == -1)
                {
                    text.Remove(caretPosition, 1);
                }
                else
                {
                    text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                    caretPosition = SelectionStart;
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
                    caretPosition = SelectionStart;
                    selectionAnchor = -1;
                }

                if (e.Key == Keys.V && clipboard != null)
                {
                    if (selectionAnchor != -1)
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        caretPosition = SelectionStart;
                        selectionAnchor = -1;
                    }

                    text.Insert(caretPosition, clipboard);
                    caretPosition += clipboard.Length;
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

                if (e.Key == Keys.Back && (caretPosition > 0 || selectionAnchor != -1))
                {
                    if (selectionAnchor == -1)
                    {
                        int removed = 0;

                        while (caretPosition > 0 && ((!char.IsWhiteSpace(text[caretPosition - 1]) && text[caretPosition - 1] != ',') || removed == 0))
                        {
                            text.Remove(caretPosition - 1, 1);
                            caretPosition--;
                            removed++;
                        }

                        if(caretPosition > 0)
                        {
                            text.Remove(caretPosition - 1, 1);
                            caretPosition--;
                            removed++;
                        }
                    }
                    else
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        caretPosition = SelectionStart;
                        selectionAnchor = -1;
                    }
                }


                if (e.Key == Keys.Left || e.Key == Keys.Right || e.Key == Keys.Up || e.Key == Keys.Down)
                {
                    if (!e.Shift)
                        selectionAnchor = -1;
                    else if (selectionAnchor == -1)
                        selectionAnchor = caretPosition;

                    if (e.Key == Keys.Left && caretPosition > 0)
                    {
                        int skipped = 0;

                        while (caretPosition > 0 && ((!char.IsWhiteSpace(text[caretPosition - 1]) && text[caretPosition - 1] != ',') || skipped == 0))
                        {
                            caretPosition--;
                            skipped++;
                        }
                    }

                    if (e.Key == Keys.Right && caretPosition < text.Length)
                    {
                        int skipped = 0;

                        while (caretPosition < text.Length && ((!char.IsWhiteSpace(text[caretPosition]) && text[caretPosition] != ',') || skipped == 0))
                        {
                            caretPosition++;
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

            caretPosition = 0;

            if (line >= lines.Length - 1)
                line = lines.Length - 1;

            for (int i = 0; i < line; i++)
            {
                caretPosition += lines[i].Length + 1;
            }

            if (pos > lines[line].Length)
                pos = lines[line].Length;

            caretPosition += pos;
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
            GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);
            line--;
            SetLineNumberAndPosition(line, pos);
        }

        private void MoveDownLine()
        {
            int line, pos;
            GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);
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

            Game.SpriteBatch.Draw(texture, new Rectangle(Position, Size), new Color(30, 30, 30, 255));

            Rectangle @default = Game.GraphicsDevice.ScissorRectangle;
            Game.GraphicsDevice.ScissorRectangle = new Rectangle(Position, Size);

            if (visible)
            {
                string[] lines = text.ToString().Split('\n');
                int line, pos;
                GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);

                for (int i = 0; i < lines.Length; i++)
                {
                    Game.SpriteBatch.DrawString(Game.EditorFont, lines[i], new Vector2(Position.X, Position.Y + i * 15), new Color(220, 220, 220, 255), 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                }

                Vector2 size = Game.EditorFont.MeasureString(lines[line].Substring(0, pos)) * fontScale;
                Game.SpriteBatch.Draw(texture, new Rectangle(Position.X + (int)size.X, Position.Y + line * 15, 1, 15), Color.White);

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
                            offset = Game.EditorFont.MeasureString(lines[i].Substring(0, startPos)) * fontScale;
                            size = Game.EditorFont.MeasureString(lines[i].Substring(startPos, pos - startPos)) * fontScale;
                        }
                        else if (i == startLine)
                        {
                            offset = Game.EditorFont.MeasureString(lines[i].Substring(0, startPos)) * fontScale;
                            size = Game.EditorFont.MeasureString(lines[i].Substring(startPos)) * fontScale;
                        }
                        else if (i == line)
                            size = Game.EditorFont.MeasureString(lines[i].Substring(0, pos)) * fontScale;
                        else
                            size = Game.EditorFont.MeasureString(lines[i]) * fontScale;

                        Game.SpriteBatch.Draw(texture, new Rectangle(Position.X + (int)offset.X, Position.Y + i * 15, (int)size.X, 15), new Color(128, 128, 128, 100));
                    }
                }
            }

            Game.GraphicsDevice.ScissorRectangle = @default;
        }
    }
}
