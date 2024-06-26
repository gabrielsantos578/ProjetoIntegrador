﻿using SGED.Context;
using SGED.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SGED.Objects.Models.Entities;

namespace SGED.Repositories.Entities;
public class EstadoRepository : IEstadoRepository
{

    private readonly AppDBContext _dbContext;

    public EstadoRepository(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Estado>> GetAll()
    {
        return await _dbContext.Estado.AsNoTracking().ToListAsync();
    }

    public async Task<Estado> GetById(int id)
    {
        return await _dbContext.Estado.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Estado>> GetByName(string nome)
    {
        return await _dbContext.Estado.Where(e => e.NomeEstado.ToUpper().Contains(nome.ToUpper())).AsNoTracking().ToListAsync();
    }

    public async Task<Estado> Create(Estado estado)
    {
        _dbContext.Estado.Add(estado);
        await _dbContext.SaveChangesAsync();
        return estado;
    }

    public async Task<Estado> Update(Estado estado)
    {
        _dbContext.Entry(estado).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return estado;
    }

    public async Task<Estado> Delete(int id)
    {
        var estado = await GetById(id);
        _dbContext.Estado.Remove(estado);
        await _dbContext.SaveChangesAsync();
        return estado;
    }
}
