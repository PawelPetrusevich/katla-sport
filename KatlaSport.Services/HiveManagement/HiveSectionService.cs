using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KatlaSport.DataAccess;
using KatlaSport.DataAccess.ProductStoreHive;
using DbHiveSection = KatlaSport.DataAccess.ProductStoreHive.StoreHiveSection;

namespace KatlaSport.Services.HiveManagement
{
    using KatlaSport.Services.Properties;

    /// <summary>
    /// Represents a hive section service.
    /// </summary>
    public class HiveSectionService : IHiveSectionService
    {
        private readonly IProductStoreHiveContext _context;
        private readonly IUserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HiveSectionService"/> class with specified <see cref="IProductStoreHiveContext"/> and <see cref="IUserContext"/>.
        /// </summary>
        /// <param name="context">A <see cref="IProductStoreHiveContext"/>.</param>
        /// <param name="userContext">A <see cref="IUserContext"/>.</param>
        public HiveSectionService(IProductStoreHiveContext context, IUserContext userContext)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userContext = userContext ?? throw new ArgumentNullException();
        }

        /// <inheritdoc/>
        public async Task<List<HiveSectionListItem>> GetHiveSectionsAsync()
        {
            var dbHiveSections = await _context.Sections.OrderBy(s => s.Id).ToArrayAsync().ConfigureAwait(false);
            var hiveSections = dbHiveSections.Select(s => Mapper.Map<HiveSectionListItem>(s)).ToList();
            return hiveSections;
        }

        /// <inheritdoc/>
        public async Task<HiveSection> GetHiveSectionAsync(int hiveSectionId)
        {
            var dbHiveSection = await _context.Sections.FirstOrDefaultAsync(s => s.Id == hiveSectionId).ConfigureAwait(false);

            if (dbHiveSection == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveSectionNotFound);
            }

            return Mapper.Map<DbHiveSection, HiveSection>(dbHiveSection);
        }

        /// <inheritdoc/>
        public async Task<List<HiveSectionListItem>> GetHiveSectionsAsync(int hiveId)
        {
            var dbHive = await _context.Hives.FirstOrDefaultAsync(x => x.Id == hiveId).ConfigureAwait(false);

            if (dbHive == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveNotFound);
            }

            var dbHiveSections = await _context.Sections.Where(s => s.StoreHiveId == hiveId).OrderBy(s => s.Id).ToArrayAsync().ConfigureAwait(false);
            var hiveSections = dbHiveSections.Select(s => Mapper.Map<HiveSectionListItem>(s)).ToList();
            return hiveSections;
        }

        /// <inheritdoc/>
        public async Task SetStatusAsync(int hiveSectionId, bool deletedStatus)
        {
            var dbHiveSection = await _context.Sections.FirstOrDefaultAsync(x => x.Id == hiveSectionId).ConfigureAwait(false);

            if (dbHiveSection == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveSectionNotFound);
            }

            if (dbHiveSection.IsDeleted != deletedStatus)
            {
                dbHiveSection.IsDeleted = deletedStatus;
                dbHiveSection.LastUpdated = DateTime.UtcNow;
                dbHiveSection.LastUpdatedBy = _userContext.UserId;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<HiveSection> CreateHiveSectionAsync(UpdateHiveSectionRequest hiveSection, int hiveId)
        {
            var dbHiveSections = await _context.Sections.FirstOrDefaultAsync(x => x.Code == hiveSection.Code).ConfigureAwait(false);
            if (dbHiveSections != null)
            {
                throw new RequestedResourceHasConflictException(Resources.HiveSectionCodeConflict);
            }

            var dbHive = await _context.Hives.FirstOrDefaultAsync(x => x.Id == hiveId).ConfigureAwait(false);

            if (dbHive == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveSectionNotFound);
            }

            var dbHiveSection = Mapper.Map<DbHiveSection>(hiveSection);
            dbHiveSection.Created = DateTime.UtcNow;
            dbHiveSection.CreatedBy = _userContext.UserId;
            dbHiveSection.LastUpdatedBy = _userContext.UserId;
            dbHiveSection.StoreHiveId = hiveId;

            _context.Sections.Add(dbHiveSection);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<HiveSection>(dbHiveSection);
        }

        /// <inheritdoc/>
        public async Task<HiveSection> UpdateHiveSectionAsync(int hiveSectionId, UpdateHiveSectionRequest hiveSection)
        {
            var dbHivesSection = await _context.Sections.FirstOrDefaultAsync(p => p.Code == hiveSection.Code && p.Id != hiveSectionId).ConfigureAwait(false);
            if (dbHivesSection != null)
            {
                throw new RequestedResourceHasConflictException(Resources.HiveSectionCodeConflict);
            }

            dbHivesSection = await _context.Sections.FirstOrDefaultAsync(p => p.Id == hiveSectionId).ConfigureAwait(false);

            if (dbHivesSection == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveSectionNotFound);
            }

            Mapper.Map(hiveSection, dbHivesSection);
            dbHivesSection.LastUpdatedBy = _userContext.UserId;

            await _context.SaveChangesAsync();

            return Mapper.Map<HiveSection>(dbHivesSection);
        }

        /// <inheritdoc/>
        public async Task DeleteHiveSectionAsync(int hiveSecionId)
        {
            var dbHivesSection = await _context.Sections.FirstOrDefaultAsync(p => p.Id == hiveSecionId).ConfigureAwait(false);

            if (dbHivesSection == null)
            {
                throw new RequestedResourceNotFoundException(Resources.HiveSectionNotFound);
            }

            if (dbHivesSection.IsDeleted == false)
            {
                throw new RequestedResourceHasConflictException(Resources.HiveSectionStatusNotDelete);
            }

            _context.Sections.Remove(dbHivesSection);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
