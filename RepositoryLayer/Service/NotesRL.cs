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
        public async Task<IList<NoteModel>> GetAllNotes(int userId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
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
                var response = table.NewRow();
                if (response != null)
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

        public async Task<NoteModel> AddReminder(int userId, int noteId, DateTime dateTime)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                paramList.Add(new StoredProcedureParameterData("@Reminder", dateTime));
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

        public async Task<string> DeleteReminder(int userId, int noteId)
        {
            try
            {
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@NoteId", noteId));
                DataTable table = await StoredProcedureExecuteReader("DeleteReminder", paramList);
                var response = table.NewRow();
          
                if (response != null)
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

        public async Task<(IList<NoteModel>, IList<NoteLabelModel>, IList<AddCollaboratorModel>)> NoteLabelCollaborator(int userId)
        {
            try
            {
                SqlCommand sqlCommand = GetCommand("GetNotesWithCollaboratorAndLabel");
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                var noteData = new NoteModel();
                IList<NoteModel> noteList = new List<NoteModel>();

                var labelData = new NoteLabelModel();
                IList<NoteLabelModel> labelList = new List<NoteLabelModel>();

                var collaboratorData = new AddCollaboratorModel();
                IList<AddCollaboratorModel> collaboratorList = new List<AddCollaboratorModel>();

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

                    labelData = new NoteLabelModel();
                    labelData.Label = sqlDataReader["Label"] as string;
                    labelData.LabelId = (sqlDataReader["LabelId"] as int?).GetValueOrDefault();
                    labelData.NoteId = (sqlDataReader["NoteId"] as int?).GetValueOrDefault();
                    labelData.UserId = (sqlDataReader["UserId"] as int?).GetValueOrDefault();

                    collaboratorData = new AddCollaboratorModel();
                    collaboratorData.CreatedId = (sqlDataReader["CreatedId"] as int?).GetValueOrDefault();
                    collaboratorData.CreatedDate = (sqlDataReader["CreatedDate"] as DateTime?).GetValueOrDefault();
                    collaboratorData.ModifiedDate = (sqlDataReader["ModifiedDate"] as DateTime?).GetValueOrDefault();
                    collaboratorData.NoteId = (sqlDataReader["NoteId"] as int?).GetValueOrDefault();
                    collaboratorData.ReceiverId = (sqlDataReader["ReceiverId"] as int?).GetValueOrDefault();
                    collaboratorData.ReceiverProfile = sqlDataReader["ReceiverProfile"] as string;

                    noteList.Add(noteData);
                    labelList.Add(labelData);
                    collaboratorList.Add(collaboratorData);
                }
                return (noteList, labelList, collaboratorList);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
