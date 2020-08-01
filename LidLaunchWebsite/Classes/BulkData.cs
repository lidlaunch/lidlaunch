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
        public int CreateBulkOrder(string name, string email, string phoneNumber, decimal orderTotal, string artworkNotes, string artworkImage, string artworkPosition, List<PaypalItem> items, string paymentCompleteGuid, string paymentGuid)
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
                    sqlComm.Parameters.AddWithValue("@paymentGuid", paymentGuid);                    

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

                    lstBulkOrders = BuildBulkOrdersList(ds);                        

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

        public List<BulkOrder> GetBulkOrdersByBatchId(int batchId)
        {
            var data = new SQLData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrdersByBatchId", data.conn);
                    sqlComm.Parameters.AddWithValue("@batchId", batchId);     

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    lstBulkOrders = BuildBulkOrdersList(ds);

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

        private List<BulkOrder> BuildBulkOrdersList(DataSet ds)
        {
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BulkOrder bulkOrder = new BulkOrder();
                        bulkOrder = BuildBulkOrder(dr, ds);

                        lstBulkOrders.Add(bulkOrder);
                    }
                }
            }

            return lstBulkOrders;

        }
        private BulkOrder BuildBulkOrder(DataRow dr, DataSet ds)
        {
            BulkOrder bulkOrder = new BulkOrder();

            bulkOrder.Id = Convert.ToInt32(dr["Id"].ToString());
            bulkOrder.CustomerName = Convert.ToString(dr["CustomerName"].ToString());
            bulkOrder.CustomerEmail = Convert.ToString(dr["CustomerEmail"].ToString());
            bulkOrder.CustomerPhone = Convert.ToString(dr["CustomerPhone"].ToString());
            bulkOrder.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
            bulkOrder.HatsOrdered = Convert.ToBoolean(dr["HatsOrdered"].ToString());
            bulkOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
            if (dr["PaymentDate"].ToString() != "")
            {
                bulkOrder.PaymentDate = Convert.ToDateTime(dr["PaymentDate"].ToString());
            }
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
            bulkOrder.PaymentCompleteGuid = Convert.ToString(dr["PaymentCompleteGuid"].ToString());
            bulkOrder.PaymentGuid = Convert.ToString(dr["PaymentGuid"].ToString());
            bulkOrder.ProjectedShipDateShort = AddBusinessDays(bulkOrder.PaymentDate, 10).ToString("MM/dd/yyyy");
            bulkOrder.ProjectedShipDateLong = AddBusinessDays(bulkOrder.PaymentDate, 14).ToString("MM/dd/yyyy");
            bulkOrder.BulkOrderBatchId = Convert.ToInt32(dr["BulkOrderBatchId"].ToString());
            bulkOrder.lstItems = new List<BulkOrderItem>();

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
                        item.lstNotes = new List<Note>();
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            foreach (DataRow drNote in ds.Tables[4].Rows)
                            {
                                if (Convert.ToInt32(drNote["BulkOrderItemId"].ToString()) == item.Id)
                                {
                                    Note note = new Note();
                                    note.Id = Convert.ToInt32(drNote["Id"].ToString());
                                    note.Text = Convert.ToString(drNote["Text"].ToString());
                                    note.CustomerAdded = Convert.ToBoolean(drNote["CustomerAdded"].ToString());
                                    note.Attachment = Convert.ToString(drNote["Attachment"].ToString());
                                    note.CreatedDate = Convert.ToDateTime(drNote["CreatedDate"].ToString());
                                    note.CreatedUserId = Convert.ToInt32(drNote["CreatedUserId"].ToString());
                                    item.lstNotes.Add(note);
                                }
                            }
                        }
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
                        design.DigitizedProductionSheet = Convert.ToString(dr3["DigitizedProductionSheet"].ToString());
                        design.EMBFile = Convert.ToString(dr3["EMBFile"].ToString());
                        design.DigitizedPreview = Convert.ToString(dr3["DigitizedPreview"].ToString());
                        design.Width = Convert.ToDecimal(dr3["Width"].ToString());
                        design.Height = Convert.ToDecimal(dr3["Height"].ToString());
                        design.X = Convert.ToDecimal(dr3["X"].ToString());
                        design.Y = Convert.ToDecimal(dr3["Y"].ToString());
                        design.EmbroideredWidth = Convert.ToDecimal(dr3["EmbroideredWidth"].ToString());
                        design.EmbroideredHeight = Convert.ToDecimal(dr3["EmbroideredHeight"].ToString());
                        design.EmbroideredX = Convert.ToDecimal(dr3["EmbroideredX"].ToString());
                        design.EmbroideredY = Convert.ToDecimal(dr3["EmbroideredY"].ToString());
                        design.CustomerApproved = Convert.ToBoolean(dr3["CustomerApproved"].ToString());

                        design.lstNotes = new List<Note>();
                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            foreach (DataRow drNote in ds.Tables[5].Rows)
                            {
                                if (Convert.ToInt32(drNote["DesignId"].ToString()) == design.Id)
                                {
                                    Note note = new Note();
                                    note.Id = Convert.ToInt32(drNote["Id"].ToString());
                                    note.Text = Convert.ToString(drNote["Text"].ToString());
                                    note.CustomerAdded = Convert.ToBoolean(drNote["CustomerAdded"].ToString());
                                    note.Attachment = Convert.ToString(drNote["Attachment"].ToString());
                                    note.CreatedDate = Convert.ToDateTime(drNote["CreatedDate"].ToString());
                                    note.CreatedUserId = Convert.ToInt32(drNote["CreatedUserId"].ToString());
                                    design.lstNotes.Add(note);
                                }
                            }
                        }

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
                        note.CustomerAdded = Convert.ToBoolean(dr4["CustomerAdded"].ToString());
                        note.Attachment = Convert.ToString(dr4["Attachment"].ToString());
                        note.CreatedDate = Convert.ToDateTime(dr4["CreatedDate"].ToString());
                        note.CreatedUserId = Convert.ToInt32(dr4["CreatedUserId"].ToString());
                        bulkOrder.lstNotes.Add(note);
                    }
                }
            }

            return bulkOrder;
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

        public bool ApproveBulkOrderDigitizing(int bulkOrderId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("ApproveBulkOrderDigitizing", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);

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

        public BulkOrder GetBulkOrder(int bulkOrderId, string paymentGuid, string paymentCompleteGuid)
        {
            var data = new SQLData();
            BulkOrder bulkOrder = new BulkOrder();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrder", data.conn);
                    sqlComm.Parameters.AddWithValue("@paymentGuid", paymentGuid);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@paymentCompleteGuid", paymentCompleteGuid);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];

                            bulkOrder = BuildBulkOrder(dr, ds);
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


        public List<OrderBatch> GetBulkOrderBatches()
        {
            var data = new SQLData();
            List<OrderBatch> lstBatches = new List<OrderBatch>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrderBatches", data.conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {
                                OrderBatch batch = new OrderBatch();
                                batch.BatchId = Convert.ToInt32(dr["Id"].ToString());
                                batch.DateBatched = Convert.ToDateTime(dr["DateBatched"].ToString());
                                batch.Status = Convert.ToString(dr["Status"].ToString());
                                lstBatches.Add(batch);
                            }           

                        }

                    }

                            
                }

                return lstBatches;
            }
            catch (Exception ex)
            {
                return lstBatches;
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


        public int CreateNote(int bulkOrderId, int bulkOrderItemId, int designId, int parentBulkOrderId, string text, string attachment, int userId)
        {
            var data = new SQLData();
            var noteId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateNote", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("noteId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@bulkOrderItemId", bulkOrderItemId);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@parentBulkOrderId", parentBulkOrderId);
                    sqlComm.Parameters.AddWithValue("@text", text);
                    sqlComm.Parameters.AddWithValue("@attachment", attachment);
                    sqlComm.Parameters.AddWithValue("@userId", userId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    noteId = (int)returnParameter.Value;                    

                }

                return noteId;
            }
            catch (Exception ex)
            {
                return noteId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public int CreateBulkOrderBatch()
        {
            var data = new SQLData();
            var batchId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBulkOrderBatch", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("batchId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;                    

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    batchId = (int)returnParameter.Value;

                }

                return batchId;
            }
            catch (Exception ex)
            {
                return batchId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        private DateTime AddBusinessDays(DateTime startDate, int businessDays)
        {
            int direction = Math.Sign(businessDays);
            if (direction == 1)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = startDate.AddDays(2);
                    businessDays = businessDays - 1;
                }
                else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    startDate = startDate.AddDays(1);
                    businessDays = businessDays - 1;
                }
            }
            else
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = startDate.AddDays(-1);
                    businessDays = businessDays + 1;
                }
                else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    startDate = startDate.AddDays(-2);
                    businessDays = businessDays + 1;
                }
            }

            int initialDayOfWeek = (int)startDate.DayOfWeek;

            int weeksBase = Math.Abs(businessDays / 5);
            int addDays = Math.Abs(businessDays % 5);

            if ((direction == 1 && addDays + initialDayOfWeek > 5) ||
                 (direction == -1 && addDays >= initialDayOfWeek))
            {
                addDays += 2;
            }

            int totalDays = (weeksBase * 7) + addDays;
            return startDate.AddDays(totalDays * direction);
        }
        


    }

    
}
