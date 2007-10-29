using System;

namespace SessionExplorer.Web.Controls
{
    public partial class DateLinks : UserControlBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PreviousDayLink.NavigateUrl = string.Format("{0}?page={1}&date={2}", Request.FilePath, 1, GetUnixDate(Date.AddDays(-1)));
            NextDayLink.NavigateUrl = string.Format("{0}?page={1}&date={2}", Request.FilePath, 1, GetUnixDate(Date.AddDays(1)));

            PreviousDayLink.Text = string.Format("&lsaquo; {0}", Date.AddDays(-1).ToShortDateString());
            NextDayLink.Text = string.Format("{0} &rsaquo;", Date.AddDays(1).ToShortDateString());

            if (Date.Year.Equals(DateTime.Now.Year) && (Date.DayOfYear >= DateTime.Now.DayOfYear))
                NextDayLink.Visible = false;
        }
    }
}