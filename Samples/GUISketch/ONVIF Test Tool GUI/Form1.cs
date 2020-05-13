using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ONVIF_Test_Tool_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }



        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage13_Click(object sender, EventArgs e)
        {

        }

        private void textBox38_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                listBox1.Enabled = false;
                button50.Enabled = false;
                button9.Enabled = false;
                textBox36.Enabled = false;
                label30.Enabled = false;
                label40.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                listBox1.Enabled = true;
                button50.Enabled = true;
                button9.Enabled = true;
                textBox36.Enabled = true;
                label30.Enabled = true;
                label40.Enabled = true;
            }
        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.SelectedImageIndex <= 0)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
        }

        private void textBox43_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            toolStripButton7.Checked = !toolStripButton7.Checked;
            if (toolStripButton7.Checked)
            {
                toolStripButton7.Image = ONVIF_Test_Tool_GUI.Properties.Resources.OK;
            }
            else
            {
                toolStripButton7.Image = ONVIF_Test_Tool_GUI.Properties.Resources.CANCEL;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void textBox66_TextChanged(object sender, EventArgs e)
        {

        }

        private void button46_Click(object sender, EventArgs e)
        {
            MessageBox.Show("VIDEO PLAYER");
        }

        private void button45_Click(object sender, EventArgs e)
        {
            MessageBox.Show("VIDEO PLAYER");
        }

        private void howDoIToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test");
        }

        private void groupBox4_Enter_1(object sender, EventArgs e)
        {

        }
    }
}
