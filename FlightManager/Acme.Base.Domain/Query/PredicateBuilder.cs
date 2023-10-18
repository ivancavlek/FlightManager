using System;
using System.Linq.Expressions;

namespace Acme.Base.Domain.Query;

/// <summary>
/// <see href="http://www.albahari.com/nutshell/predicatebuilder.aspx">PredicateBuilder class credits</see>
/// <see href="https://stackoverflow.com/questions/22406952/keep-getting-the-linq-expression-node-type-invoke-is-not-supported-in-linq-to/22407189#22407189">Corrections for PredicateBuilder class credits</see>
/// </summary>
public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() =>
        f => true;

    public static Expression<Func<T, bool>> False<T>() =>
        f => false;

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        if (leftExpression is null)
            throw new ArgumentNullException(nameof(leftExpression));
        if (rightExpression is null)
            throw new ArgumentNullException(nameof(rightExpression));

        var secondBody = rightExpression.Body.Replace(
            rightExpression.Parameters[0], leftExpression.Parameters[0]);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(leftExpression.Body, secondBody), leftExpression.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        if (leftExpression is null)
            throw new ArgumentNullException(nameof(leftExpression));
        if (rightExpression is null)
            throw new ArgumentNullException(nameof(rightExpression));

        var secondBody = rightExpression.Body.Replace(
            rightExpression.Parameters[0], leftExpression.Parameters[0]);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(leftExpression.Body, secondBody), leftExpression.Parameters);
    }

    public static Expression Replace(this Expression expression,
        Expression searchExpression, Expression replaceExpression) =>
            new ReplaceVisitor(searchExpression, replaceExpression).Visit(expression);

    private class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression _from;
        private readonly Expression _to;

        public ReplaceVisitor(Expression from, Expression to)
        {
            _from = from;
            _to = to;
        }
        public override Expression Visit(Expression node) =>
            node == _from ? _to : base.Visit(node);
    }
}