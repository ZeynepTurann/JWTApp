namespace JWTApp.Back.Infrastructure.Tools
{
    public class JwtTokenDefaults
    {
        /*
         *  ValidAudience = "http://localhost",
        ValidIssuer = "http://localhost",
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zeynepzeynepzeynep1.")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true

         */
        public const string ValidAudience = "http://localhost";
        public const string ValidIssuer = "http://localhost";
        public const string Key = "zeynepzeynepzeynep1.";
        public const int Expired = 5;
    }
}
