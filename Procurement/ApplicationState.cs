using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using POEApi.Model;
using System.Runtime.InteropServices;
using System;
using Procurement.ViewModel.Cache;

namespace Procurement
{
    public static class ApplicationState
    {
        /// <summary>
        /// Application version to check for latest release.
        /// Must begin with "Procurement ". See: VersionChecker.cs
        /// </summary>
        public static string Version = "Procurement 1.23.0";
        public static POEModel Model = new POEModel();
        public static Dictionary<string, Stash> Stash = new Dictionary<string, Stash>();
        public static Dictionary<string, Item> Inventory = new Dictionary<string, Item>();
        public static List<Character> Characters = new List<Character>();
        public static List<string> Leagues = new List<string>();
        public static System.Drawing.Text.PrivateFontCollection FontCollection = new System.Drawing.Text.PrivateFontCollection();
        private static Character currentCharacter = null;
        public static BitmapCache BitmapCache { get; private set; }
        public static string AccountName { get; set; }

        static ApplicationState()
        {
            BitmapCache = new BitmapCache(imageUrl => Model.GetImage(imageUrl));
        }

        public static Character CurrentCharacter
        {
            get { return currentCharacter; }
            set
            {
                currentCharacter = value;
                if (CharacterChanged != null)
                    CharacterChanged(Model, new PropertyChangedEventArgs("CurrentCharacter"));
            }
        }

        public static event PropertyChangedEventHandler LeagueChanged;
        public static event PropertyChangedEventHandler CharacterChanged;
        private static string currentLeague = string.Empty;

        public static string CurrentLeague
        {
            get { return currentLeague; }
            set
            {
                if (currentLeague == value)
                    return;

                currentLeague = value;
                Characters = Model.GetCharacters().Where(c => c.League == value).ToList();
                CurrentCharacter = Characters.FirstOrDefault();
                if (LeagueChanged != null)
                    LeagueChanged(Model, new PropertyChangedEventArgs("CurrentLeague"));
            }
        }

        private static Dictionary<string, List<string>> allCharactersByLeague = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> AllCharactersByLeague
        {
            get { return allCharactersByLeague; }
            set
            {
                allCharactersByLeague = value;
            }
        }

        public static void SetDefaults()
        {
            string favoriteLeague = Settings.UserSettings["FavoriteLeague"];
            if (!string.IsNullOrEmpty(favoriteLeague) && ApplicationState.Leagues.Contains(favoriteLeague))
                CurrentLeague = favoriteLeague;

            string defaultCharacter = Settings.UserSettings["FavoriteCharacter"];
            if (defaultCharacter != string.Empty && Characters.Count(c => c.Name == defaultCharacter) == 1)
            {
                CurrentCharacter = Characters.First(c => c.Name == defaultCharacter);
                return;
            }

            CurrentCharacter = Characters.FirstOrDefault();

            if (CurrentCharacter != null)
                CurrentLeague = CurrentCharacter.League;
        }

        public static void InitializeFont(byte[] fontBytes)
        {
            IntPtr data = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, data, fontBytes.Length);
            FontCollection.AddMemoryFont(data, fontBytes.Length);
            Marshal.FreeCoTaskMem(data);
        }
    }
}
