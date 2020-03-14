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
    using Newtonsoft.Json;
    using RepositoryLayer.Interface;
    using ServiceStack;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

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
        /// Stored Procedure Parameter Data class
        /// </summary>
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

        /// <summary>
        /// Stored Procedure Execute Reader method
        /// </summary>
        /// <param name="spName">spName parameter</param>
        /// <param name="spParams">spParams parameter</param>
        /// <returns>return procedure name and parameters</returns>
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
            catch (Exception exception)
            {
                throw exception;
            }
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
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Title", showNoteModel.Title));
                paramList.Add(new StoredProcedureParameterData("@Description", showNoteModel.Description));
                paramList.Add(new StoredProcedureParameterData("@Reminder", showNoteModel.Reminder));
                paramList.Add(new StoredProcedureParameterData("@Color", showNoteModel.Color));
                paramList.Add(new StoredProcedureParameterData("@Image", showNoteModel.Image));
                paramList.Add(new StoredProcedureParameterData("@CreatedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@ModifiedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@IsPin", showNoteModel.IsPin));
                paramList.Add(new StoredProcedureParameterData("@IsArchive", showNoteModel.IsArchive));
                paramList.Add(new StoredProcedureParameterData("@IsTrash", showNoteModel.IsTrash));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("NoteAdd", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = (int)row["Id"];
                }

                if (noteData != null)
                {
                    var showResponse = new NoteModel()
                    {
                       Id = noteData.Id,
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
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Id", noteId));
                paramList.Add(new StoredProcedureParameterData("@Title", showUpdateNoteModel.Title));
                paramList.Add(new StoredProcedureParameterData("@Description", showUpdateNoteModel.Description));
                paramList.Add(new StoredProcedureParameterData("@ModifiedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("NoteUpdate", paramList);
                var response = table.NewRow();
                if (response != null)
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
        public async Task<IList<NoteModel>> GetAllNotes()
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                //paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("GetAllNotes", paramList);
                IList<NoteModel> noteList = new List<NoteModel>();
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = (int)row["Id"];
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = (int)row["UserId"];
                    noteList.Add(noteData);
                }

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
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("DeleteNote", paramList);
                table.AcceptChanges();
                return "Note Deleted Successfully";
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Archive Note Method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the archived or unarchived note</returns>
        public async Task<NoteModel> ArchiveNote(int userId, int noteId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("ArchiveNote", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
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

        /// <summary>
        /// Trash Note Methd
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the trash or untrash note</returns>
        public async Task<NoteModel> TrashNote(int userId, int noteId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("TrashNote", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
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

        /// <summary>
        /// Pin Note Method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the pin or unpin note</returns>
        public async Task<NoteModel> PinNote(int userId, int noteId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("PinNote", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
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

        /// <summary>
        /// Change Color Method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="colorModel">colorModel parameter</param>
        /// <returns>return change color note</returns>
        public async Task<NoteModel> ChangeColor(int userId, int noteId, ColorModel colorModel)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@Color", colorModel.Color));
                DataTable table = await StoredProcedureExecuteReader("ColorNote", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["Id"]);
                }

                if (noteData.Id > 0)
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

        /// <summary>
        /// Add Reminder Method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="addReminder">addReminder parameter</param>
        /// <returns>return reminder</returns>
        public async Task<NoteModel> AddReminder(int userId, int noteId, AddReminder addReminder)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@Reminder", addReminder.DateTime));
                DataTable table = await StoredProcedureExecuteReader("AddReminder", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = row["Image"].ToString();
                    noteData.Color = row["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                }

                if (noteData.Id > 0)
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

            /// <summary>
            /// Delete Reminder method
            /// </summary>
            /// <param name="userId">userId parameter</param>
            /// <param name="noteId">noteId parameter</param>
            /// <returns>return the delete reminder</returns>
            public async Task<string> DeleteReminder(int userId, int noteId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                DataTable table = await StoredProcedureExecuteReader("DeleteReminder", paramList);
                table.AcceptChanges();
                return "Reminder Delete Successfully";
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Image Upload method
        /// </summary>
        /// <param name="file">file parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return image</returns>
        public async Task<NoteModel> ImageUpload(IFormFile file, int userId, int noteId)
        {
            try
            {
                UploadImage uploadImage = new UploadImage(this.configuration, file);
                var url = uploadImage.Upload(file);

                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@Image", url));
                DataTable table = await StoredProcedureExecuteReader("ImageOnNote", paramList);
                var noteData = new NoteModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                }

                if (noteData.Id > 0)
                {
                    var showResponse = new NoteModel()
                    {
                        Id = noteData.Id,
                        Title = noteData.Title,
                        Description = noteData.Description,
                        Color = noteData.Color,
                        Image = url,
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

        /// <summary>
        /// Get All Trash Notes method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return all trash notes</returns>
        public async Task<IList<NoteModel>> GetAllTrashNotes(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("AllTrashNotes", paramList);
                var noteData = new NoteModel();
                IList<NoteModel> trashList = new List<NoteModel>();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Color = row["Color"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
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

        /// <summary>
        /// Get  All Archive Notes method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return all archive notes</returns>
        public async Task<IList<NoteModel>> GetAllArchiveNotes(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("AllArchiveNotes", paramList);
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = row["Image"].ToString();
                    noteData.Color = row["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
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

        /// <summary>
        /// Get  All Pin Notes method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return all pin notes</returns>
        public async Task<IList<NoteModel>> GetAllPinNotes(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("AllPinNotes", paramList);
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Image = row["Image"].ToString();
                    noteData.Color = row["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
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

        /// <summary>
        /// Add Collaborator method
        /// </summary>
        /// <param name="createdId">createdId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="showCollaboratorModel">showCollaboratorModel parameter</param>
        /// <returns>return the collaborator add note</returns>
        public async Task<AddCollaboratorModel> AddCollaborator(int createdId, int noteId, ShowCollaboratorModel showCollaboratorModel)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@CreatedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@ModifiedDate", DateTime.Now));
                paramList.Add(new StoredProcedureParameterData("@ReceiverId", showCollaboratorModel.ReceiverId));
                DataTable table = await StoredProcedureExecuteReader("AddCollaborator", paramList);
                var collaboratorData = new AddCollaboratorModel();
                foreach (DataRow row in table.Rows)
                {
                    collaboratorData = new AddCollaboratorModel();
                    collaboratorData.Id = Convert.ToInt32(row["Id"]);
                    collaboratorData.NoteId = Convert.ToInt32(row["NoteId"]);
                    collaboratorData.CreatedId = Convert.ToInt32(row["CreatedId"]);
                    collaboratorData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    collaboratorData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    collaboratorData.ReceiverId = Convert.ToInt32(row["ReceiverId"]);
                    collaboratorData.ReceiverProfile = row["ReceiverProfile"].ToString();
                }

                if (collaboratorData.Id > 0)
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

        /// <summary>
        /// Delete Collaborator method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="collaboratorId">collaboratorId parameter</param>
        /// <returns>return delete collaborator note</returns>
        public async Task<string> DeleteCollaborator(int userId, int noteId, int collaboratorId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@CollaboratorId", collaboratorId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                DataTable table = await StoredProcedureExecuteReader("DeleteCollaborator", paramList);
                table.AcceptChanges();
                return "Delete Collaborator";
 
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Search Notes method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="searchWord">searchWord parameter</param>
        /// <returns>return searched notes</returns>
        public async Task<IList<NoteModel>> SearchNotes(int userId, SearchWordModel searchWord)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@SearchWord", searchWord.SearchWord));
                DataTable table = await StoredProcedureExecuteReader("SearchNote", paramList);
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = row["Image"].ToString();
                    noteData.Color = row["Color"].ToString();
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
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

        /// <summary>
        /// Note Label Collaborator method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the notes with label and collaborator</returns>
        public async Task<(IList<NoteModel>, IList<LabelListModel>, IList<CollaboratorListModel>)> NoteLabelCollaborator(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("GetNotesWithCollaboratorAndLabel", paramList);

                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();

                var labelData = new LabelListModel();
                IList<LabelListModel> labelList = new List<LabelListModel>();

                var collaboratorData = new CollaboratorListModel();
                IList<CollaboratorListModel> collaboratorList = new List<CollaboratorListModel>();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = Convert.ToInt32(row["Id"]);
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = (row["Reminder"] as DateTime?).GetValueOrDefault();
                    noteData.Image = row["Image"].ToString();
                    noteData.Color = row["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = Convert.ToInt32(row["UserId"]);
                    noteList.Add(noteData);

                    labelData = new LabelListModel();
                    labelData.Label = row["Label"] as string;
                    labelData.LabelId = (row["LabelId"] as int?).GetValueOrDefault();
                    labelData.NoteId = (row["NoteId"] as int?).GetValueOrDefault();
                    labelData.userId = (row["UserId"] as int?).GetValueOrDefault();
                    if (labelData.Label != null)
                    {
                        labelList.Add(labelData);
                    }

                    collaboratorData = new CollaboratorListModel();
                    collaboratorData.CreatedId = (row["CreatedId"] as int?).GetValueOrDefault();
                    collaboratorData.NoteId = (row["NoteId"] as int?).GetValueOrDefault();
                    collaboratorData.ReceiverId = (row["ReceiverId"] as int?).GetValueOrDefault();
                    collaboratorData.ReceiverProfile = row["ReceiverProfile"] as string;
                    if (collaboratorData.ReceiverId > 0)
                    {
                        collaboratorList.Add(collaboratorData);
                    }
                }

                return (noteList, labelList, collaboratorList);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Add Label By Id
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="labelById">labelById parameter</param>
        /// <returns>return added label note</returns>
        public async Task<AddLabelByLabelId> AddLabelById(int userId, int noteId, LabelById labelById)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@LabelId", labelById.LabelId));
                DataTable table = await StoredProcedureExecuteReader("AddLabelByLabelId", paramList);
                var noteData = new NoteLabelModel();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteLabelModel();
                    noteData.Label = row["Label"].ToString();
                    noteData.LabelId = (int)row["LabelId"];
                    noteData.NoteId = (int)row["NoteId"];
                    noteData.UserId = (int)row["UserId"];
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                }
                if (noteData.Label != null)
                {
                    var showResponse = new AddLabelByLabelId()
                    {
                        Label = noteData.Label,
                        LabelId = noteData.LabelId,
                        NoteId = noteData.NoteId,
                        UserId = noteData.UserId,
                        CreatedDate = noteData.CreatedDate,
                        ModifiedDate = noteData.ModifiedDate
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
        /// Empty Bin method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the empty bin</returns>
        public async Task<bool> EmptyBin(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("EmptyBin", paramList);
                var response = table.NewRow();
                if (response != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<IList<NoteModel>> GetAllReminder(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("GetAllReminder", paramList);
                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();
                foreach (DataRow row in table.Rows)
                {
                    noteData = new NoteModel();
                    noteData.Id = (int)row["Id"];
                    noteData.Title = row["Title"].ToString();
                    noteData.Description = row["Description"].ToString();
                    noteData.Reminder = Convert.ToDateTime(row["Reminder"]);
                    noteData.Image = row["Image"].ToString();
                    noteData.Color = row["Color"].ToString();
                    noteData.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    noteData.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    noteData.IsArchive = Convert.ToBoolean(row["IsArchive"]);
                    noteData.IsPin = Convert.ToBoolean(row["IsPin"]);
                    noteData.IsTrash = Convert.ToBoolean(row["IsTrash"]);
                    noteData.UserId = (int)row["UserId"];
                    noteList.Add(noteData);
                }
                if (noteList.Count> 0)
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

        public async Task<IList<AddCollaboratorModel>> GetAllCollaborator(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                DataTable table = await StoredProcedureExecuteReader("GetAllCollaborator", paramList);
                IList<AddCollaboratorModel> list = new List<AddCollaboratorModel>();
                var model = new AddCollaboratorModel();
                foreach (DataRow row in table.Rows)
                {
                    model = new AddCollaboratorModel();
                    model.Id = (int)row["Id"];
                    model.CreatedId = (int)row["CreatedId"];
                    model.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    model.ModifiedDate = Convert.ToDateTime(row["ModifiedDate"]);
                    model.NoteId = (int)row["NoteId"];
                    model.ReceiverId = (int)row["ReceiverId"];
                    model.ReceiverProfile = row["ReceiverProfile"].ToString();
                    list.Add(model);
                }
                if (list.Count > 0)
                {
                    return list;
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

        public async Task<bool> SendNotificationAsync(string token, string title, string body)
        {
            using (var client = new HttpClient())
            {
                var firebaseOptionsServerApiKey = "AAAAlOcKVxU:APA91bG0xgnX31II8BHxbESovv057xSOBu0iixdXm-hbMu37ntDr5JyNUxvKsZ-4n_OzzqEh0mDSwqQ3sVGCfGA6ApGXYtUYBxqJxUm_V4kMNWZRY59FyPBA2Hdj9guheVdRHthF3Mek";
                var firebaseOptionsSenderId = 639531374357;

                client.BaseAddress = new Uri("https://fcm.googleapis.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"key={firebaseOptionsServerApiKey}");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={firebaseOptionsSenderId}");


                var data = new
                {
                    to = token,
                    notification = new
                    {
                        body = body,
                        title = title,
                    },
                    priority = "high"
                };

                var json = JsonConvert.SerializeObject(data);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/fcm/send", httpContent);
                return result.StatusCode.Equals(HttpStatusCode.OK);
            }
        }
    }

 
}
