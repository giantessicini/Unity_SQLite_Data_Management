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

        List<int> indices = new List<int> { 0, 1, 2, 3, 4 };

        // Fisher–Yates shuffle
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        foreach (int charIndex in indices)
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
                    string word = foundWords[i].ToUpper();
                    outputTexts[i].text = ColorCharAtIndex(word, charIndex, "00FF00");
                }

                Debug.Log($"Success using character index {charIndex}");
                return;
            }
        }

        Debug.LogError("No valid vertical arrangement found for input word.");
    }

    string ColorCharAtIndex(string word, int index, string colorHex)
    {
        if (index < 0 || index >= word.Length)
            return word;

        return word.Substring(0, index)
            + $"<color=#{colorHex}>{word[index]}</color>"
            + word.Substring(index + 1);
    }

    void OnDestroy()
    {
        db?.Close();
    }
}
