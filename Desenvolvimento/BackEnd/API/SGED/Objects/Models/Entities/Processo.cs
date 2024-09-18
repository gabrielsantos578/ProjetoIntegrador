﻿using SGED.Objects.Enums.Status;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGED.Objects.Models.Entities
{
    [Table("processo")]
    public class Processo
    {
        [Column("idprocesso")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column("identificacaoprocesso")]
        public string IdentificacaoProcesso { get; set; }

        [Column("descricaoprocesso")]
        public string DescricaoProcesso { get; set; }

        [Column("situacaoprocesso")]
        public string SituacaoProcesso { get; set; }

        [Column("dataaprovacao")]
        public string DataAprovacao { get; set; }

        [Column("statusprocesso")]
        public StatusProcess Status { get; set; }

        [Column("idimovel")]
        public int IdImovel { get; set; }

        [Column("idtipoprocesso")]
        public int IdTipoProcesso { get; set; }

        [Column("idengenheiro")]
        public int? IdEngenheiro { get; set; }

        [Column("idfiscal")]
        public int? IdFiscal { get; set; }

        [Column("idresponsavel")]
        public int? IdResponsavel { get; set; }

        [Column("idaprovador")]
        public int? IdAprovador { get; set; }


        public Imovel? Imovel { get; set; }
        public TipoProcesso? TipoProcesso { get; set; }
        public Engenheiro? Engenheiro { get; set; }
        public Fiscal? Fiscal { get; set; }
        public Usuario? Responsavel { get; set; }
        public Usuario? Aprovador { get; set; }
        public ICollection<DocumentoProcesso>? DocumentosProcesso { get; set; }

    }
}