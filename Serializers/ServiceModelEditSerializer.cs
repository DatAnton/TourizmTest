using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TourizmTest.Serializers
{
    public class ServiceModelEditSerializer
    {   
        [MinLength(5), MaxLength(50)]
        public string Header { get;set; }
        [MinLength(20)]
        public string Describing { get;set; }
        public string Location { get;set; }
        [Range(0,int.MaxValue)]
        public int Price { get;set; }
        public IFormFile PhotoFile { get; set; } 
    }
}