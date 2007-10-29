using System;
using System.Configuration;
using SessionExplorer.Entities;

namespace SessionExplorer.Web.Controls
{
    public partial class SessionReport : UserControlBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            EnvironmentLabel.Text = ConfigurationManager.AppSettings["Environment"];
            ReportLabel.Text = Date.ToString("dddd, MMMM dd, yyyy");

            Sessions sessions = new Sessions(PageNumber, PageSize, Date);
            SessionReportGridView.DataSource = sessions;
            SessionReportGridView.DataBind();
            PageLinks1.NonPagedCount = sessions.NonPagedCount;
            PageLinks2.NonPagedCount = sessions.NonPagedCount;

            RetrievalStats.Text = sessions.RetrievalStats;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        protected static string GetSize(int size)
        {
            return size >= 1024 ? string.Format("{0}kb", size / 1024) : string.Format("{0}b", size);
        }
    }
}