using System;

namespace SessionExplorer.Web.Controls
{
    public partial class ObjectList : System.Web.UI.UserControl
    {
        /// <summary>
        /// Sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public Entities.SessionItemCollection DataSource
        {
            set
            {
                SessionItemRepeater.DataSource = value;
                SessionItemRepeater.DataBind();
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e) { }
    }
}