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
        public int CreateBulkOrder(string name, string email, string phoneNumber, decimal orderTotal, string artworkNotes, string artworkImage, string artworkPosition, List<PaypalItem> items, string paymentCompleteGuid, string paymentGuid, decimal shippingCost, string shipToName, string shipToAddress, string shipToCity, string shipToState, string shipToZip, string shipToPhone, string billToName, string billToAddress, string billToCity, string billToState, string billToZip, string billToPhone, bool backStitching, bool leftSideStitching, bool rightSideStitching, string backStitchingComment, string leftSideStitchingComment, string rightSideStitchingComment)
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
                    sqlComm.Parameters.AddWithValue("@shipToName", shipToName);
                    sqlComm.Parameters.AddWithValue("@shipToAddress", shipToAddress);
                    sqlComm.Parameters.AddWithValue("@shipToCity", shipToCity);
                    sqlComm.Parameters.AddWithValue("@shipToState", shipToState);
                    sqlComm.Parameters.AddWithValue("@shipToZip", shipToZip);
                    sqlComm.Parameters.AddWithValue("@shipToPhone", shipToPhone);
                    sqlComm.Parameters.AddWithValue("@billToName", billToName);
                    sqlComm.Parameters.AddWithValue("@billToAddress", billToAddress);
                    sqlComm.Parameters.AddWithValue("@billToCity", billToCity);
                    sqlComm.Parameters.AddWithValue("@billToState", billToState);
                    sqlComm.Parameters.AddWithValue("@billToZip", billToZip);
                    sqlComm.Parameters.AddWithValue("@billToPhone", billToPhone);
                    sqlComm.Parameters.AddWithValue("@backStitching", backStitching);
                    sqlComm.Parameters.AddWithValue("@leftStitching", leftSideStitching);
                    sqlComm.Parameters.AddWithValue("@rightStitching", rightSideStitching);
                    sqlComm.Parameters.AddWithValue("@backStitchingComment", backStitchingComment);
                    sqlComm.Parameters.AddWithValue("@leftStitchingComment", leftSideStitchingComment);
                    sqlComm.Parameters.AddWithValue("@rightStitchingComment", rightSideStitchingComment);

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
                        //add the shipping cost item
                        SqlCommand sqlComm3 = new SqlCommand("CreateBulkOrderItem", data.conn);
                        sqlComm3.Parameters.AddWithValue("@bulkOrderId", orderId);
                        sqlComm3.Parameters.AddWithValue("@name", "Shipping");
                        sqlComm3.Parameters.AddWithValue("@quantity", 1);
                        sqlComm3.Parameters.AddWithValue("@cost", shippingCost);
                        sqlComm3.CommandType = CommandType.StoredProcedure;
                        sqlComm3.ExecuteNonQuery();

                    }

                }

                return orderId;
            }
            catch (Exception ex)
            {
                Logger.Log("Error Creating Bulk Order: " + ex.Message.ToString());
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

        public List<BulkOrder> GetBulkOrderData(string filter)
        {
            var data = new SQLData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrders", data.conn);

                    //sqlComm.Parameters.AddWithValue("@endDate", endDate);             

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    lstBulkOrders = BuildBulkOrdersList(ds);

                    if (filter == "" || filter == null)
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.OrderPaid).ToList();
                    }
                    else if (filter == "rework")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.lstItems.Any(i => i.BulkRework.Status == "In Progress") && b.OrderPaid).ToList();                        
                    }
                    else if (filter == "45days")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.PaymentDate <= DateTime.Now && b.PaymentDate >= DateTime.Now.AddDays(-45) && b.OrderPaid).ToList();
                    }
                    else if (filter == "pending")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => !b.OrderComplete && b.OrderPaid).ToList();
                    }
                    else if (filter == "unpaid")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => !b.OrderPaid).ToList();
                    }
                    else if (filter == "revision")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.lstDesigns.Any(d => d.Revision) && !b.ReadyForProduction).ToList();
                    }
                    else if (filter == "noart")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.ArtworkImage == "").ToList();
                    }
                    else if (filter == "review")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.AdminReview && !b.DesignerReview && b.OrderPaid).ToList();
                    }
                    else if (filter == "designerReview")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.DesignerReview).ToList();
                    }
                    else if (filter == "notReady")
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => !b.ReadyForProduction).ToList();
                    }
                    else
                    {
                        lstBulkOrders = lstBulkOrders.Where(b => b.Id == Convert.ToInt32(filter.Replace("BO-", ""))).ToList();
                    }
                }

                return lstBulkOrders;
            }
            catch (Exception ex)
            {
                Logger.Log("Error Building Bulk Order Data For View Bulk Order Screen: " + ex.Message.ToString());
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

        public decimal GetBlankCost(int bulkOrderBatchId)
        {
            var data = new SQLData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            decimal totalBlankCost = 0.00M;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetHatStyleTotalsByBatch", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderBatchId", bulkOrderBatchId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);


                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        foreach(DataRow dr in ds.Tables[0].Rows)
                        {
                            var itemName = Convert.ToString(dr["ItemName"].ToString());
                            var itemQuantity = Convert.ToInt32(dr["ItemQuantity"].ToString());

                            totalBlankCost += getItemBlankCostByItemName(itemName) * itemQuantity;
                        }
                    }
                    

                }

                return totalBlankCost;
            }
            catch (Exception ex)
            {
                return totalBlankCost;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
            
        }

        public decimal getItemBlankCostByItemName(string itemName)
        {
            decimal itemCost = 0.00M;

            if(itemName.Contains("FlexFit 110 Trucker Snapback"))
            {
                return 4.65M;
            }
            if (itemName.Contains("FlexFit 6277 - MULTICAM"))
            {
                return 6.49M;
            }
            if (itemName.Contains("FlexFit 6277"))
            {
                return 5.00M;
            }
            if (itemName.Contains("FlexFit Flat Bill Fitted"))
            {
                return 5.32M;
            }
            if (itemName.Contains("FlexFit Premium 210"))
            {
                return 6.02M;
            }
            if (itemName.Contains("FlexFit Trucker"))
            {
                return 4.24M;
            }
            if (itemName.Contains("Richardson 112"))
            {
                return 3.70M;
            }
            if (itemName.Contains("Yupoong 6006 Flat Bill Trucker Snapback - MULTICAM"))
            {
                return 4.88M;
            }
            if (itemName.Contains("Yupoong 6006 Flat Bill Trucker Snapback"))
            {
                return 3.15M;
            }
            if (itemName.Contains("Yupoong 6606 Trucker Snapback - MULTICAM"))
            {
                return 4.88M;
            }
            if (itemName.Contains("Yupoong 6606 Trucker Snapback"))
            {
                return 3.15M;
            }            
            if (itemName.Contains("Yupoong Cuffed Beanie"))
            {
                return 1.84M;
            }
            if (itemName.Contains("Yupoong Short Beanie"))
            {
                return 1.85M;
            }
            if (itemName.Contains("Yupoong Flat Bill Snapback - MULTICAM"))
            {
                return 5.45M;
            }
            if (itemName.Contains("Yupoong Flat Bill Snapback"))
            {
                return 4.15M;
            }
            if (itemName.Contains("Yupoong Dad Cap - MULTICAM"))
            {
                return 5.85M;
            }
            if (itemName.Contains("Yupoong Dad Cap"))
            {
                return 4.15M;
            }

            Logger.Log(itemName);

            return 5.00M;

        }

        public List<BulkOrder> GetBulkOrdersByBatchId(int batchId)
        {
            var data = new SQLData();
            var data2 = new SQLData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            List<BulkOrder> lstReworkOrders = new List<BulkOrder>();
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

                DataSet ds2 = new DataSet();
                using (data2.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkReworkOrdersByBatchId", data2.conn);
                    sqlComm.Parameters.AddWithValue("@batchId", batchId);

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds2);

                    lstReworkOrders = BuildBulkOrdersList(ds2);
                }

                foreach(BulkOrder bulkOrder in lstReworkOrders)
                {
                    bulkOrder.HasRework = true;
                }

                lstBulkOrders.AddRange(lstReworkOrders);

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
                if (data2.conn != null)
                {
                    data2.conn.Close();
                }
            }
        }

        private List<BulkOrder> BuildBulkOrdersList(DataSet ds)
        {
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            try
            {                
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
            catch (Exception ex)
            {
                Logger.Log("ERROR BUILDING BULK ORDER LIST: " + ex.InnerException.Message);
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
            bulkOrder.OrderRefunded = Convert.ToBoolean(dr["OrderRefunded"].ToString());
            bulkOrder.PaymentCompleteGuid = Convert.ToString(dr["PaymentCompleteGuid"].ToString());
            bulkOrder.PaymentGuid = Convert.ToString(dr["PaymentGuid"].ToString());
            bulkOrder.ProjectedShipDateShort = AddBusinessDays(bulkOrder.PaymentDate, 10).ToString("MM/dd/yyyy");
            bulkOrder.ProjectedShipDateLong = AddBusinessDays(bulkOrder.PaymentDate, 14).ToString("MM/dd/yyyy");
            bulkOrder.BulkOrderBatchId = Convert.ToInt32(dr["BulkOrderBatchId"].ToString());
            bulkOrder.ShipToName = Convert.ToString(dr["ShipToName"].ToString());
            bulkOrder.ShipToAddress = Convert.ToString(dr["ShipToAddress"].ToString());
            bulkOrder.ShipToCity = Convert.ToString(dr["ShipToCity"].ToString());
            bulkOrder.ShipToState = Convert.ToString(dr["ShipToState"].ToString());
            bulkOrder.ShipToZip = Convert.ToString(dr["ShipToZip"].ToString());
            bulkOrder.ShipToPhone = Convert.ToString(dr["ShipToPhone"].ToString());
            bulkOrder.BillToName = Convert.ToString(dr["BillToName"].ToString());
            bulkOrder.BillToAddress = Convert.ToString(dr["BillToAddress"].ToString());
            bulkOrder.BillToCity = Convert.ToString(dr["BillToCity"].ToString());
            bulkOrder.BillToState = Convert.ToString(dr["BillToState"].ToString());
            bulkOrder.BillToZip = Convert.ToString(dr["BillToZip"].ToString());
            bulkOrder.BillToPhone = Convert.ToString(dr["BillToPhone"].ToString());            
            bulkOrder.ReadyForProduction = Convert.ToBoolean(dr["ReadyForProduction"].ToString());
            bulkOrder.BackStitching = Convert.ToBoolean(dr["BackStitching"].ToString());
            bulkOrder.LeftStitching = Convert.ToBoolean(dr["LeftStitching"].ToString());
            bulkOrder.RightStitching = Convert.ToBoolean(dr["RightStitching"].ToString());
            bulkOrder.BackStitchingComment = dr["BackStitchingComment"].ToString();
            bulkOrder.LeftStitchingComment = dr["LeftStitchingComment"].ToString();
            bulkOrder.RightStitchingComment = dr["RightStitchingComment"].ToString();
            bulkOrder.ArtworkEmailSent = Convert.ToBoolean(dr["ArtworkEmailSent"].ToString());
            bulkOrder.ColorConfirmationEmailSent = Convert.ToBoolean(dr["ColorConfirmationEmailSent"].ToString());
            bulkOrder.ReleaseToDigitizer = Convert.ToBoolean(dr["ReleaseToDigitizer"].ToString());
            bulkOrder.AdminReview = Convert.ToBoolean(dr["AdminReview"].ToString());
            bulkOrder.DesignerReview = Convert.ToBoolean(dr["DesignerReview"].ToString());
            bulkOrder.ReminderApprovalEmailSent = Convert.ToInt32(dr["ReminderApprovalEmailSent"].ToString());            
            bulkOrder.AdminReviewComment = Convert.ToString(dr["AdminReviewComment"].ToString());
            bulkOrder.BarcodeImage = "BO-" + bulkOrder.Id.ToString() + ".jpg";
            bulkOrder.lstItems = new List<BulkOrderItem>();

            if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow[] drsBulkOrderItems = ds.Tables[1].Select("BulkOrderId = " + bulkOrder.Id.ToString());
                foreach (DataRow dr2 in drsBulkOrderItems)
                {       
                    BulkOrderItem item = new BulkOrderItem();
                    item.BulkOrderId = bulkOrder.Id;
                    item.Id = Convert.ToInt32(dr2["Id"].ToString());
                    item.ItemName = Convert.ToString(dr2["ItemName"].ToString());
                    item.ItemQuantity = Convert.ToInt32(dr2["ItemQuantity"].ToString());
                    item.ItemCost = Convert.ToDecimal(dr2["ItemCost"].ToString());
                    item.MasterItemId = Convert.ToInt32(dr2["MasterItemId"].ToString());
                    if (item.MasterItemId > 0)
                    {
                        item.ItemThumbnail = Convert.ToString(dr2["ItemThumbnail"].ToString());
                    }
                    else
                    {
                        item.ItemThumbnail = "miscIcon.png";
                    }

                    BulkRework bulkRework = new BulkRework();
                    if (Convert.ToInt32(dr2["BulkReworkId"]) > 0)
                    {                            
                        item.BulkRework = GetBulkReworkById(Convert.ToInt32(dr2["BulkReworkId"]));
                    } 
                    else
                    {
                        item.BulkRework = bulkRework;
                    }

                    if(item.ItemName == "Shipping")
                    {
                        bulkOrder.ShippingCost = item.ItemCost;
                    }
                    item.lstNotes = new List<Note>();
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        DataRow[] drsBulkorderItemNotes = ds.Tables[4].Select("BulkOrderItemId = " + item.Id.ToString());
                        foreach (DataRow drNote in drsBulkorderItemNotes)
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
                    bulkOrder.lstItems.Add(item);
                    
                }
            }

            bulkOrder.OrderSubTotal = bulkOrder.OrderTotal - bulkOrder.ShippingCost;

            bulkOrder.lstDesigns = new List<Design>();
            if (ds.Tables[2].Rows.Count > 0)
            {
                DataRow[] drsDesigns = ds.Tables[2].Select("BulkOrderId = " + bulkOrder.Id.ToString());
                foreach (DataRow dr3 in drsDesigns)
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
                    design.InternallyApproved = Convert.ToBoolean(dr3["InternallyApproved"].ToString());
                    design.Revision = Convert.ToBoolean(dr3["Revision"].ToString());
                    design.RevisionStatus = Convert.ToString(dr3["RevisionStatus"].ToString());
                    if(design.Revision)
                    {
                        if (design.RevisionStatus == "")
                        {
                            design.RevisionStatus = "1:Pending";
                        }
                    }
                    design.Name = Convert.ToString(dr3["Name"].ToString());

                    design.lstNotes = new List<Note>();
                    design.lstRevisionNotes = new List<Note>();                    
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        DataRow[] drsDesignNotes = ds.Tables[5].Select("DesignId = " + design.Id.ToString());
                        foreach (DataRow drNote in drsDesignNotes)
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
                                if (Convert.ToBoolean(drNote["CustomerAdded"].ToString()))
                                {
                                    design.lstRevisionNotes.Add(note);
                                } else
                                {
                                    design.lstNotes.Add(note);
                                }                                    
                            }
                        }
                    }
                    design.lstCombinedNotes = new List<Note>();
                    design.lstCombinedNotes.AddRange(design.lstNotes);
                    design.lstCombinedNotes.AddRange(design.lstRevisionNotes);
                    design.lstCombinedNotes = design.lstCombinedNotes.OrderByDescending(dn => dn.CreatedDate).ToList();

                    bulkOrder.lstDesigns.Add(design);
                }
                
            }

            bulkOrder.lstNotes = new List<Note>();
            if (ds.Tables[3].Rows.Count > 0)
            {
                DataRow[] drsBulkOrderNotes = ds.Tables[3].Select("BulkOrderId = " + bulkOrder.Id.ToString());
                foreach (DataRow dr4 in drsBulkOrderNotes)
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

            bulkOrder.lstBulkOrderLog = new List<BulkOrderLog>();
            if(ds.Tables[6].Rows.Count > 0)
            {               
                foreach (DataRow dr7 in ds.Tables[6].Rows)
                {
                    BulkOrderLog log = new BulkOrderLog();
                    log.Id = Convert.ToInt32(dr7["Id"].ToString());
                    log.BulkOrderId = Convert.ToInt32(dr7["BulkOrderId"].ToString());
                    log.UserId = Convert.ToInt32(dr7["UserId"].ToString());
                    log.LogEntry = Convert.ToString(dr7["LogEntry"].ToString());
                    log.Date = Convert.ToDateTime(dr7["Date"].ToString());

                    bulkOrder.lstBulkOrderLog.Add(log);
                }
            }
            bulkOrder.lstBulkOrderLog = bulkOrder.lstBulkOrderLog.OrderByDescending(bl => bl.Date).ToList();

            return bulkOrder;
        }

        public bool AddBulkOrderItem(int bulkOrderId, string itemName, int itemQuantity, decimal itemCost)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBulkOrderItem", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@name", itemName);
                    sqlComm.Parameters.AddWithValue("@quantity", itemQuantity);
                    sqlComm.Parameters.AddWithValue("@cost", itemCost);

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

        public bool UpdateArtworkEmailSent(int bulkOrderId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateArtworkEmailSent", data.conn);
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

        public bool UpdateColorConfirmationEmailSent(int bulkOrderId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateColorConfirmationEmailSent", data.conn);
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

        

        public bool ReleaseToDigitizer (int bulkOrderId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("ReleaseToDigitizer", data.conn);
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

        public bool UpdateBulkOrderItem(int bulkOrderId, int itemId, string itemName, int itemQuantity, decimal itemCost)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderItem", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@itemId", itemId);
                    sqlComm.Parameters.AddWithValue("@name", itemName);
                    sqlComm.Parameters.AddWithValue("@quantity", itemQuantity);
                    sqlComm.Parameters.AddWithValue("@cost", itemCost);

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

        public bool UpdateBulkOrderShipTo(int bulkOrderId, string shipToAddress, string shipToCity, string shipToState, string shipToZip)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderShipTo", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@shiptoAddress", shipToAddress);
                    sqlComm.Parameters.AddWithValue("@shipToCity", shipToCity);
                    sqlComm.Parameters.AddWithValue("@shipToState", shipToState);
                    sqlComm.Parameters.AddWithValue("@shipToZip", shipToZip);

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

        public bool UpdateBulkOrder(int bulkOrderId, string customerEmail, string artworkPosition, decimal orderTotal)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrder", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@customerEmail", customerEmail);
                    sqlComm.Parameters.AddWithValue("@artworkPosition", artworkPosition);
                    sqlComm.Parameters.AddWithValue("@orderTotal", orderTotal);

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

        public bool UpdateBulkOrderArtwork(int bulkOrderId, string fileName)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderArtwork", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@fileName", fileName);

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

        public bool UpdateBulkOrderRefunded(int bulkOrderId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderRefunded", data.conn);
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

        public bool UpdateBulkOrderReminderApprovalSent(int bulkOrderId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderReminderApprovalSent", data.conn);
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

        

        public bool UpdateBulkOrderDesign(int bulkOrderId, int designId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderDesign", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

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

        public bool UpdateBulkOrderBatchId(int bulkOrderId, int batchId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderBatchId", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@batchId", batchId);

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

        public bool UpdateBulkOrderSetOrderAsShipped(int bulkOrderId, string trackingNumber)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderSetOrderAsShipped", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@trackingNumber", trackingNumber);

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

        public bool ApproveBulkOrderDigitizing(int designId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("ApproveBulkOrderDigitizing", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

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

        public bool InternallyApproveBulkOrder(int bulkOrderId, bool approve)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("InternallyApproveBulkOrder", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@approve", approve);

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

        public bool InternallyApproveBulkOrderDigitizing(int designId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("InternallyApproveBulkOrderDigitizing", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);

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
        

        public bool AddDigitizingRevision(int designId, string revisionComment, bool customerAdded, string revisionStatus)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("AddDigitizingRevision", data.conn);
                    sqlComm.Parameters.AddWithValue("@designId", designId);
                    sqlComm.Parameters.AddWithValue("@revisionComment", revisionComment);
                    sqlComm.Parameters.AddWithValue("@customerAdded", customerAdded);
                    sqlComm.Parameters.AddWithValue("@revisionStatus", revisionStatus);

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

        public BulkRework GetBulkReworkById(int reworkId)
        {
            var data = new SQLData();
            BulkRework bulkRework = new BulkRework();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkReworkById", data.conn);
                    sqlComm.Parameters.AddWithValue("@reworkId", reworkId);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            bulkRework.Id = Convert.ToInt32(dr["Id"].ToString());
                            bulkRework.BulkOrderItemId = Convert.ToInt32(dr["BulkOrderItemId"].ToString());
                            bulkRework.BulkOrderBatchId = Convert.ToInt32(dr["BulkOrderBatchId"].ToString());
                            bulkRework.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            bulkRework.Status = Convert.ToString(dr["Status"].ToString());
                            bulkRework.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            bulkRework.Note = Convert.ToString(dr["Note"].ToString());
                            bulkRework.IsMissingBlank = Convert.ToBoolean(dr["MissingBlank"].ToString());
                            bulkRework.MissingBlankName = Convert.ToString(dr["MissingBlankName"].ToString());

                        }

                    }


                }

                return bulkRework;
            }
            catch (Exception ex)
            {
                return bulkRework;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
        public List<BulkOrder> SearchBulkOrders(string email, string firstName, string lastName, string zipCode)
        {
            var data = new SQLData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("SearchBulkOrders", data.conn);
                    sqlComm.Parameters.AddWithValue("@email", email);
                    sqlComm.Parameters.AddWithValue("@firstName", firstName);
                    sqlComm.Parameters.AddWithValue("@lastName", lastName);
                    sqlComm.Parameters.AddWithValue("@zipCode", zipCode);
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
                                bulkOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                                bulkOrder.OrderComplete = Convert.ToBoolean(dr["OrderComplete"].ToString());
                                bulkOrder.PaymentGuid = Convert.ToString(dr["PaymentGuid"].ToString());
                                bulkOrder.OrderPaid = Convert.ToBoolean(dr["OrderPaid"].ToString());
                                bulkOrder.OrderRefunded = Convert.ToBoolean(dr["OrderRefunded"].ToString());
                                lstBulkOrders.Add(bulkOrder);
                            }                            
                        }
                    }

                }

                return lstBulkOrders;
            }
            catch (Exception ex)
            {
                Logger.Log("Error Building Bulk Order List: " + ex.Message.ToString());
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
        public List<Design> GetBulkDesigns(string email)
        {
            var data = new SQLData();
            List<Design> lstDesigns = new List<Design>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkDesigns", data.conn);
                    sqlComm.Parameters.AddWithValue("@email", email);
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
                                Design design = new Design();
                                design.Id = Convert.ToInt32(dr["Id"].ToString());
                                design.ArtSource = Convert.ToString(dr["ArtSource"].ToString());
                                design.PreviewImage = Convert.ToString(dr["PreviewImage"].ToString());
                                design.DigitizedFile = Convert.ToString(dr["DigitizedFile"].ToString());
                                design.DigitizedProductionSheet = Convert.ToString(dr["DigitizedProductionSheet"].ToString());
                                design.EMBFile = Convert.ToString(dr["EMBFile"].ToString());
                                design.DigitizedPreview = Convert.ToString(dr["DigitizedPreview"].ToString());
                                design.Width = Convert.ToDecimal(dr["Width"].ToString());
                                design.Height = Convert.ToDecimal(dr["Height"].ToString());
                                design.X = Convert.ToDecimal(dr["X"].ToString());
                                design.Y = Convert.ToDecimal(dr["Y"].ToString());
                                design.EmbroideredWidth = Convert.ToDecimal(dr["EmbroideredWidth"].ToString());
                                design.EmbroideredHeight = Convert.ToDecimal(dr["EmbroideredHeight"].ToString());
                                design.EmbroideredX = Convert.ToDecimal(dr["EmbroideredX"].ToString());
                                design.EmbroideredY = Convert.ToDecimal(dr["EmbroideredY"].ToString());
                                design.CustomerApproved = Convert.ToBoolean(dr["CustomerApproved"].ToString());
                                design.Revision = Convert.ToBoolean(dr["Revision"].ToString());
                                lstDesigns.Add(design);
                            }

                        }

                    }


                }

                return lstDesigns;
            }
            catch (Exception ex)
            {
                return lstDesigns;
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
                Logger.Log("Error Building Bulk Order Object: " + ex.Message.ToString());
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
                                batch.InternalStockUpdated = Convert.ToBoolean(dr["InternalStockUpdated"].ToString());
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

        public OrderBatch GetBulkOrderBatch(int batchId)
        {
            var data = new SQLData();
            OrderBatch batch = new OrderBatch();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrderBatch", data.conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@batchId", @batchId);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                                
                            batch.BatchId = Convert.ToInt32(dr["Id"].ToString());
                            batch.DateBatched = Convert.ToDateTime(dr["DateBatched"].ToString());
                            batch.Status = Convert.ToString(dr["Status"].ToString());
                            batch.InternalStockUpdated = Convert.ToBoolean(dr["InternalStockUpdated"].ToString());                               

                        }

                    }


                }

                return batch;
            }
            catch (Exception ex)
            {
                return batch;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public bool UpdateBulkOrderBatchInterntalStockUpdated(int batchId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderBatchInterntalStockUpdated", data.conn);
                    sqlComm.Parameters.AddWithValue("@batchId ", batchId);

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


        public int CreateNote(int bulkOrderId, int bulkOrderItemId, int designId, int parentBulkOrderId, string text, string attachment, int userId, bool customerAdded)
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
                    sqlComm.Parameters.AddWithValue("@userCreated", customerAdded);

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

        public DateTime AddBusinessDays(DateTime startDate, int businessDays)
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

        public int UpdateBulkRework(int quantity, string note, string status, int bulkReworkId)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkRework", data.conn);
                    sqlComm.Parameters.AddWithValue("@note ", note);
                    sqlComm.Parameters.AddWithValue("@quantity ", quantity);
                    sqlComm.Parameters.AddWithValue("@status", status);
                    sqlComm.Parameters.AddWithValue("@bulkReworkId", bulkReworkId);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                }

                return bulkReworkId;
            }
            catch (Exception ex)
            {
                return bulkReworkId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }


        public int CreateBulkRework(int bulkOrderItemId, int bulkOrderBatchId, int quantity, string note, bool missingBlank, string missingBlankName)
        {
            var data = new SQLData();
            var bulkReworkId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBulkRework", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("bulkReworkId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@bulkOrderItemId", bulkOrderItemId);
                    sqlComm.Parameters.AddWithValue("@bulkOrderBatchId", bulkOrderBatchId);
                    sqlComm.Parameters.AddWithValue("@note ", note);
                    sqlComm.Parameters.AddWithValue("@missingBlank ", missingBlank);
                    sqlComm.Parameters.AddWithValue("@missingBlankName ", missingBlankName);
                    sqlComm.Parameters.AddWithValue("@quantity ", quantity);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    bulkReworkId = (int)returnParameter.Value;
                    //updload the individual line items
                    

                }

                return bulkReworkId;
            }
            catch (Exception ex)
            {
                return bulkReworkId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public List<BulkRework> GetBulkRework()
        {
            var data = new SQLData();
            List<BulkRework> lstBulkRework = new List<BulkRework>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkRework", data.conn);
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
                                BulkRework missingBlank = new BulkRework();
                                missingBlank.Id = Convert.ToInt32(dr["Id"].ToString());
                                missingBlank.BulkOrderItemId = Convert.ToInt32(dr["BulkOrderItemId"].ToString());
                                missingBlank.BulkOrderBatchId = Convert.ToInt32(dr["BulkOrderBatchId"].ToString());
                                missingBlank.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                                missingBlank.Status = Convert.ToString(dr["Status"].ToString());
                                missingBlank.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                missingBlank.Note = Convert.ToString(dr["Note"].ToString());
                                missingBlank.MissingBlankName = Convert.ToString(dr["MissingBlankName"].ToString());

                                lstBulkRework.Add(missingBlank);
                            }

                        }

                    }


                }

                return lstBulkRework;
            }
            catch (Exception ex)
            {
                return lstBulkRework;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public Int32 GetTotalHatsCompleted()
        {
            var data = new SQLData();
            try
            {
                var total = 0;
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetTotalHatsProduced", data.conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            total = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL"].ToString());
                        }

                    }


                }
                return total;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public bool UpdateBulkReworkStatus(int bulkReworkId, string status)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkReworkStatus", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkReworkId", bulkReworkId);
                    sqlComm.Parameters.AddWithValue("@status", status);

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

        public bool SaveAdminReview(int bulkOrderId, string comment, bool designerReview)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateAdminReviewSetComment", data.conn);
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@comment", comment);
                    sqlComm.Parameters.AddWithValue("@designerReview", designerReview);

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

        public bool UpdateAdminReviewFinished(int bulkOrderId)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateAdminReviewFinished", data.conn);
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

        public bool AddBulkOrderLog(int bulkOrderId, int userId, string logEntry)
        {
            var data = new SQLData();
            var bulkOrderLogEntryId = 0;
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("AddBulkOrderLog", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("bulkOrderLogEntryId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@bulkOrderId", bulkOrderId);
                    sqlComm.Parameters.AddWithValue("@userId", userId);
                    sqlComm.Parameters.AddWithValue("@logEntry", logEntry);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    bulkOrderLogEntryId = (int)returnParameter.Value;
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

        public List<MasterBulkOrderItem> GetMasterBulkOrderItems(bool inStockOnly)
        {
            var data = new SQLData();
            List<MasterBulkOrderItem> lstItems = new List<MasterBulkOrderItem>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetMasterBulkOrderItems", data.conn);
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
                                MasterBulkOrderItem item = new MasterBulkOrderItem();
                                item.Id = Convert.ToInt32(dr["Id"].ToString());
                                item.ItemName = Convert.ToString(dr["ItemName"].ToString());
                                item.ItemStyle = Convert.ToString(dr["ItemStyle"].ToString());
                                item.OSFA = Convert.ToBoolean(dr["OSFA"].ToString());
                                item.OSFAStock = Convert.ToBoolean(dr["OSFAStock"].ToString());
                                item.LXL = Convert.ToBoolean(dr["LXL"].ToString());
                                item.LXLStock = Convert.ToBoolean(dr["LXLStock"].ToString());
                                item.SM = Convert.ToBoolean(dr["SM"].ToString());
                                item.SMStock = Convert.ToBoolean(dr["SMStock"].ToString());
                                item.XLXXL = Convert.ToBoolean(dr["XLXXL"].ToString());
                                item.XLXXLStock = Convert.ToBoolean(dr["XLXXLStock"].ToString());
                                item.ItemColor = Convert.ToString(dr["ItemColor"].ToString());
                                item.Manufacturer = Convert.ToString(dr["Manufacturer"].ToString());
                                item.Available = Convert.ToBoolean(dr["Available"].ToString());
                                item.BasePrice = Convert.ToDecimal(dr["BasePrice"].ToString());
                                item.Cost = Convert.ToDecimal(dr["Cost"].ToString());
                                item.SKU = Convert.ToString(dr["SKU"].ToString());
                                item.ThumbnailpreviewImagePath = Convert.ToString(dr["ThumbnailpreviewImagePath"].ToString());
                                item.PreviewImagePath = Convert.ToString(dr["PreviewImagePath"].ToString());
                                item.DistributorLink = Convert.ToString(dr["DistributorLink"].ToString());
                                item.FrontEndName = Convert.ToString(dr["FrontEndName"].ToString());
                                item.DisplayOrder = Convert.ToInt32(dr["DisplayOrder"].ToString());
                                item.OSFAInternalStock = Convert.ToInt32(dr["OSFAInternalStock"].ToString());
                                item.SMInternalStock = Convert.ToInt32(dr["SMInternalStock"].ToString());
                                item.LXLInternalStock = Convert.ToInt32(dr["LXLInternalStock"].ToString());
                                item.XLXXLInternalStock = Convert.ToInt32(dr["XLXXLInternalStock"].ToString());
                                item.OSFAExternalStock = Convert.ToInt32(dr["OSFAExternalStock"].ToString());
                                item.SMExternalStock = Convert.ToInt32(dr["SMExternalStock"].ToString());
                                item.LXLExternalStock = Convert.ToInt32(dr["LXLExternalStock"].ToString());
                                item.XLXXLExternalStock = Convert.ToInt32(dr["XLXXLExternalStock"].ToString());
                                item.OSFASku = Convert.ToString(dr["OSFASku"].ToString());
                                item.SMSku = Convert.ToString(dr["SMSku"].ToString());
                                item.LXLSku = Convert.ToString(dr["LXLSku"].ToString());
                                item.XLXXLSku = Convert.ToString(dr["XLXXLSku"].ToString());
                                lstItems.Add(item);

                            }

                        }

                    }

                }
                return lstItems;
            }
            catch (Exception ex)
            {
                return lstItems;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }


        public MasterBulkOrderItem GetMasterBulkOrderItem(int id)
        {
            var data = new SQLData();
            MasterBulkOrderItem item = new MasterBulkOrderItem();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetMasterBulkOrderItem", data.conn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@id", id);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            item.Id = Convert.ToInt32(dr["Id"].ToString());
                            item.ItemName = Convert.ToString(dr["ItemName"].ToString());
                            item.ItemStyle = Convert.ToString(dr["ItemStyle"].ToString());
                            item.OSFA = Convert.ToBoolean(dr["OSFA"].ToString());
                            item.OSFAStock = Convert.ToBoolean(dr["OSFAStock"].ToString());
                            item.LXL = Convert.ToBoolean(dr["LXL"].ToString());
                            item.LXLStock = Convert.ToBoolean(dr["LXLStock"].ToString());
                            item.SM = Convert.ToBoolean(dr["SM"].ToString());
                            item.SMStock = Convert.ToBoolean(dr["SMStock"].ToString());
                            item.XLXXL = Convert.ToBoolean(dr["XLXXL"].ToString());
                            item.XLXXLStock = Convert.ToBoolean(dr["XLXXLStock"].ToString());
                            item.ItemColor = Convert.ToString(dr["ItemColor"].ToString());
                            item.Manufacturer = Convert.ToString(dr["Manufacturer"].ToString());
                            item.Available = Convert.ToBoolean(dr["Available"].ToString());
                            item.BasePrice = Convert.ToDecimal(dr["BasePrice"].ToString());
                            item.Cost = Convert.ToDecimal(dr["Cost"].ToString());
                            item.SKU = Convert.ToString(dr["SKU"].ToString());
                            item.ThumbnailpreviewImagePath = Convert.ToString(dr["ThumbnailpreviewImagePath"].ToString());
                            item.PreviewImagePath = Convert.ToString(dr["PreviewImagePath"].ToString());
                            item.DistributorLink = Convert.ToString(dr["DistributorLink"].ToString());
                            item.FrontEndName = Convert.ToString(dr["FrontEndName"].ToString());
                            item.DisplayOrder = Convert.ToInt32(dr["DisplayOrder"].ToString());
                            item.OSFAInternalStock = Convert.ToInt32(dr["OSFAInternalStock"].ToString());
                            item.SMInternalStock = Convert.ToInt32(dr["SMInternalStock"].ToString());
                            item.LXLInternalStock = Convert.ToInt32(dr["LXLInternalStock"].ToString());
                            item.XLXXLInternalStock = Convert.ToInt32(dr["XLXXLInternalStock"].ToString());
                            item.OSFAExternalStock = Convert.ToInt32(dr["OSFAExternalStock"].ToString());
                            item.SMExternalStock = Convert.ToInt32(dr["SMExternalStock"].ToString());
                            item.LXLExternalStock = Convert.ToInt32(dr["LXLExternalStock"].ToString());
                            item.XLXXLExternalStock = Convert.ToInt32(dr["XLXXLExternalStock"].ToString());
                            item.OSFASku = Convert.ToString(dr["OSFASku"].ToString());
                            item.SMSku = Convert.ToString(dr["SMSku"].ToString());
                            item.LXLSku = Convert.ToString(dr["LXLSku"].ToString());
                            item.XLXXLSku = Convert.ToString(dr["XLXXLSku"].ToString());



                        }

                    }
                }

                return item;
            }
                catch (Exception ex)
            {
                return item;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }
            

        public List<MasterBulkOrderItem> GetMasterBulkOrderItemsForDropDown(bool inStockOnly)
        {
            var data = new SQLData();
            List<MasterBulkOrderItem> lstItems = new List<MasterBulkOrderItem>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetMasterBulkOrderItemsForDropDown", data.conn);
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
                                MasterBulkOrderItem item = new MasterBulkOrderItem();
                                item.Id = Convert.ToInt32(dr["Id"].ToString());
                                item.ItemName = Convert.ToString(dr["ItemName"].ToString());
                                item.ItemStyle = Convert.ToString(dr["ItemStyle"].ToString());
                                item.OSFA = Convert.ToBoolean(dr["OSFA"].ToString());
                                item.OSFAStock = Convert.ToBoolean(dr["OSFAStock"].ToString());
                                item.LXL = Convert.ToBoolean(dr["LXL"].ToString());
                                item.LXLStock = Convert.ToBoolean(dr["LXLStock"].ToString());
                                item.SM = Convert.ToBoolean(dr["SM"].ToString());
                                item.SMStock = Convert.ToBoolean(dr["SMStock"].ToString());
                                item.XLXXL = Convert.ToBoolean(dr["XLXXL"].ToString());
                                item.XLXXLStock = Convert.ToBoolean(dr["XLXXLStock"].ToString());
                                item.ItemColor = Convert.ToString(dr["ItemColor"].ToString());
                                item.Manufacturer = Convert.ToString(dr["Manufacturer"].ToString());
                                item.Available = Convert.ToBoolean(dr["Available"].ToString());
                                item.BasePrice = Convert.ToDecimal(dr["BasePrice"].ToString());
                                item.Cost = Convert.ToDecimal(dr["Cost"].ToString());
                                item.SKU = Convert.ToString(dr["SKU"].ToString());
                                item.ThumbnailpreviewImagePath = Convert.ToString(dr["ThumbnailpreviewImagePath"].ToString());
                                item.PreviewImagePath = Convert.ToString(dr["PreviewImagePath"].ToString());
                                item.DistributorLink = Convert.ToString(dr["DistributorLink"].ToString());
                                item.FrontEndName = Convert.ToString(dr["FrontEndName"].ToString());
                                item.DisplayOrder = Convert.ToInt32(dr["DisplayOrder"].ToString());
                                item.OSFAInternalStock = Convert.ToInt32(dr["OSFAInternalStock"].ToString());
                                item.SMInternalStock = Convert.ToInt32(dr["SMInternalStock"].ToString());
                                item.LXLInternalStock = Convert.ToInt32(dr["LXLInternalStock"].ToString());
                                item.XLXXLInternalStock = Convert.ToInt32(dr["XLXXLInternalStock"].ToString());
                                item.OSFAExternalStock = Convert.ToInt32(dr["OSFAExternalStock"].ToString());
                                item.SMExternalStock = Convert.ToInt32(dr["SMExternalStock"].ToString());
                                item.LXLExternalStock = Convert.ToInt32(dr["LXLExternalStock"].ToString());
                                item.XLXXLExternalStock = Convert.ToInt32(dr["XLXXLExternalStock"].ToString());
                                item.OSFASku = Convert.ToString(dr["OSFASku"].ToString());
                                item.SMSku = Convert.ToString(dr["SMSku"].ToString());
                                item.LXLSku = Convert.ToString(dr["LXLSku"].ToString());
                                item.XLXXLSku = Convert.ToString(dr["XLXXLSku"].ToString());
                                lstItems.Add(item);

                            }

                        }

                    }

                }
                return lstItems;
            }
            catch (Exception ex)
            {
                return lstItems;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public bool UpdateMasterBulkOrderItem(MasterBulkOrderItem item)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateMasterBulkOrderItem", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", item.Id);
                    sqlComm.Parameters.AddWithValue("@smstock", item.SMStock);
                    sqlComm.Parameters.AddWithValue("@lxlstock", item.LXLStock);
                    sqlComm.Parameters.AddWithValue("@xlxxlstock", item.XLXXLStock);
                    sqlComm.Parameters.AddWithValue("@osfastock", item.OSFAStock);
                    sqlComm.Parameters.AddWithValue("@available", item.Available);

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

        public bool UpdateMasterBulkOrderItemInternalInventory(MasterBulkOrderItem item)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateMasterBulkOrderItemInternalInventory", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", item.Id);
                    sqlComm.Parameters.AddWithValue("@OSFAInternalStock", item.OSFAInternalStock);
                    sqlComm.Parameters.AddWithValue("@SMInternalStock", item.SMInternalStock);
                    sqlComm.Parameters.AddWithValue("@LXLInternalStock", item.LXLInternalStock);
                    sqlComm.Parameters.AddWithValue("@XLXXLInternalStock", item.XLXXLInternalStock);

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




        public int AddBulkOrderBatchMissingItems(int bulkOrdeBatchId, int masterBulkOrderItemId, string itemName, int missingQuantity, string orderedFromSource, bool ordered, bool outOfStock, string trackingNumber)
        {
            var data = new SQLData();
            var returnId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("AddBulkOrderBatchMissingItems", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("bulkOrderBatchMissingItemsId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@bulkOrderBatchId", bulkOrdeBatchId);
                    sqlComm.Parameters.AddWithValue("@masterBulkOrderItemId", masterBulkOrderItemId);
                    sqlComm.Parameters.AddWithValue("@itemName", itemName);
                    sqlComm.Parameters.AddWithValue("@missingQuantity", missingQuantity);
                    sqlComm.Parameters.AddWithValue("@orderedFromSource", orderedFromSource);
                    sqlComm.Parameters.AddWithValue("@ordered", ordered);
                    sqlComm.Parameters.AddWithValue("@outOfStock", outOfStock);
                    sqlComm.Parameters.AddWithValue("@trackingNumber", trackingNumber);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    returnId = (int)returnParameter.Value;


                }

                return returnId;
            }
            catch (Exception ex)
            {
                return returnId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public bool UpdateBulkOrderBatchMissingItems(int id, string orderedFromSource, bool ordered, bool outOfStock, string trackingNumber)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderBatchMissingItems", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", id);
                    sqlComm.Parameters.AddWithValue("@orderedFromSource", orderedFromSource);
                    sqlComm.Parameters.AddWithValue("@ordered", ordered);
                    sqlComm.Parameters.AddWithValue("@outOfStock", outOfStock);
                    sqlComm.Parameters.AddWithValue("@trackingNumber", trackingNumber);

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

        public bool UpdateBulkOrderBatchMissingItemsQuantity(int id, int missingQuantity)
        {
            var data = new SQLData();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateBulkOrderBatchMissingItemsQuantity", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", id);
                    sqlComm.Parameters.AddWithValue("@missingQuantity", missingQuantity);

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

        public List<BulkOrderBatchMissingItems> GetBulkOrderBatchMissingItems(int bulkOrderBatchId)
        {
            var data = new SQLData();
            List<BulkOrderBatchMissingItems> lstItems = new List<BulkOrderBatchMissingItems>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrderBatchMissingItems", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", bulkOrderBatchId);

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
                                BulkOrderBatchMissingItems item = new BulkOrderBatchMissingItems();
                                item.Id = Convert.ToInt32(dr["Id"].ToString());
                                item.BulkOrderBatchId = Convert.ToInt32(dr["BulkOrderBatchId"].ToString());
                                item.MasterBulkOrderItemId = Convert.ToInt32(dr["MasterBulkOrderItemId"].ToString());
                                item.MissingQuantity = Convert.ToInt32(dr["MissingQuantity"].ToString());
                                item.OrderedFromSource = Convert.ToString(dr["OrderedFromSource"].ToString());
                                item.Ordered = Convert.ToBoolean(dr["Ordered"].ToString());
                                item.OutOfStock = Convert.ToBoolean(dr["OutOfStock"].ToString());
                                item.TrackingNumber = Convert.ToString(dr["TrackingNumber"].ToString());
                                item.ItemName = Convert.ToString(dr["ItemName"].ToString());
                                lstItems.Add(item);

                            }

                        }

                    }

                }
                return lstItems;
            }
            catch (Exception ex)
            {
                return lstItems;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public List<string> GetBulkOrdersContainingMissingBlank(int masterItemId, bool isOSFA, bool isSM, bool isLXL, bool isXLXXL, int batchId)
        {
            var data = new SQLData();
            List<string> lstBulkOrders = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBulkOrdersContainingMissingBlank", data.conn);
                    sqlComm.Parameters.AddWithValue("@masterItemId", masterItemId);
                    sqlComm.Parameters.AddWithValue("@isOSFA", isOSFA);
                    sqlComm.Parameters.AddWithValue("@isSM", isSM);
                    sqlComm.Parameters.AddWithValue("@isLXL", isLXL);
                    sqlComm.Parameters.AddWithValue("@isXLXXL", isXLXXL);
                    sqlComm.Parameters.AddWithValue("@batchId", batchId);
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
                                lstBulkOrders.Add(Convert.ToString(dr["MissingRow"]));
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


    }

    
}
