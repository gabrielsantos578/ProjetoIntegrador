using Newtonsoft.Json;
using SGED.Objects.DTO.Entities;
using SGED.Objects.Models.Entities;
using SGED.Objects.Utilities;
using SGED.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace SGED.Services.Server.Middleware
{
    public class ValidateSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private static readonly List<string> TokenTypes = new() { "Bearer" };

        public ValidateSessionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var sessaoRepository = scope.ServiceProvider.GetRequiredService<ISessaoRepository>();
            var response = new Response();

            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var tokenParts = authorizationHeader.ToString().Split(' ');

                if (tokenParts.Length == 2)
                {
                    var tokenType = tokenParts[0];
                    var tokenValue = tokenParts[1];

                    if (!TokenTypes.Contains(tokenType))
                    {
                        await WriteUnauthorizedResponse(context, response, "Erro: Tipo de token inválido!");
                        return;
                    }

                    var sessao = await sessaoRepository.GetByToken(tokenValue);

                    if (sessao == null)
                    {
                        await WriteUnauthorizedResponse(context, response, "Erro: Sessão não encontrada!");
                        return;
                    }

                    if (!sessao.StatusSessao || !sessao.ValidateToken())
                    {
                        sessao.StatusSessao = false;
                        sessao.DataHoraEncerramento = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        await sessaoRepository.Update(sessao);

                        await WriteUnauthorizedResponse(context, response, "Erro: Sessão expirada!");
                        return;
                    }
                }
                else
                {
                    await WriteUnauthorizedResponse(context, response, "Erro: Formato do token inválido!");
                    return;
                }
            }
            else
            {
                await WriteUnauthorizedResponse(context, response, "Token não informado!");
                return;
            }

            await _next(context);
        }

        private static async Task WriteUnauthorizedResponse(HttpContext context, Response response, string message)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            response.SetUnauthorized();
            response.Message = message;
            response.Data = new { errorToken = message };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

    public static class ValidateSessionMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidateSessionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateSessionMiddleware>();
        }
    }
}
