
namespace GoogleKeepAPI.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BussinessLayer.Interface;
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
    }
}