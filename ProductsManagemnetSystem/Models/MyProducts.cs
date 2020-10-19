using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsManagemnetSystem.Models
{
    public class MyProducts
    {

        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public float Price { get; set; }
        public int AvailableQuantity { get; set; }


        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
