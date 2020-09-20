using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class DigitizingOrderData
    {
        public int CreateDigitizingOrder(string email, int width, int height, string notes, int designId, decimal total)
        {
            var data = new SQLData();
            var digitizingOrderId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("     ", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("digitizingOrderId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@width", width);
                    sqlComm.Parameters.AddWithValue("@height", height);
                    sqlComm.Parameters.AddWithValue("@notes", notes);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@total", total);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    digitizingOrderId = (int)returnParameter.Value;

                }

                return digitizingOrderId;
            }
            catch (Exception ex)
            {
                return digitizingOrderId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateDigitizingOrder(int digitizingOrderId, string alterationsRequested, bool approved, bool completed, bool rework)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateDigitizingOrder", data.conn);
                    sqlComm.Parameters.AddWithValue("@digitizingOrderId", digitizingOrderId);
                    sqlComm.Parameters.AddWithValue("@alterationsRequested", alterationsRequested);
                    sqlComm.Parameters.AddWithValue("@approved", approved);
                    sqlComm.Parameters.AddWithValue("@completed", completed);
                    sqlComm.Parameters.AddWithValue("@rework", rework);

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
        
        public DigitizingOrder GetDigitizingOrder(int digitizingOrderId)
        {

            DigitizingOrder digitizingOrder = new DigitizingOrder();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetDigitizingOrder", data.conn);
                    sqlComm.Parameters.AddWithValue("@digitizingOrderId", digitizingOrderId);
                    //sqlComm.Parameters.AddWithValue("@TimeRange", TimeRange);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        digitizingOrder.Id = Convert.ToInt32(dr["Id"]);
                        digitizingOrder.Email = dr["Email"].ToString();
                        digitizingOrder.Width = Convert.ToDecimal(dr["Width"].ToString());
                        digitizingOrder.Height = Convert.ToDecimal(dr["Height"].ToString());
                        digitizingOrder.Notes = dr["Notes"].ToString();
                        digitizingOrder.DesignId = Convert.ToInt32(dr["DesignId"]);
                        digitizingOrder.StitchCount = Convert.ToInt32(dr["StitchCount"]);
                        digitizingOrder.Total = Convert.ToDecimal(dr["Total"].ToString());
                        digitizingOrder.HasPaid = Convert.ToBoolean(dr["HasPaid"].ToString());
                        digitizingOrder.Completed = Convert.ToBoolean(dr["Completed"].ToString());
                        digitizingOrder.Rework = Convert.ToBoolean(dr["Rework"].ToString());
                        digitizingOrder.Approved = Convert.ToBoolean(dr["Approved"].ToString());
                        digitizingOrder.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        digitizingOrder.AlterationsNote = (dr["AlterationsNote"].ToString());
                        
                    }
                    return digitizingOrder;
                }
                else
                {
                    return digitizingOrder;
                }
            }
            catch (Exception ex)
            {
                return digitizingOrder;
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