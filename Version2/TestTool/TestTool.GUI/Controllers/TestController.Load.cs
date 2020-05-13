using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TestTool.GUI.Data;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Data;
using TestTool.Tests.Definitions.Interfaces;
using TestTool.Tests.Definitions.UI;

namespace TestTool.GUI.Controllers
{
    /// <summary>
    /// Loadin test cases and profiles
    /// </summary>
    partial class TestController
    {

        #region LOAD

        /// <summary>
        /// Tests information
        /// </summary>
        private List<TestInfo> _testInfos;

        /// <summary>
        /// Profiles known
        /// </summary>
        private List<IProfileDefinition> _onvifProfiles;

        /// <summary>
        /// Tests list
        /// </summary>
        public List<TestInfo> TestInfos
        {
            get { return _testInfos; }
        }

        /// <summary>
        /// List of supported tests.
        /// This list is initialized when feature definition completes. Later when all results are 
        /// cleared, this list is also cleared; but if only test results are cleared, profiles tree is 
        /// coloured using this information: list of selected tests is used to define features which will be tested.
        /// </summary>
        private List<TestInfo> _testsSupported;

        /// <summary>
        /// Controls with advanced settings (not used in version 12.06)
        /// </summary>
        private Dictionary<Guid, SettingsTabPage> _controls;

        /// <summary>
        /// Types of additional settings (not used in version 12.06)
        /// </summary>
        private List<Type> _settingsTypes;

        /// <summary>
        /// Types of additional settings (not used in version 12.06)
        /// </summary>
        public List<Type> AdvancedSettingsTypes
        {
            get { return _settingsTypes; }
        }

        /// <summary>
        /// Holds profiles testing results (used to provide profile checking log when 
        /// profile is selected) 
        /// </summary>
        private Dictionary<IProfileDefinition, ProfileTestInfo> _profilesSupportInfo;

        /// <summary>
        /// Initialization.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            LoadTests();
        }

        /// <summary>
        /// Enumerates files in current directory and finds tests.
        /// </summary>
        /// <returns>List of tests loaded.</returns>
        public List<TestInfo> LoadTests()
        {
            // initialize lists
            _testInfos = new List<TestInfo>();
            _onvifProfiles = new List<IProfileDefinition>();
            _controls = new Dictionary<Guid, SettingsTabPage>();
            _settingsTypes = new List<Type>();

            Dictionary<Type, Type> controls = new Dictionary<Type, Type>();

            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);

            // enumerate files
            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    System.Reflection.Assembly assembly = Assembly.LoadFile(file);
                    
