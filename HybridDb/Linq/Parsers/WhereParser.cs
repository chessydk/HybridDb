﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HybridDb.Linq.Ast;

namespace HybridDb.Linq.Parsers
{
    internal class WhereParser : LambdaParser
    {
        public WhereParser(Stack<SqlExpression> ast) : base(ast) { }

        public SqlExpression Result
        {
            get { return ast.Peek(); }
        }

        public static SqlExpression Translate(Expression expression)
        {
            var ast = new Stack<SqlExpression>();
            new WhereParser(ast).Visit(expression);

            if (ast.Count == 0)
                return null;

            var sqlExpression = ast.Pop();
            sqlExpression = new ImplicitBooleanPredicatePropagator().Visit(sqlExpression);
            sqlExpression = new NullCheckPropagator().Visit(sqlExpression);

            return sqlExpression;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            Visit(expression.Left);
            var left = ast.Pop();

            Visit(expression.Right);
            var right = ast.Pop();

            SqlNodeType nodeType;
            switch (expression.NodeType)
            {
                case ExpressionType.And:
                    nodeType = SqlNodeType.BitwiseAnd;
                    break;
                case ExpressionType.AndAlso:
                    nodeType = SqlNodeType.And;
                    break;
                case ExpressionType.Or:
                    nodeType = SqlNodeType.BitwiseOr;
                    break;
                case ExpressionType.OrElse:
                    nodeType = SqlNodeType.Or;
                    break;
                case ExpressionType.LessThan:
                    nodeType = SqlNodeType.LessThan;
                    break;
                case ExpressionType.LessThanOrEqual:
                    nodeType = SqlNodeType.LessThanOrEqual;
                    break;
                case ExpressionType.GreaterThan:
                    nodeType = SqlNodeType.GreaterThan;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    nodeType = SqlNodeType.GreaterThanOrEqual;
                    break;
                case ExpressionType.Equal:
                    nodeType = SqlNodeType.Equal;
                    break;
                case ExpressionType.NotEqual:
                    nodeType = SqlNodeType.NotEqual;
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", expression.NodeType));
            }

            ast.Push(new SqlBinaryExpression(nodeType, left, right));

            return expression;
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    Visit(expression.Operand);
                    ast.Push(new SqlNotExpression(ast.Pop()));
                    break;
                case ExpressionType.Quote:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    Visit(expression.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", expression.NodeType));
            }

            return expression;
        }

        protected override void VisitColumnMethodCall(MethodCallExpression expression)
        {
            switch (expression.Method.Name)
            {
                case "StartsWith":
                    ast.Push(new SqlBinaryExpression(SqlNodeType.LikeStartsWith, ast.Pop(), ast.Pop()));
                    break;
                case "In":
                    ast.Push(new SqlBinaryExpression(SqlNodeType.In, ast.Pop(), ast.Pop()));
                    break;
                default:
                    throw new NotSupportedException(string.Format("The method '{0}' is not supported on column", expression.Method.Name));
            }
        }
    }
}