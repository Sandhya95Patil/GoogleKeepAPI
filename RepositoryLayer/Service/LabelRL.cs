//-----------------------------------------------------------------------
// <copyright file="ILabelBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Service
{
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// LabelRL class
    /// </summary>
    public class LabelRL : ILabelRL
    {
        /// <summary>
        /// Inject the IConfiguration interface
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes the memory for Label class
        /// </summary>
        /// <param name="configuration">configuration parameter</param>
        public LabelRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection GetConnection(string connectionName)
        {
            SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
            sqlConnection.Open();
            return sqlConnection;
        }

        public SqlCommand GetCommand(string command)
        {
            string connection = "";
            SqlConnection sqlConnection = GetConnection(connection);
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand;
        }

        class StoredProcedureParameterData
        {
            public StoredProcedureParameterData(string name, dynamic value)
            {
                this.name = name;
                this.value = value;
            }

            public string name { get; set; }
            public dynamic value { get; set; }
        }
        private async Task<DataTable> StoredProcedureExecuteReader(string spName, IList<StoredProcedureParameterData> spParams)
        {
            try
            {
                SqlCommand command = GetCommand(spName);
                for (int i = 0; i < spParams.Count; i++)
                {
                    command.Parameters.AddWithValue(spParams[i].name, spParams[i].value);
                }
                DataTable table = new DataTable();
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                table.Load(dataReader);
                return table;
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// Add Label method
        /// </summary>
        /// <param name="showLabel">showLabel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <returns>returns the added label</returns>
        public async Task<LabelModel> AddLabel(ShowLabelModel showLabel, int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Label", showLabel.Label));
                paramList.Add(new StoredProcedureParameterData("@CreatedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@ModifiedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable dataTable = await StoredProcedureExecuteReader("AddLabel", paramList);
                var labelData = new LabelModel();
                 
                foreach (DataRow row in dataTable.Rows)
                {
                    labelData = new LabelModel();
                    labelData.Id = (int)row["Id"];
                    labelData.Label = row["Label"].ToString();
                    labelData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    labelData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    labelData.UserId = (int)row["UserId"];
                }
                if (labelData.Label != null)
                {
                    return labelData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Update Label method
        /// </summary>
        /// <param name="showLabelModel">showLabelModel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>return the updated label</returns>
        public async Task<LabelModel> UpdateLabel(ShowLabelModel showLabelModel, int userId, int labelId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Id",labelId));
                paramList.Add(new StoredProcedureParameterData("@Label", showLabelModel.Label));
                paramList.Add(new StoredProcedureParameterData("@ModifiedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                return null;
                /*SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("UpdateLabel", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", labelId);
                sqlCommand.Parameters.AddWithValue("@Label", showLabelModel.Label);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var labelData = new LabelModel();
                while (sqlDataReader.Read())
                {
                    labelData = new LabelModel();
                    labelData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    labelData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                }
                sqlDataReader.Close();
                if (labelData != null)
                {
                    var showResponse = new LabelModel()
                    {
                        Id = labelData.Id,
                        Label = showLabelModel.Label,
                        CreatedDate = labelData.CreatedDate,
                        ModifiedDate = DateTime.Now,
                        UserId = userId
                    };
                    return showResponse;
                }
                else
                {
                    return null;
                }*/
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Delete Label method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>return the deleted label</returns>
        public async Task<string> DeleteLabel(int userId, int labelId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("DeleteLabel", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", labelId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                var response = await sqlCommand.ExecuteNonQueryAsync();
                sqlConnection.Close();
                if (response > 0)
                {
                    return "Label Deleted Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Get All Labels method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the all labels</returns>
        public async Task<IList<LabelModel>> GetAllLabels(int userId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("GetAllLabels", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                IList<LabelModel> labelList = new List<LabelModel>();
                var labelData = new LabelModel();
                while (sqlDataReader.Read())
                {
                    labelData = new LabelModel();
                    labelData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    labelData.Label = sqlDataReader["Label"].ToString();
                    labelData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    labelData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    labelData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                    labelList.Add(labelData);
                }
                sqlDataReader.Close();
                return labelList;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
