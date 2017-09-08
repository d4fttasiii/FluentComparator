using FluentComparator.Interfaces;
using FluentComparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentComparator
{
    public class Comparator
    {
        public static Comparator<T> Create<T>()
            where T : class
        {
            return new Comparator<T>();
        }
    }

    public class Comparator<T> :
        IComparatorInit<T>,
        IComparatorFirstAdded<T>,
        IComparatorSecondAdded<T>,
        IComparatorExecutable<T>
        where T : class
    {
        internal T _objectA;
        internal T _objectB;
        internal bool _storeDifferences;
        internal readonly List<string> _excludedProperties;

        internal Comparator()
        {
            _excludedProperties = new List<string>();
            _storeDifferences = false;
        }

        public IComparatorFirstAdded<T> Compare(T objectA)
        {
            _objectA = objectA;

            return this;
        }

        public IComparatorSecondAdded<T> To(T objectB)
        {
            _objectB = objectB;

            return this;
        }
        
        public IComparatorSecondAdded<T> ExcludeProperty(Expression<Func<T, object>> expression)
        {
            _excludedProperties.Add(GetCorrectPropertyName(expression));

            return this;
        }

        public IComparatorExecutable<T> EnableDifferences()
        {
            _storeDifferences = true;

            return this;
        }

        public IComparatorExecutable<T> DisableDifferences()
        {
            _storeDifferences = false;

            return this;
        }

        public ComparisonResult Evaluate()
        {
            var properties = _objectA.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var unequalProperties = properties
                .Where(prop => !_excludedProperties.Contains(prop.Name))
                .Where(prop => !Equals(prop, _objectA, _objectB));

            var result =  new ComparisonResult
            {
                IsEquivalent = unequalProperties.Count() == 0                
            };
            if (_storeDifferences)
            {
                result.Differences = unequalProperties.Select(prop => new Difference
                {
                    Name = prop.Name,
                    A = prop.GetValue(_objectA, null),
                    B = prop.GetValue(_objectB, null)
                });
            }            

            return result;
        }

        private bool Equals(PropertyInfo prop, object a, object b)
        {
            var AValue = prop.GetValue(a, null);
            var BValue = prop.GetValue(b, null);

            return Newtonsoft.Json.JsonConvert.SerializeObject(AValue) == Newtonsoft.Json.JsonConvert.SerializeObject(BValue);
        }

        private string GetCorrectPropertyName(Expression<Func<T, object>> expression) => GetPropertyPath(expression);

        private string GetPropertyPath(Expression expression)
        {
            switch (expression)
            {
                case MemberExpression memExpression:
                    var pre = GetPropertyPath(memExpression.Expression);
                    return (String.IsNullOrWhiteSpace(pre) ? "" : pre + ".") + memExpression.Member.Name;
                case LambdaExpression lambaExpression:
                    return GetPropertyPath(lambaExpression.Body);
                case MethodCallExpression callExpression:
                    if (callExpression.Method.Name != "Select" || callExpression.Method.Module.Name != "System.Linq.dll")
                    {
                        throw new ArgumentException("Method not supported for property path evaluation");
                    }
                    return string.Join(".", callExpression.Arguments.Select(a => GetPropertyPath(a)));
                case UnaryExpression unaryExpression:
                    return GetPropertyPath(unaryExpression.Operand);
                default:
                    return string.Empty;
            }
        }
    }
}
