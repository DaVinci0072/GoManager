using Newtonsoft.Json;
using PokemonGoGUI.AccountScheduler;
using PokemonGoGUI.GoManager;
using PokemonGoGUI.ProxyManager;
using System.Collections.Generic;
using System.Drawing;

namespace PokemonGoGUI.Models
{
    public class ProgramExportModel
    {
        public List<Manager> Managers { get; set; }
        public ProxyHandler ProxyHandler { get; set; }
        public List<Scheduler> Schedulers { get; set; }
        public bool SPF { get; set; }
        public bool ShowWelcomeMessage { get; set; }
        public byte[] AccountHeaderInfo { get; set; }
        public byte[] ProxyHeaderInfo { get; set; }
        public byte[] SchedulerHeaderInfo { get; set; }


        //Don't like how this works
        [JsonIgnore]
        public Size? WindowSize { get; set; }

        [JsonIgnore]
        public Point? WindowLocation { get; set; }
    }
}
