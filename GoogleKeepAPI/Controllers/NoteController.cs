//-----------------------------------------------------------------------
// <copyright file="NoteController.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace GoogleKeepAPI.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BussinessLayer.Interface;
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// NoteController class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        /// <summary>
        /// Inject the bussiness layer interface
        /// </summary>
        private readonly INotesBL noteBL;

        /// <summary>
        /// Initializes the memory for note controller class
        /// </summary>
        /// <param name="noteBL">noteBL parameter</param>
        public NoteController (INotesBL noteBL)
        {
            this.noteBL = noteBL;
        }

        /// <summary>
        /// Add Note method
        /// </summary>
        /// <param name="showNoteModel">showNoteModel parameter</param>
        /// <returns>return the added note</returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNote(ShowNoteModel showNoteModel)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c=>c.Type=="Id").Value);
                var data = await noteBL.AddNote(showNoteModel, claim);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Note Added Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Failed To Add Note" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Update Note method
        /// </summary>
        /// <param name="showNoteModel">showNoteModel parameter</param>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the updated note</returns>
        [HttpPut]
        [Route("{noteId}")]
        public async Task<IActionResult> UpdateNote(ShowUpdateNoteModel showNoteModel, int noteId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.UpdateNote(showNoteModel, claim, noteId);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Note Updated Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Failed To Update Note" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Get All Notes method
        /// </summary>
        /// <returns>returns the all notes</returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.GetAllNotes(claim);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "All Notes", data });
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Failed To Get All Notes" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Delete Note method
        /// </summary>
        /// <param name="noteId">noteId parameter</param>
        /// <returns>returns the deleted note</returns>
        [HttpDelete]
        [Route("{noteId}")]
        public async Task<IActionResult> DeleteNote(int noteId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.DeleteNote(claim, noteId);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Delete Note Successfully" });
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Failed To Delete Note" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{noteId}/Archive")]
        public async Task<IActionResult> ArchiveNote(int noteId)
        {
            try
            {
              //var claim = 3003;
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type=="Id").Value);
                var data = await noteBL.ArchiveNote(claim, noteId);
                if (data != null)
                {
                    if (data.IsArchive == true)
                    {
                        return this.Ok(new { status = "true", message = "Note Is Archive", data });
                    }
                    else
                    {
                        return this.Ok(new { status = "true", message = "Note Is UnArchive", data });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Note Is Not Available" });
                }
              
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }
        [HttpPost]
        [Route("{noteId}/Trash")]
        public async Task<IActionResult> TrashNote(int noteId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.TrashNote(claim, noteId);
                if (data != null)
                {
                    if (data.IsTrash == true)
                    {
                        return this.Ok(new { status = "true", message = "Note Is Trash", data });
                    }
                    else
                    {
                        return this.Ok(new { staus = "true", message = "Note Is UnTrash", data });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Note Is Not Available" });
                }
               
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{noteId}/Pin")]
        public async Task<IActionResult> PinNote(int noteId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.PinNote(claim, noteId);
                if (data != null)
                {
                    if (data.IsPin == true)
                    {
                        return this.Ok(new { status = "true", message = "Note Is Pin", data });
                    }
                    else
                    {
                        return this.Ok(new { staus = "true", message = "Note Is UnPin", data });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Note Is Not Available" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{noteId}/Color")]
        public async Task<IActionResult> ChangeColor(int noteId, ColorModel colorModel)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.ChangeColor(claim, noteId, colorModel);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Color Set Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Set Color", data });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{noteId}/Reminder")]
        public async Task<IActionResult> AddReminder(int noteId, DateTime dateTime)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.AddReminder(claim, noteId, dateTime);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Reminder Set Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Set Reminder", data });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpDelete]
        [Route("{noteId}/Reminder")]
        public async Task<IActionResult> DeleteReminder(int noteId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.DeleteReminder(claim, noteId);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Reminder Delete Successfully" });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Delete Reminder" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{noteId}/Upload")]
        public async Task<IActionResult> ImageUpload(IFormFile formFile, int noteId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.ImageUpload(formFile, claim, noteId);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Image Uploaded Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Upload Image" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet]
        [Route("Trash")]
        public async Task<IActionResult> GetAllTrashNotes()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.GetAllTrashNotes(claim);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "All Trash Notes", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Get Trash Notes" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet]
        [Route("Archive")]
        public async Task<IActionResult> GetAllArchiveNotes()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.GetAllArchiveNotes(claim);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "All Archive Notes", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Get Archive Notes" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet]
        [Route("Pin")]
        public async Task<IActionResult> GetAllPinNotes()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.GetAllPinNotes(claim);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "All Pin Notes", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Get Pin Notes" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("AddCollaborator")]
        public async Task<IActionResult> AddCollaborator(ShowCollaboratorModel showCollaboratorModel)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.AddCollaborator(claim, showCollaboratorModel);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Add Collaborator Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Add Collaborator" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpDelete]
        [Route("{noteId}/Collaborator/{collaboratorId}")]
        public async Task<IActionResult> DeleteCollaborator(int noteId, int collaboratorId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.DeleteCollaborator(claim, noteId, collaboratorId);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Delete Collaborator Successfully" });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Delete Collaborator" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet]
        [Route("NotesLabelCollaborator")]
        public async Task<IActionResult> NotesLabelCollaborator()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.NoteLabelCollaborator(claim);
                if (!data.Equals(null))
                {
                    return this.Ok(new { status = "true", message = "Notes Label Collaborator", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Failed To Get Notes" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchNotes(string searchWord)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await noteBL.SearchNotes(claim, searchWord);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Searched Notes", data });
                }
                else
                {
                    return this.Ok(new { staus = "false", message = "Notes Not Available" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }
    }
}