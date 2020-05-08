using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourizmTest.Models
{
    public class PhotoFile
    {
        public int Id { get;set; }
        public string Path { get;set; }
    }
}