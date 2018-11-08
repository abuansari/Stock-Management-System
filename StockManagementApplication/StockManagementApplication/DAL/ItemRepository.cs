﻿using StockManagementApplication.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace StockManagementApplication.DAL
{
    public class ItemRepository
    {
        private readonly string _connectionString = GenericRepository.ConnectionString();
        readonly GenericRepository _genericRepository = new GenericRepository();

        public bool Save(Item item)
        {
            try
            {
                string query = "INSERT INTO Items(CategoryId, CompanyId, Name,ReorderLevel, CreateBy, CreateDate) VALUES('" + item.CategoryId + "'," +
                               "'" + item.CompanyId + "', '" + item.Name + "', '" + item.ReorderLevel + "','" + item.CreateBy + "','" + item.CreateDate + "')";
                var rowAffected = _genericRepository.ExecuteNonQuery(query, _connectionString);
                return rowAffected > 0;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool IsNameExist(Item item)
        {
            try
            {
                var query = "SELECT * FROM Items WHERE Name='" + item.Name + "'";
                var reader = _genericRepository.ExecuteReader(query, _connectionString);
                return reader.HasRows;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public DataTable GetAllCompanyByCategoryId(Item item)
        {
            try
            {
                var query = "SELECT DISTINCT c.Id, c.Name FROM Items i INNER JOIN Companies c On c.Id= i.CompanyId WHERE CategoryId=" + item.CategoryId + "";
                var dataAdapter = _genericRepository.ExecuteAdapter(query, _connectionString);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public DataTable GetAllItemByCompanyId(Item item)
        {
            try
            {
                var query = "SELECT Id, Name FROM Items WHERE CompanyId= " + item.CompanyId + "";
                var dataAdapter = _genericRepository.ExecuteAdapter(query, _connectionString);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public SqlDataReader GetReorderLevel(Item item)
        {
            try
            {
                var query = "SELECT ReorderLevel FROM Items WHERE Id= " + item.Id + "";
                var reader = _genericRepository.ExecuteReader(query, _connectionString);
                return reader;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public DataTable GetItemReport(int? categoryId, int? companyId)
        {
            try
            {
                var query = "SELECT i.Name AS ItemName, co.Name AS CompanyName, c.Name AS CategoryName, i.ReorderLevel, ISNULL(ISNULL(SUM(s.InQuantity),0)-ISNULL(SUM(s.OutQuantity),0),0) AS AvailableQty FROM Items i " +
                            "LEFT JOIN Categories c ON c.Id= i.CategoryId " +
                            "LEFT JOIN Companies co ON co.Id= i.CompanyId " +
                            "LEFT JOIN Stocks s ON i.Id= s.ItemId " +
                            "WHERE i.CategoryId=" + categoryId + " AND (i.CompanyId=" + companyId + " OR i.CompanyId IS NULL) " +
                            "GROUP BY i.Name,c.Name, co.Name, i.ReorderLevel";
                var dataAdapter = _genericRepository.ExecuteAdapter(query, _connectionString);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
