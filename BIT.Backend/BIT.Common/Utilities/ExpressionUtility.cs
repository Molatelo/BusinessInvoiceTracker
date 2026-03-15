using System.Linq.Expressions;

namespace BIT.Common.Utilities;

public static class ExpressionUtility<T>
{
    public static Expression<Func<T, bool>>? BuildFilterExpression(string? filter, params string[] propertiesToInclude)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return null;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var expressions = new List<Expression>();

        var properties = propertiesToInclude?.Length > 0
        ? typeof(T).GetProperties().Where(p => propertiesToInclude.Contains(p.Name))
        : typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(filter);
                var containsMethod = typeof(string).GetMethod("Contains", [typeof(string), typeof(StringComparison)]);
                var comparisonConstant = Expression.Constant(StringComparison.OrdinalIgnoreCase);
                var containsExpression = Expression.Call(propertyAccess, containsMethod!, filterValue, comparisonConstant);
                var notNullExpression = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));
                var notNullAndContainsExpression = Expression.AndAlso(notNullExpression, containsExpression);
                expressions.Add(notNullAndContainsExpression);
            }
            else if (property.PropertyType == typeof(int) && int.TryParse(filter, out var intValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(intValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
            else if (property.PropertyType == typeof(long) && long.TryParse(filter, out var longValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(longValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
            else if (property.PropertyType == typeof(double) && double.TryParse(filter, out var doubleValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(doubleValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
            else if (property.PropertyType == typeof(bool) && bool.TryParse(filter, out var boolValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(boolValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
            else if (property.PropertyType == typeof(decimal) && decimal.TryParse(filter, out var decimalValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(decimalValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
            else if (property.PropertyType == typeof(Guid) && Guid.TryParse(filter, out var guidValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(guidValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
            else if (property.PropertyType == typeof(DateTime) && DateTime.TryParse(filter, out var dateTimeValue))
            {
                var propertyAccess = Expression.Property(parameter, property);
                var filterValue = Expression.Constant(dateTimeValue);
                var equalsExpression = Expression.Equal(propertyAccess, filterValue);
                expressions.Add(equalsExpression);
            }
        }

        if (expressions.Count == 0)
        {
            return null;
        }

        var combinedExpression = expressions.Aggregate(Expression.OrElse);
        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

        return lambda;
    }

    public static (LambdaExpression Expression, bool IsDescending)? BuildSortExpression(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return null;
        }

        var isDescending = false;
        var sortField = sortBy;

        if (sortBy.EndsWith(" desc", StringComparison.OrdinalIgnoreCase) ||
            sortBy.EndsWith(" descending", StringComparison.OrdinalIgnoreCase))
        {
            isDescending = true;
            sortField = sortBy[..sortBy.LastIndexOf(' ')].Trim();
        }
        else if (sortBy.EndsWith(" asc", StringComparison.OrdinalIgnoreCase) ||
                 sortBy.EndsWith(" ascending", StringComparison.OrdinalIgnoreCase))
        {
            sortField = sortBy[..sortBy.LastIndexOf(' ')].Trim();
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression propertyAccess = parameter;
        var properties = sortField.Split('.');

        try
        {
            foreach (var property in properties)
            {
                var propertyInfo = propertyAccess.Type.GetProperty(property) ?? throw new ArgumentException($"Property '{property}' not found on type '{propertyAccess.Type.Name}'");

                if (propertyInfo.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                {
                    var nullableCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));
                    var valueProperty = propertyInfo.PropertyType.GetProperty("Value");

                    if (valueProperty != null)
                    {
                        var propertyExpr = Expression.Property(propertyAccess, propertyInfo);
                        var valueExpr = Expression.Property(propertyExpr, valueProperty);

                        propertyAccess = Expression.Condition(
                            nullableCheck,
                            valueExpr,
                            Expression.Default(valueProperty.PropertyType)
                        );
                        continue;
                    }
                }

                if (!propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));
                    var propertyExpr = Expression.Property(propertyAccess, propertyInfo);

                    propertyAccess = Expression.Condition(
                        nullCheck,
                        propertyExpr,
                        Expression.Default(propertyInfo.PropertyType)
                    );
                }
                else
                {
                    propertyAccess = Expression.Property(propertyAccess, propertyInfo);
                }
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error building sort expression for '{sortBy}': {ex.Message}", ex);
        }

        var propertyType = propertyAccess.Type;
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propertyType);
        var lambda = Expression.Lambda(delegateType, propertyAccess, parameter);

        return (lambda, isDescending);
    }
}
