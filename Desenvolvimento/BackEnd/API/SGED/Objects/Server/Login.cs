﻿using Microsoft.IdentityModel.Tokens;
using SGED.Objects.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace SGED.Objects.Server
{
    public class Login
    {
        [Required(ErrorMessage = "O e-mail é requerido!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é requerida!")]
        [MinLength(6)]
        public string Senha
        {
            get => senha;
            set
            {
                senha = value.GenerateHash();
            }
        }

        // Propriedade que armazena a senha em texto claro
        private string senha;
    }
}
