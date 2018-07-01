namespace KatlaSport.Services.HiveManagement.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KatlaSport.Services.HiveManagement.DTO;

    /// <summary>
    /// Represents a hive section service.
    /// </summary>
    public interface IHiveSectionService
    {
        /// <summary>
        /// Gets a list of hive sections.
        /// </summary>
        /// <returns>A <see cref="Task{List{HiveSectionListItem}}"/>.</returns>
        Task<List<HiveSectionListItem>> GetHiveSectionsAsync();

        /// <summary>
        /// Gets a hive section.
        /// </summary>
        /// <param name="id">A hive section identifier.</param>
        /// <returns>A <see cref="Task{HiveSection}"/>.</returns>
        Task<HiveSection> GetHiveSectionAsync(int id);

        /// <summary>
        /// Gets a list of hive sections for specified hive.
        /// </summary>
        /// <param name="hiveId">A hive identifier.</param>
        /// <returns>A <see cref="Task{List{HiveSectionListItem}}"/>.</returns>
        Task<List<HiveSectionListItem>> GetHiveSectionsAsync(int hiveId);

        /// <summary>
        /// Sets deleted status for a hive section.
        /// </summary>
        /// <param name="hiveSectionId">A hive section identifier.</param>
        /// <param name="deletedStatus">Status.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task SetStatusAsync(int hiveSectionId, bool deletedStatus);

        /// <summary>
        /// Creates a new hiveSection.
        /// </summary>
        /// <param name="hiveSection">A <see cref="UpdateHiveSectionRequest"/>.</param>
        /// <param name="hiveId">A hive identifier.</param>
        /// <returns>A <see cref="Task{HiveSection}"/>.</returns>
        Task<HiveSection> CreateHiveSectionAsync(UpdateHiveSectionRequest hiveSection, int hiveId);

        /// <summary>
        /// Updates an existed hiveSection.
        /// </summary>
        /// <param name="hiveSectionId">A hive section identifier.</param>
        /// <param name="hiveSection">A <see cref="UpdateHiveSectionRequest"/>.</param>
        /// <returns>A <see cref="Task{HiveSection}"/>.</returns>
        Task<HiveSection> UpdateHiveSectionAsync(int hiveSectionId, UpdateHiveSectionRequest hiveSection);

        /// <summary>
        /// Deletes an existed hive section.
        /// </summary>
        /// <param name="hiveSecionId">A hive section identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteHiveSectionAsync(int hiveSecionId);
    }
}
