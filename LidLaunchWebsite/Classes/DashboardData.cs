using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Classes
{
    public class DashboardData
    {        
        
        public DesignerDashboard GetDesignerDashboard(int designerId)
        {

            DesignerDashboard model = new DesignerDashboard();
            List<DesignerDashboardSales> lstSales = new List<DesignerDashboardSales>();
            List<DesignerDashboardProduct> lstProducts = new List<DesignerDashboardProduct>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetDesignerDashboard", data.conn);
                    sqlComm.Parameters.AddWithValue("@id", designerId);

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
                            DesignerDashboardSales sales = new DesignerDashboardSales();

                            sales.TotalSales = Convert.ToInt32(dr["TotalSales"]);
                            sales.ProductName = dr["Name"].ToString();
                            sales.ProductImage = dr["PreviewImage"].ToString();
                            sales.SaleProfitNumber = Convert.ToInt32(dr["SaleProfitNumber"]);

                            model.Profit += sales.SaleProfitNumber * 5;
                            model.TotalProductsSold += sales.TotalSales;

                            lstSales.Add(sales);
                        }
                    }
                    model.lstSales = lstSales;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            DesignerDashboardProduct product = new DesignerDashboardProduct();

                            product.ProductId = Convert.ToInt32(dr["Id"]);
                            product.ProductName = dr["Name"].ToString();
                            product.ProductImage = dr["PreviewImage"].ToString();
                            product.Private = Convert.ToBoolean(dr["Private"]);
                            product.Approved = Convert.ToBoolean(dr["Approved"]);
                            
                            lstProducts.Add(product);
                        }
                    }
                    model.lstProducts = lstProducts;

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            model.TotalAvailableForPayout = model.Profit - Convert.ToDecimal(dr["PayoutTotal"]);
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
        public int CreateDesignerPayout (int designerId, decimal payoutAmmount, string paypalAddress)
        {
            var data = new SQLData();
            var payoutId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateDesignerPayout", data.conn);
                    SqlParameter returnParameter = sqlComm.Parameters.Add("payoutId", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    sqlComm.Parameters.AddWithValue("@designerId", designerId);
                    sqlComm.Parameters.AddWithValue("@payoutAmmount", payoutAmmount);
                    sqlComm.Parameters.AddWithValue("@payoutPaypalAddress", payoutAmmount);

                    sqlComm.CommandType = CommandType.StoredProcedure;
                    data.conn.Open();
                    sqlComm.ExecuteNonQuery();
                    payoutId = (int)returnParameter.Value;

                }

                return payoutId;
            }
            catch (Exception ex)
            {
                return payoutId;
            }
            finally
            {
                if (data.conn != null)
                {
                    data.conn.Close();
                }
            }
        }

        public AdminDashboard GetAdminDashboard()
        {

            AdminDashboard model = new AdminDashboard();
            List<Sale> lstSales = new List<Sale>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetOrders", data.conn);
                    //sqlComm.Parameters.AddWithValue("@id", designerId);

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
                            Sale sale = new Sale();

                            sale.OrderId = Convert.ToInt32(dr["OrderId"]);
                            sale.OrderProductId = Convert.ToInt32(dr["OrderProductId"]);
                            sale.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            sale.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                            sale.ProductId = Convert.ToInt32(dr["ProductId"].ToString());
                            sale.CustomerName = dr["CustomerName"].ToString();
                            sale.CustomerPhone = dr["CustomerPhone"].ToString();
                            sale.CustomerEmail = dr["CustomerEmail"].ToString();
                            sale.ShippingAddress = dr["ShippingAddress"].ToString();
                            sale.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
                            sale.ShippingPaid = Convert.ToDecimal(dr["ShippingPaid"].ToString());
                            sale.TaxesPaid = Convert.ToDecimal(dr["TaxesPaid"].ToString());
                            sale.ShippingActual = Convert.ToDecimal(dr["ShippingActual"].ToString());
                            sale.OrderStaus = dr["OrderStatus"].ToString();
                            sale.HasPaid = Convert.ToBoolean(dr["HasPaid"].ToString());
                            sale.ProductName = dr["ProductName"].ToString();
                            sale.DesignId = Convert.ToInt32(dr["DesignId"].ToString());
                            sale.DesignId = Convert.ToInt32(dr["DesignerId"].ToString());
                            sale.Size = dr["Size"].ToString();
                            sale.Produced = Convert.ToBoolean(dr["Produced"].ToString());
                            sale.Shipped = Convert.ToBoolean(dr["Shipped"].ToString());
                            sale.TrackingNumber = dr["TrackingNumber"].ToString();
                            sale.ArtSource = dr["ArtSource"].ToString();
                            sale.PreviewImage = dr["PreviewImage"].ToString();
                            sale.DigitizedFile = dr["DigitizedFile"].ToString();
                            sale.DigitizingCost = Convert.ToDecimal(dr["DigitizingCost"].ToString());
                            sale.TypeText = dr["TypeText"].ToString();
                            sale.ProductIdentifier = dr["ProductIdentifier"].ToString();
                            sale.HatColor = dr["HatColor"].ToString();


                            lstSales.Add(sale);



                        }
                    }
                    model.lstSales = lstSales;

                    model.TotalSalesCount = lstSales.Count;
                    decimal boxPrice = (decimal)0.27;
                    decimal paypalCut = (decimal)0.039;
                    decimal packagingMaterialPrice = (decimal)0.25;
                    decimal shippingPrice = (decimal)3.34;
                    decimal hatProductionPrice = (decimal)6;
                    decimal designerProfit = (decimal)5.00;

                    if (ds.Tables[1].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[1].Rows[0];
                        model.TotalSales = Convert.ToDecimal(dr["TotalSales"].ToString());
                        model.TotalSalesWithoutShipping = Convert.ToDecimal(dr["TotalSalesWithoutShipping"].ToString());
                        model.TotalShippingCollected = Convert.ToDecimal(dr["TotalShippingCollected"].ToString());
                        model.TotalShippingActual = Convert.ToDecimal(dr["TotalShippingActual"].ToString());
                        model.DigitizingTotal = Convert.ToDecimal(dr["DigitizingTotal"].ToString());
                        model.DigitizingExcess = (Convert.ToDecimal(dr["DigitizingTotalCount"]) * 8) - model.DigitizingTotal;
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            model.TotalDesignerProfits += (Convert.ToInt32(dr["SaleProfitNumber"].ToString()) * 8);
                        }
                    }

                    model.EstimatedShippingCosts = (shippingPrice * model.TotalSalesCount);
                    model.EstimatedExcessShipping = model.TotalShippingCollected - model.EstimatedShippingCosts;
                    model.EstimatedTotalProfit = ((19.99M - boxPrice - packagingMaterialPrice - hatProductionPrice - designerProfit) * model.TotalSalesCount);
                    model.EstimatedTotalProfit = model.EstimatedTotalProfit - (model.TotalSales * paypalCut);

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
        public BatchDashboard GetBatches()
        {

            BatchDashboard model = new BatchDashboard();
            List<BatchSale> lstReadySales = new List<BatchSale>();
            List<BatchSale> lstNotReadySales = new List<BatchSale>();
            List<OrderBatch> lstOrderbatches = new List<OrderBatch>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetOrdersForBatching", data.conn);
                    //sqlComm.Parameters.AddWithValue("@id", designerId);

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
                            Sale sale = new Sale();

                            sale.OrderId = Convert.ToInt32(dr["OrderId"]);
                            sale.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            sale.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                            sale.ProductId = Convert.ToInt32(dr["ProductId"].ToString());
                            sale.CustomerName = dr["CustomerName"].ToString();
                            sale.CustomerPhone = dr["CustomerPhone"].ToString();
                            sale.ShippingAddress = dr["ShippingAddress"].ToString();
                            sale.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
                            sale.ShippingPaid = Convert.ToDecimal(dr["ShippingPaid"].ToString());
                            sale.TaxesPaid = Convert.ToDecimal(dr["TaxesPaid"].ToString());
                            sale.ShippingActual = Convert.ToDecimal(dr["ShippingActual"].ToString());
                            sale.OrderStaus = dr["OrderStatus"].ToString();
                            sale.HasPaid = Convert.ToBoolean(dr["HasPaid"].ToString());
                            sale.ProductName = dr["ProductName"].ToString();
                            sale.DesignId = Convert.ToInt32(dr["DesignId"].ToString());
                            sale.DesignId = Convert.ToInt32(dr["DesignerId"].ToString());
                            sale.Size = dr["Size"].ToString();
                            sale.Produced = Convert.ToBoolean(dr["Produced"].ToString());
                            sale.Shipped = Convert.ToBoolean(dr["Shipped"].ToString());
                            sale.TrackingNumber = dr["TrackingNumber"].ToString();
                            sale.ArtSource = dr["ArtSource"].ToString();
                            sale.PreviewImage = dr["PreviewImage"].ToString();
                            sale.DigitizedFile = dr["DigitizedFile"].ToString();
                            sale.DigitizedProductionSheet = dr["DigitizedProductionSheet"].ToString();
                            sale.DigitizedPreview = dr["DigitizedPreview"].ToString();
                            sale.DigitizingCost = Convert.ToDecimal(dr["DigitizingCost"].ToString());
                            sale.TypeText = dr["TypeText"].ToString();
                            sale.ProductIdentifier = dr["ProductIdentifier"].ToString();
                            sale.HatColor = dr["HatColor"].ToString();

                            bool foundBatch = false;
                            foreach (BatchSale batch in lstReadySales) {
                                if(batch.ProductId == sale.ProductId)
                                {
                                    foundBatch = true;
                                    batch.lstSales.Add(sale);

                                    bool foundHatTypeSize = false;

                                    foreach (HatTypeSize hatTypeSize in batch.lstHatTypeSizes)
                                    {
                                        if (hatTypeSize.Size == sale.Size && hatTypeSize.Type == sale.TypeText && hatTypeSize.Color == sale.HatColor)
                                        {
                                            foundHatTypeSize = true;
                                            hatTypeSize.Count = hatTypeSize.Count + 1;

                                        }
                                    }
                                    if (!foundHatTypeSize)
                                    {
                                        HatTypeSize hatTypeSize = new HatTypeSize();
                                        hatTypeSize.Size = sale.Size;
                                        hatTypeSize.Type = sale.TypeText;
                                        hatTypeSize.Count = 1;
                                        hatTypeSize.Color = sale.HatColor;
                                        batch.lstHatTypeSizes.Add(hatTypeSize);
                                    }
                                }                                
                            }
                            if (!foundBatch)
                            {
                                BatchSale batchSale = new BatchSale();
                                batchSale.ProductName = sale.ProductName;
                                batchSale.ArtSource = sale.ArtSource;
                                batchSale.PreviewImage = sale.PreviewImage;
                                batchSale.ProductId = sale.ProductId;
                                batchSale.DigitizedFile = sale.DigitizedFile;
                                batchSale.DigitizedProductionSheet = sale.DigitizedProductionSheet;
                                batchSale.DigitizedPreview = sale.DigitizedPreview;
                                List<Sale> lstSales = new List<Sale>();
                                lstSales.Add(sale);
                                batchSale.lstSales = lstSales;
                                batchSale.lstHatTypeSizes = new List<HatTypeSize>();
                                HatTypeSize hatTypeSize = new HatTypeSize();
                                hatTypeSize.Size = sale.Size;
                                hatTypeSize.Type = sale.TypeText;
                                hatTypeSize.Color = sale.HatColor;
                                hatTypeSize.Count = 1;
                                batchSale.lstHatTypeSizes.Add(hatTypeSize);
                                lstReadySales.Add(batchSale);
                            }

                        }
                    }
                    model.lstReadySales = lstReadySales;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            Sale sale = new Sale();

                            sale.OrderId = Convert.ToInt32(dr["OrderId"]);
                            sale.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            sale.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                            sale.ProductId = Convert.ToInt32(dr["ProductId"].ToString());
                            sale.CustomerName = dr["CustomerName"].ToString();
                            sale.CustomerPhone = dr["CustomerPhone"].ToString();
                            sale.ShippingAddress = dr["ShippingAddress"].ToString();
                            sale.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
                            sale.ShippingPaid = Convert.ToDecimal(dr["ShippingPaid"].ToString());
                            sale.TaxesPaid = Convert.ToDecimal(dr["TaxesPaid"].ToString());
                            sale.ShippingActual = Convert.ToDecimal(dr["ShippingActual"].ToString());
                            sale.OrderStaus = dr["OrderStatus"].ToString();
                            sale.HasPaid = Convert.ToBoolean(dr["HasPaid"].ToString());
                            sale.ProductName = dr["ProductName"].ToString();
                            sale.DesignId = Convert.ToInt32(dr["DesignId"].ToString());
                            sale.DesignId = Convert.ToInt32(dr["DesignerId"].ToString());
                            sale.Size = dr["Size"].ToString();
                            sale.Produced = Convert.ToBoolean(dr["Produced"].ToString());
                            sale.Shipped = Convert.ToBoolean(dr["Shipped"].ToString());
                            sale.TrackingNumber = dr["TrackingNumber"].ToString();
                            sale.ArtSource = dr["ArtSource"].ToString();
                            sale.PreviewImage = dr["PreviewImage"].ToString();
                            sale.DigitizedFile = dr["DigitizedFile"].ToString();
                            sale.DigitizedProductionSheet = dr["DigitizedProductionSheet"].ToString();
                            sale.DigitizedPreview = dr["DigitizedPreview"].ToString();
                            sale.DigitizingCost = Convert.ToDecimal(dr["DigitizingCost"].ToString());
                            sale.TypeText = dr["TypeText"].ToString();
                            sale.ProductIdentifier = dr["ProductIdentifier"].ToString();
                            sale.HatColor = dr["HatColor"].ToString();

                            bool foundBatch = false;
                            foreach (BatchSale batch in lstNotReadySales)
                            {
                                if (batch.ProductId == sale.ProductId)
                                {
                                    foundBatch = true;
                                    batch.lstSales.Add(sale);

                                    bool foundHatTypeSize = false;

                                    foreach (HatTypeSize hatTypeSize in batch.lstHatTypeSizes)
                                    {
                                        if (hatTypeSize.Size == sale.Size && hatTypeSize.Type == sale.TypeText && hatTypeSize.Color == sale.HatColor)
                                        {
                                            foundHatTypeSize = true;
                                            hatTypeSize.Count = hatTypeSize.Count + 1;

                                        }
                                    }
                                    if (!foundHatTypeSize)
                                    {
                                        HatTypeSize hatTypeSize = new HatTypeSize();
                                        hatTypeSize.Size = sale.Size;
                                        hatTypeSize.Type = sale.TypeText;
                                        hatTypeSize.Color = sale.HatColor;
                                        hatTypeSize.Count = 1;
                                        batch.lstHatTypeSizes.Add(hatTypeSize);
                                    }
                                }
                            }
                            if (!foundBatch)
                            {
                                BatchSale batchSale = new BatchSale();
                                batchSale.ProductName = sale.ProductName;
                                batchSale.ArtSource = sale.ArtSource;
                                batchSale.PreviewImage = sale.PreviewImage;
                                batchSale.ProductId = sale.ProductId;
                                batchSale.DigitizedFile = sale.DigitizedFile;
                                batchSale.DigitizedProductionSheet = sale.DigitizedProductionSheet;
                                batchSale.DigitizedPreview = sale.DigitizedPreview;
                                List<Sale> lstSales = new List<Sale>();
                                lstSales.Add(sale);
                                batchSale.lstSales = lstSales;
                                batchSale.lstHatTypeSizes = new List<HatTypeSize>();
                                HatTypeSize hatTypeSize = new HatTypeSize();
                                hatTypeSize.Size = sale.Size;
                                hatTypeSize.Type = sale.TypeText;
                                hatTypeSize.Color = sale.HatColor;
                                hatTypeSize.Count = 1;
                                batchSale.lstHatTypeSizes.Add(hatTypeSize);
                                lstNotReadySales.Add(batchSale);
                            }



                        }
                    }
                    model.lstNotReadySales = lstNotReadySales;


                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            OrderBatch orderBatch = new OrderBatch();

                            orderBatch.BatchId = Convert.ToInt32(dr["Id"]);
                            orderBatch.DateBatched = Convert.ToDateTime(dr["DateBatched"]);
                            orderBatch.Status = dr["Status"].ToString();

                            lstOrderbatches.Add(orderBatch);

                        }
                    }
                    model.lstOrderBatches = lstOrderbatches;

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
        public BatchDashboard GetBatch(int batchId)
        {

            BatchDashboard model = new BatchDashboard();
            List<BatchSale> lstReadySales = new List<BatchSale>();
            List<BatchSale> lstNotReadySales = new List<BatchSale>();
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("GetBatch", data.conn);
                    sqlComm.Parameters.AddWithValue("@batchId", batchId);

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
                            Sale sale = new Sale();

                            sale.OrderId = Convert.ToInt32(dr["OrderId"]);
                            sale.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            sale.OrderDate = Convert.ToDateTime(dr["OrderDate"].ToString());
                            sale.ProductId = Convert.ToInt32(dr["ProductId"].ToString());
                            sale.CustomerName = dr["CustomerName"].ToString();
                            sale.CustomerPhone = dr["CustomerPhone"].ToString();
                            sale.ShippingAddress = dr["ShippingAddress"].ToString();
                            sale.OrderTotal = Convert.ToDecimal(dr["OrderTotal"].ToString());
                            sale.ShippingPaid = Convert.ToDecimal(dr["ShippingPaid"].ToString());
                            sale.TaxesPaid = Convert.ToDecimal(dr["TaxesPaid"].ToString());
                            sale.ShippingActual = Convert.ToDecimal(dr["ShippingActual"].ToString());
                            sale.OrderStaus = dr["OrderStatus"].ToString();
                            sale.HasPaid = Convert.ToBoolean(dr["HasPaid"].ToString());
                            sale.ProductName = dr["ProductName"].ToString();
                            sale.DesignId = Convert.ToInt32(dr["DesignId"].ToString());
                            sale.DesignId = Convert.ToInt32(dr["DesignerId"].ToString());
                            sale.Size = dr["Size"].ToString();
                            sale.Produced = Convert.ToBoolean(dr["Produced"].ToString());
                            sale.Shipped = Convert.ToBoolean(dr["Shipped"].ToString());
                            sale.TrackingNumber = dr["TrackingNumber"].ToString();
                            sale.ArtSource = dr["ArtSource"].ToString();
                            sale.PreviewImage = dr["PreviewImage"].ToString();
                            sale.DigitizedFile = dr["DigitizedFile"].ToString();
                            sale.DigitizedProductionSheet = dr["DigitizedProductionSheet"].ToString();
                            sale.DigitizedPreview = dr["DigitizedPreview"].ToString();
                            sale.DigitizingCost = Convert.ToDecimal(dr["DigitizingCost"].ToString());
                            sale.TypeText = dr["TypeText"].ToString();
                            sale.ProductIdentifier = dr["ProductIdentifier"].ToString();
                            sale.HatColor = dr["HatColor"].ToString();

                            bool foundBatch = false;
                            foreach (BatchSale batch in lstReadySales)
                            {
                                if (batch.ProductId == sale.ProductId)
                                {
                                    foundBatch = true;
                                    batch.lstSales.Add(sale);

                                    bool foundHatTypeSize = false;

                                    foreach (HatTypeSize hatTypeSize in batch.lstHatTypeSizes)
                                    {
                                        if (hatTypeSize.Size == sale.Size && hatTypeSize.Type == sale.TypeText && hatTypeSize.Color == sale.HatColor)
                                        {
                                            foundHatTypeSize = true;
                                            hatTypeSize.Count = hatTypeSize.Count + 1;

                                        }
                                    }
                                    if (!foundHatTypeSize)
                                    {
                                        HatTypeSize hatTypeSize = new HatTypeSize();
                                        hatTypeSize.Size = sale.Size;
                                        hatTypeSize.Type = sale.TypeText;
                                        hatTypeSize.Color = sale.HatColor;
                                        hatTypeSize.Count = 1;
                                        batch.lstHatTypeSizes.Add(hatTypeSize);
                                    }
                                }
                            }
                            if (!foundBatch)
                            {
                                BatchSale batchSale = new BatchSale();
                                batchSale.ProductName = sale.ProductName;
                                batchSale.ArtSource = sale.ArtSource;
                                batchSale.PreviewImage = sale.PreviewImage;
                                batchSale.ProductId = sale.ProductId;
                                batchSale.DigitizedFile = sale.DigitizedFile;
                                batchSale.DigitizedProductionSheet = sale.DigitizedProductionSheet;
                                batchSale.DigitizedPreview = sale.DigitizedPreview;
                                List<Sale> lstSales = new List<Sale>();
                                lstSales.Add(sale);
                                batchSale.lstSales = lstSales;
                                batchSale.lstHatTypeSizes = new List<HatTypeSize>();
                                HatTypeSize hatTypeSize = new HatTypeSize();
                                hatTypeSize.Size = sale.Size;
                                hatTypeSize.Type = sale.TypeText;
                                hatTypeSize.Color = sale.HatColor;
                                hatTypeSize.Count = 1;
                                batchSale.lstHatTypeSizes.Add(hatTypeSize);
                                lstReadySales.Add(batchSale);
                            }
                        }
                    }
                    model.lstReadySales = lstReadySales;


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
        public int CreateOrderBatch()
        {
            var data = new SQLData();
            var batchId = 0;
            try
            {
                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("CreateBatch", data.conn);
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
        public bool UpdateTracking(int orderProductId, string trackingNumber)
        {
            var data = new SQLData();
            try
            {

                DataSet ds = new DataSet();
                using (data.conn)
                {
                    SqlCommand sqlComm = new SqlCommand("UpdateOrderProductTracking", data.conn);
                    sqlComm.Parameters.AddWithValue("@orderProductId", orderProductId);
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
    }
}