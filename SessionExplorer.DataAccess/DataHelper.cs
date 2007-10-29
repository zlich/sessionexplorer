using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SessionExplorer.DataAccess
{
    public class DataHelper
    {
        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(Parameters parameters, string procedureName, string connectionStringKey)
        {
            IDataReader dataReader;
            Database database = DatabaseFactory.CreateDatabase(connectionStringKey);
            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(procedureName))
                {
                    InsertCommandParameters(dbCommand, parameters);
                    dataReader = database.ExecuteReader(dbCommand);
                }
            }
            catch (SqlException exception)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), exception);
            }
            catch (Exception exception)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), exception);
            }

            return dataReader;
        }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(Parameters parameters, string procedureName, string connectionStringKey)
        {
            DataSet dataSet;
            Database database = DatabaseFactory.CreateDatabase(connectionStringKey);
            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(procedureName))
                {
                    InsertCommandParameters(dbCommand, parameters);
                    dataSet = database.ExecuteDataSet(dbCommand);
                }
            }
            catch (SqlException exception)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), exception);
            }
            catch (Exception exception)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), exception);
            }

            return dataSet;
        }

        /// <summary>
        /// Executes the specified stored procedure using the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        public static void ExecuteNonQuery(Parameters parameters, string procedureName, string connectionStringKey)
        {
            Database database = DatabaseFactory.CreateDatabase(connectionStringKey);
            try
            {
                DbCommand dbCommand = database.GetStoredProcCommand(procedureName);
                InsertCommandParameters(dbCommand, parameters);
                database.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), ex);
            }
        }

        /// <summary>
        /// Executes the query and returns any output parameters in a dictionary using the parameter name as the dictionary key.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns>Dictionary object populated with the output parameters associated with the stored procedure executed. Access the parameters using their original names</returns>
        public static Dictionary<String, Object> ExecuteNonQueryWithOutputParams(Parameters parameters, string procedureName, string connectionStringKey)
        {
            Dictionary<String, Object> outputParameters = new Dictionary<String, Object>();

            Database database = DatabaseFactory.CreateDatabase(connectionStringKey);
            try
            {
                DbCommand dbCommand = database.GetStoredProcCommand(procedureName);
                InsertCommandParameters(dbCommand, parameters);
                database.ExecuteNonQuery(dbCommand);

                foreach (IDataParameter parameter in dbCommand.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                    {
                        outputParameters.Add(parameter.ParameterName, parameter.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), ex);
            }

            return outputParameters;
        }


        /// <summary>
        /// Executes the specified stored procedure using the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="connectionStringKey">The connection string key.</param>
        /// <returns></returns>
        public static int ExecuteScalar(Parameters parameters, string procedureName, string connectionStringKey)
        {
            int id;
            Database database = DatabaseFactory.CreateDatabase(connectionStringKey);
            try
            {
                DbCommand dbCommand = database.GetStoredProcCommand(procedureName);
                InsertCommandParameters(dbCommand, parameters);
                id = int.Parse(database.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception ex)
            {
                throw new DataException(string.Format("Error executing procedure: {0}", procedureName), ex);
            }
            return id;
        }


        /// <summary>
        /// Returns a connection based on the setting in the config file for the specified name.
        /// </summary>
        /// <param name="connectionName">The name of the connection in the config file.</param>
        public static DbConnection GetConnection(string connectionName)
        {
            Database database = DatabaseFactory.CreateDatabase(connectionName);
            return database.CreateConnection();
        }

        /// <summary>
        /// Inserts the command parameters.
        /// </summary>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="parameters">The parameters.</param>
        private static void InsertCommandParameters(DbCommand dbCommand, List<Parameter> parameters)
        {
            parameters.ForEach(
                delegate(Parameter parameter)
                {
                    DbParameter p = dbCommand.CreateParameter();
                    p.ParameterName = parameter.Key;
                    p.Value = IsPopulated(parameter.Value) ? parameter.Value : DBNull.Value;
                    p.Direction = parameter.ParameterDirection;
                    dbCommand.Parameters.Add(p);
                }
            );
        }

        /// <summary>
        /// Determines whether the specified object is populated.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns>
        /// 	<c>true</c> if the specified object is populated; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPopulated(Object o)
        {
            if (o == null)
            {
                return false;
            }
            switch (o.GetType().ToString())
            {
                case "System.Guid":
                    return (Guid)o != new Guid();
                case "System.DateTime":
                    return (DateTime)o != new DateTime();
                case "System.String":
                    return (String)o != String.Empty;
                //case "System.Int32":
                //    return (Int32)o != new Int32();
                default:
                    return o != null;
            }
        }

        /// <summary>
        /// Checks if the specified data reader contains the specified column.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static bool ColumnExists(IDataReader dataReader, string columnName)
        {
            return dataReader.GetSchemaTable().Columns.Contains(columnName);
            //dataReader.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            //return (dataReader.GetSchemaTable().DefaultView.Count > 0);
        }

        #region Nullables

        /// <summary>
        /// Gets the nullable String from data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static string GetNullableString(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : dataReader.GetString(dataReader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Gets the nullable char.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static char? GetNullableChar(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : (char?)dataReader.GetChar(dataReader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Gets the nullable int.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static int? GetNullableInt(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : (int?)dataReader.GetInt32(dataReader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Gets the nullable byte.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static byte? GetNullableByte(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : (byte?)dataReader.GetByte(dataReader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Gets the nullable GUID from data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static Guid? GetNullableGuid(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : (Guid?)dataReader.GetGuid(dataReader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Gets the nullable float.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static float? GetNullableFloat(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : (float?)dataReader.GetFloat(dataReader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Gets the nullable date from data reader.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public static DateTime? GetNullableDate(IDataReader dataReader, string fieldName)
        {
            return dataReader.IsDBNull(dataReader.GetOrdinal(fieldName)) ?
                       null : (DateTime?)dataReader.GetDateTime(dataReader.GetOrdinal(fieldName));
        }

        #endregion
    }
}
