using System.Collections.Generic;
using System.Runtime.Serialization;

namespace POEApi.Model.JSONProxy
{
    [DataContract]
    public class Property
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "values")]
        public List<object> Values { get; set; }

        [DataMember(Name = "displayMode")]
        public int DisplayMode { get; set; }
    }

    [DataContract]
    public class AdditionalProperty
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "values")]
        public List<List<object>> values { get; set; }

        [DataMember(Name = "displayMode")]
        public int displayMode { get; set; }

        [DataMember(Name = "progress")]
        public double progress { get; set; }
    }


    [DataContract]
    public class Requirement
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "values")]
        public List<object> Value { get; set; }

        [DataMember(Name = "displayMode")]
        public int DisplayMode { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "verified")]
        public bool Verified { get; set; }

        [DataMember(Name = "w")]
        public int W { get; set; }

        [DataMember(Name = "h")]
        public int H { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "support")]
        public bool Support { get; set; }

        [DataMember(Name = "league")]
        public string League { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "typeLine")]
        public string TypeLine { get; set; }

        [DataMember(Name = "identified")]
        public bool Identified { get; set; }

        [DataMember(Name = "properties")]
        public List<Property> Properties { get; set; }

        [DataMember(Name = "explicitMods")]
        public List<string> ExplicitMods { get; set; }

        [DataMember(Name = "descrText")]
        public string DescrText { get; set; }

        [DataMember(Name = "frameType")]
        public int frameType { get; set; }

        [DataMember(Name = "x")]
        public int X { get; set; }

        [DataMember(Name = "y")]
        public int Y { get; set; }

        [DataMember(Name = "inventoryId")]
        public string InventoryId { get; set; }

        [DataMember(Name = "socketedItems")]
        public List<Item> SocketedItems { get; set; }

        [DataMember(Name = "sockets")]
        public List<Socket> Sockets { get; set; }

        [DataMember(Name = "additionalProperties")]
        public List<AdditionalProperty> additionalProperties { get; set; }

        [DataMember(Name = "secDescrText")]
        public string SecDescrText { get; set; }

        [DataMember(Name = "implicitMods")]
        public List<string> ImplicitMods { get; set; }

        [DataMember(Name = "flavourText")]
        public List<string> FlavourText { get; set; }

        [DataMember(Name = "requirements")]
        public List<Requirement> Requirements { get; set; }

        [DataMember(Name = "nextLevelRequirements")]
        public List<Requirement> nextLevelRequirements { get; set; }

        [DataMember(Name = "socket")]
        public int Socket { get; set; }

        [DataMember(Name = "colour")]
        public string Color { get; set; }

        [DataMember(Name = "corrupted")]
        public bool Corrupted { get; set; }

        [DataMember(Name = "cosmeticMods")]
        public List<string> CosmeticMods { get; set; }

        [DataMember(Name = "craftedMods")]
        public List<string> CraftedMods { get; set; }
    }

    [DataContract]
    public class Socket
    {
        [DataMember(Name = "attr")]
        public string Attribute { get; set; }

        [DataMember(Name = "group")]
        public int Group { get; set; }
    }

    [DataContract(Name = "RootObject")]
    public class Stash
    {
        [DataMember(Name = "numTabs")]
        public int NumTabs { get; set; }

        [DataMember(Name = "items")]
        public List<Item> Items { get; set; }

        [DataMember(Name = "tabs")]
        public List<Tab> Tabs { get; set; }
    }

    [DataContract(Name = "RootObject")]
    public class Character
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "league")]
        public string League { get; set; }

        [DataMember(Name = "class")]
        public string Class { get; set; }

        [DataMember(Name = "classId")]
        public int ClassId { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }
    }

    [DataContract(Name = "RootObject")]
    public class Inventory
    {
        [DataMember(Name = "items")]
        public List<Item> Items { get; set; }
    }

    public class Colour
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
    }

    public class Tab
    {
        public string n { get; set; }
        public int i { get; set; }
        public Colour colour { get; set; }
        public string srcL { get; set; }
        public string srcC { get; set; }
        public string srcR { get; set; }
        public bool hidden { get; set; }
    }
}


