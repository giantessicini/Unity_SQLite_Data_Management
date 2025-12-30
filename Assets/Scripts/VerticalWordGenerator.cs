using System.Collections.Generic;
using UnityEngine;
using SQLite;
using TMPro;

public class VerticalWordGenerator : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> outputTexts;
    [SerializeField] private TMP_InputField inputField;
    SQLiteConnection db;

    void Awake()
    {
        string dbPath = DatabaseLoader.GetDatabasePath("WordDatabase.db");
        db = new SQLiteConnection(dbPath);
    }

    public void Generate()
    {
        string inputWord = inputField.text.Trim().ToUpper();

        if (inputWord.Length != 5)
        {
            Debug.LogError("Input word must be exactly 5 letters.");
            return;
        }

        // Try each character index (0–4)
        for (int charIndex = 0; charIndex < 5; charIndex++)
        {
            List<string> foundWords = new List<string>();
            bool failed = false;

            for (int row = 0; row < 5; row++)
            {
                char requiredChar = inputWord[row];

                var word = db.ExecuteScalar<string>(
                    @"SELECT word FROM words
                    WHERE LENGTH(word) = 5
                    AND UPPER(SUBSTR(word, ?, 1)) = ?
                    ORDER BY RANDOM()
                    LIMIT 1",
                    charIndex + 1,
                    requiredChar.ToString()
                );

                if (word == null)
                {
                    failed = true;
                    break;
                }

                foundWords.Add(word);
            }

            // Success for this index
            if (!failed)
            {
                for (int i = 0; i < 5; i++)
                {
                    outputTexts[i].text = foundWords[i].ToUpper();
                }

                Debug.Log($"Success using character index {charIndex}");
                return;
            }
        }

        Debug.LogError("No valid vertical arrangement found for input word.");
    }

    void OnDestroy()
    {
        db?.Close();
    }
}
