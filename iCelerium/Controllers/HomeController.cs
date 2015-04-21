using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
    [Audit]
    public class HomeController : Controller
    {
        public ActionResult calendar()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            var vm = new DashboardViewModel
            {
                Chart1 = this.TransDaily(),
                Chart2 = this.PieData(),
                Chart3 = this.TransDaily()
            };
            return this.View(vm);
        }

        public ActionResult Reports(string reportModel)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "------------", Value = "" });
            items.Add(new SelectListItem { Text = "Transactions", Value = "1" });
            this.ViewBag.reportModel = items;
            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }

        public Highcharts TransDaily()
        {
            List<DataClass> data = new List<DataClass>();
            SMSServersEntities tp = new SMSServersEntities();
            string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran = tp.spTransDayColPay(tDate, 1);

            foreach (var iTra in tran.ToList())
            {
                data.Add(new DataClass { ExecutionDate = iTra.Heure.Value, ExecutionValue = Math.Round(iTra.cTotal.Value, 2) });
            }
            object[,] chartData = new object[data.Count, 2];
            int i = 0;
            foreach (DataClass item in data)
            {
                chartData.SetValue(item.ExecutionDate, i, 0);
                chartData.SetValue(item.ExecutionValue, i, 1);
                i++;
            }

            List<DataClass> data1 = new List<DataClass>();

            //string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran1 = tp.spTransDayColPay(tDate, 0);

            foreach (var iTra in tran1.ToList())
            {
                data1.Add(new DataClass { ExecutionDate = iTra.Heure.Value, ExecutionValue = Math.Round(iTra.cTotal.Value, 2) });
            }
            object[,] chartData1 = new object[data1.Count, 2];
            int j = 0;
            foreach (DataClass item in data1)
            {
                chartData1.SetValue(item.ExecutionDate, j, 0);
                chartData1.SetValue(item.ExecutionValue, j, 1);
                j++;
            }

            Highcharts chart1 = new Highcharts("chart1")
                                                        .InitChart(new Chart { Type = ChartTypes.Spline })
                                                        .SetTitle(new Title { Text = String.Format("Transactions de la journee {0}",tDate) })
                                                        .SetXAxis(new XAxis { Type = AxisTypes.Category, Title = new XAxisTitle { Text = "Heure(s)" } })
                                                        .SetSeries(new[]
                      {
                          new Series { Data = new Data(chartData), Name = "Toutes Les Collectes" },
                          new Series { Data = new Data(chartData1), Name = "Tous Les Paiements" }
                      });

            return chart1;
        }

        public Highcharts PieData()
        {
            List<ZoneData> data = new List<ZoneData>();
            SMSServersEntities tp = new SMSServersEntities();
            string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran = tp.spTransPie(tDate);

            foreach (var iTra in tran.ToList())
            {
                data.Add(new ZoneData { ZoneName = iTra.ZoneName, Total = (decimal)Math.Round(iTra.Total.Value, 2) });
            }
            object[,] chartData = new object[data.Count, 2];
            int i = 0;
            foreach (ZoneData item in data)
            {
                chartData.SetValue(item.ZoneName, i, 0);
                chartData.SetValue(item.Total, i, 1);
                i++;
            }

            Data dt = new Data(chartData);

            Highcharts chart = new Highcharts("chart")
                                                      .InitChart(new Chart { PlotShadow = false })
                                                      .SetTitle(new Title { Text = String.Format("Part des collectes par zone {0}",tDate) })
                                                      .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage.toFixed(2) +' %'; }" })
                                                      .SetPlotOptions(new PlotOptions
                                                      {
                                                          Pie = new PlotOptionsPie { AllowPointSelect = true, Cursor = Cursors.Pointer, DataLabels = new PlotOptionsPieDataLabels { Color = ColorTranslator.FromHtml("#000000"), ConnectorColor = ColorTranslator.FromHtml("#000000"), Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ Math.round(this.percentage) +' %'; }" } },
                                                      })
                                                      .SetSeries(new Series
                                                      {
                                                          Type = ChartTypes.Pie,
                                                          Name = "Browser share",
                                                          Data = dt
                                                      });
            return chart;
        }

        public ActionResult PieChart()
        {
            Highcharts chart = this.PieData();

            return this.View(chart);
        }

        public ActionResult TransChart()
        {
            Highcharts chart = this.TransDaily();

            return this.View(chart);
        }
    }
}