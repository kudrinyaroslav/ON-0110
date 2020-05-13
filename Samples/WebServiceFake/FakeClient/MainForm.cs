using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Web.Services3.Design;
using System.IdentityModel.Tokens;
namespace FakeClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCallService_Click(object sender, EventArgs e)
        {
            try
            {
                CameraService.CameraService service = new CameraService.CameraService();

                UsernameClientAssertion assert = new UsernameClientAssertion("user", "password");
                Policy policy = new Policy();
                policy.Assertions.Add(assert);
                service.SetPolicy(policy);

                bool result = service.GetClientCertificateMode();
                MessageBox.Show(result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWCFProxy_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceClient service = new DeviceClient();

                //UserNameSecurityToken token = new UserNameSecurityToken("Admin", "Pa5sw0rd");
                
                SecurityBehavior sb = new SecurityBehavior();
                sb.UserName = "Admin";
                sb.Password = "Pa5sw0rd";
                service.Endpoint.Behaviors.Add(sb);
                bool result = service.GetClientCertificateMode();
                MessageBox.Show(result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
