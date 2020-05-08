using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourizmTest.Models
{
    public class Service
    {
        public int Id {get;set;}
        public string Header { get;set; }
        public string Describing { get;set; }
        public string Location { get;set; }
        public int Price { get;set; }
        public string UserId { get;set; }
        public User User { get;set; }
        public int PhotoFileId { get;set; }
        public PhotoFile PhotoFile { get; set; } 

    }
}