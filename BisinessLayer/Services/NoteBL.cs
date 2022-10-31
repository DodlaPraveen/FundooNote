using BisinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public NoteEntity AddNote(NoteModel notes, long userid)
        {
            try
            {


                return this.noteRL.AddNote(notes, userid);
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
                return this.noteRL.DeleteNote(noteid);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public NoteEntity UpdateNotes(NoteModel noteModel, long noteid)
        {
            try
            {
                return this.noteRL.UpdateNotes(noteModel, noteid);
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
                return this.noteRL.IsPinORNot(noteid);
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
                return this.noteRL.IstrashORNot(noteid);
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
                return this.noteRL.IsArchiveORNot(noteid);
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
                return this.noteRL.Color(noteid, color);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<NoteEntity> GetAllNotes()
        {
            try
            {
                return this.noteRL.GetAllNotes();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<NoteEntity> GetAllNotesbyuserid(long userid)
        {
            try
            {
                return this.noteRL.GetAllNotesbyuserid(userid);
            }
            catch (Exception)
            {

                throw;
            }
        }
    
       public NoteEntity UploadImage(long noteid, IFormFile img)
        {
            try
            {
                return this.noteRL.UploadImage(noteid, img);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
