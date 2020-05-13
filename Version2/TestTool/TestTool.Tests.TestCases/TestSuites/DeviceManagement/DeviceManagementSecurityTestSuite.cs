///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.Linq;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.TestBase;
using TestTool.Proxies.Onvif;
using System.ServiceModel;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    class DeviceManagementSecurityTestSuite : DeviceManagementTest
    {
        #region Test class header
        /// <summary>
        /// Indicates that passwords from specification should be used.
        /// </summary>
        protected bool _useEmbeddedPassword;
        /// <summary>
        /// First user-provided password to use.
        /// </summary>
        protected string _password1;
        /// <summary>
        /// Second user-provided password to use.
        /// </summary>
        protected string _password2;

        public DeviceManagementSecurityTestSuite(TestLaunchParam param)
            : base(param)
        {
            _useEmbeddedPassword = param.UseEmbeddedPassword;
            _password1 = param.Password1;
            _password2 = param.Password2;
        }

        private const string PATH = "Device Management\\Security";
        #endregion

        #region 4-1-1 SECURITY COMMAND GETUSERS
        [Test(Name = "SECURITY COMMAND GETUSERS",
            Order = "04.01.01",
            Id = "4-1-1",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            //LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetUsers })]
        public void GetUsersTest()
        {
            RunTest(() =>
            {
                User[] users = GetUsers();
                //  we should accept empty list (or null)

                BeginStep("Validate response received");

                if (users != null)
                {
                    foreach (User user in users)
                    {
                        if (!string.IsNullOrEmpty(user.Password))
                        {
                            throw new AssertException(
                                string.Format("The DUT returned Password for {0} user", user.Username));
                        }
                    }
                }
                StepPassed();

            });
        }
        #endregion

        #region 4-1-3 SECURITY COMMAND CREATEUSERS ERROR CASE
        [Test(Name = "SECURITY COMMAND CREATEUSERS ERROR CASE",
            Order = "04.01.03",
            Id = "4-1-3",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateUsers })]
        public void CreateUsersErrorCaseTest()
        {
            bool succeeded = false;
            bool userCreated = false;

            string userName1 = "OnvifTest1";
            string userName2 = "OnvifTest4";

            RunTest(() =>
            {
                bool bExitTest = false;

                // Create user
                string password1 = _useEmbeddedPassword ? "OnvifTest123" : _password1;

                User newUser1 = new User { Username = userName1, UserLevel = UserLevel.Operator, Password = password1 };

                try
                {
                    CreateUsers(new[] { newUser1 });
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                    {
                        LogStepEvent("Maximum number of supported users exceeded.");
                        StepPassed();
                        bExitTest = true;
                    }
                    else
                    {
                        throw;
                    }
                }
                // Get users 

                if (bExitTest)
                {
                    return;
                }

                // for rollback action
                userCreated = true;

                // current users list
                User[] users = GetUsers();

                Assert(users != null, "The DUD did not return any user", "Check if the DUT returned users list");

                // check that user has been created
                User user = users.Where(u => u.Username == userName1).FirstOrDefault();

                Assert(user != null,
                       string.Format("Newly created user {0} not found", userName1),
                       "Check if newly created user is present in the list");

                Assert(user.UserLevel == newUser1.UserLevel,
                       string.Format("UserLevel for {0} differs from level which has been set", user.Username),
                       "Check if user has been created correctly");

                // Create user - error case 
                newUser1.UserLevel = UserLevel.User;
                try
                {
                    RunStep(() => { Client.CreateUsers(new[] { newUser1 }); },
                        "Create User - Negative test",
                        "Sender/OperationProhibited/UsernameClash");
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                    {
                        LogStepEvent("Maximum number of supported users exceeded.");
                        StepPassed();
                        bExitTest = true;
                    }
                    else
                    {
                        throw;
                    }
                }

                DoRequestDelay();

                if (!bExitTest)
                {
                    // Create two users 

                    User newUser2 = new User
                    {
                        Username = userName2,
                        UserLevel = UserLevel.Operator,
                        Password = password1
                    };

                    try
                    {
                        RunStep(() => { Client.CreateUsers(new[] { newUser1, newUser2 }); },
                            "Create User - Negative test",
                            "Sender/OperationProhibited/UsernameClash");
                    }
                    catch (FaultException exc)
                    {
                        if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                        {
                            LogStepEvent("Maximum number of supported users exceeded.");
                            StepPassed();
                            bExitTest = true;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    DoRequestDelay();

                    if (!bExitTest)
                    {
                        users = GetUsers();

                        Assert(users != null, "The DUD did not return any user",
                               "Check if the DUT returned users list");

                        user = users.Where(u => u.Username == userName2).FirstOrDefault();

                        Assert(user == null,
                               string.Format("User {0} has been created", userName2),
                               "Check if no new users have been created");

                        user = users.Where(u => u.Username == userName1).FirstOrDefault();

                        Assert(user != null,
                               string.Format("Newly created user {0} not found", userName1),
                               "Check if previously created user is present in the list");

                        Assert(user.UserLevel == UserLevel.Operator,
                               string.Format("UserLevel for {0} differs from level which has been set", user.Username),
                               "Check if previously created user has correct level");
                    }
                }

                // user userName2 should not be created. Whole operation must be cancelled.
                DeleteUsers(new[] { userName1 });
                succeeded = true;
            },
            () =>
            {

                if (!succeeded && userCreated)
                {
                    CleanUpUsers(new[] { userName1, userName2 });
                }
            });
        }
        #endregion

        #region 4-1-4 SECURITY COMMAND DELETEUSERS
        [Test(Name = "SECURITY COMMAND DELETEUSERS",
            Order = "04.01.04",
            Id = "4-1-4",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteUsers })]
        public void DeleteUsersTest()
        {
            bool succeeded = false;
            bool userCreated = false;

            string userName1 = "OnvifTest1";
            string userName2 = "OnvifTest2";
            string userName3 = "OnvifTest3";

            RunTest(() =>
            {
                string password1 = _useEmbeddedPassword ? "OnvifTest123" : _password1;

                User newUser1 = new User { Username = userName1, UserLevel = UserLevel.Operator, Password = password1 };
                User newUser2 = new User { Username = userName2, UserLevel = UserLevel.Operator, Password = password1 };
                User newUser3 = new User { Username = userName3, UserLevel = UserLevel.User, Password = password1 };

                bool bExitTest = false;

                try
                {
                    CreateUsers(new[] { newUser1, newUser2, newUser3 });
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                    {
                        LogStepEvent("Maximum number of supported users exceeded.");
                        StepPassed();
                        bExitTest = true;
                    }
                    else
                    {
                        throw;
                    }
                }
                userCreated = true;
                if (!bExitTest)
                {
                    User[] users = GetUsers();
                    Assert(users != null, "The DUD did not return any user", "Check if the DUT returned users list");

                    User user = users.FirstOrDefault(u => u.Username == userName1);
                    User user2 = users.FirstOrDefault(u => u.Username == userName2);
                    User user3 = users.FirstOrDefault(u => u.Username == userName3);

                    Assert((user != null) && (user2 != null) && (user3 != null),
                        string.Format("User {0} has not been created", user == null ? userName1 : user2 == null ? userName2 : userName3));

                    // Delete user 

                    DeleteUsers(new[] { userName1 });

                    // Check if the user really has been deleted 

                    users = GetUsers();

                    Assert(users != null, "The DUD did not return any user", "Check if the DUT returned users list");

                    user = users.FirstOrDefault(u => u.Username == userName1);
                    user2 = users.FirstOrDefault(u => u.Username == userName2);
                    user3 = users.FirstOrDefault(u => u.Username == userName3);
                    Assert((user == null) && (user2 != null) && (user3 != null),
                        user == null ? string.Format("User {0} has not been deleted", userName1) :
                        string.Format("User {0} has been deleted", user2 == null ? userName2 : userName3),
                        "Check if the user has been deleted");

                    DeleteUsers(new[] { userName2, userName3 });

                    // Check if the user really has been deleted 

                    users = GetUsers();

                    Assert(users != null, "The DUD did not return any user", "Check if the DUT returned users list");

                    BeginStep("Check if both users have been deleted");

                    foreach (string username in new[] { userName2, userName3 })
                    {
                        user = users.Where(u => u.Username == username).FirstOrDefault();

                        if (user != null)
                        {
                            throw new AssertException(string.Format("User {0} has not been deleted", username));
                        }

                    }

                    StepPassed();

                }
                succeeded = true;
            },
            () =>
            {
                if (!succeeded && userCreated)
                {
                    CleanUpUsers(new[] { userName1, userName2, userName3 });
                }
            });
        }
        #endregion

        #region 4-1-5 SECURITY COMMAND DELETEUSERS ERROR CASE
        [Test(Name = "SECURITY COMMAND DELETEUSERS ERROR CASE",
            Order = "04.01.05",
            Id = "4-1-5",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteUsers })]
        public void DeleteUsersErrorCaseTest()
        {
            bool succeeded = false;
            bool userCreated = false;

            string userName1 = "OnvifTest1";
            string userName2 = "OnvifTest2";

            RunTest(() =>
            {
                string password1 = _useEmbeddedPassword ? "OnvifTest123" : _password1;

                User newUser1 = new User { Username = userName1, UserLevel = UserLevel.Operator, Password = password1 };

                bool bExitTest = false;

                try
                {
                    CreateUsers(new[] { newUser1 });
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                    {
                        LogStepEvent("Maximum number of supported users exceeded.");
                        StepPassed();
                        bExitTest = true;
                    }
                    else
                    {
                        throw;
                    }
                }

                if (!bExitTest)
                {
                    userCreated = true;
                    // Delete user 

                    RunStep(
                        () => { Client.DeleteUsers(new[] { userName1, userName2 }); },
                        "Delete Users - negative test",
                        "Sender/InvalidArgVal/UsernameMissing");

                    DoRequestDelay();

                    // Check if the user has not been deleted 

                    User[] users = GetUsers();

                    Assert(users != null, "The DUD did not return any user", "Check if the DUT returned users list");

                    User user = users.Where(u => u.Username == userName1).FirstOrDefault();

                    Assert(user != null,
                           string.Format("User {0} has been deleted", userName1),
                           string.Format("Check that the user {0} has not been deleted", userName1));

                    DeleteUsers(new[] { userName1 });

                    // Check if the user really has been deleted 

                    users = GetUsers();

                    Assert(users != null, "The DUD did not return any user", "Check if the DUT returned users list");

                    user = users.Where(u => u.Username == userName1).FirstOrDefault();

                    Assert(user == null,
                           string.Format("User {0} has not been deleted", userName1),
                           string.Format("Check that the user {0} has been deleted", userName1));
                }
                succeeded = true;
            },
            () =>
            {
                if (!succeeded && userCreated)
                {
                    CleanUpUsers(new[] { userName1 });
                }
            });
        }
        #endregion

        #region 4-1-6 SECURITY COMMAND DELETEUSERS DELETE ALL USERS
        [Test(Name = "SECURITY COMMAND DELETEUSERS DELETE ALL USERS",
            Order = "04.01.06",
            Id = "4-1-6",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            Interactive = true,
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.DeleteUsers })]
        public void DeleteAllUsers()
        {
            UserLevel currentUserLevel = UserLevel.Administrator;
            bool needToRestore = false;

            RunTest(() =>
            {
                User[] users = GetUsers();

                if ((users != null) && (users.Length > 0))
                {
                    List<string> allUsers = new List<string>();
                    foreach (User user in users)
                    {
                        allUsers.Add(user.Username);

                        if (user.Username == _username)
                        {
                            currentUserLevel = user.UserLevel;
                        }
                    }

                    bool bFaultReceived = false;

                    try
                    {
                        DeleteUsers(allUsers.ToArray());
                        needToRestore = true;
                    }
                    catch (FaultException exc)
                    {
                        bFaultReceived = true;

                        string reason;
                        bool bExpectedFault = exc.IsValidOnvifFault("Sender/InvalidArgVal/FixedUser", out reason);
                        if (bExpectedFault)
                        {
                            LogStepEvent("Allowed fault env:Sender/ter:InvalidArgVal/ter:FixedUser received");
                        }
                        else
                        {
                            string warning = string.Format("WARNING: The SOAP FAULT returned from the DUT is not as expected: {0}", reason);
                            LogStepEvent(warning);
                        }
                        StepPassed();
                    }

                    if (bFaultReceived)
                    {
                        User[] currentUsers = GetUsers();
                        int usersCount = 0;
                        if (currentUsers != null)
                        {
                            usersCount = currentUsers.Length;

                            needToRestore = currentUsers.Where(u => u.Username == _username).FirstOrDefault() == null;
                        }
                        Assert(usersCount == users.Length,
                            "Number of users after an unsuccessfull attempt to delete differs from initial",
                            "Since fault has been received, check if no users were deleted");

                    }
                }
                else
                {
                    LogTestEvent("No users received");
                }
            },
            () =>
            {
                if (needToRestore)
                {
                    LogTestEvent("Try to restore current user" + Environment.NewLine);
                    User user = new User()
                    {
                        Username = _username,
                        UserLevel = currentUserLevel,
                        Password = _password
                    };

                    try
                    {
                        CreateUsers(new User[] { user }, string.Format("Restore user '{0}' [Password: {1}, Level: {2}]", _username, _password, currentUserLevel));
                    }
                    catch (Exception ex)
                    {
                        StepFailed(ex);

                        Assert(
                            Operator.GetOkCancelAnswer("Failed to restore current user programmatically. Please, restore this account and click OK"),
                            "Operator did not manage to restore user",
                            "Restore user manually");

                    }
                }

            });

        }
        #endregion

        #region 4-1-7 SECURITY COMMAND SETUSER
        [Test(Name = "SECURITY COMMAND SETUSER",
            Order = "04.01.07",
            Id = "4-1-7",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetUser })]
        public void SetUserTest()
        {
            string userName1 = "OnvifTest1";
            string userName2 = "OnvifTest2";

            User existingUser1 = null;
            User existingUser2 = null;

            bool bModifyExisting = false;
            bool succeeded = false;
            bool userCreated = false;

            string password = _password;

            User currentAccountInUse = null;

            RunTest(
                () =>
                {
                    string password1 = _useEmbeddedPassword ? "OnvifTest123" : _password1;
                    string password2 = _useEmbeddedPassword ? "OnvifTest321" : _password2;

                    User newUser1 = new User { Username = userName1, UserLevel = UserLevel.Operator, Password = password1 };
                    User newUser2 = new User { Username = userName2, UserLevel = UserLevel.Operator, Password = password1 };

                    try
                    {
                        CreateUsers(new[] { newUser1, newUser2 });
                    }
                    catch (FaultException exc)
                    {
                        if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                        {
                            LogStepEvent("Maximum number of supported users exceeded.");
                            LogStepEvent("Perform the test with existing user accounts.");
                            bModifyExisting = true;
                            StepPassed();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    userCreated = true;

                    User[] users = GetUsers();
                    Assert(users != null && users.Length > 0,
                        "The DUD did not return any user", "Check if the DUT returned users list");

                    if (bModifyExisting)
                    {
                        string currentUser = _username.ToLower();
                        List<User> existingUsers = users.Where(u => u.Username.ToLower() != currentUser).ToList();
                        User current = users.Where(u => u.Username.ToLower() == currentUser).FirstOrDefault();

                        if (existingUsers.Count >= 2)
                        {
                            // we can avoid using current account 
                            existingUser1 = existingUsers[0];
                            existingUser2 = existingUsers[1];
                        }
                        else
                        {
                            if (existingUsers.Count == 1)
                            {
                                // use account which is not current and current, if found.
                                // if current not found - it is out of test scope.
                                existingUser1 = existingUsers[0];
                                existingUser2 = current;
                                currentAccountInUse = current;
                            }
                            else
                            {
                                // we should use current account.
                                // Since we check that users list is not empty - "current" is not null.
                                existingUser1 = current;
                                currentAccountInUse = current;
                            }
                        }
                    }

                    User setUser1 = new User();
                    if (bModifyExisting)
                    {
                        setUser1.Username = existingUser1.Username;
                        setUser1.UserLevel = existingUser1.UserLevel;
                    }
                    else
                    {
                        setUser1.Username = newUser1.Username;
                        setUser1.UserLevel = newUser1.UserLevel;
                    }

                    setUser1.Password = password2;

                    SetUser(new User[] { setUser1 });

                    if (currentAccountInUse != null)
                    {
                        if (currentAccountInUse.Username == setUser1.Username)
                        {
                            // current pasword has been modified
                            if (_credentialsProvider != null)
                            {
                                _credentialsProvider.Password = setUser1.Password;
                            }
                            UpdateSecurity();
                        }
                    }

                    users = GetUsers();
                    Assert(users != null && users.Length > 0,
                        "The DUD did not return any user", "Check if the DUT returned users list");

                    // --> this part can be left in the test
                    User user = users.Where(u => u.Username == setUser1.Username).FirstOrDefault();

                    Assert((user != null),
                        string.Format("Modified user {0} not found", setUser1.Username),
                        "Check if the DUT returned modified users");

                    if (bModifyExisting && existingUser2 == null)
                    {
                        LogTestEvent(string.Format("Cannot test query for two users {0}", System.Environment.NewLine));
                    }
                    else
                    {
                        setUser1.Password = password1;

                        User setUser2 = new User();

                        if (bModifyExisting)
                        {
                            setUser2.Username = existingUser2.Username;
                            setUser2.UserLevel = existingUser2.UserLevel;
                        }
                        else
                        {
                            setUser2.Username = newUser2.Username;
                            setUser2.UserLevel = newUser2.UserLevel;
                        }
                        setUser2.Password = password2;

                        SetUser(new User[] { setUser1, setUser2 });

                        if (currentAccountInUse != null)
                        {
                            if (currentAccountInUse.Username == setUser1.Username)
                            {
                                _password = setUser1.Password;
                            }

                            if (setUser2 != null && currentAccountInUse.Username == setUser2.Username)
                            {
                                _password = setUser2.Password;
                            }


                            UpdateSecurity();
                        }

                        users = GetUsers();
                        Assert(users != null && users.Length > 0,
                            "The DUD did not return any user", "Check if the DUT returned users list");

                        BeginStep("Check if the users have been modified correctly");

                        foreach (User usr in new User[] { setUser1, setUser2 })
                        {
                            user = users.Where(u => u.Username == usr.Username).FirstOrDefault();

                            if (user == null)
                            {
                                throw new AssertException(string.Format("Modified user {0} not found", usr.Username));
                            }
                        }

                        StepPassed();

                        if (!bModifyExisting)
                        {
                            DeleteUsers(new string[] { userName1, userName2 });
                        }

                        succeeded = true;
                    }
                },
                () =>
                {
                    if (bModifyExisting)
                    {
                        List<User> accountsToRestore = new List<User>();

                        if (currentAccountInUse != null)
                        {
                            LogTestEvent(string.Format("Reset password for current user ({0}).", _username));

                            // the following situations are possible:
                            // existingUser1 is current, existingUser2 is null
                            // existingUser1 is not current, existingUser2 is current

                            if (existingUser1.Username.ToLower() == currentAccountInUse.Username.ToLower())
                            {
                                existingUser1.Password = password;
                                accountsToRestore.Add(existingUser1);
                            }
                            else
                            {
                                existingUser1.Password = "Password1";
                                accountsToRestore.Add(existingUser1);
                                LogTestEvent(string.Format("Set password to Password1 for {0} used for test.", existingUser1.Username));

                                existingUser2.Password = password;
                                accountsToRestore.Add(existingUser2);
                            }
                        }
                        else
                        {
                            existingUser1.Password = "Password1";
                            accountsToRestore.Add(existingUser1);
                            LogTestEvent(string.Format("Set password to Password1 for {0} used for test.", existingUser1.Username));

                            if (existingUser2 != null)
                            {
                                LogTestEvent(string.Format("Set password to Password1 for {0} used for test.", existingUser2.Username));
                                existingUser2.Password = "Password1";
                                accountsToRestore.Add(existingUser2);
                            }
                        }
                        LogTestEvent(string.Empty);

                        SetUser(accountsToRestore.ToArray());
                        _password = password;
                    }
                    else
                    {
                        if (!succeeded && userCreated)
                        {
                            CleanUpUsers(new string[] { userName1, userName2 });
                        }
                    }

                });

        }
        #endregion

        #region 4-1-8 SECURITY COMMAND USER MANAGEMENT ERROR CASE
        [Test(Name = "SECURITY COMMAND USER MANAGEMENT ERROR CASE",
            Order = "04.01.08",
            Id = "4-1-8",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.SetUser })]
        public void SetUserErrorCaseTest()
        {
            User existingUser = null;
            string userName1 = "OnvifTest1";
            string userName2 = "OnvifTest5";

            bool succeeded = false;
            bool userCreated = false;
            bool userUnchanged = true;

            RunTest(() =>
            {
                string password1 = _useEmbeddedPassword ? "OnvifTest123" : _password1;

                bool useExisting = false;

                User newUser1 = new User { Username = userName1, UserLevel = UserLevel.Operator, Password = password1 };

                try
                {
                    CreateUsers(new[] { newUser1 });
                }
                catch (FaultException exc)
                {
                    if (exc.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                    {
                        LogStepEvent("Maximum number of supported users exceeded.");
                        LogStepEvent("Perform the test with existing user accounts.");
                        useExisting = true;
                        StepPassed();
                    }
                    else
                    {
                        throw;
                    }
                }

                User[] users = GetUsers();
                Assert(users != null && users.Length > 0,
                    "The DUD did not return any user", "Check if the DUT returned users list");

                if (useExisting)
                {
                    existingUser = users[0];
                    // try not to use current account
                    if (StringComparer.CurrentCultureIgnoreCase.Compare(existingUser.Username, _username) == 0 &&
                        users.Length > 1)
                    {
                        existingUser = users[1];
                    }
                }

                User setUser1 = new User();
                User unchanged = null;
                if (useExisting)
                {
                    unchanged = existingUser;

                    setUser1.Username = existingUser.Username;
                    setUser1.UserLevel = existingUser.UserLevel != UserLevel.User ? UserLevel.User : UserLevel.Operator;
                }
                else
                {
                    userCreated = true;
                    unchanged = newUser1;

                    setUser1.Username = newUser1.Username;
                    setUser1.UserLevel = UserLevel.User;
                }

                setUser1.Password = password1;

                User setUser2 = new User { Username = userName2, UserLevel = UserLevel.User, Password = password1 };

                RunStep(
                    () => { Client.SetUser(new User[] { setUser1, setUser2 }); },
                    "Set Users - negative test",
                    "Sender/InvalidArgVal/UsernameMissing",
                    false);

                DoRequestDelay();

                users = GetUsers();
                Assert(users != null && users.Length > 0,
                    "The DUD did not return any user", "Check if the DUT returned users list");

                BeginStep("Check if the user has not been modified");

                User user = users.Where(u => u.Username == unchanged.Username).FirstOrDefault();

                if (user == null)
                {
                    throw new AssertException(string.Format("User {0} not found", unchanged.Username));
                }

                if (user.UserLevel != unchanged.UserLevel)
                {
                    userUnchanged = false;
                    throw new AssertException(string.Format(
                            "UserLevel for {0} has been modfied. Expected: {1}, actual: {2}",
                            unchanged.Username, unchanged.UserLevel, user.UserLevel));
                }

                StepPassed();

                if (!useExisting)
                {
                    DeleteUsers(new string[] { userName1 });
                }

                succeeded = true;

                users = GetUsers();
                Assert(users != null && users.Length > 0,
                    "The DUD did not return any user", "Check if the DUT returned users list");

            },
            () =>
            {
                if (existingUser == null)
                {
                    if (!succeeded && userCreated)
                    {
                        CleanUpUsers(new string[] { userName1 });
                    }
                }
                else
                {
                    if (!userUnchanged)
                    {
                        string password;

                        if (StringComparer.CurrentCultureIgnoreCase.Compare(existingUser.Username, _username) == 0)
                        {
                            password = _password;
                        }
                        else
                        {
                            password = _useEmbeddedPassword ? _password1 : "Password1";
                        }

                        LogTestEvent(
                            string.Format(
                            "Reset UserLevel for {0} user. Set password to {1} {2}",
                            existingUser.Username, password,
                            System.Environment.NewLine));

                        existingUser.Password = password;
                        SetUser(new User[] { existingUser });
                    }
                }
            });
        }
        #endregion

        #region 4-1-9 SECURITY COMMAND CREATEUSERS
        [Test(Name = "SECURITY COMMAND CREATEUSERS",
            Order = "04.01.09",
            Id = "4-1-9",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequiredFeatures = new Feature[] { },
            RequirementLevel = RequirementLevel.Optional,
            FunctionalityUnderTest = new Functionality[] { Functionality.CreateUsers, Functionality.MaxUsers })]
        public void CreateUsersTestCheckMax()
        {
            string userName1 = "OnvifTest1";
            string userName2 = "OnvifTest2";

            var createdUsers = new List<User>();

            RunTest(() =>
            {
                var anyUserCreated = false;
                var serviceCapabilitiesRetrieved = false;
                DeviceServiceCapabilities serviceCapabilities = null;

                IEnumerable<User> users = GetUsers();

                // Create operator 
                string password1 = _useEmbeddedPassword ? "OnvifTest123" : _password1;

                Action newUserCreatedCheck = 
                    () =>
                    {
                        users = GetUsers();

                        foreach (var newUser in createdUsers)
                        {
                            var justCreated = users.FirstOrDefault(u => u.Username == newUser.Username);
                            var noUserMsg = string.Format("The DUT returned no user with Username = '{0}'", newUser.Username);
                            var userLevelDifferentMsg = null == justCreated ?  "" : string.Format("The DUT returned user with Username = '{0}', but with unexpected UserLevel = '{1}'. Expected: '{2}'", 
                                                                                                  newUser.Username, justCreated.UserLevel, newUser.UserLevel);
                            Assert(null != justCreated && justCreated.UserLevel == newUser.UserLevel,
                                   justCreated == null ? noUserMsg : userLevelDifferentMsg,
                                   "Check new user is created");

                            anyUserCreated = true;
                        }
                    };

                //Return: finish test as PASSED if true
                Func<FaultException, User, bool> faultCheck = 
                    (e, user) =>
                    {
                        if (e.IsValidOnvifFault("Receiver/Action/TooManyUsers"))
                        {

                            if (Features.ContainsFeature(Feature.GetServices))
                            {
                                if (!serviceCapabilitiesRetrieved)
                                {
                                    serviceCapabilitiesRetrieved = true;
                                    serviceCapabilities = GetServiceCapabilities();
                                }

                                if (null != serviceCapabilities.Security)
                                {
                                    if (!serviceCapabilities.Security.MaxUsersSpecified)
                                        return true;

                                    Assert(serviceCapabilities.Security.MaxUsers <= users.Count(),
                                           string.Format("The limit of existing users is {0}. But DUT has only {1} users.", serviceCapabilities.Security.MaxUsers, users.Count()),
                                           "Check limit of existing user is reached");

                                    return true;
                                }
                            }
                            else
                                return true;
                        }
                        else
                            LogStepEvent(string.Format("Warning: failed to create user with UserLevel = {0}{1}", user.UserLevel, Environment.NewLine));

                        return false;
                    };

                Action clearUsers = 
                    () =>
                    {
                        var usernames = createdUsers.Select(u => u.Username).ToArray();
                        createdUsers.Clear();
                        DeleteUsers(usernames);
                        users = users.Where(u => !usernames.Contains(u.Username));
                    };

                User newUser1 = new User { Username = userName1, UserLevel = UserLevel.Operator, Password = password1 };
                var successfullCreateUsers = false;
                try
                {
                    CreateUsers(new[] { newUser1 });
                    createdUsers.Add(newUser1);
                    anyUserCreated = true;
                    successfullCreateUsers = true;
                }
                catch (FaultException exc)
                {
                    StepPassed();

                    if (faultCheck(exc, newUser1))
                        return;
                }

                if (successfullCreateUsers)
                    newUserCreatedCheck();

                // create user
                User newUser2 = new User { Username = userName2, UserLevel = UserLevel.User, Password = password1 };
                successfullCreateUsers = false;
                try
                {
                    CreateUsers(new[] { newUser2 });
                    createdUsers.Add(newUser2);
                    successfullCreateUsers = true;
                }
                catch (FaultException exc)
                {
                    StepPassed();

                    if (faultCheck(exc, newUser2))
                        return;
                }

                if (successfullCreateUsers)
                    newUserCreatedCheck();

                if (createdUsers.Any())
                    clearUsers();

                // Create admin
                User newUser3 = new User { Username = userName1, UserLevel = UserLevel.Administrator, Password = password1 };
                successfullCreateUsers = false;
                try
                {
                    CreateUsers(new[] { newUser3 });
                    createdUsers.Add(newUser3);
                    successfullCreateUsers = true;
                    anyUserCreated = true;
                }
                catch (FaultException exc)
                {
                    StepPassed();

                    if (faultCheck(exc, newUser3))
                        return;
                }

                if (successfullCreateUsers)
                    newUserCreatedCheck();

                if (createdUsers.Any())
                    clearUsers();

                Assert(anyUserCreated, "Faild to create user with any of UserLevel", "Check if a user with any parameters has been created");
            },
            () =>
            {
                if (createdUsers.Any())
                    DeleteUsers(createdUsers.Select(u => u.Username).ToArray());
            });
        }

        [Test(Name = "GET REMOTE USER",
            Order = "04.01.10",
            Id = "4-1-10",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.RemoteUserHandling },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.GetRemoteUser })]
        public void GetRemoteUserTest()
        {

            RunTest(() =>
                    {
                        var remoteUser = GetRemoteUser();

                        Assert(null == remoteUser || string.IsNullOrEmpty(remoteUser.Password),
                               "The DUT returned non-empty remote user with non-empty password",
                               "Validating received response to GetRemoteUser command");
                    });
        }
        
        [Test(Name = "SET REMOTE USER",
            Order = "04.01.11",
            Id = "4-1-11",
            Category = Category.DEVICE,
            Path = PATH,
            Version = 1.02,
            RequiredFeatures = new Feature[] { Feature.RemoteUserHandling },
            RequirementLevel = RequirementLevel.Optional,
            LastChangedIn = "v14.12",
            FunctionalityUnderTest = new Functionality[] { Functionality.SetRemoteUser })]
        public void SetRemoteUserTest()
        {

            RunTest(() =>
                    {
                        var remoteUserName     = "ONVIFRemoteUser";
                        var remoteUserPassword = "ONVIFRemotePassword";
                        var originalRemoteUser = new RemoteUser(){ Username = remoteUserName, Password = remoteUserPassword, UseDerivedPassword = true };

                        var assertRemoteUser = new Action(() =>
                                                          {
                                                              var remoteUser = GetRemoteUser();

                                                              bool flag = false;
                                                              var msg = string.Empty;
                                                              if (null == remoteUser)
                                                                  msg = "The DUT returned empty remote user";
                                                              else if (remoteUser.Username != originalRemoteUser.Username)
                                                                  msg = string.Format("Received remote user has unexpected Username '{0}'. Expected: '{1}'", remoteUser.Username, originalRemoteUser.Username);
                                                              else if (null != remoteUser.Password)
                                                                  msg = "Received remote user has non-empty 'Password' field";
                                                              else if (remoteUser.UseDerivedPassword != originalRemoteUser.UseDerivedPassword)
                                                                  msg = string.Format("Received remote user has unexpected UseDerivedPassword flag value '{0}'. Expected: '{1}'", remoteUser.UseDerivedPassword, originalRemoteUser.UseDerivedPassword);
                                                              else
                                                                  flag = true;

                                                              Assert(flag, msg, "Validating received response to GetRemoteUser command");
                                                          });

                        SetRemoteUser(originalRemoteUser);
                        assertRemoteUser();

                        originalRemoteUser = new RemoteUser(){ Username = remoteUserName, Password = remoteUserPassword, UseDerivedPassword = false };
                        SetRemoteUser(originalRemoteUser);
                        assertRemoteUser();

                        SetRemoteUser(null);

                        Assert(null == GetRemoteUser(),
                               "The DUT returned non-empty remote",
                               "Validating received response to GetRemoteUser command");
                    });
        }        
        #endregion

        #region Function CleanUpUsers()
        void CleanUpUsers(IEnumerable<string> usernames)
        {
            LogTestEvent("Starting rollback procedure" + System.Environment.NewLine);

            List<string> usersToBeDeleted = new List<string>();

            User[] users = GetUsers();

            Assert(users != null, "The DUD did not return any user",
                   "Check if the DUT returned users list");

            foreach (string username in usernames)
            {
                User user = users.Where(u => u.Username == username).FirstOrDefault();
                if (user != null)
                {
                    usersToBeDeleted.Add(username);
                }
            }
            if (usersToBeDeleted.Count > 0)
            {
                DeleteUsers(usersToBeDeleted.ToArray());
            }
            else
            {
                LogTestEvent("No user accounts to delete");
            }
        }
        #endregion
    }
}
