// Author: Joël Luijmes
//
//
// Based on: http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ClashOfClans.Util
{
    static class Creator
    {
        public static BindingFlags Internal = BindingFlags.NonPublic | BindingFlags.Instance;
        public static BindingFlags Public = BindingFlags.Public;
    }

    static class Creator<T>
    {
        private static object _lock = new object();
        private static Dictionary<ConstructorInfo, ObjectCreator> _factories = new Dictionary<ConstructorInfo, ObjectCreator>();

        public delegate T ObjectCreator(params object[] args);

        public static ConstructorInfo GetConstructorInfo<T1>(BindingFlags bindingFlags)
            => typeof(T).GetConstructor(bindingFlags, null, new[] { typeof(T1) }, null);

        public static ConstructorInfo GetConstructorInfo<T1, T2>(BindingFlags bindingFlags)
            => typeof(T).GetConstructor(bindingFlags, null, new[] { typeof(T1), typeof(T2) }, null);

        public static ObjectCreator GetCreator(ConstructorInfo constructorInfo)
        {
            if (_factories.ContainsKey(constructorInfo))
                return _factories[constructorInfo];

            lock (_lock)
            {
                if (_factories.ContainsKey(constructorInfo))
                    return _factories[constructorInfo];

                var type = constructorInfo.DeclaringType;
                var parameters = constructorInfo.GetParameters();
                var parameter = Expression.Parameter(typeof(object[]), "args");

                var argumentExpression = new Expression[parameters.Length];
                for (var i = 0; i < argumentExpression.Length; ++i)
                {
                    var index = Expression.Constant(i);
                    var paramType = parameters[i].ParameterType;

                    var paramAccessorExpression = Expression.ArrayIndex(parameter, index);
                    var paramCastExpression = Expression.Convert(paramAccessorExpression, paramType);

                    argumentExpression[i] = paramCastExpression;
                }

                var newExpression = Expression.New(constructorInfo, argumentExpression);
                var lambda = Expression.Lambda(typeof(ObjectCreator), newExpression, parameter);
                var creator = (ObjectCreator)lambda.Compile();

                _factories[constructorInfo] = creator;

                return creator;
            }
        }
    }
}
