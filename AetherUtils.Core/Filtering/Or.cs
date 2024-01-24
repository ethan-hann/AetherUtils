using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AetherUtils.Core.Filtering
{
    public class Or
    {
        private readonly FilterExpressionList expressions = [];

        /// <summary>
        /// Create a new OR filter statement. The left and right hand sides of the supplied statements are conjoined with an OR between them.
        /// </summary>
        /// <param name="filterExpressionLeft">The left hand side of the statement.</param>
        /// <param name="filterExpressionRight">The right hand side of the statement.</param>
        public Or(IFilter filterExpressionLeft, IFilter filterExpressionRight)
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
                string strFilter = string.Empty;
                StringBuilder sb = new();
                if (expressions.Count > 0)
                {
                    for (int i = 0; i < expressions.Count - 1; i++)
                        sb = sb.Append(expressions[i].FilterString).Append(" OR ");

                    sb = sb.Append(expressions[^1].FilterString);
                    strFilter = sb.ToString();
                    strFilter = string.Format("({0})", strFilter);
                }
                return strFilter;
            }
        }
    }
}
