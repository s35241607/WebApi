using System.Linq.Expressions;
using System.Reflection;

namespace WebApi.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OperatorAttribute : Attribute
    {
        public string Operator { get; }

        public OperatorAttribute(string op)
        {
            Operator = op;
        }
    }
    public class PredicateHelpers
    {
        public static Func<T, bool> BuildDynamicPredicate<T>(T filterModel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression body = Expression.Constant(true); // 默认为 true，以保留所有元素

            foreach (var property in typeof(T).GetProperties())
            {
                object value = property.GetValue(filterModel);
                if (value != null)
                {
                    OperatorAttribute opAttribute = property.GetCustomAttribute<OperatorAttribute>();
                    string op = opAttribute?.Operator ?? "=";

                    Expression left = Expression.Property(param, property);
                    Expression right = Expression.Constant(value, property.PropertyType);

                    Expression filterExpression;
                    switch (op)
                    {
                        case "=":
                            filterExpression = Expression.Equal(left, right);
                            break;
                        case "!=":
                            filterExpression = Expression.NotEqual(left, right);
                            break;
                        case ">=":
                            filterExpression = Expression.GreaterThanOrEqual(left, right);
                            break;
                        case "<=":
                            filterExpression = Expression.LessThanOrEqual(left, right);
                            break;
                        case ">":
                            filterExpression = Expression.GreaterThan(left, right);
                            break;
                        case "<":
                            filterExpression = Expression.LessThan(left, right);
                            break;
                        case "Contains":
                            filterExpression = GetContainsExpression(left, right);
                            break;
                        case "Intersect":
                            filterExpression = GetIntersectExpression(left, right);
                            break;
                        default:
                            throw new NotSupportedException($"Operator '{op}' is not supported.");
                    }

                    body = Expression.And(body, filterExpression);
                }
            }

            return Expression.Lambda<Func<T, bool>>(body, param).Compile();
        }

        // 创建包含表达式的方法，用于处理 Contains 运算符
        static Expression GetContainsExpression(Expression left, Expression right)
        {
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            if (containsMethod == null)
            {
                throw new NotSupportedException("Contains method not found.");
            }
            return Expression.Call(left, containsMethod, right);
        }

        // 创建交集表达式的方法，用于处理 Intersect 运算符
        static Expression GetIntersectExpression(Expression left, Expression right)
        {
            MethodInfo intersectMethod = typeof(Enumerable).GetMethods()
                .Where(m => m.Name == "Intersect" && m.IsStatic && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(left.Type.GetGenericArguments()[0]);
            return Expression.Call(intersectMethod, left, right);
        }
    }
}
