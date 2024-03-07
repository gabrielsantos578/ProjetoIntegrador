﻿using Microsoft.EntityFrameworkCore;
using SGED.Context;
using SGED.Models.Entities;
using SGED.Repositories.Interfaces;

namespace SGED.Repositories.Entities
{
	public class ImovelRepository : IImovelRepository
	{
		private readonly AppDBContext _dbContext;

		public ImovelRepository(AppDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Imovel>> GetAll()
		{
			return await _dbContext.Imovel.Include(objeto => objeto.Logradouro).Include(objeto => objeto.Municipe).ToListAsync();
		}

		public async Task<Imovel> GetById(int id)
		{
			return await _dbContext.Imovel.Include(objeto => objeto.Logradouro).Include(objeto => objeto.Municipe).Where(objeto => objeto.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Imovel> Create(Imovel imovel)
		{
			_dbContext.Imovel.Add(imovel);
			await _dbContext.SaveChangesAsync();
			return imovel;
		}

		public async Task<Imovel> Update(Imovel imovel)
		{
			_dbContext.Entry(imovel).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
			return imovel;
		}

		public async Task<Imovel> Delete(int id)
		{
			var imovel = await GetById(id);
			_dbContext.Imovel.Remove(imovel);
			await _dbContext.SaveChangesAsync();
			return imovel;
		}
	}
}