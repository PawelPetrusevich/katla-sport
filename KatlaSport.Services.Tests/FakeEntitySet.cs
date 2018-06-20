using System.Collections.Generic;
using System.Linq;
using KatlaSport.DataAccess;

namespace KatlaSport.Services.Tests
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Moq;

    internal class FakeEntitySet<TEntity> : EntitySetBase<TEntity>
    where TEntity : class
    {
        private readonly IList<TEntity> _list;
        private readonly IQueryable<TEntity> _queryable;

        private readonly DbSet<TEntity> _dbSet;

        public FakeEntitySet(IList<TEntity> list)
        {
            _list = list;
            _queryable = list.AsQueryable();
            var mockSet = new Mock<DbSet<TEntity>>();
            mockSet.As<IDbAsyncEnumerable<TEntity>>().Setup(x => x.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<TEntity>(_queryable.GetEnumerator()));

            mockSet.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<TEntity>(_queryable.Provider));

            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(_queryable.Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(_queryable.ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(_queryable.GetEnumerator());

            _dbSet = mockSet.Object;
        }

        protected override IQueryable<TEntity> Queryable => _dbSet;

        public override TEntity Add(TEntity entity)
        {
            _list.Add(entity);
            return entity;
        }

        public override TEntity Remove(TEntity entity)
        {
            _list.Remove(entity);
            return entity;
        }
    }
}
