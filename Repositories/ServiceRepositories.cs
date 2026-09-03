using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;

namespace SkillHub.Repositories
{
    public sealed class ServiceRepository
    {
        private readonly DatabaseConnection _database;

        public ServiceRepository()
        {
            _database = new DatabaseConnection();
        }

        public List<ServiceCatalogItem> GetActiveServices()
        {
            List<ServiceCatalogItem> services = new List<ServiceCatalogItem>();

            const string sql = @"
                SELECT
                    ServiceId,
                    FreelancerId,
                    FreelancerName,
                    Title,
                    Description,
                    Price,
                    DeliveryDays,
                    AvailableSlots,
                    IsActive
                FROM dbo.vw_ServiceCatalog
                WHERE IsActive = 1
                ORDER BY ServiceId DESC;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    services.Add(MapService(reader));
                }
            }

            return services;
        }

        public List<ServiceCatalogItem> SearchActiveServices(string searchText)
        {
            List<ServiceCatalogItem> services = new List<ServiceCatalogItem>();

            const string sql = @"
                SELECT
                    ServiceId,
                    FreelancerId,
                    FreelancerName,
                    Title,
                    Description,
                    Price,
                    DeliveryDays,
                    AvailableSlots,
                    IsActive
                FROM dbo.vw_ServiceCatalog
                WHERE IsActive = 1
                  AND
                  (
                      Title LIKE @Search
                      OR Description LIKE @Search
                      OR FreelancerName LIKE @Search
                  )
                ORDER BY ServiceId DESC;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@Search",
                    SqlDbType.NVarChar,
                    "%" + (searchText ?? string.Empty).Trim() + "%",
                    255);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        services.Add(MapService(reader));
                    }
                }
            }

            return services;
        }

        public ServiceCatalogItem GetServiceById(int serviceId)
        {
            if (serviceId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(serviceId));
            }

            const string sql = @"
                SELECT
                    ServiceId,
                    FreelancerId,
                    FreelancerName,
                    Title,
                    Description,
                    Price,
                    DeliveryDays,
                    AvailableSlots,
                    IsActive
                FROM dbo.vw_ServiceCatalog
                WHERE ServiceId = @ServiceId
                  AND IsActive = 1;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ServiceId",
                    SqlDbType.Int,
                    serviceId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapService(reader);
                    }
                }
            }

            return null;
        }

        private static ServiceCatalogItem MapService(SqlDataReader reader)
        {
            ServiceCatalogItem item = new ServiceCatalogItem();

            item.ServiceId = Convert.ToInt32(reader["ServiceId"]);
            item.FreelancerId = Convert.ToInt32(reader["FreelancerId"]);
            item.FreelancerName = Convert.ToString(reader["FreelancerName"]);
            item.Title = Convert.ToString(reader["Title"]);
            item.Description = Convert.ToString(reader["Description"]);
            item.Price = Convert.ToDecimal(reader["Price"]);
            item.DeliveryDays = Convert.ToInt32(reader["DeliveryDays"]);
            item.AvailableSlots = Convert.ToInt32(reader["AvailableSlots"]);
            item.IsActive = Convert.ToBoolean(reader["IsActive"]);

            return item;
        }
    }
}