﻿using CRMuser.Application.DTO;
using CRMUser.domain.Interface;
using CRMUser.domain.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _userRepository.Register(entity.Adapt<User>());
                    return Accepted(entity);
                }
                return BadRequest(new { Message = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }

        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _token = await _userRepository.Login(entity);
                    if(_token is null) return Unauthorized(new { Message = "Invalid Credentials" });
                    return Ok(new { token = _token });
                }
                return BadRequest(new { Message = "Invalid Request" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }
        }
    }
}
