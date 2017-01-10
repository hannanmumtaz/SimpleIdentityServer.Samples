﻿#region copyright
// Copyright 2017 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IJwsParser _jwsParser;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;

        public AuthenticateController(IJwsParser jwsParser)
        {
            _jwsParser = jwsParser;
            _identityServerClientFactory = new IdentityServerClientFactory();
        }

        public async Task<IActionResult> Callback()
        {
            var defaultValue = default(KeyValuePair<string, StringValues>);
            if (HttpContext.Request == null || HttpContext.Request.Form == null)
            {
                throw new ArgumentNullException("form");
            }

            var idToken = HttpContext.Request.Form.FirstOrDefault(f => f.Key == "id_token");
            var accessToken = HttpContext.Request.Form.FirstOrDefault(f => f.Key == "access_token");
            var code = HttpContext.Request.Form.FirstOrDefault(f => f.Key == "code");
            if (idToken.Equals(defaultValue) && accessToken.Equals(defaultValue) && code.Equals(defaultValue))
            {
                throw new ArgumentException("One of the parameter is null");
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
