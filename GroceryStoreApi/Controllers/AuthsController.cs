
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GroceryStoreApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api-clients")]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
public class AuthsController : ControllerBase
{
    // dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

    private readonly IConfiguration _configuration;
    public AuthsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost(Name = "Register new api client")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult> Register(AuthDTO user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.ClientName),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]) // Add the audience claim here
		};

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(300),
            SigningCredentials = signingCredentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };
        var jwtToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor); // Create JwtSecurityToken

        var jwtString = tokenHandler.WriteToken(jwtToken);

        return StatusCode(StatusCodes.Status200OK, jwtString);
    }
}