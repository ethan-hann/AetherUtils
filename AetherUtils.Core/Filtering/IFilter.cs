namespace AetherUtils.Core.Filtering
{
    /// <summary>
    /// Represents an interface to create filters.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Get the full filter string for this filter.
        /// </summary>
        public string FilterString
        {
            get;
        }
    }
}
