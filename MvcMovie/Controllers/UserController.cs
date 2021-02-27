using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Data;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var userWithRoles = (from user in _context.Users 
                                select new {
                                UserId = user.Id,
                                UserName = user.UserName,
                                Name = user.Name,
                                Email = user.Email,
                                RoleNames = ( from userRole in _context.UserRoles
                                              join role in _context.Roles on 
                                              userRole.RoleId equals role.Id where
                                              userRole.UserId==user.Id
                                              select role.Name).ToList()
                                }).ToList().Select(p => new UserViewModel()
                                {
                                    UserId = p.UserId,
                                    UserName = p.UserName,
                                    Name = p.Name,
                                    Email = p.Email,
                                    Role = string.Join(",", p.RoleNames)
                                });
            return View(userWithRoles);
        }
    }
}
