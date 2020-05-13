using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMC.Controls
{
    public partial class DoorStateFlagChangeComboBox : ListBox
    {
        protected TreeNode treeViewNode { get; set; }


        public DoorStateFlagChangeComboBox(TreeNode treeViewNode_, IEnumerable<string> values)
        {
            InitializeComponent();

            treeViewNode = treeViewNode_;

            this.Parent = treeViewNode.TreeView;

            this.Leave += OnLostFocus;
            this.SelectedIndexChanged += OnLostFocus;

            this.Items.AddRange(values.ToArray());

            Size = this.GetPreferredSize(Size);
            locate();

            Focus();
        }

        private void OnLostFocus(object sender, EventArgs args)
        {
            this.Visible = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void locate()
        {
            this.Left = treeViewNode.Bounds.Left;
            this.Top = treeViewNode.Bounds.Top + treeViewNode.Bounds.Height;
        }


    }
}
