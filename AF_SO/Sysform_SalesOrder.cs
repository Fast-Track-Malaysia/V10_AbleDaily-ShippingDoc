using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace FT_ADDON.AP_SO
{
    class Sysform_SalesOrder
    {
        public static void processRightClickEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ContextMenuInfo pVal, ref bool BubbleEvent)
        {
            try
            {
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
        public static void processRightClickEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ContextMenuInfo pVal)
        {
        }
        public static void processItemEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            try
            {
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                        SAPbouiCOM.Item oItem = null;
                        SAPbouiCOM.Button oButton = null;
                        oItem = oForm.Items.Add("SHIPD", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        oItem.Left = oForm.Items.Item("2").Left + oForm.Items.Item("2").Width + 20;
                        oItem.Width = 80;
                        oItem.Top = oForm.Items.Item("2").Top;
                        oItem.Height = oForm.Items.Item("2").Height;
                        oButton = (SAPbouiCOM.Button)oItem.Specific;
                        oButton.Caption = "Shipping Doc";

                        oItem = oForm.Items.Add("SHIPL", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        oItem.Left = oForm.Items.Item("SHIPD").Left + oForm.Items.Item("SHIPD").Width + 5;
                        oItem.Width = 80;
                        oItem.Top = oForm.Items.Item("2").Top;
                        oItem.Height = oForm.Items.Item("2").Height;
                        oButton = (SAPbouiCOM.Button)oItem.Specific;
                        oButton.Caption = "Shipping List";
                        break;
                }
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
        public static void processItemEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ItemEvent pVal)
        {
            try
            {
                long docentry = 0;
                string docnum = "";
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                        if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                        {
                            if (pVal.ItemUID == "SHIPD")
                            {
                                docentry = long.Parse(oForm.DataSources.DBDataSources.Item(0).GetValue("docentry", 0).ToString());
                                docnum = oForm.DataSources.DBDataSources.Item(0).GetValue("docnum", 0).ToString();
                                InitForm.shipdoc(oForm.UniqueID, docentry, 0, docnum);
                            }
                            else if (pVal.ItemUID == "SHIPL")
                            {
                                docentry = long.Parse(oForm.DataSources.DBDataSources.Item(0).GetValue("docentry", 0).ToString());
                                InitForm.shiplist(oForm.UniqueID, docentry);
                            }
                        }
                        break;
                    case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK:
                        if (pVal.ItemUID == "38")
                        {
                            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                            {
                                if (pVal.Row >= 0)
                                {
                                    docentry = int.Parse(oForm.DataSources.DBDataSources.Item("RDR1").GetValue("DOCENTRY", pVal.Row - 1).ToString());
                                    int linenum = int.Parse(oForm.DataSources.DBDataSources.Item("RDR1").GetValue("LINENUM", pVal.Row - 1).ToString());
                                    if (pVal.ColUID == "U_FCL")
                                    {
                                        InitForm.CONM(oForm.UniqueID, docentry, linenum, pVal.Row, "FT_APSOC", "U_CONNO");
                                    }
                                    else if (pVal.ColUID == "256")
                                    {
                                    }
                                    else
                                    {
                                        decimal del = decimal.Parse(oForm.DataSources.DBDataSources.Item("RDR1").GetValue("DelivrdQty", pVal.Row - 1).ToString());
                                        if (del > 0 && pVal.ColUID != "0")
                                            InitForm.SOM(oForm.UniqueID, docentry, linenum, pVal.Row, "RDR1", "38");
                                        else
                                            FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Popup window disable!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
        public static void processMenuEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.MenuEvent pVal, ref bool BubbleEvent)
        {
            return;
        }
        public static void processMenuEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.MenuEvent pVal)
        {
            return;
        }
    }
}
