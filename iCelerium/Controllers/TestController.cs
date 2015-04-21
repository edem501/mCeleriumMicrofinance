using System.Collections.Generic;
using System.Drawing;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Linq;
using System.Web.Mvc;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    public class TestController : Controller
    {

        //public readonly SMSServersEntities db = new SMSServersEntities();
        ////
        //// GET: /Test/
        //public ActionResult Index()
        //{
        //    DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
        //.SetXAxis(new XAxis
        //{
        //    Categories = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
        //})
        //.SetSeries(new[] { 
        //    new Series{Name="Tidjani Said",Data = new Data(new object[] { 29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 })},
        //     new Series{Name="Edem Fenou",Data = new Data(new object[] { 49.9, 21.5, 136.4, 129.2,74.0, 196.0, 115.6, 188.5, 6.4, 124.1, 195.6, 154.4 })}
        //}
        //)
        //.SetTitle(new Title {Text="Nombre total de collect par agent par mois" });

        //    return View(chart);
        //}

        //public ActionResult _BasicBar()
        //{
        //    Highcharts chart = new Highcharts("chart")
        //        .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
        //        .SetTitle(new Title { Text = "Historic World Population by Region" })
        //        .SetSubtitle(new Subtitle { Text = "Source: Wikipedia.org" })
        //        .SetXAxis(new XAxis
        //        {
        //            Categories = new[] { "Africa", "America", "Asia", "Europe", "Oceania" },
        //            Title = new XAxisTitle { Text = string.Empty }
        //        })
        //        .SetYAxis(new YAxis
        //        {
        //            Min = 0,
        //            Title = new YAxisTitle
        //            {
        //                Text = "Population (millions)",
        //                Align = AxisTitleAligns.High
        //            }
        //        })
        //        .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y +' millions'; }" })
        //        .SetPlotOptions(new PlotOptions
        //        {
        //            Bar = new PlotOptionsBar
        //            {
        //                DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
        //            }
        //        })
        //        .SetLegend(new Legend
        //        {
        //            Layout = Layouts.Vertical,
        //            Align = HorizontalAligns.Right,
        //            VerticalAlign = VerticalAligns.Top,
        //            X = -100,
        //            Y = 100,
        //            Floating = true,
        //            BorderWidth = 1,
        //            BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
        //            Shadow = true
        //        })
        //        .SetCredits(new Credits { Enabled = false })
        //        .SetSeries(new[]
        //        {
        //            new Series { Name = "Year 1800", Data = new Data(new object[] { 107, 31, 635, 203, 2 }) },
        //            new Series { Name = "Year 1900", Data = new Data(new object[] { 133, 156, 947, 408, 6 }) },
        //            new Series { Name = "Year 2008", Data = new Data(new object[] { 973, 914, 4054, 732, 34 }) }
        //        });

        //    return View(chart);
        //}


        //public ActionResult BasicBar()
        //{
        //    List<DataClass> data = new List<DataClass>();
            

        //    var tran = from t in db.TTransactions
        //               where t.DateOperation.Year == 2014
        //               group t by t.DateOperation.Month into g
        //               select new { Month = g, Total = g.Count() };

        //    foreach (var iTra in tran.ToList())
        //    {
        //        data.Add(new DataClass { ExecutionDate = Months(iTra.Month.FirstOrDefault().DateOperation.Month), ExecutionValue = iTra.Total });
        //    }
        //    object[,] chartData = new object[data.Count, 2];
        //    int i = 0;
        //    foreach (DataClass item in data)
        //    {
        //        chartData.SetValue(item.ExecutionDate, i, 0);
        //        chartData.SetValue(item.ExecutionValue, i, 1);
        //        i++;
        //    }
            
        //    List<DataClass> data1 = new List<DataClass>();
        //    var tran1 = from t in db.TTransactions
        //               where t.Debit==0
        //               group t by t.DateOperation.Month into g
        //               select new { Month = g, Total = g.Count() };

        //    foreach (var iTra in tran1.ToList())
        //    {
        //        data1.Add(new DataClass { ExecutionDate = Months(iTra.Month.FirstOrDefault().DateOperation.Month), ExecutionValue = iTra.Total });
        //    }
        //    object[,] chartData1 = new object[data1.Count, 2];
        //    int j = 0;
        //    foreach (DataClass item in data1)
        //    {
        //        chartData1.SetValue(item.ExecutionDate, j, 0);
        //        chartData1.SetValue(item.ExecutionValue, j, 1);
        //        j++;
        //    }

        //    List<DataClass> data2 = new List<DataClass>();
        //    var tran2 = from t in db.TTransactions
        //                where t.Credit == 0
        //                group t by t.DateOperation.Month into g
        //                select new { Month = g, Total = g.Count() };

        //    foreach (var iTra in tran2.ToList())
        //    {
        //        data2.Add(new DataClass { ExecutionDate = Months(iTra.Month.FirstOrDefault().DateOperation.Month), ExecutionValue = iTra.Total });
        //    }
        //    object[,] chartData2 = new object[data1.Count, 2];
        //    int k = 0;
        //    foreach (DataClass item in data2)
        //    {
        //        chartData2.SetValue(item.ExecutionDate, k, 0);
        //        chartData2.SetValue(item.ExecutionValue, k, 1);
        //        k++;
        //    }
            

        //    Highcharts chart1 = new Highcharts("chart1")
        //        .InitChart(new Chart { Type = ChartTypes.Spline })
        //        .SetTitle(new Title { Text = "Transactions par mois" })
        //        .SetXAxis(new XAxis { Type = AxisTypes.Category })
        //        .SetSeries(new[]{
        //            new Series { Data = new Data(chartData), Name = "Toutes Les Transactions" },
        //            new Series { Data = new Data(chartData1), Name = "Toutes Les Collections" },
        //            new Series { Data = new Data(chartData2), Name = "Tous Les Paiements" }
        //        });

        //    return View(chart1);
        //}


        //public ActionResult BasicBar1()
        //{
        //    List<DataClass2> data = new List<DataClass2>();
        //    TempEntities tp = new TempEntities();
        //    string tDate = DateTime.Today.ToString("MM/dd/yyyy");

        //    var tran = tp.spTransDay(tDate);

        //    foreach (var iTra in tran.ToList())
        //    {
        //        data.Add(new DataClass2 { ExecutionDate = iTra.Heure.Value, ExecutionValue = iTra.cTotal.Value });
        //    }
        //    object[,] chartData = new object[data.Count, 2];
        //    int i = 0;
        //    foreach (DataClass2 item in data)
        //    {
        //        chartData.SetValue(item.ExecutionDate, i, 0);
        //        chartData.SetValue(item.ExecutionValue, i, 1);
        //        i++;
        //    }

        //    //List<DataClass> data1 = new List<DataClass>();
        //    //var tran1 = from t in db.TTransactions
        //    //            where t.Debit == 0
        //    //            group t by t.DateOperation.Month into g
        //    //            select new { Month = g, Total = g.Count() };

        //    //foreach (var iTra in tran1.ToList())
        //    //{
        //    //    data1.Add(new DataClass { ExecutionDate = Months(iTra.Month.FirstOrDefault().DateOperation.Month), ExecutionValue = iTra.Total });
        //    //}
        //    //object[,] chartData1 = new object[data1.Count, 2];
        //    //int j = 0;
        //    //foreach (DataClass item in data1)
        //    //{
        //    //    chartData1.SetValue(item.ExecutionDate, j, 0);
        //    //    chartData1.SetValue(item.ExecutionValue, j, 1);
        //    //    j++;
        //    //}

        //    //List<DataClass> data2 = new List<DataClass>();
        //    //var tran2 = from t in db.TTransactions
        //    //            where t.Credit == 0
        //    //            group t by t.DateOperation.Month into g
        //    //            select new { Month = g, Total = g.Count() };

        //    //foreach (var iTra in tran2.ToList())
        //    //{
        //    //    data2.Add(new DataClass { ExecutionDate = Months(iTra.Month.FirstOrDefault().DateOperation.Month), ExecutionValue = iTra.Total });
        //    //}
        //    //object[,] chartData2 = new object[data1.Count, 2];
        //    //int k = 0;
        //    //foreach (DataClass item in data2)
        //    //{
        //    //    chartData2.SetValue(item.ExecutionDate, k, 0);
        //    //    chartData2.SetValue(item.ExecutionValue, k, 1);
        //    //    k++;
        //    //}


        //    Highcharts chart1 = new Highcharts("chart1")
        //        .InitChart(new Chart { Type = ChartTypes.Spline })
        //        .SetTitle(new Title { Text = "Transactions par heure" })
        //        .SetXAxis(new XAxis { Type = AxisTypes.Category })
        //        .SetSeries(new[]{
        //            new Series { Data = new Data(chartData), Name = "Toutes Les Transactions" }
        //            //,
        //            //new Series { Data = new Data(chartData1), Name = "Toutes Les Collections" },
        //            //new Series { Data = new Data(chartData2), Name = "Tous Les Paiements" }
        //        });

        //    return View(chart1);
        //}

        //public ActionResult PieChartWithEvents()
        //{
            

        //    Highcharts chart = new Highcharts("chart")
        //        .InitChart(new Chart())
        //        .SetPlotOptions(new PlotOptions
        //        {
        //            Pie = new PlotOptionsPie
        //            {
        //                AllowPointSelect = true,
        //                Cursor = Cursors.Pointer,
        //                ShowInLegend = true,
        //                Events = new PlotOptionsPieEvents { Click = "function(event) { alert('The slice was clicked!'); }" },
        //                Point = new PlotOptionsPiePoint { Events = new PlotOptionsPiePointEvents { LegendItemClick = "function(event) { if (!confirm('Do you want to toggle the visibility of this slice?')) { return false; } }" } }
        //            }
        //        })
        //        .SetSeries(new Series
        //        {
        //            Type = ChartTypes.Pie,
        //            Name = "Browser share",

        //            Data = new Data(new object[]
        //                            {
        //                                new object[] { "Collects", 78 },
        //                                new object[] { "Paiements", 22 }
        //                            })
        //        });

        //    return View(chart);
        //}


        //private string Months(int code)
        //{
        //    switch(code){
        //         case 1:
        //            return "Jan";
                       
        //            case 2:
        //            return "Feb";
                       
        //            case 3:
        //            return "Mar";
                      
        //            case 4:
        //            return "Apr";
                      
        //            case 5:
        //            return "May";
                        
        //            case 6:
        //            return "Jun";
                       
        //            case 7:
        //            return "Jul";
                       
        //            case 8:
        //            return "Aug";
                       
        //            case 9:
        //            return "Sep";
                        
        //            case 10:
        //            return "Oct";
                       
        //            case 11:
        //            return "Nov";
                        
        //            case 12:
        //            return "Dec";
                       
        //    }
        //    return "";
        //}
	}
}