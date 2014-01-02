namespace ContactBook.Models
{
    using System.IO;
    using System.Web;

    using ContactBook.Controllers;

    public class NinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            //this.Bind<ContactController>().To<ContactController>();

            //this.Bind<IContactRepository>().To<Repositories.NHibernate.ContactRepository>();

            //this.Bind<IContactRepository>().ToMethod(context => MemoryRepository.Instance);

            this.Bind<IContactRepository>().To<Repositories.LinqToXml.ContactRepository>().WithConstructorArgument("fileName", context =>
                {
                    var directory = HttpContext.Current.Server.MapPath("~/App_Data");

                    if (string.IsNullOrEmpty(directory))
                    {
                        throw new DirectoryNotFoundException();
                    }

                    return Path.Combine(directory, "contactbook-linqtoxml.xml");
                });
        }
    }
}