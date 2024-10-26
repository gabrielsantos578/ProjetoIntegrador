﻿using SGED.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using SGED.Objects.DTOs.Entities;
using SGED.Objects.Utilities;
using System.Xml.Linq;
using SGED.Services.Server.Attributes;

namespace SGED.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoProcessoController : Controller
    {
        private readonly IProcessoService _processoService;
        private readonly IDocumentoProcessoService _documentoProcessoService;
        private readonly Response _response;

        public DocumentoProcessoController(IProcessoService processoService, IDocumentoProcessoService documentoProcessoService)
        {
            _processoService = processoService;
            _documentoProcessoService = documentoProcessoService;

            _response = new Response();
        }

        [HttpGet(Name = "GetAllProcessDocuments")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult<IEnumerable<DocumentoProcessoDTO>>> GetAll()
        {
            try
            {
                var documentoProcessosDTO = await _documentoProcessoService.GetAll();

                _response.SetSuccess();
                _response.Message = documentoProcessosDTO.Any() ?
                    "Lista do(s) Documento(s) de Processo(s) obtida com sucesso." :
                    "Nenhum Documento de Processo encontrado.";
                _response.Data = documentoProcessosDTO;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível adquirir a lista do(s) Documento(s) de Processo(s)!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("GetByStatus/{status:int}")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult<IEnumerable<DocumentoProcessoDTO>>> GetByStatus(int status)
        {
            try
            {
                var documentoProcessosDTO = await _documentoProcessoService.GetByStatus(status);

                _response.SetSuccess();
                _response.Message = documentoProcessosDTO.Any() ?
                    "Lista do(s) Documento(s) obtida com sucesso." :
                    "Nenhum Documento encontrado.";
                _response.Data = documentoProcessosDTO;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível adquirir a lista do(s) Documento(s)!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("GetByProcess/{idProcesso:Guid}", Name = "GetByProcess")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult<IEnumerable<DocumentoProcessoDTO>>> GetByProcess(Guid idProcesso)
        {
            try
            {
                var documentoProcessosDTO = await _documentoProcessoService.GetByProcess(idProcesso);

                _response.SetSuccess();
                _response.Message = documentoProcessosDTO.Any() ?
                    "Lista do(s) Documento(s) relacionado(s) ao Processo obtida com sucesso." :
                    "Nenhum Documento relacionado ao Processo encontrado.";
                _response.Data = documentoProcessosDTO;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível adquirir a lista do(s) Documento(s) relacionado(s) ao Processo!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id:Guid}", Name = "GetProcessDocument")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult<DocumentoProcessoDTO>> GetById(Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "Documento de Processo não encontrada!";
                    _response.Data = documentoProcessoDTO;
                    return NotFound(_response);
                };

                _response.SetSuccess();
                _response.Message = "Documento de Processo " + documentoProcessoDTO.IdentificacaoDocumento + " obtido com sucesso.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível adquirir a DocumentoProcesso informada!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPost()]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> Post([FromBody] DocumentoProcessoDTO documentoProcessoDTO)
        {
            if (documentoProcessoDTO is null)
            {
                _response.SetInvalid();
                _response.Message = "Dado(s) inválido(s)!";
                _response.Data = documentoProcessoDTO;
                return BadRequest(_response);
            }

            try
            {
                var processoDTO = await _processoService.GetById(documentoProcessoDTO.IdProcesso);
                if (processoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Processo informado não existe!";
                    _response.Data = new { errorIdProcesso = "O Processo informado não existe!" };
                    return NotFound(_response);
                }

                if (await DocumentoProcessoExists(documentoProcessoDTO))
                {
                    _response.SetConflict();
                    _response.Message = "Já existe a DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " relacionada ao Processo " + processoDTO.IdentificacaoProcesso + "!";
                    _response.Data = new { errorIdentificacaoDocumento = "Já existe a DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " relacionada ao Processo " + processoDTO.IdentificacaoProcesso + "!" };
                    return BadRequest(_response);
                }

                await _documentoProcessoService.Create(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " cadastrada com sucesso.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível cadastrar a DocumentoProcesso!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut()]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> Put([FromBody] DocumentoProcessoDTO documentoProcessoDTO)
        {
            if (documentoProcessoDTO is null)
            {
                _response.SetInvalid();
                _response.Message = "Dado(s) inválido(s)!";
                _response.Data = documentoProcessoDTO;
                return BadRequest(_response);
            }

            try
            {
                var existingDocumentoProcessoDTO = await _documentoProcessoService.GetById(documentoProcessoDTO.Id);
                if (existingDocumentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Documento Processo informado não existe!";
                    _response.Data = new { errorId = "O Documento Processo informado não existe!" };
                    return NotFound(_response);
                }

                var processoDTO = await _processoService.GetById(documentoProcessoDTO.IdProcesso);
                if (processoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Processo informado não existe!";
                    _response.Data = new { errorIdProcesso = "O Processo informado não existe!" };
                    return NotFound(_response);
                }

                if (await DocumentoProcessoExists(documentoProcessoDTO))
                {
                    _response.SetConflict();
                    _response.Message = "Já existe a DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " relacionada ao Processo " + processoDTO.IdentificacaoProcesso + "!";
                    _response.Data = new { errorIdentificacaoDocumento = "Já existe a DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " relacionada ao Processo " + processoDTO.IdentificacaoProcesso + "!" };
                    return BadRequest(_response);
                }

                await _documentoProcessoService.Update(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " alterada com sucesso.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível alterar a DocumentoProcesso!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("PutOnPending")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> PutOnPending([FromBody] Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Documento Processo informado não existe!";
                    _response.Data = new { errorId = "O Documento Processo informado não existe!" };
                    return NotFound(_response);
                }

                documentoProcessoDTO.PutOnPending();
                await _documentoProcessoService.Update(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " colocado em pendente.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível atualizar o status do Documento Processo!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("MarkAsAttached")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> MarkAsAttached([FromBody] Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Documento Processo informado não existe!";
                    _response.Data = new { errorId = "O Documento Processo informado não existe!" };
                    return NotFound(_response);
                }

                documentoProcessoDTO.MarkAsAttached();
                await _documentoProcessoService.Update(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " marcado como anexado.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível atualizar o status do Documento Processo!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("SendForAnalysis")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> SendForAnalysis([FromBody] Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Documento Processo informado não existe!";
                    _response.Data = new { errorId = "O Documento Processo informado não existe!" };
                    return NotFound(_response);
                }

                documentoProcessoDTO.SendForAnalysis();
                await _documentoProcessoService.Update(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " enviado para análise.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível atualizar o status do Documento Processo!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("Approve")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> Approve([FromBody] Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Documento Processo informado não existe!";
                    _response.Data = new { errorId = "O Documento Processo informado não existe!" };
                    return NotFound(_response);
                }

                documentoProcessoDTO.Approve();
                await _documentoProcessoService.Update(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " aprovado.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível atualizar o status do Documento Processo!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("Disapprove")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult> Disapprove([FromBody] Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "O Documento Processo informado não existe!";
                    _response.Data = new { errorId = "O Documento Processo informado não existe!" };
                    return NotFound(_response);
                }

                documentoProcessoDTO.Disapprove();
                await _documentoProcessoService.Update(documentoProcessoDTO);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " desaprovado.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível atualizar o status do Documento Processo!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{id:Guid}")]
        [AccessPermission("A", "B", "C")]
        public async Task<ActionResult<DocumentoProcessoDTO>> Delete(Guid id)
        {
            try
            {
                var documentoProcessoDTO = await _documentoProcessoService.GetById(id);
                if (documentoProcessoDTO is null)
                {
                    _response.SetNotFound();
                    _response.Message = "DocumentoProcesso não encontrada!";
                    _response.Data = new { errorId = "DocumentoProcesso não encontrada!" };
                    return NotFound(_response);
                }

                await _documentoProcessoService.Remove(id);

                _response.SetSuccess();
                _response.Message = "DocumentoProcesso " + documentoProcessoDTO.IdentificacaoDocumento + " excluída com sucesso.";
                _response.Data = documentoProcessoDTO;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.SetError();
                _response.Message = "Não foi possível excluir a DocumentoProcesso!";
                _response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        private async Task<bool> DocumentoProcessoExists(DocumentoProcessoDTO documentoProcessoDTO)
        {
            var documentoProcessosDTO = await _documentoProcessoService.GetByProcess(documentoProcessoDTO.IdProcesso);
            return documentoProcessosDTO.FirstOrDefault(c => c.Id != documentoProcessoDTO.Id && Operator.CompareString(c.IdentificacaoDocumento, documentoProcessoDTO.IdentificacaoDocumento)) is not null;
        }
    }
}