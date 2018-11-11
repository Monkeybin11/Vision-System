using System;
using System.Windows.Forms;
using System.Resources;
using Cognex.VisionPro;
using Cognex.VisionPro.Implementation.Internal;
using Cognex.VisionPro.QuickBuild;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Win32;
using System.Net.NetworkInformation;

namespace Vision_System
{
    class Utility
    {
        string mSelectedProduct = "";
        double Num1 = 0;
        string PCIName = "";

        public static string GetMacAddressByNetworkInformation()
        {
            string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
            string macAddress = string.Empty;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                        && adapter.GetPhysicalAddress().ToString().Length != 0)
                    {
                        string fRegistryKey = key + adapter.Id + "\\Connection";
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                        if (rk != null)
                        {
                            string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                            int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                            if (fPnpInstanceID.Length > 3 &&
                                fPnpInstanceID.Substring(0, 3) == "PCI")
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                for (int i = 1; i < 6; i++)
                                {
                                    macAddress = macAddress.Insert(3 * i - 1, ":");
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //这里写异常的处理  
            }
            return macAddress;
        }

        private void writeToCSV()
        {
            try
            {
                string filename = FormMain.strBaseDirectory + "Saved Data//" + mSelectedProduct;
                writedata(filename, Num1.ToString() + "," + PCIName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void writedata(string filename, string content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filename, true, System.Text.Encoding.GetEncoding("GB2312"));
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        static public ICogRecord TraverseSubRecords(ICogRecord r, string[] subs)
        {
            // Utility function to walk down to a specific subrecord
            if (r == null)
                return r;

            foreach (string s in subs)
            {
                if (r.SubRecords.ContainsKey(s))
                    r = r.SubRecords[s];
                else
                    return null;
            }

            return r;
        }

        static public void FlushAllQueues(CogJobManager jm)
        {
            // Flush all queues
            if (jm == null)
                return;

            jm.UserQueueFlush();
            jm.FailureQueueFlush();
            for (int i = 0; i < jm.JobCount; i++)
            {
                jm.Job(i).OwnedIndependent.RealTimeQueueFlush();
                jm.Job(i).ImageQueueFlush();
            }
        }

        static public int GetJobIndexFromName(CogJobManager mgr, string name)
        {
            if (mgr != null)
            {
                for (int i = 0; i < mgr.JobCount; ++i)
                    if (mgr.Job(i).Name == name)
                        return i;
            }
            return -1;
        }

        static public bool AddRecordToDisplay(CogRecordsDisplay disp, ICogRecord r, string[] subs,
          bool pickBestImage)
        {
            // Utility function to put a specific subrecord into a display
            ICogRecord addrec = Utility.TraverseSubRecords(r, subs);
            if (addrec != null)
            {
                // if this is the first record in, then always select an image
                if (disp.Subject == null)
                    pickBestImage = true;

                disp.Subject = addrec;

                if (pickBestImage)
                {
                    // select first non-empty image record, to workaround the fact that the input image tool
                    // adds an empty subrecord to the LastRun record when it is disabled (when an image file
                    // tool is used, for example)
                    for (int i = 0; i < addrec.SubRecords.Count; ++i)
                    {
                        ICogImage img = addrec.SubRecords[i].Content as ICogImage;
                        if (img != null && img.Height != 0 && img.Width != 0)
                        {
                            disp.SelectedRecordKey = addrec.RecordKey + "." + addrec.SubRecords[i].RecordKey;
                            break;
                        }
                    }
                }
                disp.Display.Fit(true);
                return true;
            }

            return false;
        }

        private static bool TypeIsNumeric(Type t)
        {
            if (t == null)
                return false;

            if (t == typeof(double) ||
              t == typeof(long) ||
              t == typeof(sbyte) || t == typeof(byte) ||
              t == typeof(short) || t == typeof(ushort) ||
              t == typeof(int) || t == typeof(uint) ||
              t == typeof(ulong))
                return true;

            return false;
        }

        private static Type GetPropertyType(object obj, string path)
        {
            if (obj == null || path == "")
                return null;

            System.Reflection.MemberInfo[] infos = CogToolTerminals.
              ConvertPathToMemberInfos(obj, obj.GetType(), path);

            if (infos.Length == 0)
                return null;

            // Return the type of the last path element.
            return CogToolTerminals.GetReturnType(infos[infos.Length - 1]);
        }

        public static void FillUserResultData(Control ctrl, ICogRecord result, string path)
        {
            FillUserResultData(ctrl, result, path, false);
        }

        public static void FillUserResultData(Control ctrl, ICogRecord result, string path, bool convertRadiansToDegrees)
        {
            // Extract the data identified by the path (if available) from the given result record.
            // Use a format string for doubles.
            string rtn;
            HorizontalAlignment align = HorizontalAlignment.Left;
            if (result == null)
                rtn = ResourceUtility.GetString("RtResultNotAvailable");
            else
            {
                object obj = null;

                try
                {
                    obj = result.SubRecords[path].Content;
                }
                catch
                {
                }

                // check if data is available
                if (obj != null && obj.GetType().FullName != "System.Object")
                {
                    if (obj.GetType() == typeof(double))
                    {
                        double d = (double)obj;
                        if (convertRadiansToDegrees)
                            d = CogMisc.RadToDeg(d);
                        rtn = d.ToString("0.000");
                    }
                    else
                        rtn = obj.ToString();

                    if (TypeIsNumeric(obj.GetType()))
                        align = HorizontalAlignment.Right;
                }
                else
                    rtn = ResourceUtility.GetString("RtResultNotAvailable");
            }

            ctrl.Text = rtn;
            TextBox box = ctrl as TextBox;
            if (box != null)
                box.TextAlign = align;
        }

        public static void SetupPropertyProvider(CogToolPropertyProvider p, Control gui, object tool, string path)
        {
            p.SetPath(gui, path);

            TextBox box = gui as TextBox;
            if (box != null)
            {
                Type t = GetPropertyType(tool, path);
                if (TypeIsNumeric(t))
                    box.TextAlign = HorizontalAlignment.Right;
            }
        }

        public static string GetThisExecutableDirectory()
        {
            string loc = Application.ExecutablePath;
            loc = System.IO.Path.GetDirectoryName(loc) + "\\";
            return loc;
        }

        public static bool AccessAllowed(string stringLevelRequired, AccessLevel currentLogin)
        {
            // return true if the currentLogin is equal to or greater than the given access
            // level (expressed as a string)
            AccessLevel needed = AccessLevel.Administrator;

            try
            {
                object obj = Enum.Parse(typeof(AccessLevel), stringLevelRequired, true);
                needed = (AccessLevel)obj;
            }
            catch (ArgumentException)
            {
            }

            return currentLogin >= needed;
        }

        /// <summary>
        /// Take a filename (generally a relative path) and determine the full path to the file to
        /// use.  First the directory containing the current .vpp file is checked for the given filename,
        /// then the directory containing this code's assembly is checked.
        /// </summary>
        public static string ResolveAssociatedFilename(string vppfname, string fname)
        {
            // check for the given file in the same directory as the developer vpp file path
            string trydev = System.IO.Path.GetDirectoryName(vppfname) + "\\" + fname;
            if (System.IO.File.Exists(trydev))
            {
                fname = trydev;
            }
            else
            {
                // otherwise use same directory as this executable
                fname = GetThisExecutableDirectory() + fname;
            }

            return fname;
        }
    }

    public class ResourceUtility
    {
        // helper class to wrap string resources for this application

        private static ResourceManager mResources;

        static ResourceUtility()
        {
            mResources = new ResourceManager("Vision_System.strings",
              System.Reflection.Assembly.GetExecutingAssembly());
        }

        public static string GetString(string resname)
        {
            string str = mResources.GetString(resname);
            if (str == null)
                str = "ERROR(" + resname + ")";
            return str;
        }

        public static string FormatString(string resname, string arg0)
        {
            try
            {
                return string.Format(GetString(resname), arg0);
            }
            catch (Exception)
            {
            }

            return "ERROR(" + resname + ")";
        }

        public static string FormatString(string resname, string arg0, string arg1)
        {
            try
            {
                return string.Format(GetString(resname), arg0, arg1);
            }
            catch (Exception)
            {
            }

            return "ERROR(" + resname + ")";
        }

    }

    /// <summary>
    /// 各种输入格式验证
    /// </summary>
    public class ValidateUtil
    {
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        #region 用户名密码格式

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string stringValue)
        {
            return Encoding.Default.GetBytes(stringValue).Length;
        }

        /// <summary>
        /// 检测用户名格式是否有效
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            int userNameLength = GetStringLength(userName);
            if (userNameLength >= 4 && userNameLength <= 20 && Regex.IsMatch(userName, @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$"))
            {   // 判断用户名的长度（4-20个字符）及内容（只能是汉字、字母、下划线、数字）是否合法
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 密码有效性
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[A-Za-z_0-9]{6,16}$");
        }
        #endregion

        #region 数字字符串检查

        /// <summary>
        /// int有效性
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        static public bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
        }
        static public bool IsValidAccountName(string name)
        {
            return Regex.IsMatch(name, @"[^\u4E00-\u9FA5]*");
        }
        /// <summary>
        /// 简单银行卡账号检查
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static public bool IsValidAccountNumber(string number)
        {
            return Regex.IsMatch(number, @"/\D/g{1}|/\D/g{3}");
        }
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="str">字符串</param> 
        public static int GetCHZNLength(string inputData)
        {
            System.Text.ASCIIEncoding n = new System.Text.ASCIIEncoding();
            byte[] bytes = n.GetBytes(inputData);

            int length = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //判断是否为汉字或全脚符号 
                {
                    length++;
                }
                length++;
            }
            return length;

        }

        #endregion

        #region 常用格式

        /// <summary>
        /// 验证身份证是否合法  15 和  18位两种
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>
        public static bool IsIdCard(string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
            {
                return false;
            }

            if (idCard.Length == 15)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }
            else if (idCard.Length == 18)
            {
                return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证年龄 
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        public static bool IsAge(string age)
        {
            if (string.IsNullOrEmpty(age))
            {
                return false;
            }

            if (age.Length >= 0)
            {
                return Regex.IsMatch(age, @"^^[0-9]*$");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static bool IsValidZip(string zip)
        {
            Regex rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(zip);
            return m.Success;
        }

        /// <summary>
        /// 固定电话有效性
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhone(string phone)
        {
            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$", RegexOptions.None);
            Match m = rx.Match(phone);
            return m.Success;
        }

        /// <summary>
        /// 手机有效性
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile)
        {
            Regex rx = new Regex(@"^(13|15|18)\d{9}$", RegexOptions.None);
            Match m = rx.Match(mobile);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="strVla"></param>
        /// <returns></returns>
        public static bool IsValidPhoneAndMobile(string number)
        {
            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(number);
            return m.Success;
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public bool IsValidURL(string url)
        {
            return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// IP有效性
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsValidIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain 有效性
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string host)
        {
            Regex r = new Regex(@"^\d+$");
            if (host.IndexOf(".") == -1)
            {
                return false;
            }
            return r.IsMatch(host.Replace(".", string.Empty)) ? false : true;
        }



        #endregion
        #region 日期检查

        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDate(string strValue)
        {
            return Regex.IsMatch(strValue, @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))");
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(string strValue)
        {
            return Regex.IsMatch(strValue, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }

        #endregion

        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>                        
        public static string CheckMathLength(string inputData, int maxLength)
        {
            if (inputData != null && inputData != string.Empty)
            {
                inputData = inputData.Trim();
                if (inputData.Length > maxLength)//按最大长度截取字符串
                {
                    inputData = inputData.Substring(0, maxLength);
                }
            }
            return inputData;
        }
        #endregion
    }
}
