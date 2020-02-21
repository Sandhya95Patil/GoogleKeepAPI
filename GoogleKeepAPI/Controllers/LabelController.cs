//-----------------------------------------------------------------------
// <copyright file="LabelController.cs" company="BridgeLabz">
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
    using CommonLayer.ShowModel;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// LabelController class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        /// <summary>
        /// Inject the business layer interface
        /// </summary>
        private readonly ILabelBL labelBL;

        /// <summary>
        /// Initializes the memory for label controller class
        /// </summary>
        /// <param name="labelBL"></param>
        public LabelController (ILabelBL labelBL)
        {
            this.labelBL = labelBL;
        }

        /// <summary>
        /// Add Label method 
        /// </summary>
        /// <param name="showLabel">showLabel parameter</param>
        /// <returns>returns the added label</returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddLabel(ShowLabelModel showLabel)
        {
            try
            {
                var claim= Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await labelBL.AddLabel(showLabel, claim);
                if (data != null)
                {
                    return Ok(new { status = "true", message = "Label Added Successfully", data });
                }
                else
                {
                    return BadRequest(new { status = "false", message = "Failed To Add Label" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Update Label method
        /// </summary>
        /// <param name="showLabel">showLabel parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>returns the updated label</returns>
        [HttpPut]
        [Route("{labelId}")]
        public async Task<IActionResult> UpdateLabel(ShowLabelModel showLabel, int labelId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await labelBL.UpdateLabel(showLabel, claim, labelId);
                if (data != null)
                {
                    return Ok(new { status = "true", message = "Label Updated Successfully", data });
                }
                else
                {
                    return BadRequest(new { status = "false", message = "Failed To Update Label" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Delete Label method
        /// </summary>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>return the deleted label</returns>
        [HttpDelete]
        [Route("{labelId}")]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await labelBL.DeleteLabel(claim, labelId);
                if (data != null)
                {
                    return Ok(new { status = "true", message = "Label Deleted Successfully" });
                }
                else
                {
                    return BadRequest(new { status = "false", message = "Failed To Delete Label" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Get All Labels method
        /// </summary>
        /// <returns>return the all labels</returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllLabels()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await labelBL.GetAllLabels(claim);
                if (data != null)
                {
                    return Ok(new { status = "true", message = "All Label", data });
                }
                else
                {
                    return BadRequest(new { status = "false", message = "Failed To Get All Labels" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

    }
}