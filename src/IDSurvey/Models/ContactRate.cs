﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IDSurvey.Models
{
    public class ContactRate
    {
        [Key]
        public string Description { get; set; }
      
        [Required]
        public int TOTAL { get; set; }

        [Required]
        public int COMPLETED { get; set; }

        [Required]
        public int WRONG_NUMBER{ get; set; }
    }
}
