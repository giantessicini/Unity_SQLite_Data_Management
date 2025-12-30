using System.Collections.Generic;
using UnityEngine;
using SQLite;
using TMPro;
using System.Collections;

public class VerticalWordGenerator : MonoBehaviour
{
    [SerializeField] private List<UIWord> outputTexts;
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
                StartCoroutine(TextRevealAnimation(foundWords, charIndex));

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

    [SerializeField] private LeanTweenType leanTweenType;
    [SerializeField] private float animationDelay = 0.5f;
    [SerializeField] private float animationWordDelay = 0.5f;
    [SerializeField] private float animationInitialScale = 0.8f;
    [SerializeField] private float animationWordDuration = 0.25f;

    IEnumerator TextRevealAnimation(List<string> foundWords, int charIndex)
    {
        if (outputTexts[0].wordText.text != "")
        {
            foreach (var text in outputTexts)
            {
                LeanTween.scale(text.gameObject, Vector3.one * animationInitialScale, animationWordDuration).setEase(leanTweenType).setOnComplete(() =>
                {
                    text.transform.localScale = Vector3.zero;
                });
                yield return new WaitForSeconds(animationWordDelay);
            }
        }
        else
        {
            foreach (var text in outputTexts)
            {
                text.transform.localScale = Vector3.zero;
            }
        }

        yield return new WaitForSeconds(animationDelay);

        for (int i = 0; i < 5; i++)
        {
            string word = foundWords[i].ToUpper();
            outputTexts[i].wordText.text = ColorCharAtIndex(word, charIndex, "00FF00");

            outputTexts[i].description = db.ExecuteScalar<string>(
            @"SELECT description FROM words WHERE UPPER(word) = ? LIMIT 1",
            word
        );
        }

        foreach (var text in outputTexts)
        {
            text.transform.localScale = Vector3.one * animationInitialScale;
            LeanTween.scale(text.gameObject, Vector3.one, animationWordDuration).setEase(leanTweenType);
            yield return new WaitForSeconds(animationWordDelay);
        }
    }

    void OnDestroy()
    {
        db?.Close();
    }
}
