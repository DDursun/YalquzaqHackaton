using System;
using System.Collections.Generic;

namespace ORRFINALL.Models
{
    public partial class EnergyDatum
    {
        public int? Xx { get; set; }
        public int? Yy { get; set; }
        public double? Long { get; set; }
        public double? Lat { get; set; }
        public double? Solar { get; set; }
        public double? Wind { get; set; }
        public int Id { get; set; }
    }

    public class Energydistances
    {
        public int Id { get; set; }
        public double? Distance { get; set; }

    }
    public class Energydistancelist
    {
        public List<Energydistances> energydistances { get; set; } = new();

    }

    public class Energyfullresponse
    { 
            public double? conventional { get; set; }
    }

    public class barchartdata
    {
        public double? monthlycapex{ get; set; }
        public double? opex_wind { get; set; }
        public double? opex_solar { get; set; }
        public double? opex_hybrid { get; set; }
        public double? savedco2 { get; set; }
    }

    public class linechart
    {
        public string Bestchoice { get; set; } 
        public List<double> solarry { get; set; } = new();
        public List<double> windarray { get; set; } = new();
        public List<double> hybridarray { get; set; } = new();
        public List<double> monthlyexpend { get; set; } = new();

    }

}
