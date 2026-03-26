using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure.Services
{
    public class LiveTokenService : ILiveTokenService
    {
        private readonly IConfiguration _configuration;

        public LiveTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateTokenAsync(string roomId, string userId, string role, bool canPublish)
        {
            var apiKey = _configuration["LiveKit:ApiKey"];
            var apiSecret = _configuration["LiveKit:ApiSecret"];
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
                throw new InvalidOperationException("LiveKit configuration is missing.");

            var ttlMinutes = 10;
            var ttlSetting = _configuration["LiveKit:TokenTtlMinutes"];
            if (int.TryParse(ttlSetting, out var ttlParsed) && ttlParsed > 0)
                ttlMinutes = ttlParsed;

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(ttlMinutes);

            var videoGrant = new Dictionary<string, object>
            {
                { "roomJoin", true },
                { "room", roomId },
                { "canPublish", canPublish },
                { "canSubscribe", true }
            };

            var payload = new JwtPayload
            {
                { "iss", apiKey },
                { "sub", userId },
                { "name", userId },
                { "metadata", role },
                { "nbf", new DateTimeOffset(now).ToUnixTimeSeconds() },
                { "exp", new DateTimeOffset(expires).ToUnixTimeSeconds() },
                { "video", videoGrant }
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(new JwtHeader(credentials), payload);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(jwt);
        }
    }
}
