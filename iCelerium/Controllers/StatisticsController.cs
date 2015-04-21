using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();

        public ActionResult TransDaily(string searchDate)
        {
            List<DataClass> data = new List<DataClass>();
            SMSServersEntities tp = new SMSServersEntities();
            string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran = tp.spTransDay(searchDate);

            foreach (var iTra in tran.ToList())
            {
                data.Add(new DataClass { ExecutionDate = iTra.Heure.Value, ExecutionValue = iTra.cTotal.Value });
            }
            object[,] chartData = new object[data.Count, 2];
            int i = 0;
            foreach (DataClass item in data)
            {
                chartData.SetValue(item.ExecutionDate, i, 0);
                chartData.SetValue(item.ExecutionValue, i, 1);
                i++;
            }

            Highcharts chart1 = new Highcharts("chart1")
                                                        .InitChart(new Chart { Type = ChartTypes.Spline })
                                                        .SetTitle(new Title { Text = "Transactions par heure" })
                                                        .SetXAxis(new XAxis { Type = AxisTypes.Category })
                                                        .SetSeries(new[]
                                                                  {
new Series { Data = new Data(chartData), Name = "Toutes Les Transactions" }
                                                                      //,
                                                                      //new Series { Data = new Data(chartData1), Name = "Toutes Les Collections" },
                                                                      //new Series { Data = new Data(chartData2), Name = "Tous Les Paiements" }
                                                                  });

            return this.PartialView(chart1);
        }
    }
}