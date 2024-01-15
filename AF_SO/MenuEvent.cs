using System;
using System.Collections.Generic;
using System.Text;

namespace FT_ADDON.AP_SO
{
    class MenuEvent
    {
        public static void processMenuEvent(ref SAPbouiCOM.MenuEvent pVal)
        {
            switch (pVal.MenuUID)
            {
                //case "FT_PROC":
                //    InitForm.PROC("", "");
                //    break;
                //default:
                //    break;
            }

        }
        public static void processMenuEvent2(ref SAPbouiCOM.MenuEvent pVal, ref bool BubbleEvent)
        {
            switch (pVal.MenuUID)
            {
                case "1281": //find
                case "1282": //add
                case "1283": //remove
                case "1284": //cancel
                case "1285": //restore
                case "1286": //close
                case "1287": //duplicate
                case "1288": //next record
                case "1289": //previous record
                case "1290": //first record
                case "1291": //last record
                case "1292": //add row
                case "1293": //delete row
                case "1294": //duplicate row
                case "1295": //copy cell above
                case "1296": //copy cell below
                case "1299": //close row
                    {
                        SAPbouiCOM.Form oForm = null;
                        try
                        {
                            oForm = FT_ADDON.SAP.SBOApplication.Forms.ActiveForm;
                        }
                        catch { }
                        if (oForm == null) return;

                        if (pVal.BeforeAction)
                        {
                            if (oForm.TypeEx == "FT_CONM")
                                UserForm_CONmodified.processMenuEventbefore(oForm, ref pVal, ref BubbleEvent);
                            else if (oForm.TypeEx == "FT_DOPTM")
                                UserForm_CONmodified.processMenuEventbefore(oForm, ref pVal, ref BubbleEvent);
                            else if (oForm.TypeEx == "FT_SHIPD")
                                UserForm_ShipDoc.processMenuEventbefore(oForm, ref pVal, ref BubbleEvent);
                        }
                        else
                        {
                            if (oForm.TypeEx == "FT_CONM")
                                UserForm_CONmodified.processMenuEventafter(oForm, ref pVal);
                            else if (oForm.TypeEx == "FT_DOPTM")
                                UserForm_CONmodified.processMenuEventafter(oForm, ref pVal);
                            else if (oForm.TypeEx == "FT_SHIPD")
                                UserForm_ShipDoc.processMenuEventafter(oForm, ref pVal);
                        }
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
