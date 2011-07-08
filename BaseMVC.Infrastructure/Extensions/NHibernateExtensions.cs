using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;

namespace BaseMVC.Infrastructure.Extensions
{
    public static class NHibernateExtensions
    {
        /// <remarks>
        /// Taken from http://sessionfactory.blogspot.com/2011/02/getting-row-count-with-future-linq.html
        /// </remarks>
        public static IFutureValue<TResult> ToFutureValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IQueryable<TSource>, TResult>> selector) where TResult : struct
        {
            var provider = (NhQueryProvider)source.Provider;
            var method = ((MethodCallExpression)selector.Body).Method;
            var expression = Expression.Call(null, method, source.Expression);
            return (IFutureValue<TResult>)provider.ExecuteFuture(expression);
        }
    }
}
