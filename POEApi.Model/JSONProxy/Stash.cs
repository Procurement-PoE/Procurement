using System.Collections.Generic;
using Newtonsoft.Json;

namespace POEApi.Model.JSONProxy
{
    public class Property
    {
        public string Name { get; set; }
        public List<object> Values { get; set; }
        public int DisplayMode { get; set; }
    }

    public class AdditionalProperty
    {
        public string Name { get; set; }
        public List<List<object>> Values { get; set; }
        public int DisplayMode { get; set; }
        public double Progress { get; set; }
    }


    public class Requirement
    {
        public string Name { get; set; }
        public List<object> Values { get; set; }
        public int DisplayMode { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public bool Verified { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public string Icon { get; set; }
        public bool Support { get; set; }
        public string League { get; set; }
        public string Name { get; set; }
        public string TypeLine { get; set; }
        public bool Identified { get; set; }
        public List<Property> Properties { get; set; }
        public List<string> ExplicitMods { get; set; }
        public string DescrText { get; set; }
        public int FrameType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string InventoryId { get; set; }
        public List<Item> SocketedItems { get; set; }
        public List<Socket> Sockets { get; set; }
        public List<AdditionalProperty> AdditionalProperties { get; set; }
        public string SecDescrText { get; set; }
        public List<string> ImplicitMods { get; set; }
        public List<string> FlavourText { get; set; }
        public List<Requirement> Requirements { get; set; }
        public List<Requirement> NextLevelRequirements { get; set; }
        public int Socket { get; set; }
        public string Colour { get; set; }
        public bool Corrupted { get; set; }
        public bool AbyssJewel { get; set; }

        public List<string> CosmeticMods { get; set; }
        public List<string> CraftedMods { get; set; }
        public List<string> EnchantMods { get; set; }
        public int Ilvl { get; set; }
        public string ProphecyText { get; set; }
        public string ProphecyDiffText { get; set; }
        public bool IsRelic { get; set; }
    }

    public class Socket
    {
        public string Attr { get; set; }
        public int Group { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Stash
    {
        public int NumTabs { get; set; }
        public List<Item> Items { get; set; }
        public List<Tab> Tabs { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Inventory
    {
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
        public string type { get; set; }
        public bool selected { get; set; }
    }
}