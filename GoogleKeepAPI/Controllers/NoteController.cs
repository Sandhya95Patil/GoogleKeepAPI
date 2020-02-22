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
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type=="Id").Value);
                var data = await noteBL.ArchiveNote(claim, noteId);
                if (data.IsArchive == true)
                {
                    return this.Ok(new { status = "true", message = "Note Is Archive", data });
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Note Is UnArchive", data });
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
                if (data.IsTrash == true)
                {
                    return this.Ok(new { status = "true", message = "Note Is Trash", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Note Is UnTrash", data });
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
                if (data.IsPin == true)
                {
                    return this.Ok(new { status = "true", message = "Note Is Pin", data });
                }
                else
                {
                    return this.BadRequest(new { staus = "false", message = "Note Is UnPin", data });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{noteId}/SetColor")]
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
    }
}