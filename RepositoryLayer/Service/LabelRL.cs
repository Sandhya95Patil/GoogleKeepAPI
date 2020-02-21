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
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("AddLabel", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Label", showLabel.Label);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
          
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var userData = new LabelModel();
                while (sqlDataReader.Read())
                {
                    userData = new LabelModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                }
                sqlDataReader.Close();
                if (userData != null)
                {
                    var showResponse = new LabelModel()
                    {
                        Id = userData.Id,
                        Label = showLabel.Label,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        UserId = userId
                    };
                    return showResponse;
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
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
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
                }
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