                    // check if this is a test assembly
                    if (assembly.GetCustomAttributes(
                        typeof(TestAssemblyAttribute),
                        false).Length > 0)
                    {
                        // Test assembly

                        // enumerate types
                        foreach (Type t in assembly.GetTypes())
                        {
                            // Load tests, if this is a test class
                            object[] attrs = t.GetCustomAttributes(typeof(TestClassAttribute), true);
                            if (attrs.Length > 0)
                            {
                                LoadTests(t);
                            }

                            // Load profiles, if this is sa profile
                            attrs = t.GetCustomAttributes(typeof(ProfileDefinitionAttribute), true);
                            if (attrs.Length > 0)
                            {
                                LoadProfile(t);
                            }

                            // Load settings controls, if this is a settings control
                            attrs = t.GetCustomAttributes(typeof(SettingsControlAttribute), true);
                            if (attrs.Length > 0)
                            {
                                SettingsControlAttribute attr = (SettingsControlAttribute)attrs[0];
                                // use only one control for each type.
                                if (!controls.ContainsKey(attr.ParametersType))
                                {
                                    controls.Add(attr.ParametersType, t);
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {

                }
            }

            // create only necessary settings controls
            foreach (Type type in _settingsTypes)
            {
                Type ctrlType = controls[type];

                object ctrl = Activator.CreateInstance(ctrlType);
                SettingsTabPage ctl = (SettingsTabPage)ctrl;
                ctl.Dock = DockStyle.Fill;

                _controls.Add(type.GUID, (SettingsTabPage)ctrl);
            }

            // notify that settings control are loaded
            if (SettingPagesLoaded != null)
            {
                SettingPagesLoaded(_controls.Values.ToList());
            }

            // Display tests
            if (View.TestTreeView != null)
            {
                View.TestTreeView.DisplayTests(_testInfos);
            }
            // Display profiles
            if (View.ProfilesView != null)
            {
                View.ProfilesView.DisplayProfiles(_onvifProfiles.OrderBy(P => P.GetProfileName()));
            }

            return _testInfos;
        }

        /// <summary>
        /// Loads tests from the type specified
        /// </summary>
        /// <param name="t"></param>
        void LoadTests(Type t)
        {
            // if this is a test class,
            if (t.GetInterfaces().Contains(typeof(ITest)))
            {
                // enumerate methods.
                foreach (MethodInfo mi in t.GetMethods())
                {
                    if (mi.DeclaringType != t)
                    {
                        //System.Diagnostics.Debug.WriteLine(string.Format("Method [{0}] in [{1}] is inherited from [{2}] - skip processing ", mi.Name, t.Name, mi.DeclaringType.Name));
                        continue;
                    }

                    object[] testAttributes = mi.GetCustomAttributes(typeof(TestAttribute), true);

                    // if this is a test method,
                    if (testAttributes.Length > 0)
                    {
                        // get test information.
                        TestAttribute attribute = (TestAttribute)testAttributes[0];

                        // check if a test needs to be updated
                        TestInfo existing =
                            _testInfos.Where(ti => ti.Id == attribute.Id && ti.Category == attribute.Category).FirstOrDefault();

                        if (existing != null)
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format("--- One more test with order {0} found [{1} in {2} and {3} in {4} ]", attribute.Order, attribute.Name, t.Name, existing.Name, existing.Method.ReflectedType.Name));
                                                        
                            if (existing.Version > attribute.Version)
                            {
                                System.Diagnostics.Debug.WriteLine("Leave test already loaded");
                                continue;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Reload newer test");
                                _testInfos.Remove(existing);
                            }
                        }

                        // add test info
                        TestInfo testInfo = new TestInfo();
                        testInfo.Method = mi;
                        testInfo.Name = attribute.Name;
                        testInfo.Group = attribute.Path;
                        testInfo.Order = attribute.Order;
                        testInfo.ExecutionOrder = attribute.ExecutionOrder;
                        testInfo.Id = attribute.Id;
                        testInfo.LastChangedIn = attribute.LastChangedIn;
                        testInfo.Category = attribute.Category;
                        testInfo.Version = attribute.Version;
                        testInfo.RequirementLevel = attribute.RequirementLevel;
                        testInfo.RequiredFeatures.AddRange(attribute.RequiredFeatures);
                        testInfo.FunctionalityUnderTest.AddRange(attribute.FunctionalityUnderTest);

                        _testInfos.Add(testInfo);

                        // check if this test requires additional settings
                        if (attribute.ParametersTypes != null)
                        {
                            foreach (Type type in attribute.ParametersTypes)
                            {
                                if (!_settingsTypes.Contains(type))
                                {
                                    _settingsTypes.Add(type);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads profile from the type
        /// </summary>
        /// <param name="t"></param>
        void LoadProfile(Type t)
        {
            if (t.GetInterfaces().Contains(typeof(IProfileDefinition)))
            {
                IProfileDefinition profile = (IProfileDefinition)Activator.CreateInstance(t, new object[0]);
                _onvifProfiles.Add(profile);
            }
        }

        /// <summary>
        /// Used to notify Management page that new settings page is loaded.
        /// </summary>
        public event Action<List<SettingsTabPage>> SettingPagesLoaded;


        #endregion

    }
}
