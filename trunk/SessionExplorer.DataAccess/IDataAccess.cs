using System.Data;

namespace SessionExplorer.DataAccess
{
    public interface IDataAccess
    {
        /// <summary>
        /// Retrieves the business entity from the database as a DataReader.
        /// </summary>
        /// <param name="id">The integer id of the entity.</param>
        /// <returns>A DataReader containing zero or one row</returns>
        IDataReader GetById(int id);

        /// <summary>
        /// Retrieves the business entity from the database as a DataReader.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A DataReader containing zero or many rows</returns>
        IDataReader GetByParameters(Parameters parameters);

        /// <summary>
        /// Retrieves the business entity from the database as a DataReader.
        /// And allows the required procedure to be specified
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <returns>
        /// A DataReader containing zero or many rows
        /// </returns>
        IDataReader GetByParameters(Parameters parameters, string storedProcedure);

        /// <summary>
        /// Saves the business entity, whose properties populate the parameters, to the database.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The integer id of the persisted entity</returns>
        int Save(Parameters parameters);
    }
}
