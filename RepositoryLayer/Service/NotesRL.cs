//-----------------------------------------------------------------------
// <copyright file="NotesRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Service
{
    using CommonLayer.ImageUpload;
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.ShowModel;
    using Microsoft.AspNetCore.Http;
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
        public async Task<UpdateResponseModel> UpdateNote(ShowUpdateNoteModel showUpdateNoteModel, int userId, int noteId)
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
               
                var response = await sqlCommand.ExecuteNonQueryAsync();
                if (response > 0)
                {
                    var showResponse = new UpdateResponseModel()
                    {
                        NoteId = noteId,
                        Title = showUpdateNoteModel.Title,
                        Description = showUpdateNoteModel.Description,
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

        public async Task<NoteModel> ArchiveNote(int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("ArchiveNote", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                }
                if (noteData != null)
                {
                    var showresponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Reminder = noteData.Reminder,
                        Color = noteData.Color,
                        Image = noteData.Image,
                        CreatedDate=noteData.CreatedDate,
                        ModifiedDate=noteData.ModifiedDate,
                        IsPin = noteData.IsPin,
                        IsArchive = noteData.IsArchive,
                        IsTrash = noteData.IsTrash,
                        UserId = userId
                    };
                    return showresponse;
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

        public async Task<NoteModel> TrashNote(int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("TrashNote", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                }
                if (noteData != null)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Reminder = noteData.Reminder,
                        Color = noteData.Color,
                        Image = noteData.Image,
                        CreatedDate= noteData.CreatedDate,
                        ModifiedDate=noteData.ModifiedDate,
                        IsPin = noteData.IsPin,
                        IsArchive = noteData.IsArchive,
                        IsTrash = noteData.IsTrash,
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

        public async Task<NoteModel> PinNote(int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("PinNote", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                }
                if (noteData != null)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Reminder = noteData.Reminder,
                        Color = noteData.Color,
                        Image = noteData.Image,
                        CreatedDate = noteData.CreatedDate,
                        ModifiedDate = noteData.ModifiedDate,
                        IsPin = noteData.IsPin,
                        IsArchive = noteData.IsArchive,
                        IsTrash = noteData.IsTrash,
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

        public async Task<NoteModel> ChangeColor(int userId, int noteId, ColorModel colorModel)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("ColorNote", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@Color", colorModel.Color);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["Id"]);
                }
                if (noteData != null)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Reminder = noteData.Reminder,
                        Image = noteData.Image,
                        Color = noteData.Color,
                        CreatedDate = noteData.CreatedDate,
                        ModifiedDate = noteData.ModifiedDate,
                        IsPin = noteData.IsPin,
                        IsArchive = noteData.IsArchive,
                        IsTrash = noteData.IsTrash,
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

        public async Task<NoteModel> AddReminder(int userId, int noteId, DateTime dateTime)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("AddReminder", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@Reminder", dateTime);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = Convert.ToDateTime(sqlDataReader["Reminder"]);
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.CreatedDate=Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                }
                if (noteData != null)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Reminder = noteData.Reminder,
                        Image = noteData.Image,
                        Color = noteData.Color,
                        CreatedDate = noteData.CreatedDate,
                        ModifiedDate = noteData.ModifiedDate,
                        IsPin = noteData.IsPin,
                        IsArchive = noteData.IsArchive,
                        IsTrash = noteData.IsTrash,
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

        public async Task<string> DeleteReminder(int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("DeleteReminder", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlConnection.Open();
                var response = await sqlCommand.ExecuteNonQueryAsync();
                if (response > 0)
                {
                    return "Reminder Delete Successfully";
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

        public async Task<NoteModel> ImageUpload(IFormFile formFile,int userId, int noteId)
        {
            try
            {
                UploadImage uploadImage = new UploadImage(this.configuration, formFile);
                var url = uploadImage.Upload(formFile);
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("ImageOnNote", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@Image", url);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                }
                sqlConnection.Close();
                if (noteData != null)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Color=noteData.Color,
                        Image=noteData.Image,
                        CreatedDate=noteData.CreatedDate,
                        ModifiedDate=noteData.ModifiedDate,
                        IsPin=noteData.IsPin,
                        IsArchive=noteData.IsArchive,
                        IsTrash=noteData.IsTrash,
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
    }
}
