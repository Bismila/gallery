using System.Web.Http;
using SimpleInjector;
using Gallery.BAL.Interfaces;
using Gallery.DAL.RepositoryClasses;
using Gallery.DAL.IRepository;
using Gallery.BAL.Services;
using System.Data;
using System.Data.SqlClient;
using Gallery.BAL.Providers;
using Gallery.DAL.EFInfrastructure.EFRepository;
using System.Data.Entity;
using Gallery.DAL.EFInfrastructure.EFContext;
using SimpleInjector.Integration.Web;

using SimpleInjector.Integration.WebApi;

namespace Gallery.Api
{
    public static class InjectionWebApi
    {
        public static void ApiApplicationStart()
        {
            var container = new Container();
            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Options.DefaultLifestyle = new WebRequestLifestyle();

            // Register your types, for instance using the scoped lifestyle:
            container.Register<IUserService, UserService>();
            container.Register<IRoleService, RoleService>();
            container.Register<IUserRepository, UserRepository>();
            //container.Register<IUserRepository, EFUserRepository>();
            container.Register<IRoleRepository, RoleRepository>();
            container.Register<IFriendRepository, FriendRepository>();
            container.Register<CustomRoleProvider>();
            container.Register<IFriendService, FriendService>();
            container.Register<IImageService, ImageService>();
            container.Register<ILikeRepository, LikeRepository>();
            container.Register<ILikeService, LikeService>();
            container.Register<ICommentRepository, CommentRepository>();
            container.Register<ICommentService, CommentService>();
            container.Register<IImageRepository, ImageRepository>();
            //container.Register<IImageRepository, EFImageRepository>();
            //container.Register<IFileService, BlobService>();
            container.Register<IFileService, LocalFileService>();

            //container.Register<DbContext, GalleryContext>();

            container.RegisterSingleton<IDbConnection>(new SqlConnection("Data Source=(local);Integrated Security=True;Initial Catalog=dbGallery"));


            // This is an extension method from the integration package.
            //container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.EnableHttpRequestMessageTracking(GlobalConfiguration.Configuration);


            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            //DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

        }
    }
}