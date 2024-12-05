namespace LibraryDemo.Core.Services;

public class TokenService(IOptions<LibrarySettings> settings, UserManager<User> userManager)
{
    private readonly LibrarySettings Settings = settings.Value;

    private readonly UserManager<User> UserManager = userManager;

    public async Task<string> GenerateTokenAsync(string userName)
    {
        var tokenDescriptor = await GenerateTokenDescriptor(userName);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    private async Task<SecurityTokenDescriptor> GenerateTokenDescriptor(string userName)
    {
        var user = await UserManager.FindByNameAsync(userName);
        var roles = await UserManager.GetRolesAsync(user!);

        var claims = GenerateClaims(user!, roles);
        var credentials = GenerateSigningCredentials();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Settings.Identity!.JwtSettings!.ExpiryInMinutes),
            SigningCredentials = credentials,
            Audience = Settings.Identity.JwtSettings.Audience,
            Issuer = Settings.Identity.JwtSettings.Issuer
        };

        return tokenDescriptor;
    }

    private SigningCredentials GenerateSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Settings.Identity!.JwtSettings!.Key);

        return new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GenerateClaims(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user!.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        return claims;
    }
}