﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace ManageScimResources.ReactJs.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}