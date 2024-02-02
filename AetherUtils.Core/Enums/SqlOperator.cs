using System.ComponentModel;

namespace AetherUtils.Core.Enums
{
    /// <summary>
    /// Represents the various operations that can be applied on search text and column names in SQL.
    /// </summary>
    public enum SqlOperator
    {

        /// <summary>
        /// &gt; - The column value is greater than the search value.
        /// </summary>
        [Description("{0} > {1}")]
        Greater,

        /// <summary>
        /// &lt; - The column value is less than the search value.
        /// </summary>
        [Description("{0} < {1}")]
        Less,

        /// <summary>
        /// = - The column value equals the search value.
        /// </summary>
        [Description("{0} = {1}")]
        Equals,

        /// <summary>
        /// %value.* - The column value begins with the search value.
        /// </summary>
        [Description("{0} LIKE {1}%")]
        StartsLike,

        /// <summary>
        /// .*value% - The column value ends with the search value.
        /// </summary>
        [Description("{0} LIKE %{1}")]
        EndsLike,

        /// <summary>
        /// %value% - The column value contains the search value.
        /// </summary>
        [Description("{0} LIKE %{1}%")]
        Like,

        /// <summary>
        /// NOT %value% - The column value does not contain the search value.
        /// </summary>
        [Description("{0} NOT LIKE {1}")]
        NotLike,

        /// <summary>
        /// &lt;= - The column value is less than or equal to the search value.
        /// </summary>
        [Description("{0} <= {1}")]
        LessOrEqual,

        /// <summary>
        /// &gt;= - The column value is greater than or equal to the search value.
        /// </summary>
        [Description("{0} >= {1}")]
        GreaterOrEqual,

        /// <summary>
        /// &lt;&gt; - The column value is not equal to the search value.
        /// </summary>
        [Description("{0} <> {1}")]
        NotEqual
    }
}
