

namespace EsportPlayerManager.Models;

public class Tournament
{
    public int TournamentId { get; set; }
    public string Name { get; set; }
    public int EntryFee { get; set; }
    public int Prize { get; set; }
    public int MinSkillRequired { get; set; }
    public int StressIncrease { get; set; }
}