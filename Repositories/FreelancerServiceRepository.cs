using SkillHub.Data;
using SkillHub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SkillHub.Repositories
{
    public class FreelancerServiceRepository
    {
        // Get all services belonging to the logged-in freelancer
        public List<Service> GetByFreelancer(int freelancerId)
        {
            List<Service> services = new List<Service>();

            DatabaseConnection databaseConnection = new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        s.ServiceId,
                        s.FreelancerId,
                        s.CategoryId,
                        c.CategoryName,
                        s.Title,
                        s.Description,
                        s.ImagePath,
                        s.Price,
                        s.DeliveryDays,
                        s.AvailableSlots,
                        s.IsActive,
                        s.CreatedAt,
                        s.UpdatedAt
                    FROM dbo.Services s
                    INNER JOIN dbo.Categories c
                        ON c.CategoryId = s.CategoryId
                    WHERE s.FreelancerId = @FreelancerId
                    ORDER BY s.CreatedAt DESC;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@FreelancerId", SqlDbType.Int).Value =
                        freelancerId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new Service
                            {
                                ServiceId =
                                    Convert.ToInt32(reader["ServiceId"]),

                                FreelancerId =
                                    Convert.ToInt32(reader["FreelancerId"]),

                                CategoryId =
                                    Convert.ToInt32(reader["CategoryId"]),

                                CategoryName =
                                    reader["CategoryName"].ToString(),

                                Title =
                                    reader["Title"].ToString(),

                                Description =
                                    reader["Description"].ToString(),

                                ImagePath =
                                    reader["ImagePath"] == DBNull.Value
                                        ? string.Empty
                                        : reader["ImagePath"].ToString(),

                                Price =
                                    Convert.ToDecimal(reader["Price"]),

                                DeliveryDays =
                                    Convert.ToInt32(reader["DeliveryDays"]),

                                AvailableSlots =
                                    Convert.ToInt32(reader["AvailableSlots"]),

                                IsActive =
                                    Convert.ToBoolean(reader["IsActive"]),

                                CreatedAt =
                                    Convert.ToDateTime(reader["CreatedAt"]),

                                UpdatedAt =
                                    reader["UpdatedAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["UpdatedAt"])
                            });
                        }
                    }
                }
            }

            return services;
        }


        // Get all active service categories
        public DataTable GetCategories()
        {
            DataTable table = new DataTable();

            DatabaseConnection databaseConnection = new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        CategoryId,
                        CategoryName
                    FROM dbo.Categories
                    WHERE IsActive = 1
                    ORDER BY CategoryName;";

                using (SqlDataAdapter adapter =
                       new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }


        // Add a new service
        public void Add(Service service)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    INSERT INTO dbo.Services
                    (
                        FreelancerId,
                        CategoryId,
                        Title,
                        Description,
                        ImagePath,
                        Price,
                        DeliveryDays,
                        AvailableSlots,
                        IsActive,
                        CreatedAt
                    )
                    VALUES
                    (
                        @FreelancerId,
                        @CategoryId,
                        @Title,
                        @Description,
                        @ImagePath,
                        @Price,
                        @DeliveryDays,
                        @AvailableSlots,
                        1,
                        SYSDATETIME()
                    );";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@FreelancerId", SqlDbType.Int).Value =
                        service.FreelancerId;

                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value =
                        service.CategoryId;

                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 150).Value =
                        service.Title;

                    command.Parameters.Add("@Description", SqlDbType.NVarChar, 1500).Value =
                        service.Description;

                    command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 300).Value =
                        string.IsNullOrWhiteSpace(service.ImagePath)
                            ? (object)DBNull.Value
                            : service.ImagePath;

                    SqlParameter priceParameter =
                        command.Parameters.Add("@Price", SqlDbType.Decimal);

                    priceParameter.Precision = 18;
                    priceParameter.Scale = 2;
                    priceParameter.Value = service.Price;

                    command.Parameters.Add("@DeliveryDays", SqlDbType.Int).Value =
                        service.DeliveryDays;

                    command.Parameters.Add("@AvailableSlots", SqlDbType.Int).Value =
                        service.AvailableSlots;

                    command.ExecuteNonQuery();
                }
            }
        }


        // Update an existing service
        public void Update(Service service)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Services
                    SET
                        CategoryId = @CategoryId,
                        Title = @Title,
                        Description = @Description,
                        ImagePath = @ImagePath,
                        Price = @Price,
                        DeliveryDays = @DeliveryDays,
                        AvailableSlots = @AvailableSlots,
                        UpdatedAt = SYSDATETIME()
                    WHERE ServiceId = @ServiceId
                      AND FreelancerId = @FreelancerId;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ServiceId", SqlDbType.Int).Value =
                        service.ServiceId;

                    command.Parameters.Add("@FreelancerId", SqlDbType.Int).Value =
                        service.FreelancerId;

                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value =
                        service.CategoryId;

                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 150).Value =
                        service.Title;

                    command.Parameters.Add("@Description", SqlDbType.NVarChar, 1500).Value =
                        service.Description;

                    command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 300).Value =
                        string.IsNullOrWhiteSpace(service.ImagePath)
                            ? (object)DBNull.Value
                            : service.ImagePath;

                    SqlParameter priceParameter =
                        command.Parameters.Add("@Price", SqlDbType.Decimal);

                    priceParameter.Precision = 18;
                    priceParameter.Scale = 2;
                    priceParameter.Value = service.Price;

                    command.Parameters.Add("@DeliveryDays", SqlDbType.Int).Value =
                        service.DeliveryDays;

                    command.Parameters.Add("@AvailableSlots", SqlDbType.Int).Value =
                        service.AvailableSlots;

                    command.ExecuteNonQuery();
                }
            }
        }


        // Activate or deactivate a service
        public void SetActive(
            int serviceId,
            int freelancerId,
            bool isActive)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Services
                    SET
                        IsActive = @IsActive,
                        UpdatedAt = SYSDATETIME()
                    WHERE ServiceId = @ServiceId
                      AND FreelancerId = @FreelancerId;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ServiceId", SqlDbType.Int).Value =
                        serviceId;

                    command.Parameters.Add("@FreelancerId", SqlDbType.Int).Value =
                        freelancerId;

                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value =
                        isActive;

                    command.ExecuteNonQuery();
                }
            }
        }


        // Delete a service
        public void Delete(
            int serviceId,
            int freelancerId)
        {
            DatabaseConnection databaseConnection = new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    DELETE FROM dbo.Services
                    WHERE ServiceId = @ServiceId
                      AND FreelancerId = @FreelancerId;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ServiceId", SqlDbType.Int).Value =
                        serviceId;

                    command.Parameters.Add("@FreelancerId", SqlDbType.Int).Value =
                        freelancerId;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
