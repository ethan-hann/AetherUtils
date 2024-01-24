using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
