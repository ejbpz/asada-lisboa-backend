using AsadaLisboaBackend.FileSystem;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.FileSystem;
using AsadaLisboaBackend.ServiceContracts.ReCaptcha;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.Account;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.Editor;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Jwt;
using AsadaLisboaBackend.Repositories.AboutUsSections;
using AsadaLisboaBackend.Repositories.Configurations;
using AsadaLisboaBackend.Repositories.Contacts;
using AsadaLisboaBackend.Repositories.Statuses;
using AsadaLisboaBackend.Repositories.Charges;
using AsadaLisboaBackend.Repositories.Images;
using AsadaLisboaBackend.Repositories.Users;
using AsadaLisboaBackend.Repositories.News;
using AsadaLisboaBackend.Services.AboutUsSections;
using AsadaLisboaBackend.Services.Configurations;
using AsadaLisboaBackend.Services.ReCaptcha;
using AsadaLisboaBackend.Services.Statuses;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.Services.Account;
using AsadaLisboaBackend.Services.Charges;
using AsadaLisboaBackend.Services.Editor;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Services.Email;
using AsadaLisboaBackend.Services.Image;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Services.Jwt;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class ServicesExtension
    {
        public static IServiceCollection ServicesRegistration(this IServiceCollection services)
        {
            // File System
            services.AddScoped<IFileSystemManager, FileSystemManager>();

            // ReCaptcha
            services.AddScoped<IReCaptchaService, ReCaptchaService>();

            // Contacts
            services.AddScoped<IContactsAdderService, ContactsAdderService>();
            services.AddScoped<IContactsGetterService, ContactsGetterService>();
            services.AddScoped<IContactsUpdaterService, ContactsUpdaterService>();
            services.AddScoped<IContactsDeleterService, ContactsDeleterService>();

            services.AddScoped<IContactsAdderRepository, ContactsAdderRepository>();
            services.AddScoped<IContactsGetterRepository, ContactsGetterRepository>();
            services.AddScoped<IContactsUpdaterRepository, ContactsUpdaterRepository>();
            services.AddScoped<IContactsDeleterRepository, ContactsDeleterRepository>();

            // About Us
            services.AddScoped<IAboutUsSectionsAdderRepository, AboutUsSectionsAdderRepository>();
            services.AddScoped<IAboutUsSectionsGetterRepository, AboutUsSectionsGetterRepository>();
            services.AddScoped<IAboutUsSectionsUpdaterRepository, AboutUsSectionsUpdaterRepository>();
            services.AddScoped<IAboutUsSectionsDeleterRepository, AboutUsSectionsDeleterRepository>();

            services.AddScoped<IAboutUsSectionsAdderService, AboutUsSectionsAdderService>();
            services.AddScoped<IAboutUsSectionsGetterService, AboutUsSectionsGetterService>();
            services.AddScoped<IAboutUsSectionsUpdaterService, AboutUsSectionsUpdaterService>();
            services.AddScoped<IAboutUsSectionsDeleterService, AboutUsSectionsDeleterService>();

            // Configurations
            services.AddScoped<IConfigurationsAdderRepository, ConfigurationsAdderRepository>();
            services.AddScoped<IConfigurationsGetterRepository, ConfigurationsGetterRepository>();
            services.AddScoped<IConfigurationsUpdaterRepository, ConfigurationsUpdaterRepository>();
            services.AddScoped<IConfigurationsDeleterRepository, ConfigurationsDeleterRepository>();

            services.AddScoped<IConfigurationsAdderService, ConfigurationsAdderService>();
            services.AddScoped<IConfigurationsGetterService, ConfigurationsGetterService>();
            services.AddScoped<IConfigurationsUpdaterService, ConfigurationsUpdaterService>();
            services.AddScoped<IConfigurationsDeleterService, ConfigurationsDeleterService>();

            // Images
            services.AddScoped<IImagesAdderRepository, ImagesAdderRepository>();
            services.AddScoped<IImagesGetterRepository, ImagesGetterRepository>();
            services.AddScoped<IImagesUpdaterRepository, ImagesUpdaterRepository>();
            services.AddScoped<IImagesDeleterRepository, ImagesDeleterRepository>();

            services.AddScoped<IImagesAdderService, ImagesAdderService>();
            services.AddScoped<IImagesGetterService, ImagesGetterService>();
            services.AddScoped<IImagesUpdaterService, ImagesUpdaterService>();
            services.AddScoped<IImagesDeleterService, ImagesDeleterService>();

            // News
            services.AddScoped<INewsAdderRepository, NewsAdderRepository>();
            services.AddScoped<INewsGetterRepository, NewsGetterRepository>();
            services.AddScoped<INewsUpdaterRepository, NewsUpdaterRepository>();
            services.AddScoped<INewsDeleterRepository, NewsDeleterRepository>();

            services.AddScoped<INewsAdderService, NewsAdderService>();
            services.AddScoped<INewsGetterService, NewsGetterService>();
            services.AddScoped<INewsUpdaterService, NewsUpdaterService>();
            services.AddScoped<INewsDeleterService, NewsDeleterService>();

            // Statuses
            services.AddScoped<IStatusesGetterRepository, StatusesGetterRepository>();
            services.AddScoped<IStatusesUpdaterRepository, StatusesUpdaterRepository>();

            services.AddScoped<IStatusesGetterService, StatusesGetterService>();
            services.AddScoped<IStatusesUpdaterService, StatusesUpdaterService>();

            // Charges
            services.AddScoped<IChargesGetterRepository, ChargesGetterRepository>();
            services.AddScoped<IChargesUpdaterRepository, ChargesUpdaterRepository>();

            services.AddScoped<IChargesGetterService, ChargesGetterService>();
            services.AddScoped<IChargesUpdaterService, ChargesUpdaterService>();

            // Editor
            services.AddScoped<IEditorAdderService, EditorAdderService>();
            services.AddScoped<IEditorUpdaterService, EditorUpdaterService>();
            services.AddScoped<IEditorDeleterService, EditorDeleterService>();

            // Account
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddScoped<IVerificationCodeService, VerificationCodeService>();

            // Users
            services.AddScoped<IUsersGetterService, UsersGetterService>();
            services.AddScoped<IUsersUpdaterService, UsersUpdaterService>();
            services.AddScoped<IUsersDeleterService, UsersDeleterService>();
            services.AddScoped<IUsersGetterRepository, UsersGetterRepository>();

            return services;
        }
    }
}
