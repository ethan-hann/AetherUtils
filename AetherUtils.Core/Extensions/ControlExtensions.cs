using System.ComponentModel;

namespace AetherUtils.Core.Extensions
{
    /// <summary>
    /// Contains extension methods for WinForm <see cref="Control"/> classes.
    /// </summary>
    public static class ControlExtensions
    {
        #region Generic Extensions
        /// <summary>
        /// Invoke the specified action on the specified control, if required.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control">The <see cref="System.Windows.Forms.Control"/> to invoke an action on.</param>
        /// <param name="action">The <see cref="System.Action"/> to invoke on the control.</param>
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            if (control.InvokeRequired)
                control.Invoke(new Action(() => action(control)), null);
            else
                action(control);
        }
        #endregion

        #region List View
        /// <summary>
        /// Resizes the columns of a <see cref="ListView"/> control to be a best-fit compromise between the header and the content.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> to auto-size the columns of.</param>
        public static void ResizeColumns(this ListView listView)
        {
            //Prevents flickering
            listView.BeginUpdate();

            Dictionary<int, int> columnSize = new Dictionary<int, int>();

            //Auto size using header
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //Grab column size based on header
            foreach (ColumnHeader colHeader in listView.Columns)
                columnSize.Add(colHeader.Index, colHeader.Width);

            //Auto size using data
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            //Grab comumn size based on data and set max width
            foreach (ColumnHeader colHeader in listView.Columns)
            {
                int nColWidth;
                if (columnSize.TryGetValue(colHeader.Index, out nColWidth))
                    colHeader.Width = Math.Max(nColWidth, colHeader.Width);
                else
                    //Default to 50
                    colHeader.Width = Math.Max(50, colHeader.Width);
            }

            listView.EndUpdate();
        }
        #endregion
    }
}
