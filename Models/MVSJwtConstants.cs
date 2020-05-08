using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace TourizmTest.Models
{
    public class MVSJwtConstants
    {
        public const string Issuer = "MyAuthServer";
        public const string Audience = "MyAuthCLient";
        public const string Key = "mysupersecret_secretkey!123";
        public const int Days = 30;
    }
}