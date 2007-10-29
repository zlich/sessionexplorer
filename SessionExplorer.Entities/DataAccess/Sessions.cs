using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using SessionExplorer.DataAccess;

namespace SessionExplorer.Entities.DataAccess
{
    internal class Sessions : IDataAccess
    {
        /// <summary>
        /// Loads the specified sessions.
        /// </summary>
        /// <param name="sessions">The sessions.</param>
        /// <param name="date">The date.</param>
        internal void Load(Entities.Sessions sessions, DateTime date)
        {
            Parameters parameters = new Parameters();
            parameters.Add("date", date);

            using (IDataReader dataReader = GetByParameters(parameters, "usp_GetDaysSessions"))
            {
                try
                {
                    Populate(dataReader, sessions);
                }
                catch (Exception ex)
                {
                    throw new DataException(string.Format("Failed to retrieve sessions from the database."), ex);
                }
            }
        }

        /// <summary>
        /// Populates the specified data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="sessions">The sessions.</param>
        private static void Populate(IDataReader dataReader, ICollection<Session> sessions)
        {
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    try
                    {
                        sessions.Add(
                            new Session(
                                dataReader.GetString(dataReader.GetOrdinal("SessionId")),
                                dataReader.GetValue(dataReader.GetOrdinal("SessionItemShort")) != DBNull.Value ? (byte[])dataReader.GetValue(dataReader.GetOrdinal("SessionItemShort")) : null,
                                dataReader.GetDateTime(dataReader.GetOrdinal("Created")),
                                dataReader.GetDateTime(dataReader.GetOrdinal("Expires"))));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to populate session collection.", e);
                    }
                }
            }
        }

        #region IDataAccess Members

        /// <summary>
        /// Retrieves the business entity from the database as a DataReader.
        /// </summary>
        /// <param name="id">The integer id of the entity.</param>
        /// <returns>A DataReader containing zero or one row</returns>
        public IDataReader GetById(int id)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Retrieves the business entity from the database as a DataReader.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// A DataReader containing zero or many rows
        /// </returns>
        public IDataReader GetByParameters(Parameters parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Retrieves the business entity from the database as a DataReader.
        /// And allows the required procedure to be specified
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <returns>
        /// A DataReader containing zero or many rows
        /// </returns>
        public IDataReader GetByParameters(Parameters parameters, string storedProcedure)
        {
            IDataReader dataReader;
            try
            {
                dataReader = DataHelper.ExecuteReader(parameters, storedProcedure, string.Format("Sessions{0}Environment", ConfigurationManager.AppSettings["Environment"]));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed to execute procedure: {0}.", storedProcedure), e);
            }
            return dataReader;
        }

        /// <summary>
        /// Saves the business entity, whose properties populate the parameters, to the database.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The integer id of the persisted entity</returns>
        public int Save(Parameters parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
