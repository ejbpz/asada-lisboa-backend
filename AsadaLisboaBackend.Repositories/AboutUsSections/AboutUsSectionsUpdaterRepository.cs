using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Repositories.AboutUsSections;

public class AboutUsSectionsUpdaterRepository(ApplicationDbContext context) : IAboutUsSectionsUpdaterRepository
{
    public async Task<AboutUsSection> UpdateAboutUsSection(Guid id, AboutUsRequestDto aboutUsRequestDto)
    {
        var affectedRows = await context.AboutUsSections
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(c => c.SectionType, aboutUsRequestDto.SectionType)
                .SetProperty(c => c.Content, aboutUsRequestDto.Content)
                .SetProperty(c => c.Order, aboutUsRequestDto.Order));

        if (affectedRows < 1)
            throw new NotFoundException("Error al modificar la sección.");

        return new AboutUsSection()
        {
            Id = id,
            Order = aboutUsRequestDto.Order,
            Content = aboutUsRequestDto.Content,
            SectionType = aboutUsRequestDto.SectionType,
        };
    }
}