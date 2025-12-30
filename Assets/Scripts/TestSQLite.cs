using UnityEngine;
using SQLite;

public class TestSQLite : MonoBehaviour
{
    void Start()
    {
        string dbPath = DatabaseLoader.GetDatabasePath("WordDatabase.db");

        using (var db = new SQLiteConnection(dbPath))
        {
            var firstWord = db.Table<WordEntry>().FirstOrDefault();

            if (firstWord != null)
            {
                Debug.Log($"WORD: {firstWord.word}");
                Debug.Log($"DESC: {firstWord.description}");
            }
            else
            {
                Debug.Log("No words found.");
            }
        }
    }
}
