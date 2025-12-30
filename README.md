# Vertical Word Generator (Unity + SQLite)
[![DB Browser for SQLite](https://img.shields.io/badge/DB%20Browser%20for%20SQLite-Official-blue)](https://sqlitebrowser.org/)
[![SQLite-net for Unity](https://img.shields.io/badge/SQLite--net-Unity%20Integration-green)](https://github.com/gilzoide/unity-sqlite-net)

This is a **demo Unity project focused on offline data management and constraint-based word generation** using a local SQLite database.

The project explores how a word-game–style mechanic can be built **entirely offline**, with thousands of words stored locally and queried at runtime. This project is intentionally scoped as a **technical demo**, not a full game.


#### Key goals:
- Offline-first word data access
- SQLite querying from Unity
- Constraint-based word selection

---

## What the App Does

The app consists of:
- A **text input field**
- A **submit button**
- A list of generated words
- A description display panel

### User Flow
1. The user types a **5-letter word** and Clicks **Submit**
2. The app attempts to generate **5 horizontal words** such that:
   - Each word is 5 letters long
   - One shared character index (randomized) forms the input word **vertically**
3. If a valid configuration is found:
   - The words are displayed on screen
   - The matching vertical characters are highlighted
4. When the user selects a generated word:
   - Its **description** is displayed on screen

If no valid arrangement is possible, the app logs an error.

---

## Word Data Pipeline

### 1. Word Extraction (Python)
A **Python script** is used to:
- Extract **thousands of 5-letter words**
- Pull their **definitions/descriptions**
- Source data from **WordNet**
- Export the result as a **CSV file**

### 2. CSV → SQLite
The CSV file is converted into a SQLite database using:

**DB Browser for SQLite**

The final database contains:
- A single table: `words`
- Columns:
  - `word` (TEXT)
  - `description` (TEXT)

This `.db` file is included in the Unity project and accessed at runtime.

---

## Database & Persistence

- The app uses **SQLite-net** for database access inside Unity
- The database is:
  - Stored in `StreamingAssets`
  - Copied to `Application.persistentDataPath` at runtime
- All queries are performed **locally**
- No internet connection is required

---

Feel free to explore, modify, and build on top of it.
