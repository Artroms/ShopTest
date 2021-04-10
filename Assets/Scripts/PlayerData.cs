using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class PlayerData
{
    public static HashSet<string> Items
    {
        get
        {
            return LoadData("Items", ref items);
        }
        set
        {
            SaveData(value, "Items", ref items);
        }
    }

    public static HashSet<string> Sample
    {
        get
        {
            return LoadData("Sample", ref sample);
        }
        set
        {
            SaveData(value, "Sample", ref sample);
        }
    }

    private static HashSet<string> items;
    private static HashSet<string> sample;

    private static HashSet<string> LoadData(string key, ref HashSet<string> savedSet)
    {
        if (savedSet == null)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var notSeperated = PlayerPrefs.GetString(key);
                string[] seperated = notSeperated.Split('@');
                savedSet = new HashSet<string>(seperated);
            }
            else
            {
                savedSet = new HashSet<string>();
            }
        }
        return new HashSet<string>(savedSet);
    }

    private static void SaveData(HashSet<string> data, string key, ref HashSet<string> savedSet)
    {
        var itemsList = new List<string>(data);
        savedSet = new HashSet<string>(data);
        var sb = new StringBuilder();
        for (int i = 0; i < itemsList.Count - 1; i++)
        {
            sb.Append(itemsList[i]);
            sb.Append('@');
        }
        if (itemsList.Count > 0)
            sb.Append(itemsList[itemsList.Count - 1]);
        PlayerPrefs.SetString(key, sb.ToString());
    }

    public static void SaveItem(string itemKey)
    {
        items.Add(itemKey);
        SaveData(items, "Items", ref items);
    }

    public static void ClearItems()
    {
        if (PlayerPrefs.HasKey("Items"))
            PlayerPrefs.DeleteKey("Items");
    }
}
