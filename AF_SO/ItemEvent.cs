using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FT_ADDON.AP_SO
{
    class ItemEvent
    {

        public static void processItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            SAPbouiCOM.Form oForm = null;
            try
            {
                oForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);
            }
            catch { }
            if (oForm == null) return;
            if (pVal.BeforeAction)
            {
                //FT_ADDON.SAP.iForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);
                BubbleEvent = true;
                switch (pVal.FormTypeEx)
                {
                    case "FT_SHIPL":
                        UserForm_ShipList.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_CFLTEXT":
                        CFLFormText.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_SHIPD":
                        UserForm_ShipDoc.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_CFL":
                        CFLForm.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_SOM":
                    case "FT_SDM":
                        UserForm_SOmodified.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_CONM":
                    case "FT_DOPTM":
                        UserForm_CONmodified.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_TEXT":
                        UserForm_TextModify.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "139":
                        Sysform_SalesOrder.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "140":
                        Sysform_SalesDelivery.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "133":
                        Sysform_SalesInvoice.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "179":
                        Sysform_SalesCN.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "143":
                        Sysform_APDO.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "141":
                        Sysform_APINV.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "181":
                        Sysform_APCN.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "142":
                        Sysform_APPO.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "180":
                        Sysform_SalesReturn.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "182":
                        Sysform_APReturn.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "34":
                        Sysform_UDF.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    //case "81":
                    //    SysForm_PickandPackManager.processItemEventbefore(oForm, ref pVal, ref BubbleEvent);
                    //    break;
                    //case "134":
                    //    Sysform_BusinessPartner.processItemEventbefore(iForm, ref pVal, ref BubbleEvent);
                    //    break;
                    default:
                        break;

                }
            }
            else
            {
                switch (pVal.FormTypeEx)
                {
                    case "FT_SHIPL":
                        UserForm_ShipList.processItemEventafter(oForm, ref pVal);
                        break;
                   case "FT_CFLTEXT":
                        CFLFormText.processItemEventafter(oForm, ref pVal);
                        break;
                   case "FT_SHIPD":
                        UserForm_ShipDoc.processItemEventafter(oForm, ref pVal);
                        break;
                    case "FT_CFL":
                        CFLForm.processItemEventafter(oForm, ref pVal);
                        break;
                    case "FT_SOM":
                    case "FT_SDM":
                        UserForm_SOmodified.processItemEventafter(oForm, ref pVal);
                        break;
                    case "FT_CONM":
                    case "FT_DOPTM":
                        UserForm_CONmodified.processItemEventafter(oForm, ref pVal);
                        break;
                    case "FT_TEXT":
                        UserForm_TextModify.processItemEventafter(oForm, ref pVal);
                        break;
                    case "139":
                        Sysform_SalesOrder.processItemEventafter(oForm, ref pVal);
                        break;
                    case "140":
                        Sysform_SalesDelivery.processItemEventafter(oForm, ref pVal);
                        break;
                    case "133":
                        Sysform_SalesInvoice.processItemEventafter(oForm, ref pVal);
                        break;
                    case "179":
                        Sysform_SalesCN.processItemEventafter(oForm, ref pVal);
                        break;
                    case "143":
                        Sysform_APDO.processItemEventafter(oForm, ref pVal);
                        break;
                    case "141":
                        Sysform_APINV.processItemEventafter(oForm, ref pVal);
                        break;
                    case "181":
                        Sysform_APCN.processItemEventafter(oForm, ref pVal);
                        break;
                    case "142":
                        Sysform_APPO.processItemEventafter(oForm, ref pVal);
                        break;
                    case "180":
                        Sysform_SalesReturn.processItemEventafter(oForm, ref pVal);
                        break;
                    case "182":
                        Sysform_APReturn.processItemEventafter(oForm, ref pVal);
                        break;
                    case "34":
                        Sysform_UDF.processItemEventafter(oForm, ref pVal);
                        break;
                    //case "81":
                    //    SysForm_PickandPackManager.processItemEventafter(oForm, ref pVal);
                    //    break;
                    //case "134":
                    //    Sysform_BusinessPartner.processItemEventafter(iForm, ref pVal);
                    //    break;
                    default:
                        break;

                }
            }
        }
        public static void processRightClickEvent(string FormUID, ref SAPbouiCOM.ContextMenuInfo pVal, ref bool BubbleEvent)
        {
            SAPbouiCOM.Form oForm = null;
            try
            {
                oForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);
            }
            catch{}
            if (oForm == null) return;
            //oForm = FT_ADDON.SAP.SBOApplication.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
            if (pVal.BeforeAction)
            {
                //FT_ADDON.SAP.iForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);
                BubbleEvent = true;
                switch (oForm.TypeEx)
                {
                    case "FT_SHIPD":
                        UserForm_ShipDoc.processRightClickEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_SOM":
                    case "FT_SDM":
                        UserForm_SOmodified.processRightClickEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "FT_CONM":
                        UserForm_CONmodified.processRightClickEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "139":
                        Sysform_SalesOrder.processRightClickEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    case "34":
                        Sysform_UDF.processRightClickEventbefore(oForm, ref pVal, ref BubbleEvent);
                        break;
                    //case "81":
                    //    SysForm_PickandPackManager.processRightClickEventbefore(oForm, ref pVal, ref BubbleEvent);
                    //    break;
                    //case "134":
                    //    Sysform_BusinessPartner.processRightClickEventbefore(iForm, ref pVal, ref BubbleEvent);
                    //    break;
                    default:
                        break;

                }
            }
            else
            {
                switch (oForm.TypeEx)
                {
                    case "FT_SHIPD":
                        UserForm_ShipDoc.processRightClickEventafter(oForm, ref pVal);
                        break;
                    case "FT_SOM":
                    case "FT_SDM":
                        UserForm_SOmodified.processRightClickEventafter(oForm, ref pVal);
                        break;
                    case "FT_CONM":
                        UserForm_CONmodified.processRightClickEventafter(oForm, ref pVal);
                        break;
                    case "139":
                        Sysform_SalesOrder.processRightClickEventafter(oForm, ref pVal);
                        break;
                    case "34":
                        Sysform_UDF.processRightClickEventafter(oForm, ref pVal);
                        break;
                    //case "81":
                    //    SysForm_PickandPackManager.processRightClickEventafter(oForm, ref pVal);
                    //    break;
                    //case "134":
                    //    Sysform_BusinessPartner.processRightClickEventafter(iForm, ref pVal);
                    //    break;
                    default:
                        break;

                }
            }
        }

    }
}
