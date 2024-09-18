﻿using SGED.DTO.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGED.Objects.Models.Entities
{
    public class Login
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
