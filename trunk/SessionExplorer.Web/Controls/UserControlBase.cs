using System;
using System.Web.UI;

namespace SessionExplorer.Web.Controls
{
    public class UserControlBase : UserControl
    {
        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>The page number.</value>
        protected int PageNumber
        {
            get { return string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : int.Parse(Request.QueryString["page"]); }
        }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        protected int PageSize
        {
            get { return string.IsNullOrEmpty(Request.QueryString["size"]) ? 10 : int.Parse(Request.QueryString["size"]); }
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>The date.</value>
        protected DateTime Date
        {
            get
            {
                DateTime date;
                try
                {
                    date = new DateTime(
                        int.Parse(Request.QueryString["date"].Substring(0, 4)),
                        int.Parse(Request.QueryString["date"].Substring(4, 2)),
                        int.Parse(Request.QueryString["date"].Substring(6, 2))
                        ).AddDays(1).AddSeconds(-1);

                }
                catch
                {
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1).AddSeconds(-1);
                }
                return date;
            }
        }

        /// <summary>
        /// Gets the unix date.
        /// </summary>
        /// <value>The unix date.</value>
        protected string UnixDate
        {
            get { return GetUnixDate(Date); }
        }

        /// <summary>
        /// Gets the unix date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string GetUnixDate(DateTime date)
        {
            return string.Format("{0}{1:00}{2:00}", date.Year, date.Month, date.Day);
        }
    }
}
