using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class BulkData
    {
        public int CreateBulkOrder(string name, string email, string phoneNumber, decimal orderTotal, string artworkNotes, string artworkImage, string artworkPosition, List<PaypalItem> items, string paymentCompleteGuid)
        {
            var data = new SQLData();
            var orderId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBulkOrder", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("bulkOrderId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@name", name);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    sqlComm.Parameters.AddWithValue("@orderTotal", orderTotal);
                    sqlComm.Parameters.AddWithValue("@artworkNotes", artworkNotes);
                    sqlComm.Parameters.AddWithValue("@artworkImage", artworkImage);
                    sqlComm.Parameters.AddWithValue("@artworkPosition", artworkPosition);
                    sqlComm.Parameters.AddWithValue("@paymentCompleteGuid", paymentCompleteGuid);
                    sqlComm.Parameters.AddWithValue("@paymentGuid", Guid.NewGuid().ToString());                    

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    orderId = (int)returnParameter.Value;
                    //updload the individual line items
                    if(orderId > 0)
                    {
                        foreach(PaypalItem item in items)
                        {
                            SqlCommand sqlComm2 = new SqlCommand("CreateBulkOrderItem", data.conn);
                            sqlComm2.Parameters.AddWithValue("@bulkOrderId", orderId);
                            sqlComm2.Parameters.AddWithValue("@name", item.name);
                            sqlComm2.Parameters.AddWithValue("@quantity", item.quantity);
                            sqlComm2.Parameters.AddWithValue("@cost", item.price);
                            sqlComm2.CommandType = CommandType.StoredProcedure;
                            sqlComm2.ExecuteNonQuery();
                        }

                    }

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

        public List<BulkOrder> GetBulkOrderData()
        {
            var data = new SQLData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrders", data.conn);
                    //sqlComm.Parameters.AddWithValue("@startDate", startDate);
                    //sqlComm.Parameters.AddWithValue("@endDate", endDate);             

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                BulkOrder bulkOrder = new BulkOrder();


                                bulkOrder.Id = Convert.ToInt32(dr["Id"].ToString());
                                bulkOrder.CustomerName = Convert.ToString(dr["CustomerName"].ToString());
                                bulkOrder.CustomerEmail = Convert.ToString(dr["CustomerEmail"].ToString());
                                bulkOrder.CustomerPhone = Convert.ToString(dr["CustomerPhone"].ToString());
                                bulkOrder.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
                                bulkOrder.HatsOrdered = Convert.ToBoolean(dr["HatsOrdered"].ToString());
                                bulkOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                                if(dr["ShipDate"].ToString() != "")
                                {
                                    bulkOrder.ShipDate = Convert.ToDateTime(dr["ShipDate"].ToString());
                                }                                
                                bulkOrder.TrackingNumber = Convert.ToString(dr["TrackingNumber"].ToString());
                                bulkOrder.OrderComplete = Convert.ToBoolean(dr["OrderComplete"].ToString());
                                bulkOrder.OrderNotes = Convert.ToString(dr["OrderNotes"].ToString());
                                bulkOrder.ArtworkImage = Convert.ToString(dr["ArtworkImage"].ToString());
                                bulkOrder.HatsOrderedSource = Convert.ToString(dr["HatsOrderedSource"].ToString());
                                bulkOrder.HatsOrderedTracking = Convert.ToString(dr["HatsOrderedTracking"].ToString());
                                bulkOrder.OrderCanceled = Convert.ToBoolean(dr["OrderCanceled"].ToString());
                                bulkOrder.ArtworkPosition = Convert.ToString(dr["ArtworkPosition"].ToString());
                                bulkOrder.OrderPaid = Convert.ToBoolean(dr["OrderPaid"].ToString());
                                bulkOrder.PaymentCompleteGuid = Convert.ToString(dr["PaymentCompleteGuid"].ToString());
                                bulkOrder.PaymentGuid = Convert.ToString(dr["PaymentGuid"].ToString());
                                bulkOrder.lstItems = new List<BulkOrderItem>();

                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    foreach (DataRow dr2 in ds.Tables[1].Rows)
                                    {
                                        if(Convert.ToInt32(dr2["BulkOrderId"].ToString()) == bulkOrder.Id)
                                        {
                                            BulkOrderItem item = new BulkOrderItem();
                                            item.BulkOrderId = bulkOrder.Id;
                                            item.Id = Convert.ToInt32(dr2["Id"].ToString());
                                            item.ItemName = Convert.ToString(dr2["ItemName"].ToString());
                                            item.ItemQuantity = Convert.ToInt32(dr2["ItemQuantity"].ToString());
                                            item.ItemCost = Convert.ToDecimal(dr2["ItemCost"].ToString());
                                            bulkOrder.lstItems.Add(item);
                                        }
                                    }
                                }

                                bulkOrder.lstDesigns = new List<Design>();
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    foreach (DataRow dr3 in ds.Tables[2].Rows)
                                    {
                                        if (Convert.ToInt32(dr3["BulkOrderId"].ToString()) == bulkOrder.Id)
                                        {
                                            Design design = new Design();
                                            design.Id = Convert.ToInt32(dr3["Id"].ToString());
                                            design.ArtSource = Convert.ToString(dr3["ArtSource"].ToString());
                                            design.PreviewImage = Convert.ToString(dr3["PreviewImage"].ToString());
                                            design.DigitizedFile = Convert.ToString(dr3["DigitizedFile"].ToString());
                                            design.DigitizedInfoImage = Convert.ToString(dr3["DigitizedInfoImage"].ToString());
                                            design.DigitizedPreview = Convert.ToString(dr3["DigitizedPreview"].ToString());
                                            design.Width = Convert.ToDecimal(dr3["Width"].ToString());
                                            design.Height = Convert.ToDecimal(dr3["Height"].ToString());
                                            design.X = Convert.ToDecimal(dr3["X"].ToString());
                                            design.Y = Convert.ToDecimal(dr3["Y"].ToString());
                                            design.EmbroideredWidth = Convert.ToDecimal(dr3["EmbroideredWidth"].ToString());
                                            design.EmbroideredHeight = Convert.ToDecimal(dr3["EmbroideredHeight"].ToString());
                                            design.EmbroideredX = Convert.ToDecimal(dr3["EmbroideredX"].ToString());
                                            design.EmbroideredY = Convert.ToDecimal(dr3["EmbroideredY"].ToString());
                                            bulkOrder.lstDesigns.Add(design);
                                        }
                                    }
                                }

                                bulkOrder.lstNotes = new List<Note>();
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    foreach (DataRow dr4 in ds.Tables[3].Rows)
                                    {
                                        if (Convert.ToInt32(dr4["BulkOrderId"].ToString()) == bulkOrder.Id)
                                        {
                                            Note note = new Note();
                                            note.Id = Convert.ToInt32(dr4["Id"].ToString());
                                            note.Text = Convert.ToString(dr4["Text"].ToString());
                                            note.UserAdded = Convert.ToBoolean(dr4["UserAdded"].ToString());
                                            note.Attachment = Convert.ToString(dr4["Attachment"].ToString());
                                            note.CreatedDate = Convert.ToDateTime(dr4["CreatedDate"].ToString());
                                            note.CreatedUserId = Convert.ToInt32(dr4["CreatedUserId"].ToString());
                                            bulkOrder.lstNotes.Add(note);
                                        }
                                    }
                                }



                                lstBulkOrders.Add(bulkOrder);
                            }                                  
                        }
                    }                        

                }

                return lstBulkOrders;
            }
            catch (Exception ex)
            {
                return lstBulkOrders;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public bool UpdateBulkOrderPaid(int bulkOrderId, bool hasPaid)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderPaid", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@hasPaid", hasPaid);

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

        public bool UpdateBulkOrderPaidByPaymentCompleteGuid(string paymentCompleteGuid)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderPaidByPaymentCompleteGuid", data.conn);
                    sqlComm.Parameters.AddWithValue("@paymentCompleteGuid", paymentCompleteGuid);

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

        

        public BulkOrder GetBulkOrderByPaymentGuid(string paymentGuid)
        {
            var data = new SQLData();
            BulkOrder bulkOrder = new BulkOrder();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrderByPaymentGuid", data.conn);
                    sqlComm.Parameters.AddWithValue("@paymentGuid", paymentGuid);           

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];

                            bulkOrder.Id = Convert.ToInt32(dr["Id"].ToString());
                            bulkOrder.CustomerName = Convert.ToString(dr["CustomerName"].ToString());
                            bulkOrder.CustomerEmail = Convert.ToString(dr["CustomerEmail"].ToString());
                            bulkOrder.CustomerPhone = Convert.ToString(dr["CustomerPhone"].ToString());
                            bulkOrder.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
                            bulkOrder.HatsOrdered = Convert.ToBoolean(dr["HatsOrdered"].ToString());
                            bulkOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                            if (dr["ShipDate"].ToString() != "")
                            {
                                bulkOrder.ShipDate = Convert.ToDateTime(dr["ShipDate"].ToString());
                            }
                            bulkOrder.TrackingNumber = Convert.ToString(dr["TrackingNumber"].ToString());
                            bulkOrder.OrderComplete = Convert.ToBoolean(dr["OrderComplete"].ToString());
                            bulkOrder.OrderNotes = Convert.ToString(dr["OrderNotes"].ToString());
                            bulkOrder.ArtworkImage = Convert.ToString(dr["ArtworkImage"].ToString());
                            bulkOrder.HatsOrderedSource = Convert.ToString(dr["HatsOrderedSource"].ToString());
                            bulkOrder.HatsOrderedTracking = Convert.ToString(dr["HatsOrderedTracking"].ToString());
                            bulkOrder.OrderCanceled = Convert.ToBoolean(dr["OrderCanceled"].ToString());
                            bulkOrder.ArtworkPosition = Convert.ToString(dr["ArtworkPosition"].ToString());
                            bulkOrder.OrderPaid = Convert.ToBoolean(dr["OrderPaid"].ToString());
                            bulkOrder.lstItems = new List<BulkOrderItem>();
                            bulkOrder.PaymentCompleteGuid = Convert.ToString(dr["PaymentCompleteGuid"].ToString());
                            bulkOrder.PaymentGuid = Convert.ToString(dr["PaymentGuid"].ToString());

                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow dr2 in ds.Tables[1].Rows)
                                {
                                    if (Convert.ToInt32(dr2["BulkOrderId"].ToString()) == bulkOrder.Id)
                                    {
                                        BulkOrderItem item = new BulkOrderItem();
                                        item.BulkOrderId = bulkOrder.Id;
                                        item.Id = Convert.ToInt32(dr2["Id"].ToString());
                                        item.ItemName = Convert.ToString(dr2["ItemName"].ToString());
                                        item.ItemQuantity = Convert.ToInt32(dr2["ItemQuantity"].ToString());
                                        item.ItemCost = Convert.ToDecimal(dr2["ItemCost"].ToString());
                                        bulkOrder.lstItems.Add(item);
                                    }
                                }
                            }
                            
                        }
                    }

                }

                return bulkOrder;
            }
            catch (Exception ex)
            {
                return bulkOrder;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public bool CreateBulkOrderDesign(int bulkOrderId, int designId)
        {
            var data = new SQLData();

            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBulkOrderDesign", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    //updload the individual line items

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

        public bool DeleteBulkOrderDesign(int bulkOrderId, int designId)
        {
            var data = new SQLData();

            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("DeleteBulkOrderDesign", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    //updload the individual line items

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






    }
}