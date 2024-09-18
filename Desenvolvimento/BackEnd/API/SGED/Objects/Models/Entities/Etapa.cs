﻿using System.ComponentModel.DataAnnotations.Schema;
using SGED.Objects.Enums.Status;
using SGED.Objects.Interfaces;
using SGED.Objects.Utilities;

namespace SGED.Objects.Models.Entities
{
    [Table("etapa")]
    public class Etapa : IPosicao
    {
        [Column("idetapa")]
        public int Id { get; set; }

        [Column("nomeetapa")]
        public string NomeEtapa { get; set; }

        [Column("descricaoetapa")]
        public string DescricaoEtapa { get; set; }

        [Column("posicaoetapa")]
        public int Posicao { get; set; }

        [Column("statusetapa")]
        public StatusData Status { get; set; }

        public TipoProcesso? TipoProcesso { get; set; }

        [Column("idtipoprocesso")]
        public int IdTipoProcesso { get; set; }

        public ICollection<TipoDocumentoEtapa>? TipoDocumentoEtapas { get; set; }
    }
}
