using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ignite
{
    public class Utilities
    {
        public static string userToken = "token";
        public static string userFullName = string.Empty;
        public static int editId = -1;
        public static int statusId = -1;
        public static string status = string.Empty;
        public static string pageName = string.Empty;

        public static int userId = -1;
        public static int branchId = -1;
        public static int userId_E;
        public static DateTime userExpiryDate;
        public static bool userActive;
        public static bool isOrgAdmin;
        public static bool isSystemAdmin;
        public static int userGroupId = -1;
        public static string userGroup_edit = string.Empty;
        public static int userGroupId_edit = -1;
        public static int companyId = 1;
        public static string companySMSName = string.Empty;
        public static int periodId = -1;
        public static int staffId = -1;
        public static bool isPolicyAdministrator;
        public static int paramEditId = -1;
        public static int paramParentId = -1;
        public static int gridIndex = -1;
        public static int gridPage = -1;
        public static string paramEditName = string.Empty;
        public static string paramParentName = string.Empty;
        public static string paramSource = string.Empty;
        public static string paramParent = string.Empty;
        public static string paramParentPath = string.Empty;
        public static string mapSearchParams = string.Empty;
        public static string branchName = string.Empty;
        public static string companyName = string.Empty;
        public static int ssMainHeight = -1;
        public static int searchId = -1;
        public static int apptype = 2;
        public static object returnedDataSet;
        public static bool headersource;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ValidatePhoneNumber(string s)
        {
            char[] allowed = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '+', ' ' };
            foreach (char c in s)
            {
                if (Array.IndexOf(allowed, c) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public static bool ValidateDate(DateTime dateFrom, DateTime dateTo)
        {
            if(dateFrom < dateTo)
            {
                return false;
            } 

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool ValidateNumRange(int from, int to)
        {
            if (from > to)
            {
                return false;
            }

            return true;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="title"></param>
        //public static void ProcessErrorHandler(string message, string title)
        //{
        //    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

        ////
        //public static void ProcessInfoHandler(string message, string title)
        //{
        //    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        public static string LoadDefaultOfficeCode()
//        {
//            string code = string.Empty;
//            try
//            {
//                RegistryUtil regUtil = new RegistryUtil();
//                if (regUtil.ValidateKey(@"HKLM\Software\NMS\Verve.DashboardManager"))
//                {
//                    code = regUtil.GetValueData(@"HKLM\Software\NMS\Verve.DashboardManager", "OfficeCode");
//                }
//            }
//                // ReSharper disable EmptyGeneralCatchClause
//            catch (Exception)
//                // ReSharper restore EmptyGeneralCatchClause
//            {
//                //Do nothing
//            }
//            return code;
//        }

        //public static void EnableLastSelected()
        //{
        //    if (scf != null)
        //    {
        //        scf.Enabled = true;
        //    }
        //}

        public static void ExceptionHandler(Exception ex, System.Web.UI.MasterPage page)
        {
            if (ex.Message.Contains("InvalidUserTokenException"))
            {

                //MessageBox.Show("Your session does not have the appropriate authorisation", "Authentication Error",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Error);
                BounceToLogin();
            }
            else if (ex.Message.Contains("ExpiredUserTokenException"))
            {

                //MessageBox.Show("Your session authorisation has expired", "Authentication Error",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Error);
                BounceToLoginExpire(page.Response);
            }
            else if (ex.Message.Contains("OperationFailedException"))
            {

                //MessageBox.Show("Data processing error occurred.", "Data Access Error",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Error);
                // BounceToLogin();
            }
            else
            {
                //MessageBox.Show(ex.Message, "Operations Services Error", MessageBoxButtons.OK,
                //                MessageBoxIcon.Error);
                //BounceToLogin();
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="page"></param>
        public static void ExceptionHandler(Exception ex, Page page)
        {
            if (ex.Message.Contains("InvalidUserTokenException"))
            {

                //MessageBox.Show("Your session does not have the appropriate authorisation", "Authentication Error",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Error);
               // BounceToLogin();
                BounceToLoginExpire(page.Response);
            }
            else if (ex.Message.Contains("ExpiredUserTokenException"))
            {

                //MessageBox.Show("Your session authorisation has expired", "Authentication Error",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Error);
                BounceToLoginExpire(page.Response);
            }
            else if (ex.Message.Contains("OperationFailedException"))
            {
            
                BounceToError(page.Response);
            }
            else
            {
                BounceToError(page.Response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NormalisePhoneNumber(string s)
        {
            return s.Replace("-", string.Empty).Replace(" ", string.Empty);
        }

        public static bool CheckGridForText(GridView dgvSomeGrid, string content)
        {
            foreach (GridViewRow row in dgvSomeGrid.Rows)
            {
                for (int i = 0; i < dgvSomeGrid.Columns.Count; i++)
                {
                    if (content == row.Cells[i].Text)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string s)
        {
            var regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return regex.IsMatch(s);
        }

        public static string Amounter(string figure)
        {
            figure = figure.Replace(",", string.Empty);

            try
            {
                figure = Convert.ToDecimal(figure).ToString();
            }
            catch
            {
                figure = "0";
            }

            return figure;
        }


        public static bool ValidateAmount(string s)
        {
            //s = Amounter(s);
            bool result;
            try
            {
                Convert.ToDecimal(s).ToString();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void BounceToLogin()
        {
            //WndMainWindow mw = WndMainWindow.GetInstance();
            //mw.suppressMessage = true;
            //mw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void BounceToLoginExpire(HttpResponse response)
        {
            response.Redirect("Login.aspx");
        }


        /// <summary>
        /// 
        /// </summary>
        public static void BounceToError(HttpResponse response)
        {
            response.Redirect("ErrorPage.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="curSymbol"></param>
        /// <param name="unitName"></param>
        /// <param name="subUnitName"></param>
        /// <param name="unitNameSingle"></param>
        /// <param name="subUnitNameSingle"></param>
        /// <returns></returns>
        public static string AmountInWords(decimal amount, string curSymbol, string unitName, string subUnitName, string unitNameSingle, string subUnitNameSingle)
        {
            string billions = string.Empty,
                   millions = string.Empty,
                   thousands = string.Empty,
                   hundreds = string.Empty,
                   cents = string.Empty,
                   amountInWords = string.Empty;

            string amountStr = amount.ToString("###0.00");
            int totAmount = Convert.ToInt32(amountStr.Substring(0, amountStr.Length - 3));
            int avalueB = totAmount / 1000000000;

            if (avalueB > 0)
            {
                billions = HundredsInWords(avalueB) + " Billion";
            }

            if (avalueB > 0)
            {
                amountInWords = billions;
            }

            int avalueM = (totAmount - avalueB * 1000000000) / 1000000;

            if (avalueM > 0)
            {
                millions = HundredsInWords(avalueM) + " Million";
            }

            if (avalueB > 0 && avalueM > 0)
            {
                amountInWords = billions + ", " + millions;
            }
            else
            {
                amountInWords += millions;
            }

            int avalueT = (totAmount - (avalueB * 1000000000) - (avalueM * 1000000)) / 1000;

            if (avalueT > 0)
            {
                thousands = HundredsInWords(avalueT) + " Thousand";
            }

            if ((avalueB > 0 || avalueM > 0) && avalueT > 0)
            {
                amountInWords += ", " + thousands;
            }
            else
            {
                amountInWords += thousands;
            }

            int avalueH = (totAmount - (avalueB * 1000000000) - (avalueM * 1000000) - (avalueT * 1000));

            if (avalueH > 0)
            {
                hundreds = HundredsInWords(avalueH);
            }

            if ((avalueB > 0 || avalueM > 0 || avalueT > 0) && avalueH > 0)
            {
                amountInWords += ", " + hundreds;
            }
            else
            {
                amountInWords += hundreds;
            }


            if (totAmount == 1)
            {
                amountInWords += " " + unitNameSingle;
            }
            else if (totAmount > 1)
            {
                amountInWords += " " + unitName;
            }

            int avalueC = Convert.ToInt32((amount - totAmount) * 100);

            if (avalueC > 0)
            {
                cents = HundredsInWords(avalueC);
            }

            if ((avalueB > 0 || avalueM > 0 || avalueT > 0 || avalueH > 0) && avalueC > 0)
            {
                amountInWords += ", " + cents;
            }
            else
            {
                amountInWords += cents;
            }

            if (avalueC == 1)
            {
                amountInWords += " " + subUnitNameSingle;
            }
            else if (avalueC > 1)
            {
                amountInWords += " " + subUnitName;
            }

            return amountInWords + " Only";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static string HundredsInWords(int amount)
        {
            string hundredStr = string.Empty, tenStr = string.Empty, unitStr = string.Empty;
            int value;

            int hundredInt = amount / 100;
            value = hundredInt;
            if (hundredInt > 0)
            {
                if (value == 1)
                {
                    hundredStr = "One Hundred";
                }
                else if (value == 2)
                {
                    hundredStr = "Two Hundred";
                }
                else if (value == 3)
                {
                    hundredStr = "Three Hundred";
                }
                else if (value == 4)
                {
                    hundredStr = "Four Hundred";
                }
                else if (value == 5)
                {
                    hundredStr = "Five Hundred";
                }
                else if (value == 6)
                {
                    hundredStr = "Six Hundred";
                }
                else if (value == 7)
                {
                    hundredStr = "Seven Hundred";
                }
                else if (value == 8)
                {
                    hundredStr = "Eight Hundred";
                }
                else if (value == 9)
                {
                    hundredStr = "Nine Hundred";
                }
            }

            int unitInt = 0;
            int tenInt = (amount / 10) - hundredInt * 10;
            value = tenInt;
            if (tenInt > 0)
            {
                if (value == 1)
                {
                    unitInt = amount - ((amount / 10) * 10);
                    value = unitInt;
                    if (value == 1)
                    {
                        tenStr = "Eleven";
                    }
                    else if (value == 2)
                    {
                        tenStr = "Twelve";
                    }
                    else if (value == 3)
                    {
                        tenStr = "Thirteen";
                    }
                    else if (value == 4)
                    {
                        tenStr = "Forteen";
                    }
                    else if (value == 5)
                    {
                        tenStr = "Fifteen";
                    }
                    else if (value == 6)
                    {
                        tenStr = "Sixteen";
                    }
                    else if (value == 7)
                    {
                        tenStr = "Seventeen";
                    }
                    else if (value == 8)
                    {
                        tenStr = "Eighteen";
                    }
                    else if (value == 9)
                    {
                        tenStr = "Nineteen";
                    }
                }
                else
                {
                    if (value == 2)
                    {
                        tenStr = "Twenty";
                    }
                    else if (value == 3)
                    {
                        tenStr = "Thirty";
                    }
                    else if (value == 4)
                    {
                        tenStr = "Forty";
                    }
                    else if (value == 5)
                    {
                        tenStr = "Fifty";
                    }
                    else if (value == 6)
                    {
                        tenStr = "Sixty";
                    }
                    else if (value == 7)
                    {
                        tenStr = "Seventy";
                    }
                    else if (value == 8)
                    {
                        tenStr = "Eighty";
                    }
                    else if (value == 9)
                    {
                        tenStr = "Ninety";
                    }

                    unitInt = amount - ((amount / 10) * 10);
                    value = unitInt;
                    if (value == 1)
                    {
                        tenStr += "-One";
                    }
                    else if (value == 2)
                    {
                        tenStr += "-Two";
                    }
                    else if (value == 3)
                    {
                        tenStr += "-Three";
                    }
                    else if (value == 4)
                    {
                        tenStr += "-Four";
                    }
                    else if (value == 5)
                    {
                        tenStr += "-Five";
                    }
                    else if (value == 6)
                    {
                        tenStr += "-Six";
                    }
                    else if (value == 7)
                    {
                        tenStr += "-Seven";
                    }
                    else if (value == 8)
                    {
                        tenStr += "-Eight";
                    }
                    else if (value == 9)
                    {
                        tenStr += "-Nine";
                    }

                    unitInt = 0;
                }
            }


            if (hundredInt > 0 && tenInt > 0)
            {
                tenStr = " and " + tenStr;
            }

            if (tenInt == 0)
            {
                unitInt = amount - ((amount / 10) * 10);
                value = unitInt;
                if (value == 1)
                {
                    unitStr += "One";
                }
                else if (value == 2)
                {
                    unitStr += "Two";
                }
                else if (value == 3)
                {
                    unitStr += "Three";
                }
                else if (value == 4)
                {
                    unitStr += "Four";
                }
                else if (value == 5)
                {
                    unitStr += "Five";
                }
                else if (value == 6)
                {
                    unitStr += "Six";
                }
                else if (value == 7)
                {
                    unitStr += "Seven";
                }
                else if (value == 8)
                {
                    unitStr += "Eight";
                }
                else if (value == 9)
                {
                    unitStr += "Nine";
                }
            }


            if ((hundredInt > 0 || tenInt > 0) && unitInt > 0)
            {
                unitStr = " and " + unitStr;
            }

            return hundredStr + tenStr + unitStr;

        }



    }
}