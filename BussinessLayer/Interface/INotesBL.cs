//-----------------------------------------------------------------------
// <copyright file="INotesBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BussinessLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.ShowModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// INotesBL interface
    /// </summary>
    public interface INotesBL
    {
        /// <summary>
        /// Add Note method
        /// </summary>
        /// <param name="showNoteModel">showNoteModel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <returns>returns the added note</returns>
        Task<NoteModel> AddNote(ShowNoteModel showNoteModel, int userId);

        /// <summary>
        /// Update Note method
        /// </summary>
        /// <param name="showUpdateNoteModel">showUpdateNoteModel</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the updated note</returns>
        Task<UpdateResponseModel> UpdateNote(ShowUpdateNoteModel showUpdateNoteModel, int userId, int noteId);

        /// <summary>
        /// Get All Notes method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>returns the all notes</returns>
        Task<IList<NoteModel>> GetAllNotes(int userId);

        /// <summary>
        /// Delete Note method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the deleted note</returns>
        Task<string> DeleteNote(int userId, int noteId);

    }
}
