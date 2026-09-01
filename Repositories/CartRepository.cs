using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;
using SkillHub.Utilities;

namespace SkillHub.Repositories
{
    public sealed class CartRepository
    {
        private readonly DatabaseConnection _database;

        public CartRepository()
        {
            _database = new DatabaseConnection();
        }

        public int GetOrCreateCart()
        {
            int clientId = UserSession.UserId;

            const string selectSql = @"
                SELECT CartId
                FROM dbo.Carts
                WHERE ClientId = @ClientId;";

            const string insertSql = @"
                INSERT INTO dbo.Carts (ClientId)
                VALUES (@ClientId);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
            {
                DatabaseConnection.AddParameter(
                    selectCommand,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                object existingCart = selectCommand.ExecuteScalar();

                if (existingCart != null && existingCart != DBNull.Value)
                {
                    return Convert.ToInt32(existingCart);
                }
            }

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
            {
                DatabaseConnection.AddParameter(
                    insertCommand,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                return Convert.ToInt32(insertCommand.ExecuteScalar());
            }
        }

        public List<CartItem> GetCartItems()
        {
            int clientId = UserSession.UserId;

            List<CartItem> items = new List<CartItem>();

            const string sql = @"
                SELECT
                    ci.CartItemId,
                    ci.CartId,
                    ci.ServiceId,
                    svc.Title AS ServiceTitle,
                    svc.FreelancerName,
                    ci.Quantity,
                    ci.UnitPrice
                FROM dbo.CartItems AS ci
                INNER JOIN dbo.Carts AS c
                    ON c.CartId = ci.CartId
                INNER JOIN dbo.vw_ServiceCatalog AS svc
                    ON svc.ServiceId = ci.ServiceId
                WHERE c.ClientId = @ClientId
                ORDER BY ci.CartItemId DESC;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CartItem item = new CartItem();

                        item.CartItemId = Convert.ToInt32(reader["CartItemId"]);
                        item.CartId = Convert.ToInt32(reader["CartId"]);
                        item.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                        item.ServiceTitle = Convert.ToString(reader["ServiceTitle"]);
                        item.FreelancerName = Convert.ToString(reader["FreelancerName"]);
                        item.Quantity = Convert.ToInt32(reader["Quantity"]);
                        item.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);

                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public void AddItem(int serviceId, int quantity)
        {
            if (serviceId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(serviceId));
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                SET NOCOUNT ON;

                DECLARE @AvailableSlots INT;
                DECLARE @Price DECIMAL(18, 2);
                DECLARE @CartId INT;
                DECLARE @ExistingQuantity INT;

                SELECT
                    @AvailableSlots = AvailableSlots,
                    @Price = Price
                FROM dbo.vw_ServiceCatalog
                WHERE ServiceId = @ServiceId
                  AND IsActive = 1;

                IF @AvailableSlots IS NULL
                BEGIN
                    THROW 51001, 'The selected service is not available.', 1;
                END;

                IF @AvailableSlots <= 0
                BEGIN
                    THROW 51002, 'The selected service has no available slots.', 1;
                END;

                IF @AvailableSlots < @Quantity
                BEGIN
                    THROW 51003, 'The requested quantity exceeds the available slots.', 1;
                END;

                SELECT @CartId = CartId
                FROM dbo.Carts
                WHERE ClientId = @ClientId;

                IF @CartId IS NULL
                BEGIN
                    INSERT INTO dbo.Carts (ClientId)
                    VALUES (@ClientId);

                    SET @CartId = CAST(SCOPE_IDENTITY() AS INT);
                END;

                SELECT @ExistingQuantity = Quantity
                FROM dbo.CartItems
                WHERE CartId = @CartId
                  AND ServiceId = @ServiceId;

                IF @ExistingQuantity IS NULL
                BEGIN
                    INSERT INTO dbo.CartItems
                    (
                        CartId,
                        ServiceId,
                        Quantity,
                        UnitPrice
                    )
                    VALUES
                    (
                        @CartId,
                        @ServiceId,
                        @Quantity,
                        @Price
                    );
                END
                ELSE
                BEGIN
                    IF @ExistingQuantity + @Quantity > @AvailableSlots
                    BEGIN
                        THROW 51004, 'The total cart quantity exceeds the available slots.', 1;
                    END;

                    UPDATE dbo.CartItems
                    SET
                        Quantity = Quantity + @Quantity,
                        UnitPrice = @Price
                    WHERE CartId = @CartId
                      AND ServiceId = @ServiceId;
                END;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ServiceId",
                    SqlDbType.Int,
                    serviceId);

                DatabaseConnection.AddParameter(
                    command,
                    "@Quantity",
                    SqlDbType.Int,
                    quantity);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateQuantity(int cartItemId, int quantity)
        {
            if (cartItemId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cartItemId));
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                UPDATE ci
                SET
                    ci.Quantity = @Quantity,
                    ci.UnitPrice = svc.Price
                FROM dbo.CartItems AS ci
                INNER JOIN dbo.Carts AS c
                    ON c.CartId = ci.CartId
                INNER JOIN dbo.vw_ServiceCatalog AS svc
                    ON svc.ServiceId = ci.ServiceId
                WHERE ci.CartItemId = @CartItemId
                  AND c.ClientId = @ClientId
                  AND svc.IsActive = 1
                  AND svc.AvailableSlots >= @Quantity;

                IF @@ROWCOUNT = 0
                BEGIN
                    THROW 51005, 'The cart item could not be updated. Check availability.', 1;
                END;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@CartItemId",
                    SqlDbType.Int,
                    cartItemId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@Quantity",
                    SqlDbType.Int,
                    quantity);

                command.ExecuteNonQuery();
            }
        }

        public void RemoveItem(int cartItemId)
        {
            if (cartItemId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cartItemId));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                DELETE ci
                FROM dbo.CartItems AS ci
                INNER JOIN dbo.Carts AS c
                    ON c.CartId = ci.CartId
                WHERE ci.CartItemId = @CartItemId
                  AND c.ClientId = @ClientId;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@CartItemId",
                    SqlDbType.Int,
                    cartItemId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                command.ExecuteNonQuery();
            }
        }

        public void ClearCart()
        {
            int clientId = UserSession.UserId;

            const string sql = @"
                DELETE ci
                FROM dbo.CartItems AS ci
                INNER JOIN dbo.Carts AS c
                    ON c.CartId = ci.CartId
                WHERE c.ClientId = @ClientId;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                command.ExecuteNonQuery();
            }
        }

        public decimal GetCartTotal()
        {
            int clientId = UserSession.UserId;

            const string sql = @"
                SELECT
                    ISNULL(SUM(
                        CAST(ci.Quantity AS DECIMAL(18, 2)) * ci.UnitPrice
                    ), 0)
                FROM dbo.CartItems AS ci
                INNER JOIN dbo.Carts AS c
                    ON c.CartId = ci.CartId
                WHERE c.ClientId = @ClientId;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return 0m;
                }

                return Convert.ToDecimal(result);
            }
        }
    }
}