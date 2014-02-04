/* using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DealStealUnreal.EFDatabaseContext
{
    public class Gift
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int countDeal { get; set; }
        public int countSteal { get; set; }
        public int countUnreal { get; set; }
        public string image { get; set; }
        public string description { get; set; }
    }
} */