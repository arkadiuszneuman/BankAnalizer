using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PkoAnalizer.Core;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Config;
using PkoAnalizer.Logic.Users;
using PkoAnalizer.Logic.Users.Models.User;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Users
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly JwtTokenConfig jwtTokenConfig;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            JwtTokenConfig jwtTokenConfig)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.jwtTokenConfig = jwtTokenConfig;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            var user = mapper.Map<User>(model);

            try
            {
                await userService.Create(user, model.Password);
                return Ok();
            }
            catch (BankAnalizerException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //will be added later

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var users = await userService.GetAll();
        //    var model = mapper.Map<IList<UserModel>>(users);
        //    return Ok(model);
        //}

        //[HttpGet("{id}")]
        //public IActionResult GetById(Guid id)
        //{
        //    var user = userService.GetById(id);
        //    var model = mapper.Map<UserModel>(user);
        //    return Ok(model);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(Guid id, [FromBody]UpdateModel model)
        //{
        //    var user = mapper.Map<User>(model);
        //    user.Id = id;

        //    try
        //    {
        //        userService.Update(user, model.Password);
        //        return Ok();
        //    }
        //    catch (PkoAnalizerException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(Guid id)
        //{
        //    userService.Delete(id);
        //    return Ok();
        //}
    }
}