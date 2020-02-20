//-----------------------------------------------------------------------
// <copyright file="AccountRL.cs" company="BridgeLabz">
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
    /// NotesRL class
    /// </summary>
    public class NotesRL : INotesRL 
    {
        /// <summary>
        /// Inject the configuration interface
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes the Notes class
        /// </summary>
        /// <param name="configuration">configuration parameter</param>
        public NotesRL (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Add Note method
        /// </summary>
        /// <param name="showNoteModel">showNoteModel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <returns>returns the added note</returns>
        public async Task<NoteModel> AddNote(ShowNoteModel showNoteModel, int userId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("NoteAdd", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Title", showNoteModel.Title);
                sqlCommand.Parameters.AddWithValue("@Description", showNoteModel.Description);
                sqlCommand.Parameters.AddWithValue("@Reminder", showNoteModel.Reminder);
                sqlCommand.Parameters.AddWithValue("@Color", showNoteModel.Color);
                sqlCommand.Parameters.AddWithValue("@Image", showNoteModel.Image);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@IsPin", showNoteModel.IsPin);
                sqlCommand.Parameters.AddWithValue("@IsArchive", showNoteModel.IsArchive);
                sqlCommand.Parameters.AddWithValue("@IsTrash", showNoteModel.IsTrash);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var userData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    userData = new NoteModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                }
                sqlDataReader.Close();

              //  var response = await sqlCommand.ExecuteNonQueryAsync();
                if (userData != null)
                {
                    var showResponse = new NoteModel()
                    {
                       Id = userData.Id,
                       Title=showNoteModel.Title,
                       Description=showNoteModel.Description,
                       Reminder=showNoteModel.Reminder,
                       Color=showNoteModel.Color,
                       Image=showNoteModel.Image,
                       CreatedDate=DateTime.Now,
                       IsPin=showNoteModel.IsPin,
                       IsArchive=showNoteModel.IsArchive,
                       IsTrash=showNoteModel.IsTrash,
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
        /// Update Note method
        /// </summary>
        /// <param name="showUpdateNoteModel">showUpdateNoteModel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the updated note</returns>
        public async Task<NoteModel> UpdateNote(ShowUpdateNoteModel showUpdateNoteModel, int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("NoteUpdate", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", noteId);
                sqlCommand.Parameters.AddWithValue("@Title", showUpdateNoteModel.Title);
                sqlCommand.Parameters.AddWithValue("@Description", showUpdateNoteModel.Description);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var userData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    userData = new NoteModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    userData.Title = sqlDataReader["Title"].ToString();
                    userData.Description = sqlDataReader["Description"].ToString();
                    userData.Reminder = Convert.ToDateTime(sqlDataReader["Reminder"]);
                    userData.Image = sqlDataReader["Image"].ToString();
                    userData.Color = sqlDataReader["Color"].ToString();
                    userData.CreatedDate=Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    userData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    userData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    userData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                }
                sqlDataReader.Close();

                //  var response = await sqlCommand.ExecuteNonQueryAsync();
                if (userData != null)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = userData.Id,
                        Title = showUpdateNoteModel.Title,
                        Description = showUpdateNoteModel.Description,
                        Reminder = userData.Reminder,
                        Color = userData.Color,
                        Image = userData.Image,
                        ModifiedDate = DateTime.Now,
                        IsPin = userData.IsPin,
                        IsArchive = userData.IsArchive,
                        IsTrash = userData.IsTrash,
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
        /// Get All Notes method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>returns the all notes</returns>
        public async Task<IList<NoteModel>> GetAllNotes(int userId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("GetAllNotes", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                IList<NoteModel> noteList = new List<NoteModel>();
                var userData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    userData = new NoteModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    userData.Title = sqlDataReader["Title"].ToString();
                    userData.Description = sqlDataReader["Description"].ToString();
                    userData.Reminder = Convert.ToDateTime(sqlDataReader["Reminder"]);
                    userData.Color = sqlDataReader["Color"].ToString();
                    userData.Image = sqlDataReader["Image"].ToString();
                    userData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    userData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    userData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    userData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    userData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    userData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                    noteList.Add(userData);
                }
                sqlDataReader.Close();
                return noteList;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Delete Note method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the deleted note</returns>
        public async Task<string> DeleteNote(int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("DeleteNote", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                var response = await sqlCommand.ExecuteNonQueryAsync();
                sqlConnection.Close();
                if (response > 0)
                {
                    return "Note Deleted Successfully";
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

       
    }
}
