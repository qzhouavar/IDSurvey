using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IDSurvey.Models
{
    public class CompleteRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string QTR { get; set; }
        [Required]
        public string WAVE { get; set; }
        [Required]
        public string TYPE { get; set; }
        [Required]
        public string SERVICE_AREA { get; set; }

        [Required]
        public int TOTAL { get; set; }

        [Required]
        public int COMPLETE { get; set; }
    }
}
