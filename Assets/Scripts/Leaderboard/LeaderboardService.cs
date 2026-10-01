using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalShooter.Leaderboard
{
    [Serializable]
    public class ScoreEntry
    {
        public int score;
        public int defeated;
        public float secondsSurvived;
        public string difficulty;
        public string date;
    }

    /// <summary>Persists the latest sessions in PlayerPrefs.</summary>
    public static class LeaderboardService
    {
        public const int MaxEntries = 5;
        const string Key = "survival_shooter_leaderboard";

        [Serializable]
        class Store { public List<ScoreEntry> entries = new List<ScoreEntry>(); }

        /// <summary>Newest session first, at most five.</summary>
        public static IReadOnlyList<ScoreEntry> Load() => Read().entries;

        public static void Add(ScoreEntry entry)
        {
            var store = Read();
            store.entries.Insert(0, entry);
            if (store.entries.Count > MaxEntries)
                store.entries.RemoveRange(MaxEntries, store.entries.Count - MaxEntries);

            PlayerPrefs.SetString(Key, JsonUtility.ToJson(store));
            PlayerPrefs.Save();
        }

        static Store Read()
        {
            string json = PlayerPrefs.GetString(Key, "");
            if (string.IsNullOrEmpty(json)) return new Store();
            var store = JsonUtility.FromJson<Store>(json);
            return store ?? new Store();
        }
    }
}
