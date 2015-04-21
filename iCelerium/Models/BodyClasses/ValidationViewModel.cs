using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iCelerium.Models.BodyClasses
{
    public class ValidationViewModel
    {
        [Display(Name = "Timestamp Transaction")]
        public System.DateTime PostingStamp { get; set; }
        [Display(Name = "Pointeur")]
        public string FullName { get; set; }
        [Display(Name = "Collecteur")]
        public string AgentName { get; set; }
        [Display(Name = "Transactions")]
        public int Numtrans { get; set; }
        [Display(Name = "Paiements")]
        [DataType(DataType.Currency)]
        public decimal SPaiments { get; set; }
        [Display(Name = "Collectes")]
        [DataType(DataType.Currency)]
        public decimal SCollectes { get; set; }
        [Display(Name = "Versement")]
        [DataType(DataType.Currency)]
        public decimal SPayable { get; set; }
    }
    public  class spToBeValidatedViewModel
    {
         [Display(Name = "Numero Collecteur")]
        public string AgentId { get; set; }
         [Display(Name = "Collecteur")]
        public string AgentName { get; set; }
        public Nullable<int> NTrans { get; set; }
        public Nullable<double> SPaiement { get; set; }
        public Nullable<double> SCollet { get; set; }
        public Nullable<double> SNouveauClient { get; set; }
    }
}