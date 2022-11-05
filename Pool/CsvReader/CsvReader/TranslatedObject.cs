namespace CsvReader;

public class TranslatedObject
{
    public TranslatedObject(string position, string cz, string en, string de)
    {
        Position = position;
        Cz = cz;
        En = en;
        De = de;
    }
    
    public string Position { get; set; }
    public string Cz { get; set; }
    public string En { get; set; }
    public string De { get; set; }
}