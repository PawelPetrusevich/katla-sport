namespace KatlaSport.Services.HiveManagement.DTO
{
    /// <summary>
    /// Represent for request creating or updating hive section.
    /// </summary>
    public class UpdateHiveSectionRequest
    {
        /// <summary>
        /// Gets or sets hive section name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets hive section code.
        /// </summary>
        public string Code { get; set; }
    }
}