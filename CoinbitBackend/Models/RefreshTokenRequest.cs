﻿using System.Text.Json.Serialization;

namespace CoinbitBackend.Models
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
