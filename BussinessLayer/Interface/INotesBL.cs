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
    using Microsoft.AspNetCore.Http;
    using System;
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
        Task<IList<NoteModel>> GetAllNotes();

        /// <summary>
        /// Delete Note method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the deleted note</returns>
        Task<string> DeleteNote(int userId, int noteId);
        Task<NoteModel> ArchiveNote(int userId, int noteId);
        Task<NoteModel> TrashNote(int userId, int noteId);
        Task<NoteModel> PinNote(int userId, int noteId);
        Task<NoteModel> ChangeColor(int userId, int noteId, ColorModel colorModel);
        Task<NoteModel> AddReminder(int userId, int noteId, AddReminder addReminder);
        Task<string> DeleteReminder(int userId, int noteId);
        Task<NoteModel> ImageUpload(IFormFile file, int userId, int noteId);
        Task<IList<NoteModel>> GetAllTrashNotes(int userId);
        Task<IList<NoteModel>> GetAllArchiveNotes(int userId);
        Task<IList<NoteModel>> GetAllPinNotes(int userId);
        Task<AddCollaboratorModel> AddCollaborator(int userId, int noteId, ShowCollaboratorModel showCollaboratorModel);
        Task<string> DeleteCollaborator(int userId, int noteId, int collaboratorId);
        Task<(IList<NoteModel>, IList<LabelListModel>, IList<CollaboratorListModel>)> NoteLabelCollaborator(int userId);
        Task<IList<NoteModel>> SearchNotes(int userId, SearchWordModel searchWord);
        Task<AddLabelByLabelId> AddLabelById(int userId, int noteId, LabelById labelById);
        Task<bool> EmptyBin(int userId);
        Task<IList<NoteModel>> GetAllReminder(int userId);
        Task<IList<AddCollaboratorModel>> GetAllCollaborator(int userId);

    }
}
