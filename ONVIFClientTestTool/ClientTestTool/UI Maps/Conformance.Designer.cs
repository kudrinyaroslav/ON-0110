﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      Этот код был создан построителем кодированных тестов ИП.
//      Версия: 11.0.0.0
//
//      Изменения, внесенные в этот файл, могут привести к неправильной работе кода и будут
//      утрачены при повторном формировании кода.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace ClientTestTool.UIMaps.ConformanceClasses
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Построитель кодированных тестов ИП", "11.0.50727.1")]
    public partial class Conformance
    {
        
        /// <summary>
        /// CheckProfiles - Используйте "CheckProfilesParams" для передачи параметров в этот метод.
        /// </summary>
        public void CheckProfiles()
        {
            #region Variable Declarations
       
            WinTabPage uIConformanceTabPage = this.UIONVIFClientTestToolWindow.UITCMainWindow.UIConformanceTabPage;
            #endregion
            if (test.profilesExpectedResult != null)
            {
                // Щелкните "Conformance" вкладка
                uIConformanceTabPage.WaitForControlReady();
                Mouse.Click(uIConformanceTabPage);

                WinCheckBox uIProfileSwitherrataCheckBox = this.UIONVIFClientTestToolWindow.UIProfilesWindow.UIProfileSwitherrataCheckBox;
                WinCheckBox uIProfileGwitherrataCheckBox = this.UIONVIFClientTestToolWindow.UIProfilesWindow.UIProfileGwitherrataCheckBox;
                WinCheckBox uIProfileCCheckBox = this.UIONVIFClientTestToolWindow.UIProfilesWindow.UIProfileCCheckBox;
                WinCheckBox uIProfileQCheckBox = this.UIONVIFClientTestToolWindow.UIProfilesWindow.UIProfileQCheckBox;
                WinCheckBox uIProfileACheckBox = this.UIONVIFClientTestToolWindow.UIProfilesWindow.UIProfileACheckBox;
                foreach (var profileExpectedResult in test.profilesExpectedResult)
                {
                    LogData logDataProfile = new LogData();
                    logDataProfile.testPath = testPath;
                    logDataProfile.expectedResult = profileExpectedResult.Key + " " + profileExpectedResult.Value;
                    switch (profileExpectedResult.Key)
                    {
                        case "Profile S":
                            {
                                if (uIProfileSwitherrataCheckBox.Enabled)
                                {
                                    logDataProfile.currentResult = "Profile S NOT SUPPORTED";
                                }
                                else
                                {
                                    if (uIProfileSwitherrataCheckBox.Checked)
                                    {
                                        logDataProfile.currentResult = "Profile S SUPPORTED";
                                    }
                                    else
                                    {
                                        logDataProfile.currentResult = "Profile S NOT SUPPORTED";
                                    }
                                }
                                break;
                            }
                        case "Profile G":
                            {
                                if (uIProfileGwitherrataCheckBox.Enabled)
                                {
                                    logDataProfile.currentResult = "Profile G NOT SUPPORTED";
                                }
                                else
                                {
                                    if (uIProfileGwitherrataCheckBox.Checked)
                                    {
                                        logDataProfile.currentResult = "Profile G SUPPORTED";
                                    }
                                    else
                                    {
                                        logDataProfile.currentResult = "Profile G NOT SUPPORTED";
                                    }
                                }
                                break;
                            }
                           
                      
                        case "Profile C":
                            {
                                if (uIProfileCCheckBox.Enabled)
                                {
                                    logDataProfile.currentResult = "Profile C NOT SUPPORTED";
                                }
                                else
                                {
                                    if (uIProfileCCheckBox.Checked)
                                    {
                                        logDataProfile.currentResult = "Profile C SUPPORTED";
                                    }
                                    else
                                    {
                                        logDataProfile.currentResult = "Profile C NOT SUPPORTED";
                                    }
                                }
                                break;
                            }
                  
                        case "Profile Q":
                            {
                                if (uIProfileQCheckBox.Enabled)
                                {
                                    logDataProfile.currentResult = "Profile Q NOT SUPPORTED";
                                }
                                else
                                {
                                    if (uIProfileQCheckBox.Checked)
                                    {
                                        logDataProfile.currentResult = "Profile Q SUPPORTED";
                                    }
                                    else
                                    {
                                        logDataProfile.currentResult = "Profile Q NOT SUPPORTED";
                                    }
                                }
                                break;
                            }
                     
                        case "Profile A":
                            {
                                if (uIProfileACheckBox.Enabled)
                                {
                                    logDataProfile.currentResult = "Profile A NOT SUPPORTED";
                                }
                                else
                                {
                                    if (uIProfileACheckBox.Checked)
                                    {
                                        logDataProfile.currentResult = "Profile A SUPPORTED";
                                    }
                                    else
                                    {
                                        logDataProfile.currentResult = "Profile A NOT SUPPORTED";
                                    }
                                }
                                break;
                            }
                       
                    }
                    if (logDataProfile.currentResult == logDataProfile.expectedResult)
                    {
                        logDataProfile.result = "PASSED";
                    }
                    else
                    {
                        logDataProfile.result = "FAILED";
                    }
                    this.logDataProfiles.Add(logDataProfile);
                }
            }
        }     
        
        #region Properties
        public virtual CheckProfilesParams CheckProfilesParams
        {
            get
            {
                if ((this.mCheckProfilesParams == null))
                {
                    this.mCheckProfilesParams = new CheckProfilesParams();
                }
                return this.mCheckProfilesParams;
            }
        }
        
        public virtual RecordedMethod1Params RecordedMethod1Params
        {
            get
            {
                if ((this.mRecordedMethod1Params == null))
                {
                    this.mRecordedMethod1Params = new RecordedMethod1Params();
                }
                return this.mRecordedMethod1Params;
            }
        }
        
        public UIONVIFClientTestToolWindow UIONVIFClientTestToolWindow
        {
            get
            {
                if ((this.mUIONVIFClientTestToolWindow == null))
                {
                    this.mUIONVIFClientTestToolWindow = new UIONVIFClientTestToolWindow();
                }
                return this.mUIONVIFClientTestToolWindow;
            }
        }
        #endregion
        
        #region Fields
        private CheckProfilesParams mCheckProfilesParams;
        
        private RecordedMethod1Params mRecordedMethod1Params;
        
        private UIONVIFClientTestToolWindow mUIONVIFClientTestToolWindow;
        #endregion
    }
    
    /// <summary>
    /// Параметры для передачи в "CheckProfiles"
    /// </summary>
    [GeneratedCode("Построитель кодированных тестов ИП", "11.0.50727.1")]
    public class CheckProfilesParams
    {
        
        #region Fields
        /// <summary>
        /// Выбор "Profile S(with errata)" флажок
        /// </summary>
        public bool UIProfileSwitherrataCheckBoxChecked = true;
        #endregion
    }
    
    /// <summary>
    /// Параметры для передачи в "RecordedMethod1"
    /// </summary>
    [GeneratedCode("Построитель кодированных тестов ИП", "11.0.50727.1")]
    public class RecordedMethod1Params
    {
        
        #region Fields
        /// <summary>
        /// Выбор "Profile S(with errata)" флажок
        /// </summary>
        public bool UIProfileSwitherrataCheckBoxChecked = true;
        
        /// <summary>
        /// Выбор "Profile G(with errata)" флажок
        /// </summary>
        public bool UIProfileGwitherrataCheckBoxChecked = true;
        #endregion
    }
    
    [GeneratedCode("Построитель кодированных тестов ИП", "11.0.50727.1")]
    public class UIONVIFClientTestToolWindow : WinWindow
    {
        
        public UIONVIFClientTestToolWindow()
        {
            #region Условия поиска
            this.SearchProperties[WinWindow.PropertyNames.Name] = "ONVIF Client Test Tool";
            this.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("ONVIF Client Test Tool");
            #endregion
        }
        
        #region Properties
        public UIProfilesWindow UIProfilesWindow
        {
            get
            {
                if ((this.mUIProfilesWindow == null))
                {
                    this.mUIProfilesWindow = new UIProfilesWindow(this);
                }
                return this.mUIProfilesWindow;
            }
        }

        public UITCMainWindow UITCMainWindow
        {
            get
            {
                if ((this.mUITCMainWindow == null))
                {
                    this.mUITCMainWindow = new UITCMainWindow(this);
                }
                return this.mUITCMainWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIProfilesWindow mUIProfilesWindow;

        private UITCMainWindow mUITCMainWindow;
        #endregion
    }

    [GeneratedCode("Построитель кодированных тестов ИП", "11.0.50727.1")]
    public class UITCMainWindow : WinWindow
    {

        public UITCMainWindow(UITestControl searchLimitContainer) :
            base(searchLimitContainer)
        {
            #region Условия поиска
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "tCMain";
            this.WindowTitles.Add("ONVIF Client Test Tool");
            #endregion
        }

        #region Properties
        public WinTabPage UIConformanceTabPage
        {
            get
            {
                if ((this.mUIConformanceTabPage == null))
                {
                    this.mUIConformanceTabPage = new WinTabPage(this);
                    #region Условия поиска
                    this.mUIConformanceTabPage.SearchProperties[WinTabPage.PropertyNames.Name] = "Conformance";
                    this.mUIConformanceTabPage.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIConformanceTabPage;
            }
        }
        #endregion

        #region Fields
        private WinTabPage mUIConformanceTabPage;
        #endregion
    }
    
    [GeneratedCode("Построитель кодированных тестов ИП", "11.0.50727.1")]
    public class UIProfilesWindow : WinWindow
    {
        
        public UIProfilesWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Условия поиска
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "Profiles";
            this.WindowTitles.Add("ONVIF Client Test Tool");
            #endregion
        }
        
        #region Properties
        public WinClient UIProfilesClient
        {
            get
            {
                if ((this.mUIProfilesClient == null))
                {
                    this.mUIProfilesClient = new WinClient(this);
                    #region Условия поиска
                    this.mUIProfilesClient.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIProfilesClient;
            }
        }
        
        public WinCheckBox UIProfileSwitherrataCheckBox
        {
            get
            {
                if ((this.mUIProfileSwitherrataCheckBox == null))
                {
                    this.mUIProfileSwitherrataCheckBox = new WinCheckBox(this);
                    #region Условия поиска
                    this.mUIProfileSwitherrataCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile S(with errata)";
                    if (!this.mUIProfileSwitherrataCheckBox.Exists)
                    {
                        this.mUIProfileSwitherrataCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile S";
                    }
                    this.mUIProfileSwitherrataCheckBox.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIProfileSwitherrataCheckBox;
            }
        }
        
        public WinCheckBox UIProfileGwitherrataCheckBox
        {
            get
            {
                if ((this.mUIProfileGwitherrataCheckBox == null))
                {
                    this.mUIProfileGwitherrataCheckBox = new WinCheckBox(this);
                    #region Условия поиска
                    this.mUIProfileGwitherrataCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile G(with errata)";
                    if (!this.mUIProfileGwitherrataCheckBox.Exists)
                    {
                        this.mUIProfileGwitherrataCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile G";
                    }
                    this.mUIProfileGwitherrataCheckBox.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIProfileGwitherrataCheckBox;
            }
        }
        
        public WinCheckBox UIProfileCCheckBox
        {
            get
            {
                if ((this.mUIProfileCCheckBox == null))
                {
                    this.mUIProfileCCheckBox = new WinCheckBox(this);
                    #region Условия поиска
                    this.mUIProfileCCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile C(with errata)";
                    if (!this.mUIProfileCCheckBox.Exists)
                    {
                        this.mUIProfileCCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile C";
                    }
                    this.mUIProfileCCheckBox.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIProfileCCheckBox;
            }
        }
        
        public WinCheckBox UIProfileQCheckBox
        {
            get
            {
                if ((this.mUIProfileQCheckBox == null))
                {
                    this.mUIProfileQCheckBox = new WinCheckBox(this);
                    #region Условия поиска
                    this.mUIProfileQCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile Q(with errata)";
                    if (!this.mUIProfileQCheckBox.Exists)
                    {
                        this.mUIProfileQCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile Q";
                    }
                    this.mUIProfileQCheckBox.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIProfileQCheckBox;
            }
        }
        
        public WinCheckBox UIProfileACheckBox
        {
            get
            {
                if ((this.mUIProfileACheckBox == null))
                {
                    this.mUIProfileACheckBox = new WinCheckBox(this);
                    #region Условия поиска
                    this.mUIProfileACheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile A(with errata)";
                    if (!this.mUIProfileACheckBox.Exists)
                    {
                        this.mUIProfileACheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Profile A";
                    }
                    this.mUIProfileACheckBox.WindowTitles.Add("ONVIF Client Test Tool");
                    #endregion
                }
                return this.mUIProfileACheckBox;
            }
        }
        #endregion
        
        #region Fields
        private WinClient mUIProfilesClient;
        
        private WinCheckBox mUIProfileSwitherrataCheckBox;
        
        private WinCheckBox mUIProfileGwitherrataCheckBox;
        
        private WinCheckBox mUIProfileCCheckBox;
        
        private WinCheckBox mUIProfileQCheckBox;
        
        private WinCheckBox mUIProfileACheckBox;
        #endregion
    }
}