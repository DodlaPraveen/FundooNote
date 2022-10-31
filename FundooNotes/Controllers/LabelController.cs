using BisinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using ReposatoryLayer.Entity;
using System.Collections.Generic;

namespace FundooNotes.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class LabelController : ControllerBase
    {
        ILabelBL lables;

        public LabelController(ILabelBL lables)
        {
            this.lables = lables;
        }
        [HttpPost("Add")]
        public IActionResult AddLabels(long noteid, string labelss)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.First(e => e.Type == "UserId").Value);
                var result = lables.Addlabel(noteid, UserId, labelss);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Labels Added Successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to add" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpDelete("Remove")]
        public IActionResult RemoveLabel(string lableName)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                if (lables.RemoveLabel(UserId, lableName))
                {
                    return this.Ok(new { success = true, message = "Label removed successfully" });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "User access denied" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Rename")]
        public IActionResult RenameLabel(string lableName, string newLabelName)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = lables.RenameLabel(UserId, lableName, newLabelName);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = "Label renamed successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { success = false, message = "Unable to rename" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("ByUser")]
        public IEnumerable<LabelEntity> GetByuserid(long noteid)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                return lables.GetlabelsByNoteid(noteid, UserId);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
