using System.ComponentModel;

namespace AetherUtils.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for manipulating WinForm <see cref="Control"/> objects.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Invoke the specified action on the specified control, if required.
        /// </summary>
        /// <typeparam name="T">The <see cref="Control"/> type.</typeparam>
        /// <param name="control">The <see cref="System.Windows.Forms.Control"/> to invoke an action on.</param>
        /// <param name="action">The <see cref="System.Action"/> to invoke on the control.</param>
        public static void InvokeIfRequired<T>(this T control, Action<T> action) where T : ISynchronizeInvoke
        {
            if (control.InvokeRequired)
                control.Invoke(new Action(() => action(control)), null);
            else
                action(control);
        }
        
        /// <summary>
        /// Resizes the columns of a <see cref="ListView"/> control to be a best-fit compromise between the header and the content.
        /// </summary>
        /// <param name="listView">The <see cref="ListView"/> to auto-size the columns of.</param>
        public static void ResizeColumns(this ListView listView)
        {
            //Prevents flickering
            listView.BeginUpdate();

            //Auto size using header
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //Grab column size based on header into a dictionary {index, width}
            var columnSize = listView.Columns.Cast<ColumnHeader>()
                .ToDictionary(colHeader => colHeader.Index, colHeader => colHeader.Width);

            //Auto size using data first
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            //Grab column size based on data and set max width
            foreach (ColumnHeader colHeader in listView.Columns)
                colHeader.Width = Math.Max(columnSize.GetValueOrDefault(colHeader.Index, 50),
                    colHeader.Width);

            listView.EndUpdate();
        }

        /// <summary>
        /// Apply a font to this control. This method is thread-safe.
        /// </summary>
        /// <param name="c">The <see cref="Control"/> to apply the font to.</param>
        /// <param name="font">The <see cref="Font"/> to apply to the control.</param>
        public static void ApplyFont(this Control c, Font font)
        {
            if (c.InvokeRequired)
                c.Invoke(() => { c.Font = font; });
            else
                c.Font = font;
        }
    }
}
