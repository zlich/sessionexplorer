using System;
using System.Web.UI;

namespace SessionExplorer.Web
{

    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the FlushButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void FlushButton_Click(object sender, EventArgs e)
        {
            Utilities.CacheHelper<Entities.Session>.Flush();
        }
    }
}
