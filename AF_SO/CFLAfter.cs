using System;
using System.Collections.Generic;
using System.Text;

namespace FT_ADDON.AP_SO
{
    class CFLAfter
    {
        public static void aftercflcustom(SAPbouiCOM.Form oForm, string ds, string col, int row, string matrixname, string rtnvalue, SAPbouiCOM.Form cflForm)
        {
            SAPbouiCOM.Item oItem = null;
            SAPbouiCOM.Matrix oMatrix = null;
            SAPbouiCOM.Grid oGrid = (SAPbouiCOM.Grid)cflForm.Items.Item("grid").Specific;
            bool first = true;
            int rowsadd = 0;
            DateTime dtvalue;
            string cmbvalue = "";

            oForm.DataSources.UserDataSources.Item("cfluid").Value = "";

            if (matrixname != "")
            {
                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(matrixname).Specific;
                oMatrix.FlushToDataSource();
                row = row - 1;

                for (int x = 0; x < oGrid.Rows.Count; x++)
                {
                    if (oGrid.Rows.IsSelected(x))
                    {
                        rowsadd++;

                        if (!first)
                        {
                            oForm.DataSources.DBDataSources.Item(ds).InsertRecord(oForm.DataSources.DBDataSources.Item(ds).Size - 1);
                            row++;
                        }
                        if (col == "U_itemcode" || col == "U_conno")
                        {
                            string column = "";

                            for (int y = 0; y < cflForm.DataSources.DataTables.Item("cfl").Columns.Count; y++)
                            {
                                column = cflForm.DataSources.DataTables.Item("cfl").Columns.Item(y).Name;
                                if (col == "U_itemcode")
                                {
                                    if (column == "dscription")
                                    {
                                        oForm.DataSources.DBDataSources.Item(ds).SetValue("U_itemname", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                    }
                                    else if (column == "itemcode")
                                    {
                                        oForm.DataSources.DBDataSources.Item(ds).SetValue("U_itemcode", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                    }
                                    else
                                        if (cflForm.DataSources.DataTables.Item("cfl").Columns.Item(y).Type == SAPbouiCOM.BoFieldsType.ft_Date)
                                        {
                                            try
                                            {
                                                dtvalue = DateTime.Parse(cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                                oForm.DataSources.DBDataSources.Item(ds).SetValue(column, row, dtvalue.ToString("yyyyMMdd"));
                                            }
                                            catch //(Exception ex)
                                            {
                                                //SAP.SBOApplication.MessageBox(column + " - " + ex.Message, 1, "Ok", "", "");
                                            }
                                        }
                                        else
                                            try
                                            {
                                                oForm.DataSources.DBDataSources.Item(ds).SetValue(column, row, cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                            }
                                            catch
                                            {
                                            }
                                }
                                else
                                    if (column == "DocEntry")
                                    {
                                        oForm.DataSources.DBDataSources.Item(ds).SetValue("U_docentry", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                    }
                                    else if (column == "LineId")
                                    {
                                        oForm.DataSources.DBDataSources.Item(ds).SetValue("U_lineid", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                    }
                                    else
                                        if (cflForm.DataSources.DataTables.Item("cfl").Columns.Item(y).Type == SAPbouiCOM.BoFieldsType.ft_Date)
                                        {
                                            try
                                            {
                                                dtvalue = DateTime.Parse(cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                                oForm.DataSources.DBDataSources.Item(ds).SetValue(column, row, dtvalue.ToString("yyyyMMdd"));
                                            }
                                            catch //(Exception ex)
                                            {
                                                //SAP.SBOApplication.MessageBox(column + " - " + ex.Message, 1, "Ok", "", "");
                                            }
                                        }
                                        else
                                            try
                                            {
                                                oForm.DataSources.DBDataSources.Item(ds).SetValue(column, row, cflForm.DataSources.DataTables.Item("cfl").GetValue(y, x).ToString());
                                            }
                                            catch
                                            {
                                            }
                            }
                        }

                        //if (col == "U_itemcode")
                        //{
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_itemcode", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(0, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_itemname", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(1, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_size", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(2, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_jcclr", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(3, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_brand", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(4, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_perfcl", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(5, x).ToString());
                        //}
                        //else if (col == "U_conno")
                        //{
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue(col, row, rtnvalue);
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_conno", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(0, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_blno", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(1, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_vessel", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(2, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_sealno", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(3, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_consize", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(4, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_netw", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(5, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_grossw", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(6, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_measure", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(7, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_batchno", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(8, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_bmd", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(9, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_bed", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(10, x).ToString());
                        //    oForm.DataSources.DBDataSources.Item(ds).SetValue("U_batchrem", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(11, x).ToString());
                        //}
                        first = false;
                    }
                }


                oItem = oForm.Items.Item(matrixname);
                oMatrix.LoadFromDataSource();
                UserForm_ShipDoc.arrangematrix(oForm, oMatrix, ds);
                oMatrix.Columns.Item(col).Cells.Item(row + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                if (rowsadd > 0 && oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                //oItem.Click(SAPbouiCOM.BoCellClickType.ct_Regular);                       
            }
            else
            {
                for (int x = 0; x < oGrid.Rows.Count; x++)
                {
                    if (oGrid.Rows.IsSelected(x))
                    {
                        if (col == "U_booking")
                        {
                            oForm.DataSources.DBDataSources.Item(ds).SetValue("U_booking", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(0, x).ToString());
                            //oForm.DataSources.DBDataSources.Item(ds).SetValue("U_vessel", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(1, x).ToString());
                            //oForm.DataSources.DBDataSources.Item(ds).SetValue("U_loading", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(2, x).ToString());
                            //oForm.DataSources.DBDataSources.Item(ds).SetValue("U_discharg", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(3, x).ToString());
                            //oForm.DataSources.DBDataSources.Item(ds).SetValue("U_shipper", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(4, x).ToString());
                            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                        }
                        if (col == "U_shipper")
                        {
                            oForm.DataSources.DBDataSources.Item(ds).SetValue("U_shipper", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(3, x).ToString());
                            oForm.DataSources.DBDataSources.Item(ds).SetValue("U_consigne", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(4, x).ToString());
                            oForm.DataSources.DBDataSources.Item(ds).SetValue("U_notify", row, cflForm.DataSources.DataTables.Item("cfl").GetValue(5, x).ToString());
                            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                        }
                    }
                }
            }

        }
        public static void aftercfl(string FormUID, string ds, string col, int row, string matrixname, string rtnvalue, SAPbouiCOM.Form cflForm)
        {
            bool overridescript = false;

            SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);
            switch (oForm.TypeEx)
            {
                case "FT_SHIPD":
                    if (col == "U_itemcode" || col == "U_conno")
                    {
                        aftercflcustom(oForm, ds, col, row, matrixname, rtnvalue, cflForm);
                        overridescript = true;
                    }
                    if (col == "U_booking" || col == "U_shipper")
                    {
                        aftercflcustom(oForm, ds, col, row, matrixname, rtnvalue, cflForm);
                        overridescript = true;
                    }
                    break;
            }

            if (overridescript) return;


            SAPbouiCOM.Item oItem = null;
            SAPbouiCOM.Matrix oMatrix = null;

            oForm.DataSources.UserDataSources.Item("cfluid").Value = "";

            if (matrixname != "")
            {
                oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(matrixname).Specific;
                oMatrix.FlushToDataSource();
                row = row - 1;
            }
            switch (FormUID)
            {
                case "":
                    break;
                default:
                    if (ds == "")
                    {
                        if (rtnvalue != null && rtnvalue != "")
                            oForm.DataSources.UserDataSources.Item(col).Value = rtnvalue;
                    }
                    else
                    {
                        if (rtnvalue != null && rtnvalue != "")
                        {
                            oForm.DataSources.DBDataSources.Item(ds).SetValue(col, row, rtnvalue);
                            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                                oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                        }
                    }
                    if (matrixname != "")
                    {
                        oItem = oForm.Items.Item(matrixname);
                        oMatrix.LoadFromDataSource();
                        oMatrix.Columns.Item(col).Cells.Item(row + 1).Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                        if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                            oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                        //oItem.Click(SAPbouiCOM.BoCellClickType.ct_Regular);                       
                    }

                    break;
            }
        }
    }
}
