using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LidLaunchWebsite.Models;

namespace LidLaunchWebsite.Classes
{
    public class ExpenseData
    {
        public int CreateExpense(string type, decimal amount, DateTime dateFrom, DateTime dateTo,  string title, string description, string attachment)
        {
            var data = new SQLData();
            var expenseId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateExpense", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("expenseId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@type", type);
                    sqlComm.Parameters.AddWithValue("@amount", amount);
                    sqlComm.Parameters.AddWithValue("@dateFrom", dateFrom);
                    sqlComm.Parameters.AddWithValue("@dateTo", dateTo);
                    sqlComm.Parameters.AddWithValue("@title", title);
                    sqlComm.Parameters.AddWithValue("@description", description);
                    sqlComm.Parameters.AddWithValue("@attachment", attachment);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    expenseId = (int)returnParameter.Value;

                }

                return expenseId;
            }
            catch (Exception ex)
            {
                return expenseId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateExpense(int id, string type, decimal amount, DateTime dateFrom, DateTime dateTo, string title, string description, string attachment)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateExpense", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", id);
                    sqlComm.Parameters.AddWithValue("@type", type);
                    sqlComm.Parameters.AddWithValue("@amount", amount);
                    sqlComm.Parameters.AddWithValue("@dateFrom", dateFrom);
                    sqlComm.Parameters.AddWithValue("@dateTo", dateTo);
                    sqlComm.Parameters.AddWithValue("@title", title);
                    sqlComm.Parameters.AddWithValue("@description", description);
                    sqlComm.Parameters.AddWithValue("@attachment", attachment);

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
        public bool DeleteExpense(int expenseId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("DeleteExpense", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", expenseId);

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
        public Expense GetExpense(int expenseId)
        {

            Expense model = new Expense();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetExpense", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", expenseId);

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

                        model.Id = Convert.ToInt32(dr["Id"]);
                        model.Type = dr["Type"].ToString();
                        model.Amount = Convert.ToDecimal(dr["Amount"].ToString());
                        model.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                        model.DateFrom = Convert.ToDateTime(dr["DateFrom"].ToString());
                        model.DateTo = Convert.ToDateTime(dr["DateTo"].ToString());
                        model.Title = dr["Title"].ToString();
                        model.Description = dr["Description"].ToString();
                        model.Attachment = dr["Attachment"].ToString();
                    }
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                return model;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public List<Expense> GetExpenses(DateTime dateFrom, DateTime dateTo)
        {

            List<Expense> model = new List<Expense>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetExpenses", data.conn);
                    sqlComm.Parameters.AddWithValue("@dateFrom", dateFrom);
                    sqlComm.Parameters.AddWithValue("@dateTo", dateTo);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Expense expense = new Expense();
                            expense.Id = Convert.ToInt32(dr["Id"]);
                            expense.Type = dr["Type"].ToString();
                            expense.Amount = Convert.ToDecimal(dr["Amount"].ToString());
                            expense.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                            expense.DateFrom = Convert.ToDateTime(dr["DateFrom"].ToString());
                            expense.DateTo = Convert.ToDateTime(dr["DateTo"].ToString());
                            expense.Title = dr["Title"].ToString();
                            expense.Description = dr["Description"].ToString();
                            expense.Attachment = dr["Attachment"].ToString();

                            model.Add(expense);
                        }
                    }
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                return model;
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