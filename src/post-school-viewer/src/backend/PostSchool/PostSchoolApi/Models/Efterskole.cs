namespace PostSchoolApi.Models;

public class Efterskole
{
    public int Id { get; set; }
    public required string Navn { get; set; }
    public required string KortBeskrivelse { get; set; }
    public required string LangBeskrivelse { get; set; }
    public required Koordinates GpsKoordinater { get; set; }
    public required string Adresse { get; set; }
    public List<string> Linjefag { get; set; } = [];
    public required List<string> Valgfag { get; set; }
    public required List<string> ObligatoriskeFag { get; set; }
    public string Type { get; set; } = "Ukendt";
}