using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestTool.GUI.Utils
{
    internal class ExpressionInfo
    {
        public string Expression { get; set; }
        public string Error { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }

    internal class FilterValidationInfo
    {
        public FilterValidationInfo()
        {
            IncorrectExpressions = new List<ExpressionInfo>();
        }

        public bool Valid { get; set; }
        public string Message { get; set; }
        public List<ExpressionInfo> IncorrectExpressions { get; private set; }
    }

    internal class FilterValidation
    {                
        class Range
        {
            public int Left { get; set; }
            public int Right { get; set; }
        }

        public static FilterValidationInfo Validatefilter(string expression)
        {
            FilterValidationInfo info = new FilterValidationInfo();
            info.Valid = true;
            if (!string.IsNullOrEmpty(expression))
            {

                List<ExpressionInfo> expressions = new List<ExpressionInfo>();
                ExpressionInfo rootExpression = new ExpressionInfo();
                int left = 0;
                while (expression[left] == ' ')
                {
                    left++;
                }
                int right = expression.Length - 1;
                while (expression[right] == ' ')
                {
                    right--;
                }
                rootExpression.Expression = expression;
                rootExpression.Left = left;
                rootExpression.Right = right;
                expressions.Add(rootExpression);

                ExtractExpressions(expressions, info);

                if (info.Valid)
                {
                    foreach (ExpressionInfo expr in expressions)
                    {
                        string err;
                        bool ok = IsValidExpression(expr.Expression, out err);
                        info.Valid &= ok;
                        if (!ok)
                        {
                            expr.Error = err;
                            info.IncorrectExpressions.Add(expr);
                        }
                    }
                }
            }

            return info;
        }
        
        static void ExtractExpressions(List<ExpressionInfo> expressions, FilterValidationInfo validationInfo)
        {
            int i = 0;
            while (i < expressions.Count)
            {
                bool removed = ParseNextExpression(i, expressions, validationInfo);
                if (!removed)
                {
                    i++;
                }
            }
        }

        static bool ParseNextExpression(int idx, List<ExpressionInfo> expressions, FilterValidationInfo validationInfo)
        {
            ExpressionInfo parentExpression = expressions[idx];
            string expression = expressions[idx].Expression.Trim();

            List<Range> constants = new List<Range>();
            List<Range> braces = new List<Range>();
            bool started = false;

            {
                int quotePosition = -1;

                Range range = null;
                while ((quotePosition = expression.IndexOf('"', quotePosition + 1)) != -1)
                {
                    started = !started;
                    if (started)
                    {
                        range = new Range();
                        range.Left = quotePosition;

                    }
                    else
                    {
                        range.Right = quotePosition;
                        constants.Add(range);
                    }
                }
            }
            if (started)
            {
                validationInfo.Valid = false;
                validationInfo.Message = "Constant not terminated";
                return false;
            }

            // braces
            int depth = 0;
            int lastPoint = 0;
            int leftBracePosition = -1;
            while (true)
            {
                int nextLeft = expression.IndexOf('(', lastPoint);
                int nextRight = expression.IndexOf(')', lastPoint);
                if (nextLeft == -1 && nextRight == -1)
                {
                    break;
                }

                if (nextLeft < nextRight && nextLeft != -1)
                {
                    if (depth == 0)
                    {
                        leftBracePosition = nextLeft;
                    }
                    lastPoint = nextLeft + 1;
                    depth++;
                }
                else
                {
                    depth--;
                    if (depth == 0)
                    {
                        Range range = new Range() { Left = leftBracePosition, Right = nextRight };
                        braces.Add(range);
                    }
                    lastPoint = nextRight + 1;
                }
            }

            if (depth != 0)
            {
                validationInfo.Valid = false;
                validationInfo.Message = "Bracket expected";
                return false;
            }

            Range enclosing = braces.Where(R => R.Left == 0 && R.Right == expression.Length - 1).FirstOrDefault();
            if (enclosing != null)
            {
                expressions.RemoveAt(idx);
                string expr2 = expression.Substring(1, expression.Length - 2).Trim();
                ExpressionInfo info = new ExpressionInfo();
                info.Expression = expr2;
                int left = 1;
                while (expression[left] == ' ')
                {
                    left++;
                }
                info.Left = parentExpression.Left + left;
                int right = 1;
                while (expression[expression.Length - right] == ' ')
                {
                    right++;
                }
                info.Right = parentExpression.Right - right;

                expressions.Add(info);

                return true;
            }


            {
                if (Split("or", true, expressions, idx, constants, braces, validationInfo))
                {
                    return true;
                }
            }

            {
                if (Split("and", true, expressions, idx, constants, braces, validationInfo))
                {
                    return true;
                }
            }

            {
                if (Split("not", false, expressions, idx, constants, braces, validationInfo))
                {
                    return true;
                }
                if (!validationInfo.Valid)
                {
                    return false;
                }
            }
            
            return false;
        }

        static bool Split(string op,
            bool binary,
            List<ExpressionInfo> expressions,
            int idx,
            List<Range> constants,
            List<Range> braces, 
            FilterValidationInfo validationInfo)
        {
            ExpressionInfo parentExpression = expressions[idx];
            string expression = parentExpression.Expression;
            int operatorPosition = expression.IndexOf(op, 0, StringComparison.InvariantCultureIgnoreCase);

            while (operatorPosition != -1)
            {
                Range quotes = constants.Where(R => R.Left < operatorPosition && R.Right > operatorPosition).FirstOrDefault();
                if (quotes == null)
                {
                    Range bracePair = braces.Where(R => R.Left < operatorPosition && R.Right > operatorPosition).FirstOrDefault();
                    if (bracePair == null)
                    {
                        if (!binary && operatorPosition != 0)
                        {
                            validationInfo.Valid = false;
                            validationInfo.Message = "Operator not found";
                            return false;
                        }
                        if (binary && operatorPosition == 0)
                        {
                            validationInfo.Valid = false;
                            validationInfo.Message = "Operand missing";
                            return false;
                        }
                        expressions.RemoveAt(idx);

                        if (binary)
                        {
                            string expr1 = expression.Substring(0, operatorPosition - 1).Trim();

                            ExpressionInfo info1 = new ExpressionInfo();
                            info1.Expression = expr1;
                            int left = 0;
                            while (expression[left] == ' ')
                            {
                                left++;
                            }
                            info1.Left = parentExpression.Left + left;
                            info1.Right = info1.Left + expr1.Length;
                            expressions.Add(info1);
                        }

                        {
                            string expr2 = expression.Substring(operatorPosition + op.Length).Trim();
                            ExpressionInfo info2 = new ExpressionInfo();
                            info2.Expression = expr2;
                            int left = operatorPosition + op.Length;
                            while (expression[left] == ' ')
                            {
                                left++;
                            }
                            info2.Left = parentExpression.Left + left;
                            info2.Right = info2.Left + info2.Expression.Length;
                            expressions.Add(info2);
                        }
                        return true;
                    }
                }

                operatorPosition = expression.IndexOf(op, operatorPosition + 1, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }


        static bool IsValidExpression(string expression, out string error)
        {
            error = string.Empty;

            if (expression.StartsWith("b", StringComparison.CurrentCultureIgnoreCase))
            {
                return IsValidBoolExpression(expression.ToLower(), out error);
            }
            else if (expression.StartsWith("c", StringComparison.CurrentCultureIgnoreCase))
            {
                return IsValidSearchExpression(expression.ToLower(), out error);
            }
            else
            {
                error = "expression should start with \"boolean\" or \"contains\"";
                return false;
            }

        }

        static bool IsValidBoolExpression(string expression, out string error)
        { 
            {
                Regex exprRegex = new Regex("^boolean(\\s)*\\(/");
                bool ok = exprRegex.IsMatch(expression);
                if (!ok)
                {
                    error = "expression should start with \"boolean(/\"";
                    return false;
                }
            }

            {
                Regex exprRegex = new Regex("^boolean(\\s)*\\(/(/([A-Za-z0-9])+)+(\\s)*\\[");
                bool ok = exprRegex.IsMatch(expression);
                if (!ok)
                {
                    error = "element path should be in form /ElementName[/ElementName]... ";
                    return false;
                }
            }

            {
                Regex exprRegex = new Regex("^boolean(\\s)*\\(/(/([A-Za-z0-9])+)+(\\s)*\\[@?([A-Za-z0-9])+(\\s)*");
                bool ok = exprRegex.IsMatch(expression);
                if (!ok)
                {
                    error = "Node name can contain only alphanumeric symbols and '@' ('@' only at the beginning)";
                    return false;
                }
            }

            {
                Regex exprRegex = new Regex("^boolean(\\s)*\\(/(/([A-Za-z0-9])+)+(\\s)*\\[@?([A-Za-z0-9])+(\\s)*(=|>|<|!=|<=|>=)");
                bool ok = exprRegex.IsMatch(expression);
                if (!ok)
                {
                    error = "operator can be one of the following: =, !=, >, >=, <, <=";
                    return false;
                }
            }

            Regex pathExprRegex = new Regex("^boolean(\\s)*\\(/(/([A-Za-z0-9])+)+(\\s)*\\[@?([A-Za-z0-9])+(\\s)*(=|>|<|!=|<=|>=)(\\s)*\"([A-Za-z0-9\\.\\-])+\"\\]\\)$");

            bool expressionOk = pathExprRegex.IsMatch(expression);

            if (!expressionOk)
            {
                error = "right operand should be quoted string";
            }
            else 
            {
                error = null;
            }
            return expressionOk;        
        }

        static bool IsValidSearchExpression(string expression, out string error)
        {
            error = null;

            {
                Regex exprRegex = new Regex("^contains(\\s)*\\(/");
                bool ok = exprRegex.IsMatch(expression);
                if (!ok)
                {
                    error = "expression should start with \"contains(/\"";
                    return false;
                }
            }

            {
                Regex exprRegex = new Regex("^^contains(\\s)*\\((\\s)*(/([A-Za-z0-9])+)+(\\s)*");
                bool ok = exprRegex.IsMatch(expression);
                if (!ok)
                {
                    error = "element path should be in form /ElementName[/ElementName]... ";
                    return false;
                }            
            }

            Regex pathExprRegex = new Regex("^contains(\\s)*\\((\\s)*(/([A-Za-z0-9])+)+(\\s)*\\,(\\s)*\"([A-Za-z0-9])+\"(\\s)*\\)$");

            bool expressionOk = pathExprRegex.IsMatch(expression);

            if (!expressionOk)
            {
                error = "right operand should be quoted string";
            }

            return expressionOk;
        }

    
    }
}
