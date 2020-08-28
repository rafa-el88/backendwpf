using DATA.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace SECURITY.Domain.Global
{
    public class Token
    {
        private IConfiguration _conf;
        private SigningConfigurations _signConf;
        public Token(IConfiguration Configuration, SigningConfigurations signingConfigurations)
        {
            _conf = Configuration;
            _signConf = signingConfigurations;
        } 
        public object Criar2(User loginUser)
        {

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(loginUser.Email, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")), 
                        new Claim(JwtRegisteredClaimNames.GivenName, loginUser.Nome)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromMinutes(int.Parse(_conf["Jwt:MinutesToExpire"]));

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _conf["Jwt:Issuer"],
                Audience = _conf["Jwt:Audience"],
                SigningCredentials = _signConf.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new
            {
                authenticated = true,
                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = "Bearer " + token,
                message = "OK"
            };
            //Adicionei a palavra Bearer, por algum motivo o swagger nao estava adicionando automatico

        }

    }
}
