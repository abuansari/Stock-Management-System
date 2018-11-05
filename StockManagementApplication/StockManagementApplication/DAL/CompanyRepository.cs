﻿using StockManagementApplication.Models;
using System;

namespace StockManagementApplication.DAL
{
    public class CompanyRepository
    {
        private readonly string _connectionString = GenericRepository.ConnectionString();
        readonly GenericRepository _genericRepository = new GenericRepository();

        public bool Save(Company company)
        {
            try
            {
                var query = "INSERT INTO Companies VALUES('" + company.Name + "', '" + company.CreateBy + "','" +
                            company.CreateDate + "')";
                var rowAffected = _genericRepository.ExecuteNonQuery(query, _connectionString);
                return rowAffected > 0;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
