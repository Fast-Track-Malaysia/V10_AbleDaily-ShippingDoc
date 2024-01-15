using System;
using System.Collections.Generic;
using System.Text;

namespace FT_ADDON.AP_SO
{
    class FTAPSO
    {
        public FTAPSO()
        {
            // Get an instantialized application object 
            FT_ADDON.SAP.setApplication();

            // Set Connection Context from UIAPI cookie to DIAPI
            if (!(FT_ADDON.SAP.setConnectionContext() == 0))
            {
                FT_ADDON.SAP.SBOApplication.MessageBox("Failed setting a connection to DIAPI", 1, "Ok", "", "");
                System.Environment.Exit(0); //  Terminating the Add-On Application
            }

            // Connect to SBO Database through DIAPI
            if (!(FT_ADDON.SAP.connectToCompany() == 0))
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(FT_ADDON.SAP.SBOCompany.GetLastErrorCode().ToString() + ": "+FT_ADDON.SAP.SBOCompany.GetLastErrorDescription(),1,"OK","","" );
                System.Environment.Exit(0); //  Terminating the Add-On Application
            }

            // Display status
            FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Addon Initializing...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            // Add deligates to events
            FT_ADDON.SAP.SBOApplication.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            FT_ADDON.SAP.SBOApplication.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
            FT_ADDON.SAP.SBOApplication.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            FT_ADDON.SAP.SBOApplication.ProgressBarEvent += new SAPbouiCOM._IApplicationEvents_ProgressBarEventEventHandler(SBO_Application_ProgressBarEvent);
            FT_ADDON.SAP.SBOApplication.RightClickEvent += new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(SBO_Application_RightClickEvent);
            FT_ADDON.SAP.SBOApplication.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(ref SBO_Application_FormDataEvent);

            // Add UDT, UDF, Menu Item
            FT_ADDON.SAP.formUID = 0;
            FT_ADDON.SAP.createStatusForm();
            FT_ADDON.SAP.getStatusForm();
            initEnviroment();

            // Display status
            FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Addon successfully initialized.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        private void SBO_Application_RightClickEvent(ref SAPbouiCOM.ContextMenuInfo EventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            ItemEvent.processRightClickEvent(EventInfo.FormUID, ref EventInfo, ref BubbleEvent);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            ItemEvent.processItemEvent(FormUID, ref pVal, ref BubbleEvent);
        }

