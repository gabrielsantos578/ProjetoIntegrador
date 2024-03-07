﻿using SGED.Repositories.Interfaces;

namespace SGED.Services.Server.Tasks

{
    public class RemoveSessionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveSessionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var _sessaoRepository = scope.ServiceProvider.GetRequiredService<ISessaoRepository>();
            var _usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    // Obtém todas as sessões agrupadas por usuário e ordenadas pelo ID
                    var sessionsGroupedByUser = await _sessaoRepository.GetAllSessionsGroupedByUser();

                    foreach (var userSessions in sessionsGroupedByUser)
                    {
                        // Verifica se o grupo possui mais de uma sessão
                        if (userSessions.Count() > 1)
                        {
                            // Remove todas as sessões com StatusSessao igual a false
                            foreach (var session in userSessions)
                            {
                                if (!session.StatusSessao) await _sessaoRepository.Delete(session.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    return;
                }

                // Aguarda 10 minutos antes de verificar novamente
                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
        }
    }
}