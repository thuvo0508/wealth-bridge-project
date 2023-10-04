using AutoMapper;
using BorrowerProject.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WealthBridge.Core.Database;
using WealthBridge.Core.Models;
using WealthBridge.Core.Services.Interface;

namespace WealthBridge.Core.Services
{
    public class BorrowerService : IBorrowerService
    {
        private WealthBridgeDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BorrowerService(
            WealthBridgeDbContext context,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public void Create(SignUpRequest model)
        {
            // validate
            if (_context.Borrower.Any(x => x.NameFirst == model.Email))
                throw new Exception("User with the email '" + model.Email + "' already exists");

            // map model to new user object
            var user = _mapper.Map<Borrower>(model);

            // save user
            _context.Borrower.Add(user);
            _context.SaveChanges();
        }
    }
}
