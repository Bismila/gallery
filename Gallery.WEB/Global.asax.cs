using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using Gallery.BAL.Interfaces;
using Gallery.DAL.RepositoryClasses;
using Gallery.DAL.IRepository;
using Gallery.BAL.Services;
using System.Data;
using System.Data.SqlClient;
using Gallery.BAL.Providers;
using Gallery.DAL.EFInfrastructure.EFRepository;
using System.Data.Entity;
using SimpleInjector.Lifestyles;
using Gallery.DAL.EFInfrastructure.EFContext;
using SimpleInjector.Integration.Web;

namespace Gallery.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 1. Create a new Simple Injector container
            var container = new Container();
            container.Options.DefaultLifestyle = new WebRequestLifestyle();

            // 2. Configure the container (register). See below for more configuration examples
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

            container.Register<DbContext, GalleryContext>();

            container.RegisterSingleton<IDbConnection>(new SqlConnection("Data Source=(local);Integrated Security=True;Initial Catalog=dbGallery"));



            // 3. Optionally verify the container's configuration.
            container.Verify();

            // 4. Store the container for use by the application
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
    }
}
