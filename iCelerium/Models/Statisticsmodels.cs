using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace iCelerium.Models
{

    public class ZoneData
    {

        public String ZoneName { get; set; }
        public Decimal? Total { get; set; }
    }
    public class DataClass3
    {
        public int ExecutionDate { get; set; }
        public Decimal? ExecutionValue { get; set; }
    }
    public class DataClass2
    {
        public int ExecutionDate { get; set; }
        public int ExecutionValue { get; set; }
    }
    public class DataClass1
    {
        public DateTime ExecutionDate { get; set; }
        public double ExecutionValue { get; set; }
    }
    public class DataClass
    {
        public int ExecutionDate { get; set; }
        public Double ExecutionValue { get; set; }
    }
    public class DashboardViewModel
    {

        public Highcharts Chart1 { get; set; }
        public Highcharts Chart2 { get; set; }
        public Highcharts Chart3 { get; set; }
    }

    public class Widget
    {
        public string mTotal { get; set; }
        public string mPerc { get; set; }
        public string mNewClient { get; set; }
        public string mUsers { get; set; }
    }

    public class PostingViewModel
    {
        public string AgentId { get; set; }
        [Display(Name = "Nom Collecteur")]
        public string AgentName { get; set; }
        [Display(Name = "Nombre de transaction")]
        public int? NTrans { get; set; }
        [Display(Name = "Montant des paiements")]
        [DataType(DataType.Currency)]
        public double? SPaiement { get; set; }
        [Display(Name = "Montant des collectes")]
        [DataType(DataType.Currency)]
        public double? SCollet { get; set; }

        [Display(Name = "Montant Nouveau membre")]
        [DataType(DataType.Currency)]
        public double? SNouveauClient { get; set; }

        [Display(Name = "Montant Payable")]
        [DataType(DataType.Currency)]
        public decimal? Payable { get; set; }
    }

    public class PostingDetails
    {
        public PostingViewModel Live { get; set; }
        public PostingViewModel Upload { get; set; }
        public string agentName { get; set; }
    }
}