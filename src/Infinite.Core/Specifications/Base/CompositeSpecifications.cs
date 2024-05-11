using System.Linq.Expressions;

namespace Infinite.Core.Specifications.Base;

public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : BaseSpecification<T> where T: class, IEntity
{
    public Expression<Func<T, bool>> GetFilterExpression()
    {
        var leftExpression = left.FilterCondition;
        var rightExpression = right.FilterCondition;

        var paramExpr = Expression.Parameter(typeof(T));
        var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
        var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        return finalExpr;
    }

    public override Expression<Func<T, bool>> FilterCondition => GetFilterExpression();
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : BaseSpecification<T> where T: class, IEntity
{
    public Expression<Func<T, bool>> GetFilterExpression()
    {
        var leftExpression = left.FilterCondition;
        var rightExpression = right.FilterCondition;

        var paramExpr = Expression.Parameter(typeof(T));
        var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
        var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        return finalExpr;
    }

    public override Expression<Func<T, bool>> FilterCondition => GetFilterExpression();
}

public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    protected override Expression VisitParameter(ParameterExpression node)
        => base.VisitParameter(_parameter);

    internal ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }
}