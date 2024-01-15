using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FT_ADDON.AP_SO
{
    class InitForm
    {
        public static void shiplist(string FUID, long docentry)
        {
            FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize Listing window...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
            creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
            creationPackage.FormType = "FT_SHIPL";
            SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);
            oForm.Title = "Shipping Document List";
            SAPbouiCOM.Form oSForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FUID);
            oForm.Left = oSForm.Left;
            oForm.Width = 600;
            oForm.Top = oSForm.Top;
            oForm.Height = 500;

            oForm.DataSources.UserDataSources.Add("FUID", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 50);
            oForm.DataSources.UserDataSources.Add("sdoc", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 50);

            oForm.DataSources.UserDataSources.Item("FUID").Value = FUID;
            oForm.DataSources.UserDataSources.Item("sdoc").Value = docentry.ToString();

            SAPbouiCOM.Grid oGrid = null;
            SAPbouiCOM.Item oItem = null;

            oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Left = 5;
            oItem.Width = 65;
            oItem.Top = oForm.Height - 60;
            oItem.Height = 20;

            oItem = oForm.Items.Add("grid", SAPbouiCOM.BoFormItemTypes.it_GRID);
            oItem.Left = 5;
            oItem.Width = oForm.Width - 25;
            oItem.Top = 5;
            oItem.Height = oForm.Height - 70;

            oGrid = (SAPbouiCOM.Grid)oItem.Specific;
            oForm.DataSources.DataTables.Add("list");
            try
            {
                string sql = "select docentry, U_sdoc, U_pino, U_docdate, U_booking, U_set, U_shipper, U_consigne, U_notify, U_loading, U_discharg, U_vessel, U_country, U_itemdesc from [@FT_SHIPD] where U_sdoc = " + docentry.ToString();
                oForm.DataSources.DataTables.Item("list").ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }

            oGrid.DataTable = oForm.DataSources.DataTables.Item("list");

            foreach (SAPbouiCOM.GridColumn column in oGrid.Columns)
            {
                column.Editable = false;
                if (column.UniqueID == "docentry" || column.UniqueID == "U_sdoc")
                    column.Visible = false;
                else
                {
                    if (column.UniqueID == "U_pino")
                        column.TitleObject.Caption = "SC No";
                    if (column.UniqueID == "U_docdate")
                        column.TitleObject.Caption = "Date";
                }
            }

            oGrid.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;

            oSForm.State = SAPbouiCOM.BoFormStateEnum.fs_Minimized;
            oSForm.Freeze(true);
            oForm.Visible = true;

            FT_ADDON.SAP.SBOApplication.StatusBar.SetText("", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None);
        }
        public static void shipdoc(string FormUID, long docentry, long shipdocentry, string docnum)
        {
            try
            {
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize popup window...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
                creationPackage.FormType = "FT_SHIPD";
                creationPackage.ObjectType = "FT_SHIPD";
                
                SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);

                oForm.Title = "Shipping Document";
                SAPbouiCOM.Form oSForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);

                //oForm.Left = oSForm.Left;
                oForm.Width = 1000;
                //oForm.Top = oSForm.Top;
                oForm.Height = 600;

                SAPbouiCOM.Item oItem = null;
                SAPbouiCOM.Button oButton = null;
                SAPbouiCOM.EditText oEdit = null;
                SAPbouiCOM.ComboBox oCombo = null;

                oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 5;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 75;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "Cancel";

                SAPbouiCOM.UserDataSource uds = null;

                uds = oForm.DataSources.UserDataSources.Add("cfluid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 50);
                uds.Value = "";

                //oItem = oForm.Items.Add("ROW", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "row");
                //oItem.Left = 55;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;

                //oItem = oForm.Items.Add("st_ROW", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                //((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Row No#";
                //oItem.Left = 5;
                //oItem.Width = 50;
                //oItem.Top = 5;
                //oItem.Height = 15;

                uds = oForm.DataSources.UserDataSources.Add("fuid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = FormUID;

                uds = oForm.DataSources.UserDataSources.Add("docentry", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docentry.ToString();
                uds = oForm.DataSources.UserDataSources.Add("docnum", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docnum;

                SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                oCFLs = oForm.ChooseFromLists;

                SAPbouiCOM.ChooseFromList oCFL = null;
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
                oCFLCreationParams.MultiSelection = false;
                oCFLCreationParams.ObjectType = "4";
                oCFLCreationParams.UniqueID = "CFLitem";
                oCFL = oCFLs.Add(oCFLCreationParams);

                //oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
                //oCFLCreationParams.MultiSelection = false;
                //oCFLCreationParams.ObjectType = "4";
                oCFLCreationParams.UniqueID = "CFLcon";
                oCFL = oCFLs.Add(oCFLCreationParams);

                oCFLCreationParams.UniqueID = "CFLitem1";
                oCFL = oCFLs.Add(oCFLCreationParams);

                oCFLCreationParams.UniqueID = "CFLitem2";
                oCFL = oCFLs.Add(oCFLCreationParams);

                SAPbouiCOM.BoFormItemTypes itemtype = SAPbouiCOM.BoFormItemTypes.it_EDIT;
                //SAPbouiCOM.BoFormItemTypes linktype = SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON;
                SAPbouiCOM.BoFormItemTypes itemtypecmb = SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX;
                SAPbouiCOM.DataTable dt = null;// oForm.DataSources.DataTables.Add("99");

                SAPbouiCOM.Column oColumn = null;
                //SAPbouiCOM.UserDataSource oUds = null;
                SAPbouiCOM.BoDataType datatype = SAPbouiCOM.BoDataType.dt_SHORT_TEXT;
                SAPbobsCOM.UserTable oUT = null;
                SAPbobsCOM.UserFields oUF = null;
                string linkedtable = "";
                SAPbouiCOM.Matrix oMatrix = null;

                string dsname = "";
                int top = -15;
                int cnt = -1;
                string columnname = "";
                Boolean end = false;
                dsname = "FT_SHIPD";
                oForm.DataSources.DBDataSources.Add("@" + dsname);

                top = top + 20;

                columnname = "docnum";
                oItem = oForm.Items.Add("8", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                ((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Doc No";
                oItem.Left = 5;
                oItem.Top = top;
                oItem.Width = 150;
                oItem.Height = 15;

                oItem = oForm.Items.Add(columnname, SAPbouiCOM.BoFormItemTypes.it_EDIT);
                ((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "@" + dsname, columnname);
                oItem.Left = 155;
                oItem.Top = top;
                oItem.Width = 150;
                oItem.Height = 15;
                oItem.LinkTo = ("8").ToString();
                //oItem.Enabled = false;

                oForm.AutoManaged = true;
                oForm.DataBrowser.BrowseBy = "docnum";
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False);

                //columnname = "U_sdoc";
                //oItem = oForm.Items.Add("9", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                //((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Docentry";
                //oItem.Left = oForm.Width - 20 - 155 - 155;
                //oItem.Top = top;
                //oItem.Width = 150;
                //oItem.Height = 15;

                //oItem = oForm.Items.Add(columnname, SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "@" + dsname, columnname);
                //oItem.Left = oForm.Width - 20 - 155;
                //oItem.Top = top;
                //oItem.Width = 150;
                //oItem.Height = 15;
                //oItem.LinkTo = ("9").ToString();
                //oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False);

                oUT = (SAPbobsCOM.UserTable)SAP.SBOCompany.UserTables.Item(dsname);
                oUF = oUT.UserFields;

                SAPbobsCOM.Recordset oSeq = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string sql = "";
                string aliasid = "";
                int seq = 0;

                sql = "select AliasID from CUFD where TableID = '@" + dsname + "' order by U_seq, FieldID";
                aliasid = "";
                oSeq.DoQuery(sql);
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    columnname = oUF.Fields.Item(aliasid).Name;

                    if (columnname != "U_sdoc")
                    {
                        cnt++;

                        if (cnt % 2 == 0)
                        {
                            top = top + 20;
                            end = false;
                        }
                        else
                            end = true;

                        oItem = oForm.Items.Add((seq * 10).ToString(), SAPbouiCOM.BoFormItemTypes.it_STATIC);
                        ((SAPbouiCOM.StaticText)oItem.Specific).Caption = oUF.Fields.Item(aliasid).Description;
                        if (end)
                            oItem.Left = oForm.Width - 60 - 155 - 155;
                        else
                            oItem.Left = 5;
                        oItem.Top = top;
                        oItem.Width = 150;
                        oItem.Height = 15;

                        linkedtable = oUF.Fields.Item(aliasid).LinkedTable;
                        if (linkedtable != "")
                        {
                            oItem = oForm.Items.Add(columnname, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
                            oCombo = (SAPbouiCOM.ComboBox)oItem.Specific;
                            oCombo.DataBind.SetBound(true, "@" + dsname, columnname);

                            dt = oForm.DataSources.DataTables.Add(linkedtable);
                            dt.ExecuteQuery("select code, name from [@" + linkedtable + "]");
                            for (int y = 0; y < dt.Rows.Count; y++)
                            {
                                ((SAPbouiCOM.ComboBox)oItem.Specific).ValidValues.Add(dt.GetValue(0, y).ToString(), dt.GetValue(1, y).ToString());
                            }
                        }
                        else
                        {
                            oItem = oForm.Items.Add(columnname, SAPbouiCOM.BoFormItemTypes.it_EDIT);
                            oEdit = (SAPbouiCOM.EditText)oItem.Specific;
                            oEdit.DataBind.SetBound(true, "@" + dsname, columnname);

                            if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                            {
                                //oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False);
                            }
                            //if (columnname == "U_set")
                            //    oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False);
                        }

                        if (end)
                            oItem.Left = oForm.Width - 60 - 155;
                        else
                            oItem.Left = 155;
                        oItem.Top = top;
                        oItem.Width = 150;
                        oItem.Height = 15;
                        oItem.LinkTo = (seq * 10).ToString();

                        switch (columnname)
                        {
                            case "U_shipper":
                            case "U_itemdesc":
                            case "U_booking":
                            case "U_country":
                            case "U_loading":
                            case "U_discharg":
                                oItem = oForm.Items.Add("c" + columnname.Substring(1, columnname.Length - 1), SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                                if (end)
                                    oItem.Left = oForm.Width - 66;
                                else
                                    oItem.Left = 155 + 149;
                                oItem.Top = top - 2;
                                oItem.Width = 20;
                                oItem.Height = 20;
                                oButton = ((SAPbouiCOM.Button)(oItem.Specific));
                                oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Image;
                                oButton.Image = Application.StartupPath + @"\CFL.BMP";
                                oCFLCreationParams.UniqueID = "c" + columnname.Substring(1, columnname.Length - 1);
                                oCFL = oCFLs.Add(oCFLCreationParams);
                                oButton.ChooseFromListUID = "c" + columnname.Substring(1, columnname.Length - 1);

                                break;
                            case "":
                                oCFLCreationParams.UniqueID = "c" + columnname.Substring(1, columnname.Length - 1);
                                oCFL = oCFLs.Add(oCFLCreationParams);
                                oEdit.ChooseFromListUID = "c" + columnname.Substring(1, columnname.Length - 1);
                                //oEdit.ChooseFromListAlias = "U_booking";
                                break;
                            case "U_consigne":
                            case "U_notify":
                                break;
                        }
                    }

                    oSeq.MoveNext();

                }

                top = top + 20;
                oForm.DataSources.UserDataSources.Add("FolderDS", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);

                SAPbouiCOM.Folder oFolder = null;
                //oItem = oForm.Items.Add("fgrid1", SAPbouiCOM.BoFormItemTypes.it_FOLDER);
                //oItem.Left = 5;
                //oItem.Width = 100;
                //oItem.Top = top;
                //oItem.Height = 19;
                //oItem.AffectsFormMode = false;
                //oFolder = (SAPbouiCOM.Folder)oItem.Specific;
                //oFolder.Caption = "Item Details";
                //oFolder.DataBind.SetBound(true, "", "FolderDS");
                //oFolder.Select();

                oItem = oForm.Items.Add("fgrid2", SAPbouiCOM.BoFormItemTypes.it_FOLDER);
                oItem.Left = 5;
                oItem.Width = 100;
                oItem.Top = top;
                oItem.Height = 19;
                oItem.AffectsFormMode = false;
                oFolder = (SAPbouiCOM.Folder)oItem.Specific;
                oFolder.Caption = "Container Details";
                oFolder.DataBind.SetBound(true, "", "FolderDS");
                oFolder.Select();

                oItem = oForm.Items.Add("fgrid3", SAPbouiCOM.BoFormItemTypes.it_FOLDER);
                oItem.Left = 105;
                oItem.Width = 100;
                oItem.Top = top;
                oItem.Height = 19;
                oItem.AffectsFormMode = false;
                oFolder = (SAPbouiCOM.Folder)oItem.Specific;
                oFolder.Caption = "COA Result";
                oFolder.DataBind.SetBound(true, "", "FolderDS");
                oFolder.GroupWith("fgrid2");

                top = top + 20;

                //grid 1 - start                
                oItem = oForm.Items.Add("grid1", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = top;
                oItem.Height = oForm.Height - top - 60;
                oItem.FromPane = 1;
                oItem.ToPane = 1;
                oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;
                
                dsname = "FT_SHIP1";
                oForm.DataSources.DBDataSources.Add("@" + dsname);
                datatype = ObjectFunctions.changeUIFieldsTypeToUIDataType(oForm.DataSources.DBDataSources.Item("@" + dsname).Fields.Item("VisOrder").Type);
                //datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "VisOrder";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Editable = false;

                oUT = (SAPbobsCOM.UserTable)SAP.SBOCompany.UserTables.Item(dsname);
                oUF = oUT.UserFields;


                sql = "select AliasID from CUFD where TableID = '@" + dsname + "' order by U_seq, FieldID";
                aliasid = "";
                oSeq.DoQuery(sql);
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    columnname = oUF.Fields.Item(aliasid).Name;

                    linkedtable = oUF.Fields.Item(aliasid).LinkedTable;
                    if (linkedtable == "")
                    {
                        //if (columnname == "U_size")
                        //    linkedtable = "0009";
                        //else if (columnname == "U_jcclr")
                        //    linkedtable = "0007";
                        //else if (columnname == "U_brand")
                        //    linkedtable = "0004";
                        //else if (columnname == "U_perfcl")
                        //    linkedtable = "00011";
                    }
                    if (linkedtable != "")
                    {
                        oColumn = oMatrix.Columns.Add(columnname, itemtypecmb);
                        oColumn.DisplayDesc = true;
                        dt = oForm.DataSources.DataTables.Add(linkedtable);
                        dt.ExecuteQuery("select code, name from [@" + linkedtable + "]");
                        for (int y = 0; y < dt.Rows.Count; y++)
                        {
                            oColumn.ValidValues.Add(dt.GetValue(0, y).ToString(), dt.GetValue(1, y).ToString());                            
                        }
                        
                    }
                    else
                    {
                        //if (columnname == "U_itemcode")
                        //{
                        //    oColumn = oMatrix.Columns.Add(columnname, linktype);
                        //}
                        //else
                        oColumn = oMatrix.Columns.Add(columnname, itemtype);
                    }
                    oColumn.TitleObject.Caption = oUF.Fields.Item(aliasid).Description;
                    if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                        oColumn.Width = 200;
                    else
                        oColumn.Width = 100;

                    oColumn.DataBind.SetBound(true, "@" + dsname, columnname);

                    if (columnname == "U_itemcode")
                    {
                        oColumn.ChooseFromListUID = "CFLitem";
                        //((SAPbouiCOM.LinkedButton)oColumn.ExtendedObject).LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Items;
                    }

                    oSeq.MoveNext();
                }
                
                //grid 2 - start
                oItem = oForm.Items.Add("grid2", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = top;
                oItem.Height = oForm.Height - top - 60;
                oItem.FromPane = 2;
                oItem.ToPane = 2;
                oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;

                dsname = "FT_SHIP2";
                oForm.DataSources.DBDataSources.Add("@" + dsname);
                datatype = ObjectFunctions.changeUIFieldsTypeToUIDataType(oForm.DataSources.DBDataSources.Item("@" + dsname).Fields.Item("VisOrder").Type);
                //datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "VisOrder";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Editable = false;

                oUT = (SAPbobsCOM.UserTable)SAP.SBOCompany.UserTables.Item(dsname);
                oUF = oUT.UserFields;
                // aaaaaa

                sql = "select AliasID from CUFD where TableID = '@" + dsname + "' order by U_seq, FieldID";
                aliasid = "";
                oSeq.DoQuery(sql);
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    columnname = oUF.Fields.Item(aliasid).Name;
                    {
                        if (columnname == "U_conno" || columnname == "U_itemcode" || columnname == "U_itemname" || columnname == "U_desc" || columnname == "U_perfcl")
                        {
                        }
                        else
                        {
                            oSeq.MoveNext();
                            continue;
                        }

                        linkedtable = oUF.Fields.Item(aliasid).LinkedTable;
                        if (linkedtable == "")
                        {
                            //if (columnname == "U_consize")
                            //    linkedtable = "00021";
                            if (columnname == "U_perfcl")
                                linkedtable = "00011";
                        }
                        if (linkedtable != "")
                        {
                            oColumn = oMatrix.Columns.Add(columnname, itemtypecmb);
                            oColumn.DisplayDesc = true;
                            dt = oForm.DataSources.DataTables.Add(linkedtable);
                            dt.ExecuteQuery("select code, name from [@" + linkedtable + "]");
                            for (int y = 0; y < dt.Rows.Count; y++)
                            {
                                oColumn.ValidValues.Add(dt.GetValue(0, y).ToString(), dt.GetValue(1, y).ToString());
                            }
                        }
                        else
                        {
                            oColumn = oMatrix.Columns.Add(columnname, itemtype);
                        }
                        datatype = ObjectFunctions.changeDIFieldTypesToDIDataType(oUF.Fields.Item(aliasid).Type);
                        oColumn.TitleObject.Caption = oUF.Fields.Item(aliasid).Description;
                        if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                            oColumn.Width = 200;
                        else
                            oColumn.Width = 100;

                        oColumn.DataBind.SetBound(true, "@" + dsname, columnname);

                        if (columnname == "U_conno")
                        {
                            oColumn.ChooseFromListUID = "CFLcon";
                        }
                        if (columnname == "U_item1")
                        {
                            oColumn.ChooseFromListUID = "CFLitem1";
                        }
                        if (columnname == "U_item2")
                        {
                            oColumn.ChooseFromListUID = "CFLitem2";
                        }
                        if (columnname == "U_docentry" || columnname == "U_lineid")
                            oColumn.Editable = false;
                    }

                    oSeq.MoveNext();
                }

                // bbbbbb
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    columnname = oUF.Fields.Item(aliasid).Name;
                    {
                        if (columnname == "U_conno" || columnname == "U_itemcode" || columnname == "U_itemname" || columnname == "U_desc" || columnname == "U_perfcl")
                        {
                            oSeq.MoveNext();
                            continue;
                        }

                        linkedtable = oUF.Fields.Item(aliasid).LinkedTable;
                        if (linkedtable == "")
                        {
                            //if (columnname == "U_consize")
                            //    linkedtable = "00021";
                            if (columnname == "U_perfcl")
                                linkedtable = "00011";
                        }
                        if (linkedtable != "")
                        {
                            oColumn = oMatrix.Columns.Add(columnname, itemtypecmb);
                            oColumn.DisplayDesc = true;
                            dt = oForm.DataSources.DataTables.Add(linkedtable);
                            dt.ExecuteQuery("select code, name from [@" + linkedtable + "]");
                            for (int y = 0; y < dt.Rows.Count; y++)
                            {
                                oColumn.ValidValues.Add(dt.GetValue(0, y).ToString(), dt.GetValue(1, y).ToString());
                            }
                        }
                        else
                        {
                            oColumn = oMatrix.Columns.Add(columnname, itemtype);
                        }
                        datatype = ObjectFunctions.changeDIFieldTypesToDIDataType(oUF.Fields.Item(aliasid).Type);
                        oColumn.TitleObject.Caption = oUF.Fields.Item(aliasid).Description;
                        if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                            oColumn.Width = 200;
                        else
                            oColumn.Width = 100;

                        oColumn.DataBind.SetBound(true, "@" + dsname, columnname);

                        if (columnname == "U_conno")
                        {
                            oColumn.ChooseFromListUID = "CFLcon";
                        }
                        if (columnname == "U_item1")
                        {
                            oColumn.ChooseFromListUID = "CFLitem1";
                        }
                        if (columnname == "U_item2")
                        {
                            oColumn.ChooseFromListUID = "CFLitem2";
                        }
                        if (columnname == "U_docentry" || columnname == "U_lineid")
                            oColumn.Editable = false;
                    }

                    oSeq.MoveNext();
                }

                //grid 3 - start
                oItem = oForm.Items.Add("grid3", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = top;
                oItem.Height = oForm.Height - top - 60;
                oItem.FromPane = 3;
                oItem.ToPane = 3;
                oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;

                dsname = "FT_SHIP3";
                oForm.DataSources.DBDataSources.Add("@" + dsname);
                datatype = ObjectFunctions.changeUIFieldsTypeToUIDataType(oForm.DataSources.DBDataSources.Item("@" + dsname).Fields.Item("VisOrder").Type);
                //datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "VisOrder";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Editable = false;

                oUT = (SAPbobsCOM.UserTable)SAP.SBOCompany.UserTables.Item(dsname);
                oUF = oUT.UserFields;


                sql = "select AliasID from CUFD where TableID = '@" + dsname + "' order by U_seq, FieldID";
                aliasid = "";
                oSeq.DoQuery(sql);
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    columnname = oUF.Fields.Item(aliasid).Name;

                    linkedtable = oUF.Fields.Item(aliasid).LinkedTable;
                    if (columnname == "U_prodtype")
                    {
                        linkedtable = "0010";
                    }
                    if (linkedtable != "")
                    {
                        oColumn = oMatrix.Columns.Add(columnname, itemtypecmb);
                        oColumn.DisplayDesc = true;
                        dt = oForm.DataSources.DataTables.Add(linkedtable);
                        dt.ExecuteQuery("select code, name from [@" + linkedtable + "]");
                        for (int y = 0; y < dt.Rows.Count; y++)
                        {
                            oColumn.ValidValues.Add(dt.GetValue(0, y).ToString(), dt.GetValue(1, y).ToString());
                        }
                    }
                    else
                    {
                            oColumn = oMatrix.Columns.Add(columnname, itemtype);
                    }
                    datatype = ObjectFunctions.changeDIFieldTypesToDIDataType(oUF.Fields.Item(aliasid).Type);
                    oColumn.TitleObject.Caption = oUF.Fields.Item(aliasid).Description;
                    if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                        oColumn.Width = 200;
                    else
                        oColumn.Width = 100;

                    oColumn.DataBind.SetBound(true, "@" + dsname, columnname);

                    oSeq.MoveNext();

                }
                // grid - end
                //UserForm_CONmodified.retrieveRow(oForm, docentry, linenum, dsname);

                //ObjectFunctions.customFormMatrixSetting(oForm, "grid1", SAP.SBOCompany.UserName, dsname);

                SAPbouiCOM.Condition oCon = null;
                SAPbouiCOM.Conditions oCons = new SAPbouiCOM.Conditions();

                SAPbouiCOM.Conditions oCons1 = new SAPbouiCOM.Conditions();

                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                if (shipdocentry > 0)
                {
                    oCon = oCons1.Add();
                    oCon.BracketOpenNum = 1;
                    oCon.Alias = "docentry";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = shipdocentry.ToString();
                    oCon.BracketCloseNum = 1;

                    oForm.DataSources.DBDataSources.Item("@FT_SHIPD").Query(oCons1);
                    oForm.DataSources.DBDataSources.Item("@FT_SHIP1").Query(oCons1);
                    oForm.DataSources.DBDataSources.Item("@FT_SHIP2").Query(oCons1);
                    oForm.DataSources.DBDataSources.Item("@FT_SHIP3").Query(oCons1);
                    ((SAPbouiCOM.Matrix)oForm.Items.Item("grid1").Specific).LoadFromDataSource();
                    ((SAPbouiCOM.Matrix)oForm.Items.Item("grid2").Specific).LoadFromDataSource();
                    ((SAPbouiCOM.Matrix)oForm.Items.Item("grid3").Specific).LoadFromDataSource();

                    oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                }
                else
                {                    
                    oCon = oCons.Add();
                    oCon.BracketOpenNum = 1;
                    oCon.Alias = "U_sdoc";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = oForm.DataSources.UserDataSources.Item("docentry").Value.ToString();
                    oCon.BracketCloseNum = 1;

                    oForm.DataSources.DBDataSources.Item("@FT_SHIPD").Query(oCons);

                    if (oForm.DataSources.DBDataSources.Item("@FT_SHIPD").Size == 0)
                    {
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;


                        oForm.DataSources.DBDataSources.Item("@FT_SHIPD").SetValue("U_sdoc", 0, docentry.ToString());
                        oForm.DataSources.DBDataSources.Item("@FT_SHIPD").SetValue("U_pino", 0, docnum);
                        
                        oForm.DataSources.DBDataSources.Item("@FT_SHIP1").SetValue("VisOrder", 0, "1");
                        oForm.DataSources.DBDataSources.Item("@FT_SHIP2").SetValue("VisOrder", 0, "1");
                        oForm.DataSources.DBDataSources.Item("@FT_SHIP3").SetValue("VisOrder", 0, "1");
                        ((SAPbouiCOM.Matrix)oForm.Items.Item("grid1").Specific).LoadFromDataSource();
                        ((SAPbouiCOM.Matrix)oForm.Items.Item("grid2").Specific).LoadFromDataSource();
                        ((SAPbouiCOM.Matrix)oForm.Items.Item("grid3").Specific).LoadFromDataSource();
                        //((SAPbouiCOM.Matrix)oForm.Items.Item("grid1").Specific).AddRow(1, -1);                   
                        //((SAPbouiCOM.Matrix)oForm.Items.Item("grid2").Specific).AddRow(1, -1);
                        //((SAPbouiCOM.Matrix)oForm.Items.Item("grid3").Specific).AddRow(1, -1);

                        rs.DoQuery("select max(U_set) from [@FT_SHIPD] where U_sdoc = " + docentry.ToString());
                        if (rs.RecordCount > 0)
                        {
                            rs.MoveFirst();
                            int set = int.Parse(rs.Fields.Item(0).Value.ToString()) + 1;
                            oForm.DataSources.DBDataSources.Item("@FT_SHIPD").SetValue("U_set", 0, set.ToString());
                        }
                        //rs.DoQuery("select max(docentry) from [@FT_SHIPD]");
                        //if (rs.RecordCount > 0)
                        //{
                        //    long docnum = long.Parse(rs.Fields.Item(0).Value.ToString()) + 1;
                        //    oForm.DataSources.DBDataSources.Item("@FT_SHIPD").SetValue("docnum", 0, docnum.ToString());
                        //}
                    }
                    else
                    {
                        oCon = oCons1.Add();
                        oCon.BracketOpenNum = 1;
                        oCon.Alias = "docentry";
                        oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                        oCon.CondVal = oForm.DataSources.DBDataSources.Item("@FT_SHIPD").GetValue("docentry", 0).ToString();
                        oCon.BracketCloseNum = 1;

                        oForm.DataSources.DBDataSources.Item("@FT_SHIPD").Query(oCons1);
                        oForm.DataSources.DBDataSources.Item("@FT_SHIP1").Query(oCons1);
                        oForm.DataSources.DBDataSources.Item("@FT_SHIP2").Query(oCons1);
                        oForm.DataSources.DBDataSources.Item("@FT_SHIP3").Query(oCons1);
                        ((SAPbouiCOM.Matrix)oForm.Items.Item("grid1").Specific).LoadFromDataSource();
                        ((SAPbouiCOM.Matrix)oForm.Items.Item("grid2").Specific).LoadFromDataSource();
                        ((SAPbouiCOM.Matrix)oForm.Items.Item("grid3").Specific).LoadFromDataSource();

                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    }
                }
                rs = null;

                oForm.Items.Item("fgrid2").Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                oSForm.State = SAPbouiCOM.BoFormStateEnum.fs_Minimized;
                oSForm.Freeze(true);
                //oForm.PaneLevel = 1;
                oForm.Visible = true;
                //oMatrix.Columns.Item(1).Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("popup window initialize completed!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");

            }
        }
        public static void TEXT(string FormUID, long docentry, int linenum, int row, string dsname, string matrixname, string value)
        {
            try
            {
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize popup window...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
                creationPackage.FormType = "FT_TEXT";
                creationPackage.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed;
                //creationPackage.FormType 
                SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);
                oForm.Title = "Row Text Detail...";
                //oForm.Left = oSForm.Left;
                oForm.Width = 500;
                //oForm.Top = oSForm.Top;
                oForm.Height = 400;

                SAPbouiCOM.Item oItem;
                SAPbouiCOM.Button oButton;

                oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 5;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 75;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "Cancel";

                SAPbouiCOM.UserDataSource uds = null;

                uds = oForm.DataSources.UserDataSources.Add("DocEntry", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docentry.ToString();

                //oItem = oForm.Items.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).String = docentry.ToString();

                //oItem.Left = 140;
                //oItem.Width = 10;
                //oItem.Top = oForm.Height - 60;
                //oItem.Height = 20;
                //oItem.Enabled = true;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("LineNo", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = linenum.ToString();

                //oItem = oForm.Items.Add("LineNo", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).String = linenum.ToString();

                //oItem.Left = 150;
                //oItem.Width = 10;
                //oItem.Top = oForm.Height - 60;
                //oItem.Height = 20;
                //oItem.Enabled = true;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("FUID", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = FormUID;

                //oItem = oForm.Items.Add("FUID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "fuid");
                //oItem.Left = 160;
                //oItem.Width = 10;
                //oItem.Top = oForm.Height - 60;
                //oItem.Height = 20;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("DSNAME", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = dsname;

                //oItem = oForm.Items.Add("DSNAME", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).String = dsname;
                //oItem.Left = 160;
                //oItem.Width = 10;
                //oItem.Top = oForm.Height - 60;
                //oItem.Height = 20;

                uds = oForm.DataSources.UserDataSources.Add("text", SAPbouiCOM.BoDataType.dt_LONG_TEXT);
                uds.Value = value;

                oItem = oForm.Items.Add("TEXT", SAPbouiCOM.BoFormItemTypes.it_EXTEDIT);
                ((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "text");

                oItem.Left = 5;
                oItem.Width = oForm.Width - 30;
                oItem.Top = 5;
                oItem.Height = oForm.Height - 90;
                oItem.Enabled = true;
                oItem.Visible = true;

                //oForm.Items.Item("FUID").Visible = false;
                //oForm.Items.Item("LineNo").Visible = false;
                //oForm.Items.Item("DocEntry").Visible = false;
                //oForm.Items.Item("DSNAME").Visible = false;

                oForm.DataSources.DBDataSources.Add("INV1");

                oItem = oForm.Items.Item("1");
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";
                oForm.Visible = true;
                //oForm.Modal = true;
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                //oForm
                //oSForm.State = SAPbouiCOM.BoFormStateEnum.;
                //oSForm.Freeze(true);

                //((SAPbouiCOM.EditText)oSForm.Items.Item("FUID").Specific).Value = oForm.UniqueID.ToString();
                SAPbouiCOM.Form oSForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);
                oSForm.DataSources.UserDataSources.Item("cfluid").Value = oForm.UniqueID;

                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("popup window initialize completed!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
        public static void CONM(string FormUID, long docentry, int linenum, int row, string dsname, string mustcol)
        {
            try
            {
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize popup window...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning); 
                SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
                creationPackage.FormType = "FT_CONM";
                SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);
                oForm.Title = "Container";
                //oForm.Left = oSForm.Left;
                oForm.Width = 600;
                //oForm.Top = oSForm.Top;
                oForm.Height = 500;

                SAPbouiCOM.Item oItem;
                SAPbouiCOM.Button oButton;

                oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 5;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 75;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "Cancel";

                SAPbouiCOM.UserDataSource uds = null;

                uds = oForm.DataSources.UserDataSources.Add("row", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = row.ToString();

                oItem = oForm.Items.Add("ROW", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                ((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "row");
                oItem.Left = 55;
                oItem.Width = 65;
                oItem.Top = 5;
                oItem.Height = 15;
                oItem.Enabled = false;

                oItem = oForm.Items.Add("st_ROW", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                ((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Row No#";
                oItem.Left = 5;
                oItem.Width = 50;
                oItem.Top = 5;
                oItem.Height = 15;

                uds = oForm.DataSources.UserDataSources.Add("fuid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = FormUID;

                //oItem = oForm.Items.Add("FUID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "fuid");
                //oItem.Left = 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("docentry", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docentry.ToString();

                //oItem = oForm.Items.Add("DOCENTRY", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "docentry");
                //oItem.Left = 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("linenum", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = linenum.ToString();

                //oItem = oForm.Items.Add("LINENUM", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "linenum");
                //oItem.Left = 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("ds", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = dsname;

                //oItem = oForm.Items.Add("DS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "ds");
                //oItem.Left = 5 + 65 + 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("mustcol", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = mustcol;

                //oItem = oForm.Items.Add("MUSTCOL", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "mustcol");
                //oItem.Left = 5 + 65 + 5 + 65 + 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                oItem = oForm.Items.Add("grid1", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = 25;
                oItem.Height = oForm.Height - 90;
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;

                SAPbouiCOM.BoFormItemTypes itemtype = SAPbouiCOM.BoFormItemTypes.it_EDIT;
                SAPbouiCOM.BoFormItemTypes itemtypecmb = SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX;

                SAPbouiCOM.Column oColumn = null;
                //SAPbouiCOM.UserDataSource oUds = null;
                SAPbouiCOM.BoDataType datatype = SAPbouiCOM.BoDataType.dt_SHORT_TEXT;
                string columnname = "";

                oForm.DataSources.DBDataSources.Add("@" + dsname);
                datatype = ObjectFunctions.changeUIFieldsTypeToUIDataType(oForm.DataSources.DBDataSources.Item("@" + dsname).Fields.Item("VisOrder").Type);
                //datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "VisOrder";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Editable = false;
                

                SAPbobsCOM.UserTable oUT = (SAPbobsCOM.UserTable)SAP.SBOCompany.UserTables.Item(dsname);
                SAPbobsCOM.UserFields oUF = null;
                oUF = oUT.UserFields;
                string linkedtable = "";
                ////////////////////////////
                //string alias = "";
                //string[] descpt = { "", "", "", "", "" };
                //int temp = 0;
                //SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                ////oRec.DoQuery("SELECT AliasID from CUFD where TableID='@" + dsname + "' order by Descr");
                //oRec.DoQuery("SELECT AliasID from CUFD where TableID='@" + dsname + "' order by U_seq, FieldID");

                //if (oRec.RecordCount > 0)
                //{
                //    oRec.MoveFirst();
                //    while (!oRec.EoF)
                //    {
                //        alias = oRec.Fields.Item(0).Value.ToString();
                //        columnname = oUF.Fields.Item("U_" + alias).Name;
                //        descpt = oUF.Fields.Item(columnname).Description.Split('|');

                //        if (descpt.GetUpperBound(0) > 1)
                //            if (int.TryParse(descpt[2], out temp))
                //            {
                //                if (temp == 0)
                //                {
                //                    oRec.MoveNext();
                //                    continue;
                //                }
                //            }


                //        linkedtable = oUF.Fields.Item(columnname).LinkedTable;
                //        //if (linkedtable != "")
                //        //{
                //        //    oColumn = oMatrix.Columns.Add(columnname, itemtypecmb);
                //        //}
                //        //else
                //        //{
                //        oColumn = oMatrix.Columns.Add(columnname, itemtype);
                //        //}
                //        datatype = ObjectFunctions.changeDIFieldTypesToDIDataType(oUF.Fields.Item(columnname).Type);
                //        descpt = oUF.Fields.Item(columnname).Description.Split('|');
                //        if (int.TryParse(descpt[0], out temp))
                //        {
                //            oColumn.TitleObject.Caption = descpt[1];
                //        }
                //        else
                //        {
                //            oColumn.TitleObject.Caption = oUF.Fields.Item(columnname).Description;
                //        }
                //        if (oUF.Fields.Item(columnname).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                //            oColumn.Width = 200;
                //        else
                //            oColumn.Width = 100;
                //        //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, oUF.Fields.Item(x).Size);
                //        oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                //        if (columnname == "U_LINENO")
                //            oColumn.Visible = false;
                //        else if (descpt.GetUpperBound(0) > 1)
                //            if (int.TryParse(descpt[2], out temp))
                //                if (temp == 1)
                //                {
                //                    oColumn.Editable = false;
                //                }

                //        //else if (columnname == "U_CONNO")
                //        //{
                //        //    SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                //        //    oCFLs = oForm.ChooseFromLists;

                //        //    SAPbouiCOM.ChooseFromList oCFL = null;
                //        //    SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                //        //    oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                //        //    oCFLCreationParams.MultiSelection = false;
                //        //    oCFLCreationParams.ObjectType = "1";
                //        //    oCFLCreationParams.UniqueID = "CFL1";

                //        //    oCFL = oCFLs.Add(oCFLCreationParams);

                //        //    oColumn.ChooseFromListUID = "CFL1";
                //        //}

                //        oRec.MoveNext();
                //    }
                //}
                ////////////////////////////////

                SAPbobsCOM.Recordset oSeq = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string sql = "";
                string aliasid = "";
                int seq = 0;

                sql = "select AliasID from CUFD where TableID = '@" + dsname + "' order by U_seq, FieldID";
                aliasid = "";
                oSeq.DoQuery(sql);
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    //if (x == 1) x = 2;
                    //else if (x == 2) x = 1;

                    columnname = oUF.Fields.Item(aliasid).Name;
                    //if (columnname == "U_DOCNO" || columnname == "U_LINENO")
                    //    continue;
                    linkedtable = oUF.Fields.Item(aliasid).LinkedTable;
                    //if (linkedtable != "")
                    //{
                    //    oColumn = oMatrix.Columns.Add(columnname, itemtypecmb);
                    //}
                    //else
                    //{
                        oColumn = oMatrix.Columns.Add(columnname, itemtype);
                    //}
                    datatype = ObjectFunctions.changeDIFieldTypesToDIDataType(oUF.Fields.Item(aliasid).Type);
                    oColumn.TitleObject.Caption = oUF.Fields.Item(aliasid).Description;
                    if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                        oColumn.Width = 200;
                    else
                        oColumn.Width = 100;
                    //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, oUF.Fields.Item(x).Size);
                    oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                    if (columnname == "U_LINENO")
                        oColumn.Visible = false;
                    
                    //else if (columnname == "U_CONNO")
                    //{
                    //    SAPbouiCOM.ChooseFromListCollection oCFLs = null;
                    //    oCFLs = oForm.ChooseFromLists;

                    //    SAPbouiCOM.ChooseFromList oCFL = null;
                    //    SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                    //    oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));

                    //    oCFLCreationParams.MultiSelection = false;
                    //    oCFLCreationParams.ObjectType = "1";
                    //    oCFLCreationParams.UniqueID = "CFL1";

                    //    oCFL = oCFLs.Add(oCFLCreationParams);

                    //    oColumn.ChooseFromListUID = "CFL1";
                    //}
                    //if (x == 2) x = 1;
                    //else if (x == 1) x = 2;

                    oSeq.MoveNext();
                }
                

                /*
                datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "#";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "", columnname);
                oColumn.Editable = false;

                datatype = SAPbouiCOM.BoDataType.dt_SHORT_TEXT;
                columnname = "U_CONNO";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Container No";
                oColumn.Width = 100;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 100);
                oColumn.DataBind.SetBound(true, "", columnname);

                datatype = SAPbouiCOM.BoDataType.dt_DATE;
                columnname = "U_CONDATE";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Container Date";
                oColumn.Width = 100;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "", columnname);

                datatype = SAPbouiCOM.BoDataType.dt_LONG_TEXT;
                columnname = "U_REF";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Reference";
                oColumn.Width = 300;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 500);
                oColumn.DataBind.SetBound(true, "", columnname);

                datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "U_ITEMNO";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Item No";
                oColumn.Width = 100;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "", columnname);
                oColumn.Visible = false;
                */

                datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "LineId";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Item No";
                oColumn.Width = 100;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Visible = false;

                UserForm_CONmodified.retrieveRow(oForm, docentry, linenum, dsname);

                ObjectFunctions.customFormMatrixSetting(oForm, "grid1", SAP.SBOCompany.UserName, dsname);

                //uds = oForm.DataSources.UserDataSources.Add("cfluid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 50);
                //uds.Value = "";

                oForm.Visible = true;
                oMatrix.Columns.Item(1).Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("popup window initialize completed!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success); 

            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");

            }
        }
        public static void DOPTM(string FormUID, long docentry, int linenum, int row, string dsname, string mustcol)
        {
            try
            {
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize popup window...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
                creationPackage.FormType = "FT_DOPTM";
                SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);
                oForm.Title = "Product Type Analysis";
                //oForm.Left = oSForm.Left;
                oForm.Width = 600;
                //oForm.Top = oSForm.Top;
                oForm.Height = 500;

                SAPbouiCOM.Item oItem;
                SAPbouiCOM.Button oButton;

                oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 5;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 75;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "Cancel";

                SAPbouiCOM.UserDataSource uds = null;

                uds = oForm.DataSources.UserDataSources.Add("row", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = row.ToString();

                oItem = oForm.Items.Add("ROW", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                ((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "row");
                oItem.Left = 55;
                oItem.Width = 65;
                oItem.Top = 5;
                oItem.Height = 15;
                oItem.Enabled = false;

                oItem = oForm.Items.Add("st_ROW", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                ((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Row No#";
                oItem.Left = 5;
                oItem.Width = 50;
                oItem.Top = 5;
                oItem.Height = 15;

                uds = oForm.DataSources.UserDataSources.Add("fuid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = FormUID;

                //oItem = oForm.Items.Add("FUID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "fuid");
                //oItem.Left = 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("docentry", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docentry.ToString();

                //oItem = oForm.Items.Add("DOCENTRY", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "docentry");
                //oItem.Left = 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("linenum", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = linenum.ToString();

                //oItem = oForm.Items.Add("LINENUM", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "linenum");
                //oItem.Left = 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("ds", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = dsname;

                //oItem = oForm.Items.Add("DS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "ds");
                //oItem.Left = 5 + 65 + 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("mustcol", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = mustcol;

                //oItem = oForm.Items.Add("MUSTCOL", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "mustcol");
                //oItem.Left = 5 + 65 + 5 + 65 + 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                oItem = oForm.Items.Add("grid1", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = 25;
                oItem.Height = oForm.Height - 90;
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;

                SAPbouiCOM.BoFormItemTypes itemtype = SAPbouiCOM.BoFormItemTypes.it_EDIT;
                SAPbouiCOM.Column oColumn = null;
                //SAPbouiCOM.UserDataSource oUds = null;
                SAPbouiCOM.BoDataType datatype = SAPbouiCOM.BoDataType.dt_SHORT_TEXT;
                string columnname = "";

                oForm.DataSources.DBDataSources.Add("@" + dsname);
                datatype = ObjectFunctions.changeUIFieldsTypeToUIDataType(oForm.DataSources.DBDataSources.Item("@" + dsname).Fields.Item("VisOrder").Type);
                //datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "VisOrder";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Editable = false;

                SAPbobsCOM.UserTable oUT = (SAPbobsCOM.UserTable)SAP.SBOCompany.UserTables.Item(dsname);
                SAPbobsCOM.UserFields oUF = null;
                oUF = oUT.UserFields;

                SAPbobsCOM.Recordset oSeq = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string sql = "";
                string aliasid = "";
                int seq = 0;

                sql = "select AliasID from CUFD where TableID = '@" + dsname + "' order by U_seq, FieldID";
                aliasid = "";
                oSeq.DoQuery(sql);
                oSeq.MoveFirst();
                seq = -1;

                while (!oSeq.EoF)
                {
                    seq++;

                    aliasid = "U_" + oSeq.Fields.Item(0).Value.ToString();

                    columnname = oUF.Fields.Item(aliasid).Name;
                    //if (columnname == "U_DOCNO" || columnname == "U_LINENO")
                    //    continue;
                    oColumn = oMatrix.Columns.Add(columnname, itemtype);
                    datatype = ObjectFunctions.changeDIFieldTypesToDIDataType(oUF.Fields.Item(aliasid).Type);
                    oColumn.TitleObject.Caption = oUF.Fields.Item(aliasid).Description;
                    if (oUF.Fields.Item(aliasid).Type == SAPbobsCOM.BoFieldTypes.db_Memo)
                        oColumn.Width = 200;
                    else
                        oColumn.Width = 100;
                    //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, oUF.Fields.Item(x).Size);
                    oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                    if (columnname == "U_LINENO")
                        oColumn.Visible = false;

                    oSeq.MoveNext();
                }
                /*
                datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "#";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "#";
                oColumn.Width = 20;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "", columnname);
                oColumn.Editable = false;

                datatype = SAPbouiCOM.BoDataType.dt_SHORT_TEXT;
                columnname = "U_CONNO";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Container No";
                oColumn.Width = 100;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 100);
                oColumn.DataBind.SetBound(true, "", columnname);

                datatype = SAPbouiCOM.BoDataType.dt_DATE;
                columnname = "U_CONDATE";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Container Date";
                oColumn.Width = 100;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "", columnname);

                datatype = SAPbouiCOM.BoDataType.dt_LONG_TEXT;
                columnname = "U_REF";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Reference";
                oColumn.Width = 300;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 500);
                oColumn.DataBind.SetBound(true, "", columnname);

                datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "U_ITEMNO";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Item No";
                oColumn.Width = 100;
                oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "", columnname);
                oColumn.Visible = false;
                */

                datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                columnname = "LineId";
                oColumn = oMatrix.Columns.Add(columnname, itemtype);
                oColumn.TitleObject.Caption = "Item No";
                oColumn.Width = 100;
                //oUds = oForm.DataSources.UserDataSources.Add(columnname, datatype, 0);
                oColumn.DataBind.SetBound(true, "@" + dsname, columnname);
                oColumn.Visible = false;

                UserForm_CONmodified.retrieveRow(oForm, docentry, linenum, dsname);

                //ObjectFunctions.customFormMatrixSetting(oForm, "grid1", SAP.SBOCompany.UserName, dsname);
                
                oForm.Visible = true;
                oMatrix.Columns.Item(1).Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("popup window initialize completed!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success); 

            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");

            }
        }
        public static void SOM(string FormUID, long docentry, int linenum, int row, string dsname, string matrixname)
        {
            try
            {
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize popup window...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                SAPbouiCOM.Form oSForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);

                SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
                creationPackage.FormType = "FT_SOM";
                SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);
                oForm.Title = "Sales Order Modified";
                //oForm.Left = oSForm.Left;
                oForm.Width = 700;
                //oForm.Top = oSForm.Top;
                oForm.Height = 500;

                SAPbouiCOM.Item oItem;
                SAPbouiCOM.Button oButton;

                oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 5;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 75;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "Cancel";

                SAPbouiCOM.UserDataSource uds = null;

                uds = oForm.DataSources.UserDataSources.Add("row", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = row.ToString();

                oItem = oForm.Items.Add("ROW", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                ((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "row");
                oItem.Left = 55;
                oItem.Width = 65;
                oItem.Top = 5;
                oItem.Height = 15;
                oItem.Enabled = false;

                oItem = oForm.Items.Add("st_ROW", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                ((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Row No#";
                oItem.Left = 5;
                oItem.Width = 50;
                oItem.Top = 5;
                oItem.Height = 15;

                uds = oForm.DataSources.UserDataSources.Add("fuid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = FormUID;

                //oItem = oForm.Items.Add("FUID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "fuid");
                //oItem.Left = 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("docentry", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docentry.ToString();

                //oItem = oForm.Items.Add("DOCENTRY", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "docentry");
                //oItem.Left = 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("linenum", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = linenum.ToString();

                //oItem = oForm.Items.Add("LINENUM", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "linenum");
                //oItem.Left = 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("ds", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = dsname;

                //oItem = oForm.Items.Add("DS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "ds");
                //oItem.Left = 5 + 65 + 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                SAPbouiCOM.Matrix oSMatrix = (SAPbouiCOM.Matrix)oSForm.Items.Item(matrixname).Specific;

                oItem = oForm.Items.Add("grid1", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = 25;
                oItem.Height = oForm.Height - 90;
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;

                if (oSMatrix.RowCount > 0)
                {
                    SAPbouiCOM.Conditions oCons = (SAPbouiCOM.Conditions)SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                    SAPbouiCOM.DBDataSource oDS = (SAPbouiCOM.DBDataSource)oForm.DataSources.DBDataSources.Add(dsname);

                    oDS.Clear();
                    SAPbouiCOM.Condition oCon = oCons.Add();
                    oCon.BracketOpenNum = 1;
                    oCon.Alias = "DocEntry";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = docentry.ToString();
                    oCon.BracketCloseNum = 1;
                    oCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                    oCon = oCons.Add();
                    oCon.BracketOpenNum = 1;
                    oCon.Alias = "DelivrdQty";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_THAN;
                    oCon.CondVal = "0";
                    oCon.BracketCloseNum = 1;

                    SAPbouiCOM.Column oColumn = null;
                    oColumn = oMatrix.Columns.Add("LineNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                    oColumn.DataBind.SetBound(true, dsname, "LineNum");
                    oColumn.Visible = false;

                    copyUDFMatrixColumns(oSForm, oSMatrix, oForm, oMatrix, dsname, dsname);

                    oDS.Query(oCons);
                    oMatrix.LoadFromDataSource();

                    string templinenum = "";
                    string temprow = "";
                    for (int x = 0; x < oSForm.DataSources.DBDataSources.Item(dsname).Size; x++)
                    {
                        if (decimal.Parse(oSForm.DataSources.DBDataSources.Item(dsname).GetValue("DelivrdQty", x).ToString()) > 0)
                        {
                            templinenum = oSForm.DataSources.DBDataSources.Item(dsname).GetValue("LineNum", x).ToString();
                            temprow = ((SAPbouiCOM.EditText)oSMatrix.Columns.Item("0").Cells.Item(x + 1).Specific).String;
                            for (int y = 1; y <= oMatrix.RowCount; y++)
                            {
                                if (((SAPbouiCOM.EditText)oMatrix.Columns.Item("LineNum").Cells.Item(y).Specific).String == templinenum)
                                {
                                    ((SAPbouiCOM.EditText)oMatrix.Columns.Item("0").Cells.Item(y).Specific).String = temprow;
                                }
                            }
                        }
                    }
                }

                oItem = oForm.Items.Item("1");
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                //oForm.Items.Item("TEMP").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                //oForm.Items.Item("DS").Visible = false;
                //oForm.Items.Item("LINENUM").Visible = false;
                //oForm.Items.Item("DOCENTRY").Visible = false;
                //oForm.Items.Item("FUID").Visible = false;
                //oForm.Items.Item("st_ROW").Visible = false;
                //oForm.Items.Item("ROW").Visible = false;


                oForm.Visible = true;
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                oSForm.State = SAPbouiCOM.BoFormStateEnum.fs_Minimized;
                oSForm.Freeze(true);
                oMatrix.Columns.Item(1).Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("popup window initialize completed!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");

            }
        }
        public static void SDM(string FormUID, long docentry, int linenum, int row, string dsname, string matrixname)
        {
            try
            {
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Initialize popup window...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                SAPbouiCOM.Form oSForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FormUID);

                SAPbouiCOM.FormCreationParams creationPackage = (SAPbouiCOM.FormCreationParams)FT_ADDON.SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.UniqueID = "FT_" + (FT_ADDON.SAP.getNewformUID().ToString());
                creationPackage.FormType = "FT_SDM";
                SAPbouiCOM.Form oForm = FT_ADDON.SAP.SBOApplication.Forms.AddEx(creationPackage);
                switch (dsname)
                {
                    case "INV1":
                        oForm.Title = "A/R Invoice UDF Modified";
                        break;
                    case "PDN1":
                        oForm.Title = "Goods Receipt PO UDF Modified";
                        break;
                    case "DLN1":
                        oForm.Title = "Delivery UDF Modified";
                        break;
                    case "RIN1":
                        oForm.Title = "A/R Credit Note UDF Modified";
                        break;
                    case "PCH1":
                        oForm.Title = "A/P Invoice UDF Modified";
                        break;
                    case "RPC1":
                        oForm.Title = "A/P Credit Memo UDF Modified";
                        break;
                    case "POR1":
                        oForm.Title = "Purchase Order UDF Modified";
                        break;
                    case "RDN1":
                        oForm.Title = "Return UDF Modified";
                        break;
                    case "RPD1":
                        oForm.Title = "Goods Return UDF Modified";
                        break;
                    default:
                        oForm.Title = "User Define Fields UDF Modified";
                        break;

                }
                //oForm.Left = oSForm.Left;
                oForm.Width = 700;
                //oForm.Top = oSForm.Top;
                oForm.Height = 500;

                SAPbouiCOM.Item oItem;
                SAPbouiCOM.Button oButton;

                oItem = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 5;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";

                oItem = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oItem.Left = 75;
                oItem.Width = 65;
                oItem.Top = oForm.Height - 60;
                oItem.Height = 20;
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "Cancel";

                SAPbouiCOM.UserDataSource uds = null;

                uds = oForm.DataSources.UserDataSources.Add("row", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = row.ToString();

                oItem = oForm.Items.Add("ROW", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                ((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "row");
                oItem.Left = 55;
                oItem.Width = 65;
                oItem.Top = 5;
                oItem.Height = 15;
                oItem.Enabled = false;

                oItem = oForm.Items.Add("st_ROW", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                ((SAPbouiCOM.StaticText)oItem.Specific).Caption = "Row No#";
                oItem.Left = 5;
                oItem.Width = 50;
                oItem.Top = 5;
                oItem.Height = 15;

                uds = oForm.DataSources.UserDataSources.Add("fuid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = FormUID;

                //oItem = oForm.Items.Add("FUID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "fuid");
                //oItem.Left = 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("docentry", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = docentry.ToString();

                //oItem = oForm.Items.Add("DOCENTRY", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "docentry");
                //oItem.Left = 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("linenum", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = linenum.ToString();

                //oItem = oForm.Items.Add("LINENUM", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "linenum");
                //oItem.Left = 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;

                uds = oForm.DataSources.UserDataSources.Add("ds", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
                uds.Value = dsname;

                //oItem = oForm.Items.Add("DS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                //((SAPbouiCOM.EditText)oItem.Specific).DataBind.SetBound(true, "", "ds");
                //oItem.Left = 5 + 65 + 5 + 65 + 5 + 65 + 5;
                //oItem.Width = 65;
                //oItem.Top = 5;
                //oItem.Height = 15;
                //oItem.Enabled = false;
                //oItem.Visible = false;
 
                SAPbouiCOM.Matrix oSMatrix = (SAPbouiCOM.Matrix)oSForm.Items.Item(matrixname).Specific;

                oItem = oForm.Items.Add("grid1", SAPbouiCOM.BoFormItemTypes.it_MATRIX);
                oItem.Left = 5;
                oItem.Width = oForm.Width - 25;
                oItem.Top = 25;
                oItem.Height = oForm.Height - 90;
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oItem.Specific;
                if (oSMatrix.RowCount > 0)
                {
                    SAPbouiCOM.Conditions oCons = (SAPbouiCOM.Conditions)SAP.SBOApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions);
                    SAPbouiCOM.DBDataSource oDS = (SAPbouiCOM.DBDataSource)oForm.DataSources.DBDataSources.Add(dsname);

                    oDS.Clear();
                    SAPbouiCOM.Condition oCon = oCons.Add();
                    oCon.BracketOpenNum = 1;
                    oCon.Alias = "DocEntry";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = docentry.ToString();
                    oCon.BracketCloseNum = 1;

                    SAPbouiCOM.Column oColumn = null;
                    oColumn = oMatrix.Columns.Add("LineNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                    oColumn.DataBind.SetBound(true, dsname, "LineNum");
                    oColumn.Visible = false;

                    copyUDFMatrixColumns(oSForm, oSMatrix, oForm, oMatrix, dsname, dsname);

                    oDS.Query(oCons);
                    oMatrix.LoadFromDataSource();

                    string templinenum = "";
                    string temprow = "";
                    for (int x = 0; x < oSForm.DataSources.DBDataSources.Item(dsname).Size; x++)
                    {
                        templinenum = oSForm.DataSources.DBDataSources.Item(dsname).GetValue("LineNum", x).ToString();
                        temprow = ((SAPbouiCOM.EditText)oSMatrix.Columns.Item("0").Cells.Item(x + 1).Specific).String;
                        for (int y = 1; y <= oMatrix.RowCount; y++)
                        {
                            if (((SAPbouiCOM.EditText)oMatrix.Columns.Item("LineNum").Cells.Item(y).Specific).String == templinenum)
                            {
                                ((SAPbouiCOM.EditText)oMatrix.Columns.Item("0").Cells.Item(y).Specific).String = temprow;
                            }
                        }
                    }
                }

                oItem = oForm.Items.Item("1");
                oButton = (SAPbouiCOM.Button)oItem.Specific;
                oButton.Caption = "OK";
                //oForm.Items.Item("TEMP").Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                //oForm.Items.Item("DS").Visible = false;
                //oForm.Items.Item("LINENUM").Visible = false;
                //oForm.Items.Item("DOCENTRY").Visible = false;
                //oForm.Items.Item("FUID").Visible = false;
                //oForm.Items.Item("st_ROW").Visible = false;
                //oForm.Items.Item("ROW").Visible = false;


                oForm.Visible = true;
                oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                oSForm.State = SAPbouiCOM.BoFormStateEnum.fs_Minimized;
                oSForm.Freeze(true);
                oMatrix.Columns.Item(1).Cells.Item(1).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                FT_ADDON.SAP.SBOApplication.StatusBar.SetText("popup window initialize completed!", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");

            }
        }
        public static void copyUDFMatrixColumns(SAPbouiCOM.Form oSForm, SAPbouiCOM.Matrix oSMatrix, SAPbouiCOM.Form oForm, SAPbouiCOM.Matrix oMatrix, string dsname, string targetdsname)
        {
            try
            {
                string columnname;
                int width = 0;
                string temp = "";
                string title = "";
                SAPbouiCOM.BoFormItemTypes itemtype = SAPbouiCOM.BoFormItemTypes.it_EDIT;
                SAPbouiCOM.Column oSColumn = null;
                SAPbouiCOM.Column oColumn = null;

                SAPbouiCOM.BoFieldsType fieldType = SAPbouiCOM.BoFieldsType.ft_Text;
                int size = 0;
                SAPbouiCOM.BoDataType datatype = SAPbouiCOM.BoDataType.dt_SHORT_TEXT;
                SAPbouiCOM.LinkedButton oLink;
                SAPbouiCOM.LinkedButton oSLink;

                for (int col = 0; col < oSMatrix.Columns.Count; col++)
                {
                    if (oSMatrix.Columns.Item(col).Visible)
                    {
                        oSColumn = oSMatrix.Columns.Item(col);
                        title = oSColumn.Title;//oSMatrix.Columns.Item(col).Title;
                        width = oSColumn.Width;//oSMatrix.Columns.Item(col).Width;
                        columnname = oSColumn.UniqueID.ToString();//oSMatrix.Columns.Item(col).UniqueID.ToString();
                        switch (columnname)
                        {
                            case "0":
                                break;
                            default:
                                if (!columnname.Contains("U_"))
                                    continue;
                                break;
                        }
                        itemtype = oSColumn.Type;//oSMatrix.Columns.Item(col).Type;
                        oColumn = oMatrix.Columns.Add(columnname, itemtype);
                        oColumn.TitleObject.Caption = title;
                        oColumn.Width = width;
                        temp = oSColumn.DataBind.Alias;
                        if (temp == null || columnname == "0")
                        {
                            size = 0;
                            datatype = SAPbouiCOM.BoDataType.dt_SHORT_NUMBER;
                            oColumn.DataBind.SetBound(true, targetdsname, "BaseLine");
                        }
                        else
                        {
                            size = oSForm.DataSources.DBDataSources.Item(dsname).Fields.Item(oSColumn.DataBind.Alias).Size;
                            fieldType = oSForm.DataSources.DBDataSources.Item(dsname).Fields.Item(oSColumn.DataBind.Alias).Type;
                            datatype = ObjectFunctions.changeUIFieldsTypeToUIDataType(fieldType);
                            oColumn.DataBind.SetBound(true, targetdsname, columnname);
                        }
                        oColumn.Editable = oSColumn.Editable;
                        if (itemtype == SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON)
                        {
                            oSLink = (SAPbouiCOM.LinkedButton)oSColumn.ExtendedObject;
                            oLink = (SAPbouiCOM.LinkedButton)oColumn.ExtendedObject;
                            //SAP.SBOApplication.MessageBox(oSLink.LinkedObject.ToString(), 1, "ok", "", "");
                            oLink.LinkedObject = oSLink.LinkedObject;
                        }
                        else if (itemtype == SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
                        {
                            oColumn.DisplayDesc = true;
                            for (int x = 0; x < oSColumn.ValidValues.Count; x++ )
                            {
                                if (oSColumn.ValidValues.Item(x).Description.ToUpper() != "DEFINE NEW")
                                oColumn.ValidValues.Add(oSColumn.ValidValues.Item(x).Value, oSColumn.ValidValues.Item(x).Description);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");

            }
        }
        public static void copyUDFMatrixColumnsValues(SAPbouiCOM.Form oSForm, SAPbouiCOM.Matrix oSMatrix, SAPbouiCOM.Form oForm, SAPbouiCOM.Matrix oMatrix, int row)
        {
            try
            {
                string columnname;
                SAPbouiCOM.BoFormItemTypes itemtype = SAPbouiCOM.BoFormItemTypes.it_EDIT;

                string temp = "";
                oMatrix.AddRow(1, oMatrix.RowCount);
                for (int col = 0; col < oMatrix.Columns.Count; col++)
                {
                    if (oMatrix.Columns.Item(col).Visible)
                    {
                        columnname = oMatrix.Columns.Item(col).UniqueID.ToString();
                        switch (columnname)
                        {
                            case "LINENUM":
                                break;
                            default:
                                if (!columnname.Contains("U_"))
                                    continue;
                                break;
                        }
                        itemtype = oMatrix.Columns.Item(columnname).Type;
                        if (itemtype == SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
                        {
                            temp = ((SAPbouiCOM.ComboBox)(oSMatrix.Columns.Item(columnname).Cells.Item(row).Specific)).Selected.Value.ToString();
                            ((SAPbouiCOM.ComboBox)(oMatrix.Columns.Item(columnname).Cells.Item(oMatrix.RowCount).Specific)).Select(temp, SAPbouiCOM.BoSearchKey.psk_ByValue);
                        }
                        else if (itemtype == SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX)
                        {
                            if (((SAPbouiCOM.CheckBox)oSMatrix.Columns.Item(columnname).Cells.Item(row).Specific).Checked)
                            {
                                oMatrix.Columns.Item(columnname).Cells.Item(oMatrix.RowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular, 0);
                            }
                        }
                        else if (itemtype == SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON)
                        {
                            temp = ((SAPbouiCOM.EditText)oSMatrix.Columns.Item(columnname).Cells.Item(row).Specific).String;
                            ((SAPbouiCOM.EditText)(oMatrix.Columns.Item(columnname).Cells.Item(oMatrix.RowCount).Specific)).String = temp;
                        }
                        else if (itemtype == SAPbouiCOM.BoFormItemTypes.it_EDIT)
                        {
                            temp = ((SAPbouiCOM.EditText)oSMatrix.Columns.Item(columnname).Cells.Item(row).Specific).String;
                            ((SAPbouiCOM.EditText)(oMatrix.Columns.Item(columnname).Cells.Item(oMatrix.RowCount).Specific)).String = temp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
    }
}
