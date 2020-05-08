using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace TourizmTest.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolesManager;

        public RolesController(RoleManager<IdentityRole> rolesManager)
        {
            _rolesManager = rolesManager;
        }

        public List<IdentityRole> GetAllRoles()
        {
            List<IdentityRole> roles = _rolesManager.Roles.ToList();
            return roles;
        }

    } 
}
