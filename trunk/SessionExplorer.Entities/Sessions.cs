using System;
using System.Collections.Generic;

using SessionExplorer.Utilities;

namespace SessionExplorer.Entities
{
    public class Sessions : List<Session>
    {
        private int nonPagedCount;
        private string retrievalStats;

        /// <summary>
        /// Gets or sets the non paged count.
        /// </summary>
        /// <value>The non paged count.</value>
        public int NonPagedCount
        {
            get { return nonPagedCount; }
            set { nonPagedCount = value; }
        }

        /// <summary>
        /// Gets or sets the retrieval stats.
        /// </summary>
        /// <value>The retrieval stats.</value>
        public string RetrievalStats
        {
            get { return retrievalStats; }
            set { retrievalStats = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sessions"/> class.
        /// </summary>
        public Sessions() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Sessions"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public Sessions(DateTime date)
        {
            string cacheKey = string.Format("Sessions - {0:d}", date);
            DateTime timerStart = DateTime.Now;
            Sessions sessions = CacheHelper<Sessions>.Get(cacheKey);
            if (sessions != null && sessions.Count > 0)
            {
                foreach (Session session in sessions)
                    Add(session);
                retrievalStats =
                    string.Format("Fetched {0:0,0} entries from the cache in {1:g} seconds.", Count,
                                  (DateTime.Now.Ticks - timerStart.Ticks) / 10000000f);
            }
            else
            {
                timerStart = DateTime.Now;
                new DataAccess.Sessions().Load(this, new DateTime(date.Year, date.Month, date.Day).AddDays(1).AddSeconds(-1));
                retrievalStats = string.Format("Fetched {0:0,0} entries from the database in {1:g} seconds.", Count, (DateTime.Now.Ticks - timerStart.Ticks) / 10000000f);
                if (Count > 0)
                    CacheHelper<Sessions>.Add(cacheKey, this);
            }
            nonPagedCount = Count;
            Sort(delegate(Session s1, Session s2) { return s2.Created.CompareTo(s1.Created); });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sessions"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="date">The date.</param>
        public Sessions(int pageNumber, int pageSize, DateTime date)
        {
            Sessions allSessions = new Sessions(date);
            nonPagedCount = allSessions.Count;
            retrievalStats = allSessions.RetrievalStats;

            //filter on page
            int firstIndex = (pageNumber - 1) * pageSize;
            int lastIndex = ((firstIndex + pageSize) <= allSessions.Count) ? (firstIndex + pageSize) : allSessions.Count;
            for (int i = firstIndex; i < lastIndex; i++)
                Add(allSessions[i]);
        }
    }
}
