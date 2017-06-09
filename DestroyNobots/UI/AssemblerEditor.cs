using System;
using DestroyNobots.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;

namespace DestroyNobots.UI
{
    public class AssemblerEditor : IRenderable, IInputElementContainer
    {
        #region Drawing variables and properties
        private Texture2D texture;
        private bool visible;
        private float fontScale;

        public int LineHeight { get { return (int)(60 * fontScale); } }
        public int VerticalScroll { get; set; }
        public int HorizontalScroll { get; set; }
        public Rectangle Bounds { get { return new Rectangle(Position, Size); } }
        public Point Position { get; set; }
        public Point Size { get; set; }
        #endregion

        #region Text variables and properties
        private StringBuilder text;
        private int selectionAnchor;
        private int caretPosition;
        private string clipboard;
        private int caretAccumulator;

        public int SelectionStart { get { return caretPosition > selectionAnchor ? selectionAnchor : caretPosition; } }
        public int SelectionEnd { get { return caretPosition > selectionAnchor ? caretPosition : selectionAnchor; } }
        #endregion

        #region Components
        private AssemblerEditorScrollbar horizontalScrollBar;
        private AssemblerEditorScrollbar verticalScrollBar;
        #endregion

        IEnumerable<IInputElement> IInputElementContainer.Children { get { return Children; } }
        public List<IInputElement> Children { get; private set; }
        public bool IsFocusable { get { return true; } }
        public GUI GUI { get; set; }
        public DestroyNobotsGame Game { get { return GUI.Game; } }

        public AssemblerEditor()
        {
            Children = new List<IInputElement>();

            text = new StringBuilder("");
            selectionAnchor = -1;
            clipboard = null;
            fontScale = 12 / 48.0f;
            verticalScrollBar = new AssemblerEditorScrollbar(this);
            horizontalScrollBar = new AssemblerEditorScrollbar(this) { IsHorizontal = true };

            Children.Add(verticalScrollBar);
            Children.Add(horizontalScrollBar);

            Size = new Point(300, 300);
        }

        public void Show()
        {
            visible = true;
            GUI.Game.InputManager.InputElementManager.Root = this;
        }

        public void Hide()
        {
            visible = false;
        }

        public float GetTextWidth()
        {
            string[] lines = text.ToString().Split('\n');
            string maxLine = lines[0];

            for (int i = 1; i < lines.Length; i++)
                if (maxLine.Length < lines[i].Length)
                    maxLine = lines[i];

            return (Game.EditorFont.MeasureString(maxLine) * fontScale).X;
        }

        public int GetLineCount()
        {
            int count = text.ToString().Count(p => p == '\n');
            return count <= 0 ? 1 : count;
        }

        private void SetLineNumberAndPosition(int line, int pos)
        {
            string[] lines = text.ToString().Split('\n');

            if (line < 0 || line >= lines.Length)
                return;

            caretPosition = GetIndexFromLineNumberAndPosition(line, pos);
        }

        private int GetIndexFromLineNumberAndPosition(int line, int pos)
        {
            string[] lines = text.ToString().Split('\n');

            if (line < 0 || line >= lines.Length)
                return -1;

            int caretPosition = 0;

            if (line >= lines.Length - 1)
                line = lines.Length - 1;

            for (int i = 0; i < line; i++)
            {
                caretPosition += lines[i].Length + 1;
            }

            if (pos > lines[line].Length)
                pos = lines[line].Length;

            return caretPosition + pos;
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

        #region Keyboard input handling
        void IInputElement.OnKeyDown(KeyboardEventArgs e)
        {
            HandleTextIsertionKeys(e);
            HandleTextMovement(e);
            HandleTextDeletion(e);
            HandleTab(e);
            HandleTextOperations(e);
            ScrollToCaret();

            if (e.Control)
            {
                if(e.Key == Keys.B)
                {
                    byte[] code = Game.b.Computer.Processor.GetAssociatedCompiler().Compile(text.ToString());
                    var memory = new Assembler.SafeMemory(code.Length, Assembler.BinaryMultiplier.B);
                    memory.Write(0, code);
                    Game.b.Computer.PowerDown();
                    Game.b.Computer.SwitchROM(memory);
                    Game.b.Computer.PowerUp();
                }
            }
        }

        void IInputElement.OnKeyUp(KeyboardEventArgs e)
        {

        }

        private void HandleTextIsertionKeys(KeyboardEventArgs e)
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
                    int line, pos;
                    GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);

