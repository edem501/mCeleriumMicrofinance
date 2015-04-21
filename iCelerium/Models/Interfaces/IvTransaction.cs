using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCelerium.Models.Interfaces
{
    interface IvTransaction
    {
        [Display(Name = "Date Operation")]
        System.DateTime Date { get; set; }

        [Display(Name = "Agent Collecteur")]
         string Nom_Commercial { get; set; }

        [Display(Name = "Membre")]
         string Nom_Client { get; set; }

        [Display(Name = "Solde Ouverture")]
        [DataType(DataType.Currency)]
        double Solde_Ouverture { get; set; }

        [Display(Name = "Debit")]
        [DataType(DataType.Currency)]
         double Debit { get; set; }
        [Display(Name = "Credit")]
        [DataType(DataType.Currency)]
         double Credit { get; set; }

        [Display(Name = "Solde Fermeture")]
        [DataType(DataType.Currency)]
         double Solde_Fermeture { get; set; }

        [ScaffoldColumn(false)]
         int id { get; set; }

        [ScaffoldColumn(false)]
         string AgentId { get; set; }
    }
}
