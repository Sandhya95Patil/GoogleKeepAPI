using CommonLayer.Model;
using CommonLayer.ShowModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NotesRL : INotesRL 
    {
        private readonly IConfiguration configuration;
        public NotesRL (IConfiguration configuration)
        {
            this.configuration = configuration;
        }
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

        public async Task<NoteModel> UpdateNote(ShowNoteModel showNoteModel, int userId, int noteId)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                string noteData = $"select * from Notes where Id='{noteId}'";
                SqlCommand sqlCommand = new SqlCommand(noteData, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", noteId);
                if (showNoteModel.Title != null)
                {
                    sqlCommand.Parameters.AddWithValue("@Title", showNoteModel.Title);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@Title", noteData);
                }
                if (showNoteModel.Description != null)
                {
                    sqlCommand.Parameters.AddWithValue("@Description", showNoteModel.Description);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@Description", noteData);

                }
                sqlCommand.Parameters.AddWithValue("@Reminder", showNoteModel.Reminder);
                sqlCommand.Parameters.AddWithValue("@Color", showNoteModel.Color);
                sqlCommand.Parameters.AddWithValue("@Image", showNoteModel.Image);
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
                        Title = showNoteModel.Title,
                        Description = showNoteModel.Description,
                        Reminder = showNoteModel.Reminder,
                        Color = showNoteModel.Color,
                        Image = showNoteModel.Image,
                        ModifiedDate = DateTime.Now,
                        IsPin = showNoteModel.IsPin,
                        IsArchive = showNoteModel.IsArchive,
                        IsTrash = showNoteModel.IsTrash,
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
