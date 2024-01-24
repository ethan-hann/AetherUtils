using AetherUtils.Core.Enums;
using AetherUtils.Core.Extensions;
using System.Text;

namespace AetherUtils.Core.Filtering
{
    /// <summary>
    /// Class representing a Filter statement using various <see cref="SqlOperator"/>.
    /// </summary>
    /// <remarks>
    /// Create a new Filter statement with the specified column name, <see cref="SqlOperator"/> operator, and filter value (search text).
    /// </remarks>
    /// <param name="columnName">The name of the column this statement is for.</param>
    /// <param name="sqlOperator">The <see cref="SqlOperators"/> that this statement applies.</param>
    /// <param name="filterValue">The search text to apply to the column using the specified <see cref="SqlOperators"/>.</param>
    public class SqlFilter(string columnName, SqlOperator sqlOperator, string initialFilterValue) : IFilter
    {
        /// <summary>
        /// The search text to apply to the column using the <see cref="SqlOperator"/> defined.
        /// </summary>
        public string FilterValue
        {
            get
            {
                return initialFilterValue;
            }
            set
            {
                initialFilterValue = value.Replace("'", "''");
            }
        }

        /// <summary>
        /// Get the final filter string for this <see cref="SqlFilter"/>.
        /// </summary>
        public string FilterString
        {
            get
            {
                return string.Format(sqlOperator.ToDescriptionString(), columnName, FilterValue);
            }
        }
    }
}
