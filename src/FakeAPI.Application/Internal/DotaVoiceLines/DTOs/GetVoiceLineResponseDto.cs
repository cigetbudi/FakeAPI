namespace FakeAPI.Application.Internal.DotaVoiceLines.DTOs;

public class GetVoiceLineResponseDto
{
    public int Id { get; set; }
    public string VoiceLine { get; set; } = string.Empty;
    public string HeroName { get; set; } = string.Empty;
    public int Length { get; set; }
}
