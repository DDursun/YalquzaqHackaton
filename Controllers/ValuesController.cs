using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ORRFINALL.Models;
using System.Collections.Generic;

namespace ORRWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        //private readonly SpotfireContext _dbContext;

        // ValuesController(SpotfireContext context)
       // {
       //     _dbContext = context;
       // }


        [HttpGet("{consumption}")]
        public barchartdata Getenergies(double consumption)
        {
            var default_price = 0.10; 

            using (var context = new SpotfireContext())
            {
                barchartdata barchartdata = new barchartdata();

                barchartdata.monthlycapex = consumption * default_price;
                barchartdata.savedco2 = consumption * 0.14 * 0.055; //CO2 in kg
                barchartdata.opex_hybrid = 1.3;
                barchartdata.opex_wind = 2.5;
                barchartdata.opex_solar = 6.15;

                
                return barchartdata;
            }
        }


        [HttpGet("closest/{longt}/{lat}/{consumption}/{typecoefficient}")]
        public linechart Getclosestenergies(double longt, double lat, double consumption, double typecoefficient)
        {

            var area_of_panel = 2.0;
            var priceperwatt = 2; //change it
            List<double> solararray = new();
            linechart chartdata = new();

            using (var context = new SpotfireContext())
            {
                var data = context.EnergyData.ToList();

                var secondflag = 100;
                var distanceflag = 2000.0;
                for (int i = 0; i < data.Count; i++)
                    {

                    var distance = Math.Pow((data[i].Long ?? 0) - longt, 2) + Math.Pow((data[i].Lat ?? 0) - lat, 2);
    
                    if (distance < distanceflag)
                    {
                        distanceflag = distance;
                        secondflag = data[i].Id;
                    };
                }


                // SOLAR CALCULATION

                var result = data.Where(x => x.Id == secondflag);
                var energyperpannel = (result.Select(x => x.Solar).First()??0 * area_of_panel * 0.2);

                var requiredwatt = consumption / 720;  //hours 

                var numberofpannels = Math.Ceiling(requiredwatt / energyperpannel);
                var solarcapex = numberofpannels * 400 * 1.5; 

                //var solarcapex =  * (numberofpanels ?? 0) * priceperwatt; // Problematic
               // var windcapex = 
               // var hybridcapex = 

                for (int i = 0; i<200; i++)
                {
                    var opex_solar = 6.15;

                    var number = solarcapex + i * opex_solar;

                    solararray.Add(number);
                }


                chartdata.solarry.AddRange(solararray);

                // WIND Calculation
                var surfaceareawind = 28.28;
                var capexofturbine = 2000.00;

                var resultwind = data.Where(x => x.Id == secondflag);

                var monthlyexpenditure = surfaceareawind * resultwind.Select(x => x.Wind).First() * 30;

                for (int i = 0; i < 200; i++)
                {
                    var opex_wind = 2.5;

                    var number = capexofturbine + i * opex_wind;

                    chartdata.windarray.Add(number);
                }


                //Hybrid 
                //typecoeeffiecient
                var capexhybrid = 3500;

                var monthlyexpenditurehybrid = consumption * typecoefficient;

                    for (int i = 0; i < 200; i++)
                     {
                     var opex_hybrid = 1.3;

                     var number = capexhybrid + i * opex_hybrid;

                    chartdata.hybridarray.Add(number);
                    }

                //Calculation of monthly expenditure with electicity

                var costofelectricity = 0.10;
                Random rnd = new Random();
                List<double> expenselist = new();

                for (int i = 0; i < 200; i++)
                {
                    var value = consumption * costofelectricity;
                    value += i;

                    expenselist.Add(value);
                }

                chartdata.monthlyexpend.AddRange(Cumulative(expenselist));

                //Console.WriteLine(chartdata.solarry.Count);



                var totalsavingsolar = Math.Abs(chartdata.monthlyexpend.Last() - chartdata.solarry.Last()) - Math.Abs(chartdata.monthlyexpend.First() - chartdata.solarry.First());
                var totalsavingwind = Math.Abs(chartdata.monthlyexpend.Last() - chartdata.windarray.Last()) - Math.Abs(chartdata.monthlyexpend.First() - chartdata.windarray.First());
                var totalsavinghybrid = Math.Abs(chartdata.monthlyexpend.Last() - chartdata.hybridarray.Last()) - Math.Abs(chartdata.monthlyexpend.First() - chartdata.hybridarray.First());

                var bestchoice = "";

                if ((totalsavingwind > totalsavingsolar) && (totalsavingwind > totalsavinghybrid))
                {
                    bestchoice = "Wind";
                }
                else if ((totalsavinghybrid > totalsavingsolar) && (totalsavinghybrid > totalsavingwind))
                {
                    bestchoice = "Hybrid";

                }
                else
                {
                    bestchoice = "Solar";

                };

                chartdata.Bestchoice = bestchoice;

                return chartdata;
            }
            
            
        }
        private List<double> Cumulative(List<double> values)
            {
                List<double> list = new List<double>();
                for (int i = 0; i < values.Count; i++)
                {
                    if (i > 0)
                    {
                        double tmp = 0;
                        for (int j = 0; j <= i; j++)
                        {
                            tmp += values[j];
                        }
                        list.Add(Math.Round(tmp, 6, MidpointRounding.AwayFromZero));
                    }
                    else
                        list.Add(Math.Round(values[i], 6, MidpointRounding.AwayFromZero));
                }
                return list;
            
        }
    }


}

