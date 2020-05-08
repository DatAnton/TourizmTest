using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TourizmTest.Serializers
{
    public class ServiceModelSerializer
    {   
        [Required]
        [MinLength(5), MaxLength(50)]
        public string Header { get;set; }
        [Required]
        [MinLength(20)]
        public string Describing { get;set; }
        [Required]
        public string Location { get;set; }
        [Required, Range(0, int.MaxValue)]
        public int Price { get;set; }
        [Required]
        public IFormFile PhotoFile { get; set; } 
    }
}