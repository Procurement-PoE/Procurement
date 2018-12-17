﻿using System.Collections.Generic;
using System.ComponentModel;
using POEApi.Model;
using Procurement.View;

namespace Procurement.ViewModel
{
    public class InventoryViewModel : ObservableBase
    {
        private InventoryView inventoryView;

        private string character;
        public string Character
        {
            get { return character; }
            set
            {
                if (value != character)
                {
                    character = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Character> characters;
        public List<Character> Characters
        {
            get { return characters; }
            set
            {
                characters = value;
                OnPropertyChanged();
            }
        }

        public List<string> Leagues
        {
            get { return ApplicationState.Leagues; }
        }

        public string CurrentLeague
        {
            get { return ApplicationState.CurrentLeague; }
        }

        public InventoryViewModel(InventoryView inventoryView)
        {
            this.Character = ApplicationState.CurrentCharacter.Name;
            this.Characters = ApplicationState.Characters;
            ApplicationState.LeagueChanged += new PropertyChangedEventHandler(ApplicationState_LeagueChanged);
            ApplicationState.CharacterChanged += new PropertyChangedEventHandler(ApplicationState_CharacterChanged);
            this.inventoryView = inventoryView;
        }

        void ApplicationState_CharacterChanged(object sender, PropertyChangedEventArgs e)
        {
            Character = ApplicationState.CurrentCharacter.Name;
        }

        void ApplicationState_LeagueChanged(object sender, PropertyChangedEventArgs e)
        {
            Characters = ApplicationState.Characters;
            Character = ApplicationState.CurrentCharacter.Name;
        }
    }
}
