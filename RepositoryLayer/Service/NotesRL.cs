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

        public SqlConnection GetConnection(string connectionName)
        {
            SqlConnection connection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
            connection.Open();
            return connection;
        }
        public SqlCommand GetCommand(string command)
        {
            string con = "";
            SqlConnection sqlConnection = GetConnection(con);
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand;
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
                //string command = "NoteAdd";
                SqlCommand sqlCommand = GetCommand("NoteAdd");
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
                SqlCommand sqlCommand = GetCommand("NoteUpdate");
                sqlCommand.Parameters.AddWithValue("@Id", noteId);
                sqlCommand.Parameters.AddWithValue("@Title", showUpdateNoteModel.Title);
                sqlCommand.Parameters.AddWithValue("@Description", showUpdateNoteModel.Description);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);              
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
                SqlCommand sqlCommand = GetCommand("GetAllNotes");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                IList<NoteModel> noteList = new List<NoteModel>();
                var userData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    userData = new NoteModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    userData.Title = sqlDataReader["Title"].ToString();
                    userData.Description = sqlDataReader["Description"].ToString();
                    userData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
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
                SqlCommand sqlCommand = GetCommand("DeleteNote");
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                var response = await sqlCommand.ExecuteNonQueryAsync();
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
                SqlCommand sqlCommand = GetCommand("ArchiveNote");
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                }
                if (noteData.Id == noteId)
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
                SqlCommand sqlCommand = GetCommand("TrashNote");
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                }
                if (noteData.Id == noteId)
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
                SqlCommand sqlCommand = GetCommand("PinNote");
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                }
                if (noteData.Id==noteId)
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
                SqlCommand sqlCommand = GetCommand("ColorNote");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@Color", colorModel.Color);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
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
                SqlCommand sqlCommand = GetCommand("AddReminder");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@Reminder", dateTime);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
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
                SqlCommand sqlCommand = GetCommand("DeleteReminder");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
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
                SqlCommand sqlCommand = GetCommand("ImageOnNote");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                sqlCommand.Parameters.AddWithValue("@Image", url);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                }
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

        public async Task<IList<NoteModel>> GetAllTrashNotes(int userId)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("AllTrashNotes");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                IList<NoteModel> trashList = new List<NoteModel>();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    trashList.Add(noteData);
                }
                if (noteData != null)
                {
                    return trashList;
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

        public async Task<IList<NoteModel>> GetAllArchiveNotes(int userId)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("AllArchiveNotes");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                    noteList.Add(noteData);
                }
                if (noteData != null)
                {
                    return noteList;
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

        public async Task<IList<NoteModel>> GetAllPinNotes(int userId)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("AllPinNotes");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                    noteList.Add(noteData);
                }
                if (noteData != null)
                {
                    return noteList;
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

        public async Task<AddCollaboratorModel> AddCollaborator(int createdId, ShowCollaboratorModel showCollaboratorModel)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("AddCollaborator");
                sqlCommand.Parameters.AddWithValue("@NoteId", showCollaboratorModel.NoteId);
                sqlCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                sqlCommand.Parameters.AddWithValue("@ReceiverId", showCollaboratorModel.ReceiverId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var collaboratorData = new AddCollaboratorModel();
                while (sqlDataReader.Read())
                {
                    collaboratorData = new AddCollaboratorModel();
                    collaboratorData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    collaboratorData.NoteId = Convert.ToInt32(sqlDataReader["NoteId"]);
                    collaboratorData.CreatedId = Convert.ToInt32(sqlDataReader["CreatedId"]);
                    collaboratorData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    collaboratorData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    collaboratorData.ReceiverId = Convert.ToInt32(sqlDataReader["ReceiverId"]);
                    collaboratorData.ReceiverProfile = sqlDataReader["ReceiverProfile"].ToString();
                }
                if (collaboratorData != null)
                {
                    var showResponse = new AddCollaboratorModel()
                    {
                        Id = collaboratorData.Id,
                        NoteId = collaboratorData.NoteId,
                        CreatedId = createdId,
                        CreatedDate = collaboratorData.CreatedDate,
                        ModifiedDate = collaboratorData.ModifiedDate,
                        ReceiverId = collaboratorData.ReceiverId,
                        ReceiverProfile = collaboratorData.ReceiverProfile
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


        public async Task<string> DeleteCollaborator(int userId, int noteId, int collaboratorId)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("DeleteCollaborator");
                sqlCommand.Parameters.AddWithValue("@CollaboratorId", collaboratorId);
                sqlCommand.Parameters.AddWithValue("@NoteId", noteId);
                var response = await sqlCommand.ExecuteNonQueryAsync();
                if (response > 0)
                {
                    return "Collaborator Delete Successfully";
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


        public async Task<IList<NotesLabelCollaboratorModel>> NoteLabelCollaborator(int userId)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("GetNotesWithCollaboratorAndLabel");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NotesLabelCollaboratorModel();
                IList<NotesLabelCollaboratorModel> noteList = new List<NotesLabelCollaboratorModel>();

                while (sqlDataReader.Read())
                {
                    noteData = new NotesLabelCollaboratorModel();
                    noteData.NoteId = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);

                    noteData.Label = sqlDataReader["Label"] as string;
                    noteData.LabelId = (sqlDataReader["LabelId"] as int?).GetValueOrDefault();
                    noteData.ReceiverId = (sqlDataReader["ReceiverId"] as int?).GetValueOrDefault();
                    noteData.ReceiverProfile = sqlDataReader["ReceiverProfile"] as string;

                    noteList.Add(noteData);
                }
                return noteList;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);  
            }
        }

        public async Task<IList<NoteModel>> SearchNotes(int userId, string searchWord)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("SearchNote");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@SearchWord", searchWord);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                while (sqlDataReader.Read())
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    noteData.Title = sqlDataReader["Title"].ToString();
                    noteData.Description = sqlDataReader["Description"].ToString();
                    noteData.Reminder = (sqlDataReader["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = sqlDataReader["Image"].ToString();
                    noteData.Color = sqlDataReader["Color"].ToString();
                    noteData.IsPin = Convert.ToBoolean(sqlDataReader["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(sqlDataReader["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(sqlDataReader["IsTrash"]);
                    noteData.CreatedDate = Convert.ToDateTime(sqlDataReader["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(sqlDataReader["ModifiedDate"]);
                    noteData.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                    noteList.Add(noteData);
                }
                if (noteList != null)
                {
                    return noteList;
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
