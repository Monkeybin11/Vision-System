using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_System
{
    /// <summary>
    /// 触发是上升沿还是下降沿
    /// </summary>
    public enum TriggerMode
    {
        FirstOrIndependentTrigger,  // 独立的触发信号，一个触发信号控制一个相机拍照
        AttachedTrigger             // 一个触发信号控制相机先后拍照两次
    }

    /// <summary>
    /// 触发是上升沿还是下降沿
    /// </summary>
    public enum TriggerControl
    {
        SoftwareTrigger,  // Software表示上位机软件捕获PLC触发信号，通过软件进行触发
        HardwareTrigger   // Hardware表示PLC直接控制相机触发IO进行拍照
    }

    public class JobHelper
    {
        private int _ToolBlockCnt = 10;

        #region 检测工具运行结果，计数统计等信息
        //检测工具运行总计数
        private List<int> _TotalCountForItem = new List<int>();
        //检测工具失效总计数
        private List<int> _FailCountForItem = new List<int>();
        //检测工具通过率
        private List<double?> _YieldForItem = new List<double?>();
        //检测工具运行结果
        private List<bool> _PassFailResultForItem = new List<bool>();
        //检测工具对应的失效模式关键字
        private List<string> _FailuremodeKeyWd = new List<string>();
        //指定关键字的失效判定结果
        private List<bool> _FailResultForKeyWd = new List<bool>();
        //指定关键字的失效计数
        private List<int> _FailCountForKeyWd = new List<int>();
        //检测工具的名称索引，用于查找失效模式关键字，string为ToolBlock名字（失效名称），int为对应的序号
        private Dictionary<int, string> toolNameDictionary = new Dictionary<int, string>();
        //CCD检测总结果
        private bool _JobTotalResult = false;
        //CCD运行次数总数
        private int _JobTotalRunCnt = 0;
        //CCD运行结果OK总数
        private int _JobTotalRunPass = 0;
        //CCD检测通过率, 为null时表示mJobTotalRunCnt为0
        private double? _JobYield = 0.0;
        //ToolBlock的输入
        private List<object> _inputData = new List<object>();
        //ToolBlock的输出结果
        private List<object> _outputData = new List<object>();
        #endregion

        #region job setting
        // CCD名字
        private string _ccdName;
        // 是否保存图片，默认为保存，最终图片是否保存还要根据总的设置要求进行判断
        private bool _isSaveImage = true;
        //单个CCD的子料号列表
        private List<string> _singleCCDPartNoList = new List<string>();
        // CCD触发延时
        private int _CCDAcqDelayTimeMilliSecond;
        // OK/NG信号发送脉冲时间
        private int _DoOutTimePeriod;
        //检测工具是否使能
        private List<bool> _isToolBlockEnabledForItem = new List<bool>();
        // 光源是否通过软件控制
        private bool _isLightControl = true;
        //检测job是否需要控制光源频闪
        private bool _isLightFlash = false;
        //光源打开延时的时间
        private int _lightElapseTime = 200;
        //光源名称
        private string _lightSourceName = "";
        //触发源名称
        private string _triggerSourceName = "";
        // 是单次触发还是在触发完成后二次拍照(共用触发源),默认的是单个触发信号控制一次拍照
        private TriggerMode _triggerMode = TriggerMode.FirstOrIndependentTrigger;
        // 触发信号来源:Software/Hardware
        private TriggerControl _triggerControl = TriggerControl.SoftwareTrigger;
        // 如果是二次触发，触发源对应的第一次拍照的CCD序号，从0开始计数，
        // 如果是独立的触发信号，且不需要二次拍照，则为null
        private int? _attachedCCDIndex = null;
        private string _showOutputDataName = "";
        #endregion jot setting end

        public string CcdName { get => _ccdName; set => _ccdName = value; }
        public bool IsSaveImage { get => _isSaveImage; set => _isSaveImage = value; }
        public int DoOutTimePeriod { get => _DoOutTimePeriod; set => _DoOutTimePeriod = value; }
        public int CCDAcqDelayTimeMilliSecond { get => _CCDAcqDelayTimeMilliSecond; set => _CCDAcqDelayTimeMilliSecond = value; }
        public List<int> TotalCountForItem { get => _TotalCountForItem; set => _TotalCountForItem = value; }
        public List<int> FailCountForItem { get => _FailCountForItem; set => _FailCountForItem = value; }
        public List<double?> YieldForItem { get => _YieldForItem; set => _YieldForItem = value; }
        public int ToolBlockCnt { get => _ToolBlockCnt; set => _ToolBlockCnt = value; }
        public List<bool> IsToolBlockEnabledForItem { get => _isToolBlockEnabledForItem; set => _isToolBlockEnabledForItem = value; }
        public List<bool> PassFailResultForItem { get => _PassFailResultForItem; set => _PassFailResultForItem = value; }
        public bool JobTotalResult { get => _JobTotalResult; set => _JobTotalResult = value; }
        public int JobTotalRunCnt { get => _JobTotalRunCnt; set => _JobTotalRunCnt = value; }
        public int JobTotalRunPass { get => _JobTotalRunPass; set => _JobTotalRunPass = value; }
        public double? JobYield { get => _JobYield; set => _JobYield = value; }
        public bool IsLightControl { get => _isLightControl; set => _isLightControl = value; }
        public bool IsLightFlash { get => _isLightFlash; set => _isLightFlash = value; }
        public int LightElapseTime { get => _lightElapseTime; set => _lightElapseTime = value; }
        public string LightSourceName { get => _lightSourceName; set => _lightSourceName = value; }
        public Dictionary<int, string> ToolBlockNameDictionary { get => toolNameDictionary; set => toolNameDictionary = value; }
        public List<string> FailuremodeKeyWd { get => _FailuremodeKeyWd; set => _FailuremodeKeyWd = value; }
        public List<bool> FailResultForKeyWd { get => _FailResultForKeyWd; set => _FailResultForKeyWd = value; }
        public List<int> FailCountForKeyWd { get => _FailCountForKeyWd; set => _FailCountForKeyWd = value; }
        public string TriggerSourceName { get => _triggerSourceName; set => _triggerSourceName = value; }
        public TriggerMode TriggerMode { get => _triggerMode; set => _triggerMode = value; }
        public TriggerControl TriggerControl { get => _triggerControl; set => _triggerControl = value; }
        public List<string> SingleCCDPartNoList { get => _singleCCDPartNoList; set => _singleCCDPartNoList = value; }
        public int? AttachedCCDIndex { get => _attachedCCDIndex; set => _attachedCCDIndex = value; }
        public List<object> OutputData { get => _outputData; set => _outputData = value; }
        public List<object> InputData { get => _inputData; set => _inputData = value; }
        public string ShowOutputDataName { get => _showOutputDataName; set => _showOutputDataName = value; }

        /// <summary>
        /// 读取每个Job的设置：光源是否频闪，检测工具是否使能
        /// </summary>
        /// <param name="camIndex"></param>
        /// <param name="strConfigPath"></param>
        /// <param name="toolBlock"></param>
        public void ReadJobSetting(int camIndex, string strConfigPath, CogToolBlock toolBlock)
        {
            CogToolBlock tool;
            string strTemp;
            string[] strArray;
            IniFile ConfigIniFile = new IniFile(strConfigPath);

            // CCD名称，显示在界面的名字
            strTemp = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "Name");
            if (!string.IsNullOrEmpty(strTemp))
            {
                CcdName = strTemp;
            }

            // 单个CCD图片是否保存
            strTemp = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "ImageSave");
            if (!string.IsNullOrEmpty(strTemp))
            {
                IsSaveImage = strTemp == "1" ? true : false;
            }

            // 获取单个CCD的料号列表
            strTemp = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "SingleCCDPNList");
            if (!string.IsNullOrEmpty(strTemp))
            {
                strArray = strTemp.Split(',');
                for (int j = 0; j < strArray.Length; j++)
                {
                    SingleCCDPartNoList.Add(strArray[j]);
                }
            }

            // 料号是否共享
            strTemp = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "DoOutTime");

            // 读取相机取像延时时间
            CCDAcqDelayTimeMilliSecond = Convert.ToInt16(
                ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "DelayTime"));
            DoOutTimePeriod = Convert.ToInt16(
                ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "DoOutTime"));

            // 触发控制选项
            TriggerControl = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "TriggerControl")
                == "Software" ? TriggerControl.SoftwareTrigger : TriggerControl.HardwareTrigger;

            // 触发源名称
            strTemp = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "TriggerName");

            // 触发源形式：如果是单独的触发， TriggerMode.FirstOrIndependentTrigger
            if (!strTemp.Contains("Completed"))
            {
                TriggerMode = TriggerMode.FirstOrIndependentTrigger;
                TriggerSourceName = strTemp;
                AttachedCCDIndex = null;
            }
            // 触发源形式：如果是共用触发源二次触发， TriggerMode = TriggerMode.AttachedTrigger
            else
            {
                TriggerMode = TriggerMode.AttachedTrigger;
                TriggerSourceName = strTemp;
                string strAttached = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "AttachedCCDIndex");
                if (!string.IsNullOrEmpty(strAttached))
                {
                    AttachedCCDIndex = Convert.ToInt32(strAttached) - 1;
                }
            }

            // 光源是否通过上位机控制
            IsLightControl = ConfigIniFile.IniReadValue(
                    "CCD" + (camIndex + 1), "LightControl") == "1" ? true : false;

            // 光源是否频闪
            IsLightFlash = ConfigIniFile.IniReadValue(
                    "CCD" + (camIndex + 1), "LightFlash") == "1" ? true : false;

            // 光源名称
            LightSourceName = ConfigIniFile.IniReadValue(
                "CCD" + (camIndex + 1), "LightName");

            // 图表要显示的数据的名称，图表显示的数据可以从ToolBlock的output中取出
            ShowOutputDataName = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "ShowDataName");

            // 读取每个CCD检测工具的使能状态
            // 默认状态下每个toolBlock是开启的
            strTemp = ConfigIniFile.IniReadValue("CCD" + (camIndex + 1), "ToolEnable");
            strArray = strTemp.Split(',');

            ToolBlockCnt = toolBlock.Tools.Count;
            for (int j = 0; j < ToolBlockCnt; j++)
            {
                // 读取每个CCD检测工具的使能状态
                // 默认状态下每个toolBlock是开启的
                IsToolBlockEnabledForItem.Add(strArray[j] == "1" ? true : false);
                tool = (CogToolBlock)toolBlock.Tools[j];
                TotalCountForItem.Add(0);
                FailCountForItem.Add(0);
                YieldForItem.Add(1.0);
                PassFailResultForItem.Add(true);
                ToolBlockNameDictionary.Add(
                    j,
                    tool.Name);
            }

            // 初始化失效模式关键字
            strTemp = ConfigIniFile.IniReadValue(
                "CCD" + (camIndex + 1),
                "FailModeKW");
            strArray = strTemp.Split(',');
            for (int j = 0; j < strArray.Length; j++)
            {
                if (!string.IsNullOrEmpty(strArray[j]))
                {
                    FailuremodeKeyWd.Add(strArray[j]);
                }
            }

            // 初始化失效模式统计数据
            for (int k = 0; k < FailuremodeKeyWd.Count; k++)
            {
                FailResultForKeyWd.Add(true);
                FailCountForKeyWd.Add(0);
            }
        }
    }
}
