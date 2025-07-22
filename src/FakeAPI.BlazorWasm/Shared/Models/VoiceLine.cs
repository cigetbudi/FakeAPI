namespace FakeAPI.BlazorWasm.Shared.Models;
public class VoiceLine
{
    public int Id { get; set; }
    public string VoiceLineText { get; set; } = default!;
    public string HeroName { get; set; } = default!;
    public int Length { get; set; }
}