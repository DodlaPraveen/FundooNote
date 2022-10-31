using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ReposatoryLayer.AppControl;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReposatoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        private readonly Context context;
        private readonly IConfiguration Iconfiguration;
        public const string CLOUD_NAME = "dtywik6ov";
        public const string API_KEY = "528372398824342";
        public const string API_Secret = "DxTDlCWJDsMJ31v_EZd_ozkbP4A";
        public static Cloudinary cloud;

        public NoteRL(Context context, IConfiguration Iconfiguration)
        {
            this.context = context;
            this.Iconfiguration = Iconfiguration;
        }
        public NoteEntity AddNote(NoteModel notes, long userid)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();
                noteEntity.Title = notes.Title;
                noteEntity.Note = notes.Note;
                noteEntity.Color = notes.Color;
                noteEntity.Image = notes.Image;
                noteEntity.IsArchive = notes.IsArchive;
                noteEntity.IsPin = notes.IsPin;
                noteEntity.IsTrash = notes.IsTrash;
                noteEntity.userid = userid;
                noteEntity.Createat = notes.Createat;
                noteEntity.Modifiedat = notes.Modifiedat;
                this.context.Notes.Add(noteEntity);
                int result = this.context.SaveChanges();
                if (result > 0)
                {
                    return noteEntity;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteNote(long noteid)
        {
            try
            {
                var result = this.context.Notes.FirstOrDefault(x => x.NoteID == noteid);
                context.Remove(result);
                int deletednote = this.context.SaveChanges();
                if (deletednote > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity UpdateNotes(NoteModel notes, long noteid)
        {
            try
            {
                NoteEntity result = context.Notes.Where(e => e.NoteID == noteid).FirstOrDefault();
                if (result != null)
                {
                    //NoteEntity noteEntity = new NoteEntity();
                    result.Title = notes.Title;
                    result.Note = notes.Note;
                    result.Color = notes.Color;
                    result.Image = notes.Image;
                    result.IsArchive = notes.IsArchive;
                    result.IsPin = notes.IsPin;
                    result.IsTrash = notes.IsTrash;
                    context.Notes.Update(result);
                    context.SaveChanges();
                    return result;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity IsPinORNot(long noteid)
        {
            try
            {
                NoteEntity result = this.context.Notes.FirstOrDefault(x => x.NoteID == noteid);
                if (result.IsPin == true)
                {
                    result.IsPin = false;
                    this.context.SaveChanges();
                    return result;
                }
                result.IsPin = true;
                this.context.SaveChanges();
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity IstrashORNot(long noteid)
        {
            try
            {
                NoteEntity result = this.context.Notes.FirstOrDefault(x => x.NoteID == noteid);
                if (result.IsTrash == true)
                {
                    result.IsTrash = false;
                    this.context.SaveChanges();
                    return result;
                }
                result.IsTrash = true;
                this.context.SaveChanges();
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity IsArchiveORNot(long noteid)
        {
            try
            {
                NoteEntity result = this.context.Notes.FirstOrDefault(x => x.NoteID == noteid);
                if (result.IsArchive == true)
                {
                    result.IsArchive = false;
                    this.context.SaveChanges();
                    return result;
                }
                result.IsArchive = true;
                this.context.SaveChanges();
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity Color(long noteid, string color)
        {
            try
            {
                NoteEntity note = this.context.Notes.FirstOrDefault(x => x.NoteID == noteid);
                if (note.Color != null)
                {
                    note.Color = color;
                    this.context.SaveChanges();
                    return note;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<NoteEntity> GetAllNotes()
        {
            return context.Notes.ToList();
        }
        public IEnumerable<NoteEntity> GetAllNotesbyuserid(long userid)
        {
            return context.Notes.Where(n => n.userid == userid).ToList();
        }
        public NoteEntity UploadImage(long noteid, IFormFile img)
        {
            try
            {
                var noteId = this.context.Notes.FirstOrDefault(e => e.NoteID == noteid);
                if (noteId != null)
                {
                    Account acc = new Account(CLOUD_NAME, API_KEY, API_Secret);
                    cloud = new Cloudinary(acc);
                    var imagePath = img.OpenReadStream();
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(img.FileName, imagePath)
                    };
                    var uploadresult = cloud.Upload(uploadParams);
                    noteId.Image = img.FileName;
                    context.Notes.Update(noteId);
                    int upload = context.SaveChanges();
                    if (upload > 0)
                    {
                        return noteId;
                    }
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
