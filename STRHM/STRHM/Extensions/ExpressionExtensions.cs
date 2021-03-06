﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using STRHM.Attributes;

namespace STRHM.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<T>(this Expression<Func<T, object>> exp) where T : class
        {
            return exp.GetMemberExpression().Member.Name;
        }

        public static bool IsPropertySerializable<T>(this Expression<Func<T, object>> exp) where T : class
        {
            return exp.GetMemberExpression().Member
                .GetCustomAttributes(typeof(SerializableRedisPropertyAttribute), false)
                .Any();
        }

        private static MemberExpression GetMemberExpression<T>(this Expression<Func<T, object>> exp) where T : class
        {
            // source: https://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
            MemberExpression body = exp.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (body == null)
                throw new ArgumentException("Cannot get name of expression property");

            return body;
        }
    }
}