        private void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //SBO_Application.MessageBox("A Shut Down Event has been caught" + Environment.NewLine + "Terminating Add On...", 1, "Ok", "", "");
                    System.Environment.Exit(0);
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    //SBO_Application.MessageBox("A Company Change Event has been caught", 1, "Ok", "", "");
                    System.Environment.Exit(0);
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    //SBO_Application.MessageBox("A Languge Change Event has been caught", 1, "Ok", "", "");
                    break;
            }
        }

        private void SBO_Application_ProgressBarEvent(ref SAPbouiCOM.ProgressBarEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void SBO_Application_StatusBarEvent(string Text, SAPbouiCOM.BoStatusBarMessageType MessageType)
        {
            //SBO_Application.MessageBox(@"Status bar event with message: """ + Text + @""" has been sent", 1, "Ok", "", "");
        }

        private void SBO_Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //FormDataEvent.process_FormDataEvent(ref BusinessObjectInfo,ref BubbleEvent);
        }

        private void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!pVal.BeforeAction) MenuEvent.processMenuEvent(ref pVal);

            MenuEvent.processMenuEvent2(ref pVal, ref BubbleEvent);
        }

        private void initEnviroment()
        {
            // -------------------------------------------------------
            // Add UDT, UDF, Add Menu Item
            // -------------------------------------------------------

            FT_ADDON.ApplicationCommon app = new ApplicationCommon();
            FT_ADDON.SAP.SBOCompany.StartTransaction();

            if (!app.createTable("FT_APSOC", "AP SO Container", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_APSOC"))
            {
                //if (!app.createField("@FT_APSOC", "DOCNO", "DOCENTRY", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                //if (!app.createField("@FT_APSOC", "LINENO", "LINENUM", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_APSOC", "LINENO", "Actual LINENUM", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_APSOC", "CONNO", "Container No", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_APSOC", "BOOKNO", "BKG REF", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_APSOC", "REF", "Reference", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
            }

            if (!app.createTable("FT_APDOPT", "AP DO Product Type Result", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_APDOPT"))
            {
                //if (!app.createField("@FT_APDOPT", "DOCNO", "DOCENTRY", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                //if (!app.createField("@FT_APDOPT", "LINENO", "LINENUM", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_APDOPT", "LINENO", "Actual LINENUM", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
            }
            if (!app.createTable("FT_APDOC", "AP DO Container", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_APDOC"))
            {
                //if (!app.createField("@FT_APDOPT", "DOCNO", "DOCENTRY", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                //if (!app.createField("@FT_APDOPT", "LINENO", "LINENUM", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_APDOC", "LINENO", "Actual LINENUM", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_APDOC", "CONNO", "Container No", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_APDOC", "BookNo", "Booking No", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
            }
            /// Custom Form Setting table - start
            if (!app.createTable("FT_CFS", "Custom Form Setting", SAPbobsCOM.BoUTBTableType.bott_MasterData)) goto ErrorHandler;
            if (!app.tableGotField("@FT_CFS"))
            {
                if (!app.createField("@FT_CFS", "FNAME", "Form Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, "")) goto ErrorHandler;
                if (!app.createField("@FT_CFS", "USRID", "User ID", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, "")) goto ErrorHandler;
            }
            if (!app.udfExist("@FT_CFS", "MATRIX"))
                if (!app.createField("@FT_CFS", "MATRIX", "Matrix Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, "")) goto ErrorHandler;
            if (!app.udfExist("@FT_CFS", "DSNAME"))
                if (!app.createField("@FT_CFS", "DSNAME", "Table Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, "")) goto ErrorHandler;

            if (!app.createTable("FT_CFSDL", "Custom Form Setting Detail", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_CFSDL"))
            {
                if (!app.createField("@FT_CFSDL", "CNAME", "Column Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 20, "")) goto ErrorHandler;
                if (!app.createField("@FT_CFSDL", "NONVIEW", "Cannot View", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "1")) goto ErrorHandler;
                if (!app.createField("@FT_CFSDL", "NONEDIT", "Cannot Edit", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "1")) goto ErrorHandler;
            }
            /// Custom Form Setting table - end
            if (!app.createTable("FT_SHIPD", "Shipping Document", SAPbobsCOM.BoUTBTableType.bott_Document)) goto ErrorHandler;
            if (!app.tableGotField("@FT_SHIPD"))
            {
                if (!app.createField("@FT_SHIPD", "sdoc", "SO Docentry", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "pino", "SC No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "docdate", "Date", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "set", "Set", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "booking", "Booking No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "country", "Country Of Origin", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "shipper", "Shipper", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "notify", "NotifyParty", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "consigne", "Consignee", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "vessel", "Vessel Name", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "loading", "Port of Loading", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "shipterm", "Shipping Term", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "discharg", "Port of Discharge", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "itemdesc", "Product Description", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "shiprem", "Shipping Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIPD", "botani", "Botanical Name and Plants", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "disting", "Distinguishing Remarks", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIPD", "voyno", "Voyage No", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIPD", "blno", "BL No", SAPbobsCOM.BoFieldTypes.db_Memo, 0, "")) goto ErrorHandler;
            }
            if (!app.createTable("FT_SHIP1", "Shipping Item Detail", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_SHIP1"))
            {
                if (!app.createField("@FT_SHIP1", "itemcode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP1", "itemname", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP1", "size", "Size", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP1", "jcclr", "JCColour", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP1", "brand", "Brand", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP1", "desc", "Description(Document)", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP1", "perfcl", "Qty/FCL", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
            }            
            
            if (!app.createTable("FT_SHIP2", "Shipping Document Cont Detail", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_SHIP2"))
            {
                if (!app.createField("@FT_SHIP2", "conno", "Container No", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "blno", "BLNo", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "vessel", "Vessel Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "sealno", "Seal No", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "insso", "Inspection Seal  NO", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "consize", "Container Size", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "shiplin", "Shipping Liner", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "loadqty", "LoadQty", SAPbobsCOM.BoFieldTypes.db_Float, 0, "0", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "netw", "NetWeight", SAPbobsCOM.BoFieldTypes.db_Float, 0, "0", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "grossw", "GrossWeight", SAPbobsCOM.BoFieldTypes.db_Float, 0, "0", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "measure", "Measurement", SAPbobsCOM.BoFieldTypes.db_Float, 0, "0", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "qty1", "Quantity1", SAPbobsCOM.BoFieldTypes.db_Float, 0, "", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "batchno", "Batch No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bmd", "Batch Manufacturing Date", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bed", "Batch Expire Date", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "netw1", "Net Weight1", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "qty2", "Quantity2", SAPbobsCOM.BoFieldTypes.db_Float, 0, "", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "batchno2", "Batch No2", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bmd2", "Batch Manufacturing Date2", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bed2", "Batch Expire Date2", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "netw2", "Net Weight2", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "qty3", "Quantity3", SAPbobsCOM.BoFieldTypes.db_Float, 0, "", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "batchno3", "Batch No3", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bmd3", "Batch Manufacturing Date3", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bed3", "Batch Expire Date3", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "netw3", "Net Weight3", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;

                if (!app.createField("@FT_SHIP2", "qty4", "Quantity4", SAPbobsCOM.BoFieldTypes.db_Float, 0, "", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "batchno4", "Batch No4", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bmd4", "Batch Manufacturing Date4", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bed4", "Batch Expire Date4", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "netw4", "Net Weight4", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "qty5", "Quantity5", SAPbobsCOM.BoFieldTypes.db_Float, 0, "", false, SAPbobsCOM.BoFldSubTypes.st_Quantity)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "batchno5", "Batch No5", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bmd5", "Batch Manufacturing Date5", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "bed5", "Batch Expire Date5", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "netw5", "Net Weight5", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;

                if (!app.createField("@FT_SHIP2", "datein", "Date In", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "dateout", "Date Out", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                
                if (!app.createField("@FT_SHIP2", "docentry", "Source Doecntry", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "lineid", "Source Lineid", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
            
                
                //if (!app.createField("@FT_SHIP2", "batchrem", "Batch Remarks", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "item1", "Item 1", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "uom1", "UOM 1", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "item2", "Item 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "uom2", "UOM 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
            }

            if (!app.udfExist("@FT_SHIP2", "itemcode"))
            {
                if (!app.createField("@FT_SHIP2", "itemcode", "Item Code", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "itemname", "Item Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "size", "Size", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "jcclr", "JCColour", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP2", "brand", "Brand", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "desc", "Description(Document)", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP2", "perfcl", "Qty/FCL", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
            }

            if (!app.createTable("FT_SHIP3", "Shipping Document COA Result", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)) goto ErrorHandler;
            if (!app.tableGotField("@FT_SHIP3"))
            {
                if (!app.createField("@FT_SHIP3", "batchno", "Batch No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP3", "coano", "COA No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP3", "coadate", "COA Date", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
                if (!app.createField("@FT_SHIP3", "gana", "Group Analysis", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP3", "ana", "Analysis", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP3", "spec", "Specification", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;
                if (!app.createField("@FT_SHIP3", "result", "Result", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "")) goto ErrorHandler;

                //if (!app.createField("@FT_SHIP3", "prodtype", "Product Type", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "ffa", "FFA (As Palmitic)", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "colour", "Colour (2.25 Lovibond Cell)", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "mni", "MNI", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "iv", "IV", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "cloud", "Cloud Point (ºC)", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "pv", "Peroxide Value", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;
                //if (!app.createField("@FT_SHIP3", "slip", "Slip Melting Point (ºC)", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "")) goto ErrorHandler;

            }
            if (!app.createTable("FT_BUYER", "Buyer", SAPbobsCOM.BoUTBTableType.bott_NoObject)) goto ErrorHandler;
            if (!app.tableGotField("@FT_BUYER"))
            {
                if (!app.createField("@FT_BUYER", "bname", "Buyer Name", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "discharg", "Port of Discharge", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "ship1", "Shipper Address 1", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "ship2", "Shipper Address 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "ship3", "Shipper Address 3", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "ship4", "Shipper Address 4", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "ship5", "Shipper Address 5", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "con1", "Consignee Address 1", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "con2", "Consignee Address 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "con3", "Consignee Address 3", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "con4", "Consignee Address 4", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "con5", "Consignee Address 5", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "notify1", "Notify Party Address 1", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "notify2", "Notify Party Address 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "notify3", "Notify Party Address 3", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "notify4", "Notify Party Address 4", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
                if (!app.createField("@FT_BUYER", "notify5", "Notify Party Address 5", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "")) goto ErrorHandler;
            }

            if (!app.udfExist("CUFD", "seq"))
                if (!app.createField("CUFD", "seq", "Seq No", SAPbobsCOM.BoFieldTypes.db_Numeric, 0, "0")) goto ErrorHandler;
            
            if (!app.createUDO("FT_SHIPD", "Shipping Document", SAPbobsCOM.BoUDOObjType.boud_Document, "FT_SHIPD", "FT_SHIP1|FT_SHIP2|FT_SHIP3", "", SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, "")) goto ErrorHandler; ;

            //if (!app.udfExist("@FT_APSOC", "CONDATE"))
            //if (!app.createField("@FT_APSOC", "CONDATE", "Ref. Date", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
            /*
            if (!app.udfExist("@FT_APSOC", "DATE1"))
                if (!app.createField("@FT_APSOC", "DATE1", "Ref. Date 1", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "DATE2"))
                if (!app.createField("@FT_APSOC", "DATE2", "Ref. Date 2", SAPbobsCOM.BoFieldTypes.db_Date, 0)) goto ErrorHandler;

            if (!app.udfExist("@FT_APSOC", "STRING1"))
                if (!app.createField("@FT_APSOC", "STRING1", "String 1", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "STRING2"))
                if (!app.createField("@FT_APSOC", "STRING2", "String 2", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "STRING3"))
                if (!app.createField("@FT_APSOC", "STRING3", "String 3", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "STRING4"))
                if (!app.createField("@FT_APSOC", "STRING4", "String 4", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "STRING5"))
                if (!app.createField("@FT_APSOC", "STRING5", "String 5", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "STRING6"))
                if (!app.createField("@FT_APSOC", "STRING6", "String 6", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            if (!app.udfExist("@FT_APSOC", "STRING7"))
                if (!app.createField("@FT_APSOC", "STRING7", "String 7", SAPbobsCOM.BoFieldTypes.db_Alpha, 50)) goto ErrorHandler;
            */
            //if (!app.udfExist("OCRG", "PROGRP"))
            //    if (!app.createField("OCRG", "PROGRP", "Promotion Group", SAPbobsCOM.BoFieldTypes.db_Alpha, 10)) goto ErrorHandler;

            //if (!app.createTable("FT_PROG","Promotion Group Setup",SAPbobsCOM.BoUTBTableType.bott_MasterData)) goto ErrorHandler;


            if (!app.createUDO("FT_CFS", "Custom Form Setting", SAPbobsCOM.BoUDOObjType.boud_MasterData, "FT_CFS", "FT_CFSDL", "", SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tNO, SAPbobsCOM.BoYesNoEnum.tYES, SAPbobsCOM.BoYesNoEnum.tNO, "")) goto ErrorHandler; ;

            if (FT_ADDON.SAP.SBOCompany.InTransaction) FT_ADDON.SAP.SBOCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            FT_ADDON.SAP.hideStatus();

            //app.createMenuItem("FT_INTERDB", "Inter Company Posting", "43520", "2048", true, SAPbouiCOM.BoMenuType.mt_POPUP);

            //app.createMenuItem("FT_SCNS2JV", "AR Service Credit Note to JV", "FT_INTERDB", "", true, SAPbouiCOM.BoMenuType.mt_STRING);
            //app.createMenuItem("FT_SCN2JV", "AR Credit Note to JV", "FT_INTERDB", "FT_SCNS2JV", true, SAPbouiCOM.BoMenuType.mt_STRING);
            //app.createMenuItem("FT_SINVS2JV", "AR Service Invoice to JV", "FT_INTERDB", "FT_SCN2JV", true, SAPbouiCOM.BoMenuType.mt_STRING);
            //app.createMenuItem("FT_SINV2JV", "AR Invoice to JV", "FT_INTERDB", "FT_SINVS2JV", true, SAPbouiCOM.BoMenuType.mt_STRING);

            GC.WaitForPendingFinalizers();
            return;

        ErrorHandler:
            if (FT_ADDON.SAP.SBOCompany.InTransaction) FT_ADDON.SAP.SBOCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            FT_ADDON.SAP.SBOApplication.StatusBar.SetText("Addon was teminated.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            FT_ADDON.SAP.hideStatus();
            System.Environment.Exit(0);
        }
        private bool checkTables()
        {
            try
            {
                FT_ADDON.SAP.setStatus("Checking Table...");

                string ls_sql = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[@FT_APSOC]') AND type in (N'U')) select 1";
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)FT_ADDON.SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(ls_sql);
                if (rs.RecordCount > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
                return false;
            }
       }
        private bool createTables()
        {
            try
            {
                FT_ADDON.SAP.setStatus("Creating Table : FT_APSOC");

                string ls_sql = "CREATE TABLE [dbo].[@FT_APSOC] ( DOCENTRY numeric(10,0) NOT NULL, LINENUM int NOT NULL, ITEMNO int NOT NULL, CONNO varchar(50) NULL , REF varchar(255) NULL , PRIMARY KEY (DOCENTRY,LINENUM,ITEMNO))";

                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)FT_ADDON.SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(ls_sql);
                return true;
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
                return false;
            }
        }
    }
}
