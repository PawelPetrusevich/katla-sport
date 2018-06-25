using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions = System.Data.Entity.QueryableExtensions;

namespace KatlaSport.DataAccess
{
    using System;
    using System.Linq.Expressions;

    public static class QueriableExtensions
    {
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source)
            where T : class
        {
            if (source is EntitySet<T>)
            {
                source = (source as EntitySet<T>).DbSet;
            }

            return Extensions.ToListAsync(source);
        }

        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> source)
            where T : class
        {
            if (source is EntitySet<T>)
            {
                source = (source as EntitySet<T>).DbSet;
            }

            return Extensions.ToArrayAsync(source);
        }

        public static Task<int> CountAsync<T>(this IQueryable<T> source)
            where T : class
        {
            if (source is EntitySet<T>)
            {
                source = (source as EntitySet<T>).DbSet;
            }

            return Extensions.CountAsync(source);
        }

        public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
            where T : class
        {
            if (source is EntitySet<T>)
            {
                source = (source as EntitySet<T>).DbSet;
            }

            return Extensions.SingleOrDefaultAsync(source, predicate);
        }
    }
}
