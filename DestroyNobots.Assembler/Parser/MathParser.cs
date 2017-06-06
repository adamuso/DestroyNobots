using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DestroyNobots.Assembler.Parser
{
    public class MathParser
    {
        Dictionary<string, int> priorities;

        public MathParser()
        {
            priorities = new Dictionary<string, int>();
            priorities["+"] = 0;
            priorities["-"] = 0;
            priorities["*"] = 1;
            priorities["/"] = 1;
            priorities["%"] = 1;
        }

        public int eval(string expression)
        {
            Queue<object> onp = toONP(expression);
            Stack<int> stack = new Stack<int>();

            while (onp.Count > 0)
            {
                if (onp.Peek() is int)
                {
                    stack.Push((int)onp.Dequeue());
                }
                else if (onp.Peek() is string)
                {
                    string cur = (string)onp.Dequeue();

                    if (priorities.ContainsKey(cur))
                    {
                        int a = stack.Pop();
                        int b = stack.Pop();

                        if (cur == "+")
                            stack.Push(b + a);
                        else if (cur == "-")
                            stack.Push(b - a);
                        else if (cur == "*")
                            stack.Push(b * a);
                        else if (cur == "/")
                            stack.Push(b / a);
                        else if (cur == "%")
                            stack.Push(b % a);
                    }
                }
            }

            return stack.Pop();
        }

        public Queue<object> toONP(string expression)
        {
            Queue<object> ret = new Queue<object>();
            string cur = "";
            Stack<string> stack = new Stack<string>();
            char c;
            int i = 0;

            while(i < expression.Length)
            {
                cur = "";
                c = expression[i];

                while (char.IsWhiteSpace(c))
                {
                    i++;
                    c = expression[i];
                }

                if (char.IsDigit(c))
                {
                    while (char.IsDigit(c))
                    {
                        cur += c;
                        i++;

                        if (i >= expression.Length)
                            c = '\0';
                        else
                            c = expression[i];
                    }

                    while (char.IsWhiteSpace(c))
                    {
                        i++;
                        c = expression[i];
                    }

                    ret.Enqueue(int.Parse(cur));
                }
                else if (char.IsLetter(c))
                {
                    while (char.IsLetterOrDigit(c))
                    {
                        cur += c;
                        i++;

                        if (i >= expression.Length)
                            c = '\0';
                        else
                            c = expression[i];
                    }

                    while (char.IsWhiteSpace(c))
                    {
                        i++;
                        c = expression[i];
                    }

                    stack.Push(cur);                    
                }
                else if (c == '+' || c == '-' || c == '%' || c == '*' || c == '/')
                {
                    cur += c;

                    string top = stack.Count > 0 ? stack.Peek() : "";

                    if(priorities.ContainsKey(top))
                        while (priorities[top] >= priorities[cur])
                        {
                            ret.Enqueue(stack.Pop());
                            top = stack.Count > 0 ? stack.Peek() : "";

                            if (!priorities.ContainsKey(top))
                                break;
                        }

                    stack.Push(cur);
                    i++;
                }
                else if (c == '(')
                {
                    cur += c;
                    stack.Push(cur);
                    i++;
                }
                else if (c == ')')
                {
                    i++;
                    string top = stack.Count > 0 ? stack.Peek() : "";

                    while (top != "(")
                    {
                        ret.Enqueue(stack.Pop());
                        top = stack.Count > 0 ? stack.Peek() : "";

                        if (!priorities.ContainsKey(top))
                            break;
                    }

                    stack.Pop();
                }
            }

            while (stack.Count > 0)
            {
                ret.Enqueue(stack.Pop());
            }

            return ret;
        }
    }
}
