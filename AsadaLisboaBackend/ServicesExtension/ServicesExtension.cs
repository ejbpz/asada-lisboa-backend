using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;
using AsadaLisboaBackend.RepositoryContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.Principals;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.ReCaptchas;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.Accounts;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Jwts;
using AsadaLisboaBackend.ServiceContracts.SearchGlobal;
using AsadaLisboaBackend.Repositories.AboutUsSections;
using AsadaLisboaBackend.Repositories.Configurations;
using AsadaLisboaBackend.Repositories.DocumentTypes;
using AsadaLisboaBackend.Repositories.Categories;
using AsadaLisboaBackend.Repositories.Documents;
using AsadaLisboaBackend.Repositories.Contacts;
using AsadaLisboaBackend.Repositories.Statuses;
using AsadaLisboaBackend.Repositories.Charges;
using AsadaLisboaBackend.Repositories.Images;
using AsadaLisboaBackend.Repositories.Users;
using AsadaLisboaBackend.Repositories.News;
using AsadaLisboaBackend.Services.AboutUsSections;
using AsadaLisboaBackend.Services.Configurations;
using AsadaLisboaBackend.Services.MemoryCaches;
using AsadaLisboaBackend.Services.Principals;
using AsadaLisboaBackend.Services.Categories;
using AsadaLisboaBackend.Services.ReCaptchas;
using AsadaLisboaBackend.Services.Documents;
using AsadaLisboaBackend.Services.Statuses;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.Services.Accounts;
using AsadaLisboaBackend.Services.Charges;
using AsadaLisboaBackend.Services.Editors;
using AsadaLisboaBackend.Services.Emails;
using AsadaLisboaBackend.Services.Images;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Services.Jwts;
using AsadaLisboaBackend.Services.SearchGlobal;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class ServicesExtension
    {
        public static IServiceCollection ServicesRegistration(this IServiceCollection services)
        {
            // Cache
            services.AddScoped<IMemoryCachesService, MemoryCachesService>();

            // File System
            services.AddScoped<IFileSystemsManager, FileSystemsManager>();

            // ReCaptcha
            services.AddScoped<IReCaptchasService, ReCaptchasService>();

            // Document Types
            services.AddScoped<IDocumentTypesGetterRepository, DocumentTypesGetterRepository>();

            // Principals
            services.AddScoped<IPrincipalsGetterService, PrincipalsGetterService>();

            // Contacts
            services.AddScoped<IContactsUpdaterService, ContactsUpdaterService>();
            services.AddScoped<IContactsDeleterService, ContactsDeleterService>();
            services.AddScoped<IContactsGetterService, ContactsGetterService>();
            services.AddScoped<IContactsAdderService, ContactsAdderService>();

            services.AddScoped<IContactsAdderRepository, ContactsAdderRepository>();
            services.AddScoped<IContactsGetterRepository, ContactsGetterRepository>();
            services.AddScoped<IContactsUpdaterRepository, ContactsUpdaterRepository>();
            services.AddScoped<IContactsDeleterRepository, ContactsDeleterRepository>();

            // About Us
            services.AddScoped<IAboutUsSectionsUpdaterRepository, AboutUsSectionsUpdaterRepository>();
            services.AddScoped<IAboutUsSectionsDeleterRepository, AboutUsSectionsDeleterRepository>();
            services.AddScoped<IAboutUsSectionsGetterRepository, AboutUsSectionsGetterRepository>();
            services.AddScoped<IAboutUsSectionsAdderRepository, AboutUsSectionsAdderRepository>();

            services.AddScoped<IAboutUsSectionsAdderService, AboutUsSectionsAdderService>();
            services.AddScoped<IAboutUsSectionsGetterService, AboutUsSectionsGetterService>();
            services.AddScoped<IAboutUsSectionsUpdaterService, AboutUsSectionsUpdaterService>();
            services.AddScoped<IAboutUsSectionsDeleterService, AboutUsSectionsDeleterService>();

            // Configurations
            services.AddScoped<IConfigurationsUpdaterRepository, ConfigurationsUpdaterRepository>();
            services.AddScoped<IConfigurationsDeleterRepository, ConfigurationsDeleterRepository>();
            services.AddScoped<IConfigurationsGetterRepository, ConfigurationsGetterRepository>();
            services.AddScoped<IConfigurationsAdderRepository, ConfigurationsAdderRepository>();

            services.AddScoped<IConfigurationsAdderService, ConfigurationsAdderService>();
            services.AddScoped<IConfigurationsGetterService, ConfigurationsGetterService>();
            services.AddScoped<IConfigurationsUpdaterService, ConfigurationsUpdaterService>();
            services.AddScoped<IConfigurationsDeleterService, ConfigurationsDeleterService>();

            // Documents
            services.AddScoped<IDocumentsUpdaterRepository, DocumentsUpdaterRepository>();
            services.AddScoped<IDocumentsDeleterRepository, DocumentsDeleterRepository>();
            services.AddScoped<IDocumentsGetterRepository, DocumentsGetterRepository>();
            services.AddScoped<IDocumentsAdderRepository, DocumentsAdderRepository>();

            services.AddScoped<IDocumentsAdderService, DocumentsAdderService>();
            services.AddScoped<IDocumentsGetterService, DocumentsGetterService>();
            services.AddScoped<IDocumentsUpdaterService, DocumentsUpdaterService>();
            services.AddScoped<IDocumentsDeleterService, DocumentsDeleterService>();

            // Images
            services.AddScoped<IImagesUpdaterRepository, ImagesUpdaterRepository>();
            services.AddScoped<IImagesDeleterRepository, ImagesDeleterRepository>();
            services.AddScoped<IImagesGetterRepository, ImagesGetterRepository>();
            services.AddScoped<IImagesAdderRepository, ImagesAdderRepository>();

            services.AddScoped<IImagesAdderService, ImagesAdderService>();
            services.AddScoped<IImagesGetterService, ImagesGetterService>();
            services.AddScoped<IImagesUpdaterService, ImagesUpdaterService>();
            services.AddScoped<IImagesDeleterService, ImagesDeleterService>();

            // News
            services.AddScoped<INewsUpdaterRepository, NewsUpdaterRepository>();
            services.AddScoped<INewsDeleterRepository, NewsDeleterRepository>();
            services.AddScoped<INewsGetterRepository, NewsGetterRepository>();
            services.AddScoped<INewsAdderRepository, NewsAdderRepository>();

            services.AddScoped<INewsAdderService, NewsAdderService>();
            services.AddScoped<INewsGetterService, NewsGetterService>();
            services.AddScoped<INewsUpdaterService, NewsUpdaterService>();
            services.AddScoped<INewsDeleterService, NewsDeleterService>();

            // Statuses
            services.AddScoped<IStatusesUpdaterRepository, StatusesUpdaterRepository>();
            services.AddScoped<IStatusesGetterRepository, StatusesGetterRepository>();

            services.AddScoped<IStatusesGetterService, StatusesGetterService>();
            services.AddScoped<IStatusesUpdaterService, StatusesUpdaterService>();

            // Categories
            services.AddScoped<ICategoriesDeleterRepository, CategoriesDeleterRepository>();
            services.AddScoped<ICategoriesGetterRepository, CategoriesGetterRepository>();
            services.AddScoped<ICategoriesAdderRepository, CategoriesAdderRepository>();

            services.AddScoped<ICategoriesGetterService, CategoriesGetterService>();
            services.AddScoped<ICategoriesDeleterService, CategoriesDeleterService>();

            // Charges
            services.AddScoped<IChargesUpdaterRepository, ChargesUpdaterRepository>();
            services.AddScoped<IChargesDeleterRepository, ChargesDeleterRepository>();
            services.AddScoped<IChargesGetterRepository, ChargesGetterRepository>();
            services.AddScoped<IChargesAdderRepository, ChargesAdderRepository>();

            services.AddScoped<IChargesAdderService, ChargesAdderService>();
            services.AddScoped<IChargesGetterService, ChargesGetterService>();
            services.AddScoped<IChargesDeleterService, ChargesDeleterService>();
            services.AddScoped<IChargesUpdaterService, ChargesUpdaterService>();

            // Editor
            services.AddScoped<IEditorsAdderService, EditorsAdderService>();
            services.AddScoped<IEditorsUpdaterService, EditorsUpdaterService>();
            services.AddScoped<IEditorsDeleterService, EditorsDeleterService>();

            // Account
            services.AddScoped<IJwtsService, JwtsService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IEmailsSenderService, EmailsSenderService>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddScoped<IVerificationCodeService, VerificationCodeService>();

            // Users
            services.AddScoped<IUsersGetterService, UsersGetterService>();
            services.AddScoped<IUsersUpdaterService, UsersUpdaterService>();
            services.AddScoped<IUsersDeleterService, UsersDeleterService>();
            services.AddScoped<IUsersGetterRepository, UsersGetterRepository>();

            //SearchGlobal
            services.AddScoped<ISearchGlobalService, SearchGlobalService>();

            return services;
        }
    }
}
