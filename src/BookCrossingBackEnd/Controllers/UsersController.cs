﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Entities;
using Infastructure.Configuration;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]


    
    public class UsersController : Controller
    {

        private IUser serice { get; set; }
        private IConfiguration configuration { get; set; }
        private IToken tokenService { get; set; }

        public UsersController(IUser userService, IConfiguration configuration, IToken tokenService)
        {
            this.serice = userService;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }   

        /// <summary>
        /// Get list of all users (only for admin)
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
 
        public IEnumerable<string> Get()
        {
            return new string[]
            {
                "Hello",
                "World"
            };
        }

    


        /// <summary>
        /// Get user by id (only for admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        
       
        // PUT api/<controller>/5
        /// <summary>
        /// Function for updating info about user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Update(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function for deleting user (only for admin)
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}