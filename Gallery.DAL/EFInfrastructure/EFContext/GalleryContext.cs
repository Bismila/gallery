using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.EFInfrastructure.EFContext
{
    public class GalleryContext: DbContext
    {
        public GalleryContext() 
            : base("name=DefaultConnection")
        {
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<Image> Images { get; set; }
        public IDbSet<Friend> Friends { get; set; }

        // Переопределяем метод OnModelCreating для добавления
        // настроек конфигурации
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // настройка полей с помощью Fluent API
            modelBuilder.Entity<Image>()
                .Ignore(i => i.userName);

            modelBuilder.Entity<Friend>()
               .Ignore(f => f.Users);

        }
    }
}
