using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Services.Searches;
using AsadaLisboaBackend;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AsadaLisboaBackend.Tests.Searches
{
    public class SearchesGetterServiceTest : IAsyncLifetime, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly Fixture _fixture;
        private readonly WebApplicationFactory<Program> _factory;

        private readonly PostgreSqlContainer _postgresContainer;

        private DbContextOptions<ApplicationDbContext> _options = null!;

        public SearchesGetterServiceTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _fixture = new Fixture();

            _fixture.Customize<New>(composer => composer
               .With(x => x.Slug, () => $"slug-{Guid.NewGuid().ToString().Substring(0, 5)}"));

            _fixture.Customize<Document>(composer => composer
              .With(x => x.Slug, () => $"slug-{Guid.NewGuid().ToString().Substring(0, 5)}")
              .With(x => x.Url, () => $"doc-{Guid.NewGuid().ToString().Substring(0, 5)}.pdf"));

            _fixture.Customize<Image>(composer => composer
              .With(x => x.Url, () => $"img-{Guid.NewGuid().ToString().Substring(0, 5)}.jpg"));

            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(x => _fixture.Behaviors.Remove(x));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Customize<DateTime>(c =>
                     c.FromFactory(() => DateTime.UtcNow));

            _postgresContainer = new PostgreSqlBuilder("postgres:16")
                .WithDatabase("testdb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(_postgresContainer.GetConnectionString())
                .Options;

            await using var context = new ApplicationDbContext(_options);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgresContainer.DisposeAsync();
        }

        [Fact]
        public async Task Search_Should_Return_Empty_When_Query_Is_Invalid()
        {
            // Arrange
            await using var context = new ApplicationDbContext(_options);

            var service = new SearchesGetterService(context);

            // Act
            var result = await service.Search("a");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Search_Should_Return_News_Documents_And_Images()
        {
            // Arrange
            await using var context = new ApplicationDbContext(_options);
            
            var publishedStatus = await context.Statuses
                .FirstOrDefaultAsync(x => x.Name == "Publicado");

            if (publishedStatus == null)
            {

                publishedStatus = _fixture.Build<Status>()
                    .With(x => x.Name, "Publicado")
                    .OmitAutoProperties()
                    .Create();

                context.Statuses.Add(publishedStatus);
                await context.SaveChangesAsync();
            }


            var documentType = await context.DocumentTypes
               .FirstOrDefaultAsync(x => x.Name == "PDF");

            if (documentType == null)
            {
                documentType = _fixture.Build<DocumentType>()
                    .With(x => x.Name, "PDF")
                    .OmitAutoProperties()
                    .Create();

                context.DocumentTypes.Add(documentType);
                await context.SaveChangesAsync();
            }

            var news = _fixture.Build<New>()
                .With(x => x.Title, "Costa Rica News")
                .With(x => x.Description, "Important news")
                .With(x => x.ImageUrl, "/news/cr-new.jpg")
                .With(x => x.Slug, "costa-rica-news")
                .With(x => x.Status, publishedStatus)
                .OmitAutoProperties()
                .Create();

            var document = _fixture.Build<Document>()
                .With(x => x.Title, "Costa Rica Document")
                .With(x => x.Description, "Official document")
                .With(x => x.Url, "/doc/test.pdf")
                .With(x => x.Slug, "costa-rica-document")               
                .With(x => x.Status, publishedStatus)
                .With(x => x.DocumentTypeId, documentType.Id)
                .With(x => x.DocumentType, documentType)
                .OmitAutoProperties()
                .Create();

            var image = _fixture.Build<Image>()
                .With(x => x.Title, "Costa Rica Image")
                .With(x => x.Description, "Beautiful image")
                .With(x => x.Url, "/img/test.jpg")
                .With(x => x.Slug, "costa-rica-image")
                .With(x => x.Status, publishedStatus)
                .OmitAutoProperties()
                .Create();

            context.News.Add(news);
            context.Documents.Add(document);
            context.Images.Add(image);

            await context.SaveChangesAsync();

            var service = new SearchesGetterService(context);

            // Act
            var result = await service.Search("Costa");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);

            result.Should().Contain(x =>
                x.Type == "Noticia" &&
                x.Url.Contains("costa-rica-news"));

            result.Should().Contain(x =>
                x.Type == "Documento" &&
                x.Url.Contains("costa-rica-document"));

            result.Should().Contain(x =>
                x.Type == "Imagen" &&
                x.Url.Contains("costa-rica-image"));
        }

        [Fact]
        public async Task Search_Should_Not_Return_Unpublished_Content()
        {
            // Arrange
            await using var context = new ApplicationDbContext(_options);


            var draftStatus = await context.Statuses
               .FirstOrDefaultAsync(x => x.Name == "Borrador");

            if (draftStatus == null)
            {

                draftStatus = _fixture.Build<Status>()
                .With(x => x.Name, "Borrador")
                .OmitAutoProperties()
                .Create();

                context.Statuses.Add(draftStatus);
                await context.SaveChangesAsync();
            }          

            var news = _fixture.Build<New>()
                .With(x => x.Title, "Hidden News")
                .With(x => x.Description, "Should not appear")
                 .With(x => x.ImageUrl, "/new/Hidden.jpg")
                .With(x => x.Status, draftStatus)
                .OmitAutoProperties()
                .Create();

            context.News.Add(news);

            await context.SaveChangesAsync();

            var service = new SearchesGetterService(context);

            // Act
            var result = await service.Search("Hidden");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Search_Should_Filter_By_Query()
        {
            // Arrange
            await using var context = new ApplicationDbContext(_options);
            
            context.News.RemoveRange(context.News);
            context.Documents.RemoveRange(context.Documents);
            context.Images.RemoveRange(context.Images);

            await context.SaveChangesAsync();

            var publishedStatus = await context.Statuses
                .FirstOrDefaultAsync(x => x.Name == "Publicado");

            if (publishedStatus == null){
            
                publishedStatus = _fixture.Build<Status>()
                    .With(x => x.Name, "Publicado")
                    .OmitAutoProperties()
                    .Create();

                context.Statuses.Add(publishedStatus);
                await context.SaveChangesAsync();
             }

            var matchingNews = _fixture.Build<New>()
                .With(x => x.Title, "Asada")
                .With(x => x.Description, "Asada Lisboa")
                 .With(x => x.ImageUrl, "/news/asada.jpg")
                 .With(x => x.Slug, "Asada")
                .With(x => x.Status, publishedStatus)
                .OmitAutoProperties()
                .Create();

            var nonMatchingNews = _fixture.Build<New>()
                .With(x => x.Title, "Politics")
                .With(x => x.Description, "Government")
                 .With(x => x.ImageUrl, "/news/politics.jpg")
                 .With(x => x.Slug, "politics")
                .With(x => x.Status, publishedStatus)
                .OmitAutoProperties()
                .Create();

            context.News.AddRange(matchingNews, nonMatchingNews);

            await context.SaveChangesAsync();

            var service = new SearchesGetterService(context);

            // Act
            var result = await service.Search("Asada");

            // Assert
            result.Should().HaveCount(1);

            result.First().Title.Should().Contain("Asada");
        }

        [Fact]
        public async Task Search_Should_Limit_Results_To_Six_Per_Entity()
        {
            // Arrange
            await using var context = new ApplicationDbContext(_options);

            context.News.RemoveRange(context.News);
            context.Documents.RemoveRange(context.Documents);
            context.Images.RemoveRange(context.Images);

            await context.SaveChangesAsync();

            var publishedStatus = await context.Statuses
                .FirstOrDefaultAsync(x => x.Name == "Publicado");

            if (publishedStatus == null)
            {
                publishedStatus = _fixture.Build<Status>()
                    .With(x => x.Name, "Publicado")
                    .OmitAutoProperties()
                    .Create();

                context.Statuses.Add(publishedStatus);
                await context.SaveChangesAsync();
            }

            var news = _fixture.Build<New>()
                .With(x => x.Title, "Cancele sus recibos con Sinpe")
                .With(x => x.Description, "Cancele sus recibos por medio de Sinpe")
                .With(x => x.ImageUrl, "/news/cancele-sus-recibos.jpg")
                .With(x => x.Slug,() => "cancele-sus-recibos-con-sinpe")
                .With(x => x.Status, publishedStatus)
                .OmitAutoProperties()
                .Create();

            context.News.AddRange(news);

            await context.SaveChangesAsync();

            var service = new SearchesGetterService(context);

            // Act
            var result = await service.Search("Cancele sus recibos con Sinpe");

            // Assert
            result.Should().HaveCount(1);
        }
    }
}