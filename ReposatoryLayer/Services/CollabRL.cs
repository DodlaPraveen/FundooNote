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
    public class CollabRL : ICollabRL
    {
        private readonly Context context;
        private readonly IConfiguration Iconfiguration;
        public CollabRL(Context context, IConfiguration Iconfiguration)
        {
            this.context = context;
            this.Iconfiguration = Iconfiguration;
        }
        public CollabEntity AddCollab(long noteid, long userid, string email)
        {
            try
            {
                CollabEntity Entity = new CollabEntity();
                Entity.CollabEmail = email;
                Entity.Userid = userid;
                Entity.Noteid = noteid;
                this.context.Collaborator.Add(Entity);
                int result = this.context.SaveChanges();
                if (result > 0)
                {
                    return Entity;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool Remove(long collabid)
        {
            try
            {
                var result = this.context.Collaborator.FirstOrDefault(x => x.CollabId == collabid);
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
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteid)
        {
            return context.Collaborator.Where(n => n.Noteid == noteid).ToList();
        }
    }
}
