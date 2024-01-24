using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Filtering
{
    /// <summary>
    /// Represents an AND boolean filter statement.
    /// </summary>
    public class ANDFilter : IFilter
    {
        private readonly FilterExpressionList expressions = new();

        /// <summary>
        /// Create a new AND filter statement. The left and right hand sides of the supplied statements are conjoined with an AND between them.
        /// </summary>
        /// <param name="filterExpressionLeft">The left hand side of the statement.</param>
        /// <param name="filterExpressionRight">The right hand side of the statement.</param>
        public ANDFilter(IFilter filterExpressionLeft, IFilter filterExpressionRight)
        {
            expressions.Add(filterExpressionLeft);
            expressions.Add(filterExpressionRight);
        }

        /// <summary>
        /// Get the filter string for this statement.
        /// </summary>
        public string FilterString
        {
            get
            {
                var strFilter = string.Empty;
                StringBuilder sb = new();
                if (expressions.Count <= 0) return strFilter;
                
                for (var i = 0; i < expressions.Count - 1; i++)
                    sb = sb.Append(expressions[i].FilterString).Append(" AND ");

                sb = sb.Append(expressions[^1].FilterString);
                strFilter = sb.ToString();
                strFilter = $"({strFilter})";
                return strFilter;
            }
        }
    }
}
