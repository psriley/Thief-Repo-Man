using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace AutoAvenger
{
    public class Player
    {
        public int Score;
        public List<Item> Items = new List<Item>();
        public PlayerData Data;
        
        private const string PATH = "data.json";

        public Player() 
        {
            Data = Load();
        }

        public void Save(PlayerData data)
        {
            string serializedText = JsonSerializer.Serialize<PlayerData>(data);
            File.WriteAllText(PATH, serializedText);
            Debug.WriteLine(serializedText);
        }

        public PlayerData Load()
        {
            try
            {
                var data = File.ReadAllText(PATH);
                return JsonSerializer.Deserialize<PlayerData>(data);
            }
            // 'data.json' file does not exist, so we should create it, and set it to empty.
            catch (Exception e)
            {
                Debug.WriteLine(e);
                PlayerData data = new PlayerData();
                // Create 'data.json' PlayerData file to save and load from if it does not already exist.
                string serializedText = JsonSerializer.Serialize<PlayerData>(data);
                File.WriteAllText(PATH, serializedText);
                return data; // MAYBE RETURN A FRIENDLY MESSAGE EXPLAINING WHY LOAD DIDN'T WORK?
            }
        }
    }
}
