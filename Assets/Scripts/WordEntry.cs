using SQLite;

[Table("words")]
public class WordEntry
{
    public string word { get; set; }
    public string description { get; set; }
}