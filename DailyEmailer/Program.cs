using System;
using System.Collections.Generic;
using System.Linq;
using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;

namespace DailyEmailer
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailFunctions emailFunctions = new EmailFunctions();
            BulkData bulkData = new BulkData();
            DesignData designData = new DesignData();


            List<BulkOrder> lstBulkOrders = bulkData.GetBulkOrderData("");

            List<BulkOrder> lstNoArtworkOrders = lstBulkOrders.Where(bo => bo.OrderPaid && !bo.OrderComplete && !bo.ReadyForProduction && (bo.ArtworkImage == "" || (!bo.lstDesigns.Any(d => d.DigitizedPreview != "") && bo.OrderNotes.Contains("ARTWORK PRE-EXISTING")))).ToList();
            List<BulkOrder> lstPendingApproval = lstBulkOrders.Where(bo => bo.OrderPaid && !bo.OrderComplete && !bo.ReadyForProduction && bo.lstDesigns.Any(d => d.CustomerApproved == false) && bo.lstDesigns.Any(d => d.InternallyApproved) && !bo.lstDesigns.Any(d => d.Revision) && bo.lstDesigns.Any(d => d.DigitizedPreview != "")).ToList();


            //loop through and send customers the missing artwork email for those that need it sent
            foreach (BulkOrder bulkOrder in lstNoArtworkOrders)
            {
                if(bulkOrder.OrderNotes.Contains("ARTWORK PRE-EXISTING :"))
                {
                    //send link to choose their own pre-existing design
                    if (!bulkOrder.ArtworkEmailSent)
                    {
                        var success = emailFunctions.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunctions.assignPreExistingArtworkEmail(bulkOrder.PaymentGuid), "Order #" + bulkOrder.Id.ToString() + " Pre-Existing Artwork", "");

                        if (success)
                        {
                            bulkData.UpdateArtworkEmailSent(bulkOrder.Id);
                            var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(bulkOrder.Id, 0, "Pre-Existing Artwork Email Sent From Automated Emailer");
                        }
                    }
                }
                else
                {
                    if(bulkOrder.ArtworkImage == "" && !bulkOrder.ArtworkEmailSent)
                    {
                        var success = emailFunctions.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunctions.requestArtworkEmail(bulkOrder.Id.ToString()), "Order #" + bulkOrder.Id.ToString() + " Artwork Request", "");

                        if (success)
                        {
                            bulkData.UpdateArtworkEmailSent(bulkOrder.Id);
                            var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(bulkOrder.Id, 0, "Missing Artwork Email Sent From Automated Emailer");
                        }
                    }
                }
            }

        }
    }
}
