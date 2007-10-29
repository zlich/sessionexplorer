using System;

namespace SessionExplorer.Web.Controls
{
    public partial class PageLinks : UserControlBase
    {
        private int nonPagedCount;
        protected int lastPage;

        /// <summary>
        /// Sets the non paged count.
        /// </summary>
        /// <value>The non paged count.</value>
        public int NonPagedCount
        {
            set
            {
                nonPagedCount = value;
                if (nonPagedCount > 0)
                {
                    lastPage = (int)Math.Ceiling((decimal)nonPagedCount / PageSize);
                    LastLink.NavigateUrl = string.Format("{0}?page={1}&date={2}", Request.FilePath, lastPage, UnixDate);
                    LastImageLink.NavigateUrl = LastLink.NavigateUrl;
                    LastLink.Enabled = (PageNumber < lastPage);
                    LastImageLink.Enabled = LastLink.Enabled;
                    LastImageLink.ImageUrl = LastImageLink.Enabled ? "~/Images/last-active.png" : "~/Images/last-inactive.png";

                    NextLink.Enabled = (PageNumber < lastPage);
                    NextImageLink.Enabled = NextLink.Enabled;
                    NextImageLink.ImageUrl = NextImageLink.Enabled ? "~/Images/next-active.png" : "~/Images/next-inactive.png";

                    int firstOnPage = ((PageNumber * PageSize) - PageSize) + 1;
                    int lastOnPage = Math.Min((PageNumber * PageSize), nonPagedCount);
                    EntriesLabel.Text = string.Format("Entries {0} - {1} of {2}", firstOnPage, lastOnPage, nonPagedCount);
                }
                else
                {
                    Visible = false;
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FirstLink.Enabled = !PageNumber.Equals(1);
            FirstImageLink.Enabled = FirstLink.Enabled;
            FirstImageLink.ImageUrl = FirstImageLink.Enabled ? "~/Images/first-active.png" : "~/Images/first-inactive.png";
            PreviousLink.Enabled = !PageNumber.Equals(1);
            PreviousImageLink.Enabled = PreviousLink.Enabled;
            PreviousImageLink.ImageUrl = PreviousImageLink.Enabled ? "~/Images/previous-active.png" : "~/Images/previous-inactive.png";

            FirstLink.NavigateUrl = string.Format("{0}?page={1}&date={2}", Request.FilePath, 1, UnixDate);
            FirstImageLink.NavigateUrl = FirstLink.NavigateUrl;
            PreviousLink.NavigateUrl = string.Format("{0}?page={1}&date={2}", Request.FilePath, PageNumber - 1, UnixDate);
            PreviousImageLink.NavigateUrl = PreviousLink.NavigateUrl;
            NextLink.NavigateUrl = string.Format("{0}?page={1}&date={2}", Request.FilePath, PageNumber + 1, UnixDate);
            NextImageLink.NavigateUrl = NextLink.NavigateUrl;
        }
    }
}