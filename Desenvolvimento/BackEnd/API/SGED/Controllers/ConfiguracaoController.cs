﻿using Microsoft.AspNetCore.Mvc;
using SGED.Objects.Utilities;
using SGED.Services.Entities;
using SGED.Services.Interfaces;

namespace SGED.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConfiguracaoController : Controller
	{
		private readonly IConfiguracaoService _configuracaoService;
		private readonly Response _response;

		public ConfiguracaoController(IConfiguracaoService configuracaoService)
		{
			_configuracaoService = configuracaoService;
			_response = new Response();
		}

		[HttpGet]
		public async Task<ActionResult> GetAll()
		{
			try
			{
				var configuracoesDTO = await _configuracaoService.GetAll();
				_response.SetSuccess();
				_response.Message = configuracoesDTO.Any() ? "Lista de configurações obtida com sucesso." : "Nenhuma configuração encontrada.";
				_response.Data = configuracoesDTO;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.SetError();
				_response.Message = "Não foi possível adquirir a lista de configurações!";
				_response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}

		[HttpGet("{id:int}", Name = "GetConfiguracao")]
		public async Task<ActionResult> GetById(int id)
		{
			try
			{
				var configuracaoDTO = await _configuracaoService.GetById(id);
				if (configuracaoDTO is null)
				{
					_response.SetNotFound();
					_response.Message = "Configuração não encontrado!";
					_response.Data = configuracaoDTO;
					return NotFound(_response);
				};

				_response.SetSuccess();
				_response.Message = "Configuração " + configuracaoDTO.Descricao + " obtido com sucesso.";
				_response.Data = configuracaoDTO;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.SetError();
				_response.Message = "Não foi possível adquirir o Configuração informado!";
				_response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}

		[HttpPut("{id:int}/Ativar")]
		public async Task<ActionResult> Activate(int id)
		{
			try
			{
				var configuracaoDTO = await _configuracaoService.GetById(id);
				if (configuracaoDTO is null)
				{
					_response.SetNotFound();
					_response.Message = "Configuração não encontrada!";
					_response.Data = configuracaoDTO;
					return NotFound(_response);
				}
				else if (configuracaoDTO.Valor == true)
				{
					_response.SetSuccess();
					_response.Message = "Esta configuração já está ativada!";
					_response.Data = configuracaoDTO;
					return Ok(_response);
				}
				else
				{
					var ativar = await _configuracaoService.Activate(configuracaoDTO.Id);
					_response.SetSuccess();
					_response.Message = "Configuração ativada com sucesso.";
					_response.Data = ativar;
					return Ok(_response);
				}

			}
			catch (Exception ex)
			{
				_response.SetError();
				_response.Message = "Não foi possível ativar a configuração!";
				_response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}

		[HttpPut("{id:int}/Desativar")]
		public async Task<ActionResult> Disable(int id)
		{
			try
			{
				var configuracaoDTO = await _configuracaoService.GetById(id);
				if (configuracaoDTO is null)
				{
					_response.SetNotFound();
					_response.Message = "Configuração não encontrada!";
					_response.Data = configuracaoDTO;
					return NotFound(_response);
				}
				else if (configuracaoDTO.Valor == false)
				{
					_response.SetSuccess();
					_response.Message = "Esta configuração já está desativada!";
					_response.Data = configuracaoDTO;
					return Ok(_response);
				}
				else
				{
					var ativar = await _configuracaoService.Disable(configuracaoDTO.Id);
					_response.SetSuccess();
					_response.Message = "Configuração desativada com sucesso.";
					_response.Data = ativar;
					return Ok(_response);
				}
			}
			catch (Exception ex)
			{
				_response.SetError();
				_response.Message = "Não foi possível desativar a configuração!";
				_response.Data = new { ErrorMessage = ex.Message, StackTrace = ex.StackTrace ?? "No stack trace available!" };
				return StatusCode(StatusCodes.Status500InternalServerError, _response);
			}
		}
	}
}