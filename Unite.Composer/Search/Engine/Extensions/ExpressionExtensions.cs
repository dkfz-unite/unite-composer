using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Unite.Composer.Search.Engine.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines member access expression with other expression.
        /// </summary>
        /// <typeparam name="T1">Source element type</typeparam>
        /// <typeparam name="T2">Source element member type</typeparam>
        /// <typeparam name="T3">Target element or value type</typeparam>
        /// <param name="source">Member access expression</param>
        /// <param name="target">Target expression</param>
        /// <returns>Combined expression in form 'Source.Member.Target'.</returns>
        public static Expression<Func<T1, T3>> Join<T1, T2, T3>(
            this Expression<Func<T1, T2>> source,
            Expression<Func<T2, T3>> target)
        {
            //var memberExpression = source.Body as MemberExpression;

            //if (memberExpression == null)
            //{
            //    throw new ArgumentException(nameof(source));
            //}

            var expression = Expression.Lambda<Func<T1, T3>>(target.Body, source.Parameters);
            var visitor = new RecombinationVisitor(source.Body, target.Parameters[0]);

            return (Expression<Func<T1, T3>>)visitor.Visit(expression);
        }


        private class RecombinationVisitor : ExpressionVisitor
        {
            //private MemberExpression _memberExpression;
            //private ParameterExpression _parameterExpression;

            //public RecombinationVisitor(MemberExpression memberExpression, ParameterExpression parameterExpression)
            //{
            //    _memberExpression = memberExpression;
            //    _parameterExpression = parameterExpression;
            //}

            //[return: NotNullIfNotNull("node")]
            //public override Expression Visit(Expression node)
            //{
            //    return base.Visit(node == _parameterExpression ? _memberExpression : node);
            //}

            private Expression _memberExpression;
            private ParameterExpression _parameterExpression;

            public RecombinationVisitor(Expression memberExpression, ParameterExpression parameterExpression)
            {
                _memberExpression = memberExpression;
                _parameterExpression = parameterExpression;
            }

            [return: NotNullIfNotNull("node")]
            public override Expression Visit(Expression node)
            {
                return base.Visit(node == _parameterExpression ? _memberExpression : node);
            }
        }
    }
}
