namespace FakeAPI.Domain.Entities.Common;

public class DotaVoiceline
{
    public int Id { get; set; }

    public string VoiceLine { get; set; } = string.Empty;

    public string HeroName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
