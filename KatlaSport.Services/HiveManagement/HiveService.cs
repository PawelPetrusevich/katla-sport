using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KatlaSport.DataAccess.ProductStoreHive;
using DbHive = KatlaSport.DataAccess.ProductStoreHive.StoreHive;

namespace KatlaSport.Services.HiveManagement
{
    using System.Threading.Tasks;

    using KatlaSport.DataAccess;
    using KatlaSport.Services.Properties;

    /// <summary>
    /// Represents a hive service.
    /// </summary>
    public class HiveService : IHiveService
    {
        private readonly IProductStoreHiveContext _context;
        private readonly IUserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HiveService"/> class with specified <see cref="IProductStoreHiveContext"/> and <see cref="IUserContext"/>.
        /// </summary>
        /// <param name="context">A <see cref="IProductStoreHiveContext"/>.</param>
        /// <param name="userContext">A <see cref="IUserContext"/>.</param>
        public HiveService(IProductStoreHiveContext context, IUserContext userContext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userContext = userContext ?? throw new ArgumentNullException();
        }

        /// <inheritdoc/>
        public async Task<List<HiveListItem>> GetHivesAsync()
        {
            var dbHives = await _context.Hives.OrderBy(h => h.Id).ToArrayAsync().ConfigureAwait(false);
            var hives = dbHives.Select(h => Mapper.Map<HiveListItem>(h)).ToList();

            foreach (HiveListItem hive in hives)
            {
                hive.HiveSectionCount = _context.Sections.Where(s => s.StoreHiveId == hive.Id).Count();
            }

            return hives;
        }

        /// <inheritdoc/>
        public async Task<Hive> GetHiveAsync(int hiveId)
        {
            var dbHives = await _context.Hives.SingleOrDefaultAsync(h => h.Id == hiveId).ConfigureAwait(false);

            if (dbHives == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveNotFound);
            }

            return Mapper.Map<DbHive, Hive>(dbHives);
        }

        /// <inheritdoc/>
        public async Task<Hive> CreateHiveAsync(UpdateHiveRequest createRequest)
        {
            var hive = await _context.Hives.SingleOrDefaultAsync(h => h.Code == createRequest.Code).ConfigureAwait(false);

            if (hive != null)
            {
                throw new RequestedResourceHasConflictException(Resources.HiveCodeConflict);
            }

            var dbHive = Mapper.Map<UpdateHiveRequest, DbHive>(createRequest);

            dbHive.CreatedBy = _userContext.UserId;

            dbHive.LastUpdatedBy = _userContext.UserId;

            _context.Hives.Add(dbHive);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<Hive>(dbHive);
        }

        /// <inheritdoc/>
        public async Task<Hive> UpdateHiveAsync(int hiveId, UpdateHiveRequest updateRequest)
        {
            var dbHive = await _context.Hives.SingleOrDefaultAsync(p => p.Code == updateRequest.Code && p.Id != hiveId).ConfigureAwait(false);
            if (dbHive != null)
            {
                throw new RequestedResourceHasConflictException(Resources.HiveCodeConflict);
            }

            dbHive = await _context.Hives.SingleOrDefaultAsync(p => p.Id == hiveId).ConfigureAwait(false);

            if (dbHive == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveNotFound);
            }

            Mapper.Map(updateRequest, dbHive);
            dbHive.LastUpdatedBy = _userContext.UserId;

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<Hive>(dbHive);
        }

        /// <inheritdoc/>
        public async Task DeleteHiveAsync(int hiveId)
        {
            var dbHive = await _context.Hives.SingleOrDefaultAsync(p => p.Id == hiveId).ConfigureAwait(false);
            if (dbHive != null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveNotFound);
            }

            if (dbHive.IsDeleted == false)
            {
                throw new RequestedResourceHasConflictException(Resources.HiveStatusNotDelete);
            }

            _context.Hives.Remove(dbHive);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task SetStatusAsync(int hiveId, bool deletedStatus)
        {
            var dbHive = await _context.Hives.SingleOrDefaultAsync(x => x.Id == hiveId).ConfigureAwait(false);

            if (dbHive == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveNotFound);
            }

            if (dbHive.IsDeleted != deletedStatus)
            {
                dbHive.IsDeleted = deletedStatus;
                dbHive.LastUpdated = DateTime.UtcNow;
                dbHive.LastUpdatedBy = _userContext.UserId;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
