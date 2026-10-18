using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.AboutUs;

public class AboutUsResponseDto
{
    public Guid Id { get; set; }
    public string SectionType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public byte Order { get; set; }
}

public static partial class AboutUsExtensions
{
    public static Expression<Func<Models.AboutUsSection, AboutUsResponseDto>> MapAboutUsResponseDto()
    {
        return aboutUs => new AboutUsResponseDto
        {
            Id = aboutUs.Id,
            Order = aboutUs.Order,
            Content = aboutUs.Content,
            SectionType = aboutUs.SectionType,
        };
    }

    public static AboutUsResponseDto ToAboutUsResponseDto(this Models.AboutUsSection aboutUs)
    {
        return new AboutUsResponseDto()
        {
            Id = aboutUs.Id,
            Order = aboutUs.Order,
            Content = aboutUs.Content,
            SectionType = aboutUs.SectionType,
        };
    }
}