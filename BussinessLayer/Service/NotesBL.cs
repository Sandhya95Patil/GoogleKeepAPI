//-----------------------------------------------------------------------
// <copyright file="NotesBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BussinessLayer.Service
{
    using BussinessLayer.Interface;
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.ShowModel;
    using RepositoryLayer.Interface;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// NotesBL class
    /// </summary>
    public class NotesBL : INotesBL
    {
        /// <summary>
        /// Inject the notes interface of repository layer
        /// </summary>
        private readonly INotesRL noteRL;

        /// <summary>
        /// Initializes the memory for Notes class
        /// </summary>
        /// <param name="noteRL">noteRL parameter</param>
        public NotesBL (INotesRL noteRL)
        {
            this.noteRL = noteRL;
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
                return await noteRL.AddNote(showNoteModel, userId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Update Note method
        /// </summary>
        /// <param name="showNoteModel">showNoteModel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the updated note</returns>
        public async Task<UpdateResponseModel> UpdateNote(ShowUpdateNoteModel showNoteModel, int userId, int noteId)
        {
            try
            {
                return await noteRL.UpdateNote(showNoteModel, userId, noteId);
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
        /// <returns>returns the all note</returns>
        public async Task<IList<NoteModel>> GetAllNotes(int userId)
        {
            try
            {
                return await noteRL.GetAllNotes(userId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Delete Note methos
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the deleted note</returns>
        public async Task<string> DeleteNote(int userId, int noteId)
        {
            try
            {
                return await noteRL.DeleteNote(userId, noteId); 
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        } 
    }
}
