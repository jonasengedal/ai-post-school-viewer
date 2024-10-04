namespace PostSchoolApi.Models;

public class Efterskole
{
    public int Id { get; set; }
    public required string Navn { get; set; }
    public string? KortBeskrivelse { get; set; }
    public string? LangBeskrivelse { get; set; }
    public Koordinates? GpsKoordinater { get; set; }
    public string? Adresse { get; set; }
    public List<string>? Linjefag { get; set; } = [];
    public List<string> Valgfag { get; set; } = [];
    public List<string> ObligatoriskeFag { get; set; } = [];
    public string Type { get; set; } = "Ukendt";
}