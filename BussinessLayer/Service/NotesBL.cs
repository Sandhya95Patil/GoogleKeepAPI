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
    using Microsoft.AspNetCore.Http;
    using RepositoryLayer.Interface;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// NotesBL class
    /// </summary>
    public class NotesBL :  INotesBL
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
        public async Task<IList<NoteModel>> GetAllNotes()
        {
            try
            {
                return await noteRL.GetAllNotes();
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

        /// <summary>
        /// Archive Note
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the archive or unarchive note</returns>
        public async Task<NoteModel> ArchiveNote(int userId, int noteId)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return await noteRL.ArchiveNote(userId, noteId);
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
        /// Trash Note
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the trash or untrash note</returns>
        public async Task<NoteModel> TrashNote(int userId, int noteId)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return await noteRL.TrashNote(userId, noteId);
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
        /// Pin Note
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the pin or unpin note</returns>
        public async Task<NoteModel> PinNote(int userId, int noteId)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return await noteRL.PinNote(userId, noteId);
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
        /// Change Color
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="colorModel">colorModel parameter</param>
        /// <returns>return the change color note</returns>
        public async Task<NoteModel> ChangeColor(int userId, int noteId, ColorModel colorModel)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return await noteRL.ChangeColor(userId, noteId, colorModel);
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
        /// Add Reminder
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="addReminder">addReminder parameter</param>
        /// <returns>return the add reminder note</returns>
        public Task<NoteModel> AddReminder(int userId, int noteId, AddReminder addReminder)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return noteRL.AddReminder(userId, noteId, addReminder);
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
        /// Delete Reminder
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the delete reminder note</returns>
        public async Task<string> DeleteReminder(int userId, int noteId)
        {
            try
            {
                 if (userId > 0 && noteId > 0)
                {
                    return await noteRL.DeleteReminder(userId, noteId);
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
        /// Image Upload
        /// </summary>
        /// <param name="file">file parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>return the image upload note</returns>
        public async Task<NoteModel> ImageUpload(IFormFile file, int userId, int noteId)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return await noteRL.ImageUpload(file, userId, noteId);
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
        /// Get All Trash Notes
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the all trash notes</returns>
        public async Task<IList<NoteModel>> GetAllTrashNotes(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return await noteRL.GetAllTrashNotes(userId);
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
        /// Get All Archive Notes
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the archive notes</returns>
        public async Task<IList<NoteModel>> GetAllArchiveNotes(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return await noteRL.GetAllArchiveNotes(userId);
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
        /// Get All Pin Notes
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the pin notes</returns>
        public async Task<IList<NoteModel>> GetAllPinNotes(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return await noteRL.GetAllPinNotes(userId);
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
        /// Add Collaborator
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="showCollaboratorModel">showCollaboratorModel parameter</param>
        /// <returns>return the add collaborator note</returns>
        public Task<AddCollaboratorModel> AddCollaborator(int userId, int noteId, ShowCollaboratorModel showCollaboratorModel)
        {
            try
            {
                if (userId > 0)
                {
                    return this.noteRL.AddCollaborator(userId, noteId, showCollaboratorModel);
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
        /// Delete Collaborator
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="collaboratorId">collaboratorId parameter</param>
        /// <returns>return the delete collaborator note</returns>
        public async Task<string> DeleteCollaborator(int userId, int noteId, int collaboratorId)
        {
            try
            {
                if (userId > 0 && noteId > 0 && collaboratorId > 0)
                {
                    return await noteRL.DeleteCollaborator(userId, noteId, collaboratorId);
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
        /// Search Notes
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="searchWord">searchWord parameter</param>
        /// <returns>return the searched notes</returns>
        public async Task<IList<NoteModel>> SearchNotes(int userId, SearchWordModel searchWord)
        {
            try
            {
                if (userId > 0)
                {
                    return await noteRL.SearchNotes(userId, searchWord);
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
        /// Note Label Collaborator
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the notes with label and collaborator</returns>
        public Task<(IList<NoteModel>, IList<LabelListModel>, IList<CollaboratorListModel>)> NoteLabelCollaborator(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return noteRL.NoteLabelCollaborator(userId);
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
        /// Add Label By Id
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <param name="labelById">labelById parameter</param>
        /// <returns>return the note with added label</returns>
        public async Task<AddLabelByLabelId> AddLabelById(int userId, int noteId, LabelById labelById)
        {
            try
            {
                if (userId > 0)
                {
                    return await noteRL.AddLabelById(userId, noteId, labelById);
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
        /// Empty the  bin
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public async Task<bool> EmptyBin(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return await noteRL.EmptyBin(userId);
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
                if (userId > 0)
                {
                    return await noteRL.GetAllReminder(userId);
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
                if (userId > 0)
                {
                    return await noteRL.GetAllCollaborator(userId);
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
