using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class OrderData
    {
        public int CreateOrder(decimal total, string firstName, string lastName, string email, string phone, string address, string city, string state, string zip, string addressBill, string cityBill, string stateBill, string zipBill, string paymentGuid, int userId)
        {
            var data = new SQLData();
            var orderId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateOrder", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("orderId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@total", total);
                    sqlComm.Parameters.AddWithValue("@firstName", firstName);
                    sqlComm.Parameters.AddWithValue("@lastName", lastName);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@phone", phone);
                    sqlComm.Parameters.AddWithValue("@address", address);
                    sqlComm.Parameters.AddWithValue("@city", city);
                    sqlComm.Parameters.AddWithValue("@state", state);
                    sqlComm.Parameters.AddWithValue("@zip", zip);
                    sqlComm.Parameters.AddWithValue("@addressBill", addressBill);
                    sqlComm.Parameters.AddWithValue("@cityBill", cityBill);
                    sqlComm.Parameters.AddWithValue("@stateBill", stateBill);
                    sqlComm.Parameters.AddWithValue("@zipBill", zipBill);
                    sqlComm.Parameters.AddWithValue("@paymentGuid", paymentGuid);
                    sqlComm.Parameters.AddWithValue("@userId", userId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    orderId = (int)returnParameter.Value;

                }

                return orderId;
            }
            catch (Exception ex)
            {
                return orderId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public int CreateOrderProduct(int orderId, int productId, string size, int typeId)
        {
            var data = new SQLData();
            var orderProductId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateOrderProduct", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("orderProductId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@orderId", orderId);
                    sqlComm.Parameters.AddWithValue("@productId", productId);
                    sqlComm.Parameters.AddWithValue("@size", size);
                    sqlComm.Parameters.AddWithValue("@typeId", typeId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    orderProductId = (int)returnParameter.Value;

                }

                return orderProductId;
            }
            catch (Exception ex)
            {
                return orderProductId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateOrderHasPaid(string PaymentCode)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateOrderHasPaid", data.conn);
                    sqlComm.Parameters.AddWithValue("@paymentCode", PaymentCode);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool CheckProductHasFreeShipping(int Id)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CheckProductHasFreeShipping", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", Id);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    return Convert.ToBoolean(ds.Tables[0].Rows[0]["FreeShipping"]);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
    }
}