///////////////////////////////////////////////////////////////////////////
//!  @author        Anna Tarasova
////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestTool.GUI.Controllers;
using TestTool.GUI.Data;

namespace TestTool.GUI
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitText();
        }

        void InitText()
        {
            ApplicationInfo info = ContextController.GetApplicationInfo();
            lblToolInfo.Text = info.ToolVersionFull;
            
            linkOnvif.Links.Add(0, linkOnvif.Text.Length).LinkData = linkOnvif.Text;
            
            this.tbAgreement.Text = @"ONVIF LICENSE AGREEMENT" + Environment.NewLine +
                                    "This is a legal agreement between you (either individual or an entity) and ONVIF. By installing, copying or otherwise using the SOFTWARE, you are agreeing to be bound by the terms of this agreement." + Environment.NewLine + Environment.NewLine +
                                    "ONVIF SOFTWARE LICENSE" + Environment.NewLine +
                                    "1. GRANT OF LICENSE. ONVIF grants to you as ONVIF Member the right to use the SOFTWARE. The SOFTWARE is in \"use\" on a computer when it is loaded into temporary memory (i.e. RAM) or installed into permanent memory (e.g. hard disk, CD-ROM or other storage device) of that computer." + Environment.NewLine + Environment.NewLine +
                                    "2. COPYRIGHT. The SOFTWARE is owned by ONVIF and/or its licensor(s), if any, and is protected by copyright laws and international treaty provisions. Therefore you must treat the SOFTWARE like any other copyrighted material (e.g. a book or a musical recording) except that you may either (a) make a copy of the SOFTWARE solely for backup or archival purposes or (b) transfer the SOFTWARE to a single hard disk provided you keep the original solely for backup purposes." + Environment.NewLine + Environment.NewLine +
                                    "3. OTHER RESTRICTIONS. You may not rent, lease or sublicense the SOFTWARE. You may not reverse engineer, decompile, or disassemble the SOFTWARE." + Environment.NewLine + Environment.NewLine +
                                    "4. THIRD PARTY Software. The SOFTWARE may contain third party software, which requires notices and/or additional terms and conditions. Such required third party software notices and/or additional terms and conditions are located in the readme file or other product documentation. By accepting this license agreement, you are also accepting the additional terms and conditions, if any, set forth therein." + Environment.NewLine + Environment.NewLine +
                                    "5. TERMINATION. This License is effective until terminated. Your rights under this License will terminate automatically without notice from ONVIF if you fail to comply with any term(s) of this License. Upon the termination of this License, you shall cease all use of the SOFTWARE and destroy all copies, full or partial, of the SOFTWARE." + Environment.NewLine + Environment.NewLine +
                                    "6. GOVERNING LAW. This agreement shall be deemed performed in and shall be construed by the laws of Switzerland." + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                                    "DISCLAIMER " + Environment.NewLine + Environment.NewLine +
                                    "THE SOFTWARE IS DELIVERED AS IS WITHOUT WARRANTY OF ANY KIND. THE ENTIRE RISK AS TO THE RESULTS AND PERFORMANCE OF THE SOFTWARE IS ASSUMED BY THE PURCHASER/THE USER/YOU. ONVIF DISCLAIMS ALL WARRANTIES, WHETHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT, OR ANY WARRANTY ARISING OUT OF ANY PROPOSAL, SPECIFICATION OR SAMPLE WITH RESPECT TO THE SOFTWARE." + Environment.NewLine +
                                    "ONVIF AND/OR ITS LICENSOR(S) SHALL NOT BE LIABLE FOR LOSS OF DATA, LOSS OF PRODUCTION, LOSS OF PROFIT, LOSS OF USE, LOSS OF CONTRACTS OR FOR ANY OTHER CONSEQUENTIAL, ECONOMIC OR INDIRECT LOSS WHATSOEVER IN RESPECT OF SALE, PURCHASE, DELIVERY, USE OR DISPOSITION OF THE SOFTWARE." + Environment.NewLine +
                                    "ONVIF TOTAL LIABILITY FOR ALL CLAIMS IN ACCORDANCE WITH THE SALE, PURCHASE, DELIVERY AND USE OF THE SOFTWARE SHALL NOT EXCEED THE PRICE PAID FOR THE SOFTWARE.";
        
        }

        private void linkLabelBy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            if (null != target && target.StartsWith("www"))
            {
                System.Diagnostics.Process.Start(target);
            }

        }




    }
}
