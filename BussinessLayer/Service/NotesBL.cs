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

        public Task<NoteModel> AddReminder(int userId, int noteId, DateTime dateTime)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return noteRL.AddReminder(userId, noteId, dateTime);
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

        public async Task<NoteModel> ImageUpload(IFormFile formFile, int userId, int noteId)
        {
            try
            {
                if (userId > 0 && noteId > 0)
                {
                    return await noteRL.ImageUpload(formFile, userId, noteId);
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

        public Task<AddCollaboratorModel> AddCollaborator(int userId, ShowCollaboratorModel showCollaboratorModel)
        {
            try
            {
                if (userId > 0)
                {
                    return noteRL.AddCollaborator(userId, showCollaboratorModel);
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

        public async Task<IList<NoteModel>> SearchNotes(int userId, string searchWord)
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

        public Task<(IList<NoteModel>, IList<NoteLabelModel>, IList<AddCollaboratorModel>)> NoteLabelCollaborator(int userId)
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
    }
}
