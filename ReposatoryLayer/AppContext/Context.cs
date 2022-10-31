using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReposatoryLayer.AppControl
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<LabelEntity> Labels { get; set; }
        public DbSet<CollabEntity> Collaborator { get; set; }
    }
    
}
