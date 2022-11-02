using BisinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using ReposatoryLayer.Entity;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ReposatoryLayer.AppControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly IMemoryCache memoryCache;
        private readonly Context context;
        private readonly IDistributedCache distributedCache;



        public NotesController(INoteBL noteBL, IMemoryCache memoryCache, Context context, IDistributedCache distributedCache)
        {
            this.noteBL = noteBL;
            this.memoryCache = memoryCache;
            this.context = context;
            this.distributedCache = distributedCache;



        }
        [HttpPost("AddNote")]
        public IActionResult AddNote(NoteModel noteModel)
        {
            try
            {
                long UserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = noteBL.AddNote(noteModel, UserId);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Note added successfully", Response = result });
                }

                return BadRequest(new { success = false, message = "Note not added", });
            }

            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("Remove")]
        public IActionResult DeleteNotes(long noteid)
        {
            try
            {
                if (noteBL.DeleteNote(noteid))
                {
                    return this.Ok(new { Success = true, message = "Note Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to delete note" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult updateNotes(NoteModel addnote, long noteid)
        {
            try
            {
                
                var result = noteBL.UpdateNotes(addnote, noteid);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Note Updated Successfully", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to Update note" });
                }
            }
            catch (Exception)
            {
                return this.BadRequest(new { Success = false, message = "Unable to Update note" });
            }
        }

        [Authorize]
        [HttpPut("Pin")]
        public IActionResult Ispinornot(long noteid)
        {
            try
            {
                var result = noteBL.IsPinORNot(noteid);
                if (result != null)
                {
                    return this.Ok(new { Success=true,message = "Note unPinned ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Note Pinned Successfully" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Trash")]
        public IActionResult Istrashornot(long noteid)
        {
            try
            {
                var result = noteBL.IstrashORNot(noteid);
                if (result != null)
                {
                    return this.Ok(new { message = "Note Restored ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { message = "Note is in trash" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Archive")]
        public IActionResult IsArchiveORNot(long noteid)
        {
            try
            {
                var result = noteBL.IsArchiveORNot(noteid);
                if (result != null)
                {
                    return this.Ok(new { message = "Note Unarchived ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { message = "Note Archived Successfully" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Color")]
        public IActionResult Color(long noteid, string color)
        {
            try
            {
                var result = noteBL.Color(noteid, color);
                if (result != null)
                {
                    return this.Ok(new { message = "Color is changed ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { message = "Unable to change color" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("ByUser")]
        public IEnumerable<NoteEntity> GetAllNotesbyuser(long userid)
        {
            try
            {
                return noteBL.GetAllNotesbyuserid(userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("AllNotes")]
        public IEnumerable<NoteEntity> GetAllNote()
        {
            try
            {
                return noteBL.GetAllNotes();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("Upload")]
        public IActionResult UploadImage(long noteid, IFormFile img)
        {
            try
            {
                var result = noteBL.UploadImage(noteid, img);
                if (result != null)
                {
                    return this.Ok(new { message = "uploaded ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { message = "Not uploaded" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("RedisCache")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NodeList";
            string serializedNotesList;
            var NotesList = new List<NoteEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNotesList);
            }
            else
            {
                //NotesList = await context.Notes.ToListAsync();
                NotesList = (List<NoteEntity>)noteBL.GetAllNotes();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }

    }
}
