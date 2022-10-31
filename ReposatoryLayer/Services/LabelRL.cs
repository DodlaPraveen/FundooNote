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
    public class LabelRL : ILabelRL
    {
        private readonly Context context;
        private readonly IConfiguration Iconfiguration;
        public LabelRL(Context context, IConfiguration Iconfiguration)
        {
            this.context = context;
            this.Iconfiguration = Iconfiguration;
        }

        public LabelEntity Addlabel(long noteid, long userid, string label)
        {
            try
            {
                LabelEntity Entity = new LabelEntity();
                Entity.LabelName = label;
                Entity.Userid = userid;
                Entity.Noteid = noteid;
                this.context.Labels.Add(Entity);
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
        public IEnumerable<LabelEntity> GetlabelsByNoteid(long noteid, long userid)
        {
            return context.Labels.Where(e => e.Noteid == noteid && e.Userid == userid).ToList();
        }
        public bool RemoveLabel(long userID, string labelName)
        {
            try
            {
                var result = this.context.Labels.FirstOrDefault(x => x.Userid == userID && x.LabelName == labelName);
                if (result != null)
                {
                    context.Remove(result);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<LabelEntity> RenameLabel(long userID, string oldLabelName, string labelName)
        {
            IEnumerable<LabelEntity> labels;
            labels = context.Labels.Where(x => x.Userid == userID && x.LabelName == oldLabelName).ToList();
            if (labels != null)
            {
                foreach (var newlabel in labels)
                {
                    newlabel.LabelName = labelName;
                }
                context.SaveChanges();
                return labels;
            }
            return null;
        }

    }
}