                    text.Insert(caretPosition, "\n");
                    caretPosition++;

                    string[] lines = text.ToString().Split('\n');

                    for (int i = 0; i < lines[line].Length; i++)
                        if (char.IsWhiteSpace(lines[line][i]))
                        {
                            text.Insert(caretPosition, lines[line][i]);
                            caretPosition++;
                        }
                        else
                            break;
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
        }

        private void HandleTab(KeyboardEventArgs e)
        {
            if (e.Key == Keys.Tab)
            {
                if (selectionAnchor == -1)
                {
                    if (e.Shift)
                    {
                        int line, pos;
                        GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);

                        string[] lines = text.ToString().Split('\n');
                        int count = 0;

                        for (int i = pos - 1; i >= 0 && count < 4; i--, count++)
                            if (char.IsWhiteSpace(lines[line][i]) && lines[line][i] != '\n')
                            {
                                text.Remove(caretPosition - 1, 1);
                                caretPosition--;
                            }
                            else
                                break;
                    }
                    else
                    {
                        text.Insert(caretPosition, "    ");
                        caretPosition += 4;
                    }
                }
                else
                {
                    if (e.Shift)
                    {
                        int startLine, startPos, endLine, endPos;
                        GetLineNumberAndPositionFromIndex(SelectionStart, out startLine, out startPos);
                        GetLineNumberAndPositionFromIndex(SelectionEnd, out endLine, out endPos);

                        for (int i = startLine; i <= endLine; i++)
                        {
                            for (int j = 3; j >= 0; j--)
                            {
                                int pos = GetIndexFromLineNumberAndPosition(i, j);

                                if (pos >= text.Length)
                                    continue;

                                if (char.IsWhiteSpace(text[pos]) && text[pos] != '\n')
                                {
                                    text.Remove(pos, 1);

                                    if (SelectionStart == caretPosition)
                                    {
                                        if (i == startLine)
                                            caretPosition -= 1;

                                        selectionAnchor -= 1;
                                    }
                                    else
                                    {
                                        if (i == startLine)
                                            selectionAnchor -= 1;

                                        caretPosition -= 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int startLine, startPos, endLine, endPos;
                        GetLineNumberAndPositionFromIndex(SelectionStart, out startLine, out startPos);
                        GetLineNumberAndPositionFromIndex(SelectionEnd, out endLine, out endPos);

                        for (int i = startLine; i <= endLine; i++)
                        {
                            text.Insert(GetIndexFromLineNumberAndPosition(i, 0), "    ");

                            if (SelectionStart == caretPosition)
                                selectionAnchor += 4;
                            else
                                caretPosition += 4;
                        }

                        if (SelectionStart == caretPosition)
                            caretPosition += 4;
                        else
                            selectionAnchor += 4;
                    }
                }
            }
        }

        private void HandleTextMovement(KeyboardEventArgs e)
        {
            if(!e.Control)
            {
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
            else
            {
                if (e.Key == Keys.Left || e.Key == Keys.Right)
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

        private void HandleTextDeletion(KeyboardEventArgs e)
        {

            if (e.Key == Keys.Back && (caretPosition > 0 || selectionAnchor != -1))
            {
                if (selectionAnchor == -1)
                {
                    if (!e.Control)
                    {
                        text.Remove(caretPosition - 1, 1);
                        caretPosition--;
                    }
                    else
                    {
                        int removed = 0;

                        while (caretPosition > 0 && ((!char.IsWhiteSpace(text[caretPosition - 1]) && text[caretPosition - 1] != ',') || removed == 0))
                        {
                            text.Remove(caretPosition - 1, 1);
                            caretPosition--;
                            removed++;
                        }

                        if (caretPosition > 0)
                        {
                            text.Remove(caretPosition - 1, 1);
                            caretPosition--;
                            removed++;
                        }
                    }
                }
                else
                {
                    text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                    caretPosition = SelectionStart;
                    selectionAnchor = -1;
                }
            }

            if (e.Key == Keys.Delete && caretPosition < text.Length)
            {
                if (selectionAnchor == -1)
                {
                    text.Remove(caretPosition, 1);
                }
                else
                {
                    if (e.Control)
                    {

                    }
                    else
                    {
                        text.Remove(SelectionStart, SelectionEnd - SelectionStart);
                        caretPosition = SelectionStart;
                        selectionAnchor = -1;
                    }
                }
            }

        }

        private void HandleTextOperations(KeyboardEventArgs e)
        {
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
            }
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

        private void ScrollToCaret()
        {
            string[] lines = text.ToString().Split('\n');
            int line, pos;
            GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);
            Vector2 size = Game.EditorFont.MeasureString(lines[line].Substring(0, pos)) * fontScale;
            Rectangle caretRectangle = new Rectangle(Position.X - HorizontalScroll + (int)size.X, Position.Y + line * LineHeight - VerticalScroll * LineHeight, 1, LineHeight);

            // scrolling to caret
            while (caretRectangle.X > Bounds.Right - 20)
            {
                HorizontalScroll += 60;
                caretRectangle = new Rectangle(Position.X - HorizontalScroll + (int)size.X, Position.Y + line * LineHeight - VerticalScroll * LineHeight, 1, LineHeight);
            }

            while (caretRectangle.X < Bounds.Left)
            {
                HorizontalScroll = MathHelper.Clamp(HorizontalScroll - 60, 0, HorizontalScroll);
                caretRectangle = new Rectangle(Position.X - HorizontalScroll + (int)size.X, Position.Y + line * LineHeight - VerticalScroll * LineHeight, 1, LineHeight);
            }

            while (caretRectangle.Y > Bounds.Bottom - 20)
            {
                VerticalScroll += 1;
                caretRectangle = new Rectangle(Position.X - HorizontalScroll + (int)size.X, Position.Y + line * LineHeight - VerticalScroll * LineHeight, 1, LineHeight);
            }

            while (caretRectangle.Y < Bounds.Top)
            {
                VerticalScroll = MathHelper.Clamp(VerticalScroll - 1, 0, VerticalScroll);
                caretRectangle = new Rectangle(Position.X - HorizontalScroll + (int)size.X, Position.Y + line * LineHeight - VerticalScroll * LineHeight, 1, LineHeight);
            }

            horizontalScrollBar.UpdateOffsets();
            verticalScrollBar.UpdateOffsets();
        }
        #endregion

        private Tuple<string, Color>[] HighlightText(string text, Color @default)
        {
            List<string> keywords = new List<string>() { "mov", "add", "sub", "dec", "inc", "jmp", "call", "ret", "cmp" };
            List<Tuple<string, Color>> data = new List<Tuple<string, Color>>();
            string[] words = text.Split(' ', ',');
            string connect = "";
            string separator;
            int separatorOffset = 0;

            for(int i = 0; i < words.Length; i++)
            {
                if (words[i].Length + separatorOffset < text.Length)
                    separator = "" + text[words[i].Length + separatorOffset];
                else
                    separator = "";

                separatorOffset += words[i].Length + 1;

                if (keywords.Contains(words[i]))
                {
                    if (!string.IsNullOrEmpty(connect))
                        data.Add(new Tuple<string, Color>(connect, @default));

                    connect = separator;
                    data.Add(new Tuple<string, Color>(words[i], new Color(30, 140, 230, 255)));
                }
                else if(words[i].Length > 0 && words[i].All(p => char.IsDigit(p) || p == 'x'))
                {
                    if (!string.IsNullOrEmpty(connect))
                        data.Add(new Tuple<string, Color>(connect, @default));

                    connect = separator;
                    data.Add(new Tuple<string, Color>(words[i], new Color(150, 180, 150, 255)));
                }
                else
                    connect += words[i] + separator;
            }

            if (!string.IsNullOrEmpty(connect))
                data.Add(new Tuple<string, Color>(connect, @default));

            return data.ToArray();
        }

        public void Draw(GameTime gt)
        {
            if (!visible)
                return;

            if (texture == null)
            {
                texture = new Texture2D(Game.GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.White });
            }

            // drawing background
            Game.SpriteBatch.Draw(texture, new Rectangle(Position, Size), new Color(30, 30, 30, 255));

            Rectangle @default = Game.GraphicsDevice.ScissorRectangle;
            Game.GraphicsDevice.ScissorRectangle = new Rectangle(Position, Size);

            if (visible)
            {
                string[] lines = text.ToString().Split('\n');
                int line, pos;
                Vector2 size;

                // drawing selection
                if (selectionAnchor != -1)
                {
                    int startLine, startPos;

                    GetLineNumberAndPositionFromIndex(SelectionStart, out startLine, out startPos);
                    GetLineNumberAndPositionFromIndex(SelectionEnd, out line, out pos);

                    for (int i = startLine; i <= line; i++)
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

                        Game.SpriteBatch.Draw(texture, new Rectangle(Position.X - HorizontalScroll + (int)offset.X, Position.Y + i * LineHeight - VerticalScroll * LineHeight, (int)size.X, LineHeight), new Color(40, 80, 120, 255));
                    }
                }

                // drawing text
                GetLineNumberAndPositionFromIndex(caretPosition, out line, out pos);

                for (int i = VerticalScroll; i < lines.Length; i++)
                {
                    if (Position.Y + i * LineHeight - VerticalScroll * LineHeight > Size.Y)
                        break;

                    Tuple<string, Color>[] data = HighlightText(lines[i], new Color(220, 220, 220, 255));
                    float xpos = 0;

                    for(int j = 0; j < data.Length; j++)
                    {
                        Game.SpriteBatch.DrawString(Game.EditorFont, data[j].Item1, new Vector2(Position.X - HorizontalScroll + xpos, Position.Y + i * LineHeight - VerticalScroll * LineHeight), data[j].Item2, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                        xpos += Game.EditorFont.MeasureString(data[j].Item1).X * fontScale;
                    }

                    //Game.SpriteBatch.DrawString(Game.EditorFont, lines[i], new Vector2(Position.X - HorizontalScroll, Position.Y + i * LineHeight - VerticalScroll * LineHeight), new Color(220, 220, 220, 255), 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                }

                // drawing caret
                size = Game.EditorFont.MeasureString(lines[line].Substring(0, pos)) * fontScale;
                Rectangle caretRectangle = new Rectangle(Position.X - HorizontalScroll + (int)size.X, Position.Y + line * LineHeight - VerticalScroll * LineHeight, 1, LineHeight);

                caretAccumulator += (int)gt.ElapsedGameTime.TotalMilliseconds;

                if (caretAccumulator > 400)
                {
                    Game.SpriteBatch.Draw(texture, caretRectangle, Color.White);

                    if (caretAccumulator > 800)
                        caretAccumulator -= 800;
                }
            }

            Game.GraphicsDevice.ScissorRectangle = @default;

            
            foreach(IInputElement child in Children)
            {
                if(child is IRenderable)
                    ((IRenderable)child).Draw(gt);
            }
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            foreach(IInputElement child in Children)
            {
                if (child.Bounds.Contains(e.State.Position))
                    child.OnMouseMove(e);

                if (e.Handled)
                    break;
            }
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            foreach (IInputElement child in Children)
            {
                if (child.Bounds.Contains(e.State.Position))
                    child.OnMouseDown(e);

                if (e.Handled)
                    break;
            }
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            foreach (IInputElement child in Children)
            {
                if (child.Bounds.Contains(e.State.Position))
                    child.OnMouseUp(e);

                if (e.Handled)
                    break;
            }
        }
    }
}
