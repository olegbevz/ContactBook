namespace UMSSoft.ContactList.Models
{
    using System.IO;
    using System.Web;

    using UMSSoft.ContactList.Controllers;

    public class NinjectModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            this.Bind<ContactController>().To<ContactController>();

            //this.Bind<IContactRepository>().ToMethod(context => MemoryRepository.Instance);

            this.Bind<IContactRepository>().To<XmlRepository>().WithConstructorArgument("filePath", context =>
                {
                    var directory = HttpContext.Current.Server.MapPath("~/App_Data");

                    if (string.IsNullOrEmpty(directory))
                    {
                        throw new DirectoryNotFoundException();
                    }

                    return Path.Combine(directory, "contacts.xml");
                });
        }
    }
}