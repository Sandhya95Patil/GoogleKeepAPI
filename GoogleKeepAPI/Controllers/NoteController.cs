using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLayer.Interface;
using CommonLayer.ShowModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleKeepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INotesBL noteBL;
        public NoteController (INotesBL noteBL)
        {
            this.noteBL = noteBL;
        }
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

        [HttpPut]
        [Route("{noteId}")]
        public async Task<IActionResult> UpdateNote(ShowNoteModel showNoteModel, int noteId)
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
    }
}