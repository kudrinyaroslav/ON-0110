namespace ONVIF_Test_Tool_GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "169.254.136.46",
            "0de55be0-668f-44e9-88c3-4c965f572ab2"}, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "169.254.136.47",
            "8e555be0-668f-44e9-88c3-4c965f572ab7"}, -1);
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("4.1.1 NVT IPV4 STATIC IP", 1, 1);
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("4.1.2 NVT IPV4 LINK LOCAL ADDRESS", 3, 3);
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("4.1.4 NVT IPV6 STATIC IP", 6, 6);
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("IP Configuration", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode19,
            treeNode20,
            treeNode21});
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("5.6 NVT SEARCH USING UNICAST PROBE MESSAGE", 5, 5);
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Device Discovery", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode23});
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("6.2.11 NVT NETWORK COMMAND NTP CONFIGURATION GETNTP", 2, 2);
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("6.2.15 NVT NETWORK COMMAND NTP CONFIGURATION SETNTP NTPMANUAL INVALID IPV4", 4, 4);
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Capabilities", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode25,
            treeNode26});
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("6.2.13 NVT NETWORK COMMAND NTP CONFIGURATION SETNTP NTPMANUAL IPV6", 7, 7);
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Network", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode28});
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("System", 0, 0);
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Security", 0, 0);
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Device Management", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode29,
            treeNode30,
            treeNode31});
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Media Profile");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Video Configuration");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Audio Configuration");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("PTZ Configuration");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Metadata Configuration");
            System.Windows.Forms.TreeNode treeNode84 = new System.Windows.Forms.TreeNode("Media Streaming");
            System.Windows.Forms.TreeNode treeNode85 = new System.Windows.Forms.TreeNode("Error Handling");
            System.Windows.Forms.TreeNode treeNode86 = new System.Windows.Forms.TreeNode("Media Configuration", 0, 0, new System.Windows.Forms.TreeNode[] {
            treeNode33,
            treeNode34,
            treeNode35,
            treeNode36,
            treeNode37,
            treeNode84,
            treeNode85});
            System.Windows.Forms.TreeNode treeNode87 = new System.Windows.Forms.TreeNode("Video Streaming");
            System.Windows.Forms.TreeNode treeNode88 = new System.Windows.Forms.TreeNode("Audio & Video Streamong");
            System.Windows.Forms.TreeNode treeNode89 = new System.Windows.Forms.TreeNode("Real Time Streaming", new System.Windows.Forms.TreeNode[] {
            treeNode87,
            treeNode88});
            System.Windows.Forms.TreeNode treeNode90 = new System.Windows.Forms.TreeNode("Event Handling");
            System.Windows.Forms.TreeNode treeNode91 = new System.Windows.Forms.TreeNode("Preset Operations");
            System.Windows.Forms.TreeNode treeNode92 = new System.Windows.Forms.TreeNode("Home Position Operations");
            System.Windows.Forms.TreeNode treeNode102 = new System.Windows.Forms.TreeNode("Auxiliary Operations");
            System.Windows.Forms.TreeNode treeNode103 = new System.Windows.Forms.TreeNode("Absolute Position Spaces");
            System.Windows.Forms.TreeNode treeNode104 = new System.Windows.Forms.TreeNode("Relative Translation Spaces");
            System.Windows.Forms.TreeNode treeNode105 = new System.Windows.Forms.TreeNode("Continuous Velocity Spaces");
            System.Windows.Forms.TreeNode treeNode106 = new System.Windows.Forms.TreeNode("Speed Spaces");
            System.Windows.Forms.TreeNode treeNode107 = new System.Windows.Forms.TreeNode("Predefined PTZ spaces", new System.Windows.Forms.TreeNode[] {
            treeNode103,
            treeNode104,
            treeNode105,
            treeNode106});
            System.Windows.Forms.TreeNode treeNode108 = new System.Windows.Forms.TreeNode("PTZ Control", new System.Windows.Forms.TreeNode[] {
            treeNode91,
            treeNode92,
            treeNode102,
            treeNode107});
            System.Windows.Forms.TreeNode treeNode109 = new System.Windows.Forms.TreeNode("11.1 NVT USER TOKEN PROFILE", 2, 2);
            System.Windows.Forms.TreeNode treeNode110 = new System.Windows.Forms.TreeNode("Security", new System.Windows.Forms.TreeNode[] {
            treeNode109});
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "PASS",
            "All OK"}, -1);
            System.Windows.Forms.ListViewItem listViewItem20 = new System.Windows.Forms.ListViewItem(new string[] {
            "2",
            "FAIL",
            "Error description"}, -1);
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("Request1");
            System.Windows.Forms.TreeNode treeNode57 = new System.Windows.Forms.TreeNode("Request2");
            System.Windows.Forms.TreeNode treeNode58 = new System.Windows.Forms.TreeNode("GetWsdlUrl", new System.Windows.Forms.TreeNode[] {
            treeNode56,
            treeNode57});
            System.Windows.Forms.TreeNode treeNode59 = new System.Windows.Forms.TreeNode("Request1");
            System.Windows.Forms.TreeNode treeNode60 = new System.Windows.Forms.TreeNode("GetCapabilities", new System.Windows.Forms.TreeNode[] {
            treeNode59});
            System.Windows.Forms.TreeNode treeNode61 = new System.Windows.Forms.TreeNode("Device Managment", new System.Windows.Forms.TreeNode[] {
            treeNode58,
            treeNode60});
            System.Windows.Forms.TreeNode treeNode62 = new System.Windows.Forms.TreeNode("Request1");
            System.Windows.Forms.TreeNode treeNode63 = new System.Windows.Forms.TreeNode("GetNodes", new System.Windows.Forms.TreeNode[] {
            treeNode62});
            System.Windows.Forms.TreeNode treeNode64 = new System.Windows.Forms.TreeNode("PTZ", new System.Windows.Forms.TreeNode[] {
            treeNode63});
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label77 = new System.Windows.Forms.Label();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.textBox24 = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.textBox22 = new System.Windows.Forms.TextBox();
            this.button51 = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.comboBox10 = new System.Windows.Forms.ComboBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button50 = new System.Windows.Forms.Button();
            this.textBox36 = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.textBox31 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.textBox28 = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.textBox29 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.textBox26 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.textBox27 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.textBox30 = new System.Windows.Forms.TextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.textBox32 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.textBox33 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.textBox35 = new System.Windows.Forms.TextBox();
            this.textBox34 = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.textBox46 = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.textBox42 = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.textBox43 = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.textBox44 = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.textBox45 = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.textBox41 = new System.Windows.Forms.TextBox();
            this.textBox38 = new System.Windows.Forms.TextBox();
            this.textBox37 = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.label68 = new System.Windows.Forms.Label();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.label69 = new System.Windows.Forms.Label();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.label70 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label66 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label65 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label64 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.button46 = new System.Windows.Forms.Button();
            this.button29 = new System.Windows.Forms.Button();
            this.button28 = new System.Windows.Forms.Button();
            this.textBox39 = new System.Windows.Forms.TextBox();
            this.textBox40 = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.button45 = new System.Windows.Forms.Button();
            this.textBox59 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button43 = new System.Windows.Forms.Button();
            this.button44 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.button37 = new System.Windows.Forms.Button();
            this.label61 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.button41 = new System.Windows.Forms.Button();
            this.button42 = new System.Windows.Forms.Button();
            this.label63 = new System.Windows.Forms.Label();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.button38 = new System.Windows.Forms.Button();
            this.button39 = new System.Windows.Forms.Button();
            this.button40 = new System.Windows.Forms.Button();
            this.label60 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.button34 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.button36 = new System.Windows.Forms.Button();
            this.label59 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.button33 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.label58 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.button16 = new System.Windows.Forms.Button();
            this.textBox58 = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.textBox57 = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.label74 = new System.Windows.Forms.Label();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.textBox64 = new System.Windows.Forms.TextBox();
            this.label75 = new System.Windows.Forms.Label();
            this.textBox65 = new System.Windows.Forms.TextBox();
            this.label76 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.button49 = new System.Windows.Forms.Button();
            this.label71 = new System.Windows.Forms.Label();
            this.textBox62 = new System.Windows.Forms.TextBox();
            this.label72 = new System.Windows.Forms.Label();
            this.button48 = new System.Windows.Forms.Button();
            this.textBox63 = new System.Windows.Forms.TextBox();
            this.label73 = new System.Windows.Forms.Label();
            this.button47 = new System.Windows.Forms.Button();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label35 = new System.Windows.Forms.Label();
            this.textBox60 = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.textBox61 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.tabPage15 = new System.Windows.Forms.TabPage();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.textBox47 = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.textBox48 = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.textBox49 = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.textBox50 = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.textBox51 = new System.Windows.Forms.TextBox();
            this.label53 = new System.Windows.Forms.Label();
            this.textBox52 = new System.Windows.Forms.TextBox();
            this.textBox53 = new System.Windows.Forms.TextBox();
            this.textBox54 = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.tabPage16 = new System.Windows.Forms.TabPage();
            this.textBox55 = new System.Windows.Forms.TextBox();
            this.textBox56 = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.tabPage17 = new System.Windows.Forms.TabPage();
            this.label56 = new System.Windows.Forms.Label();
            this.tabPage18 = new System.Windows.Forms.TabPage();
            this.label57 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tabPage11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.tabPage15.SuspendLayout();
            this.tabPage16.SuspendLayout();
            this.tabPage17.SuspendLayout();
            this.tabPage18.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(832, 487);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(824, 461);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.textBox9);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.textBox12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.textBox13);
            this.groupBox3.Location = new System.Drawing.Point(8, 312);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(808, 146);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Test Execution Information";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(727, 110);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 17;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox9
            // 
            this.textBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox9.Location = new System.Drawing.Point(166, 71);
            this.textBox9.Multiline = true;
            this.textBox9.Name = "textBox9";
            this.textBox9.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox9.Size = new System.Drawing.Size(555, 62);
            this.textBox9.TabIndex = 14;
            this.textBox9.Text = "Corvallis, Oregon";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(160, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Executing Organization Address:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(150, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Executing Organization Name:";
            // 
            // textBox12
            // 
            this.textBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox12.Location = new System.Drawing.Point(166, 45);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(555, 20);
            this.textBox12.TabIndex = 8;
            this.textBox12.Text = "Net Video Consulting";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Test Operator Name:";
            // 
            // textBox13
            // 
            this.textBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox13.Location = new System.Drawing.Point(166, 19);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(555, 20);
            this.textBox13.TabIndex = 6;
            this.textBox13.Text = "Tom Galvin";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Location = new System.Drawing.Point(8, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(808, 191);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Device Under Test Information";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(727, 162);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(727, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 98);
            this.button1.TabIndex = 16;
            this.button1.Text = "Get From Device";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Other Information:";
            // 
            // textBox8
            // 
            this.textBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox8.Location = new System.Drawing.Point(166, 123);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox8.Size = new System.Drawing.Size(555, 62);
            this.textBox8.TabIndex = 14;
            this.textBox8.Text = "Test Run";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Firmware Version:";
            // 
            // textBox7
            // 
            this.textBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox7.Location = new System.Drawing.Point(166, 97);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(555, 20);
            this.textBox7.TabIndex = 12;
            this.textBox7.Text = "24";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Serial Number:";
            // 
            // textBox6
            // 
            this.textBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox6.Location = new System.Drawing.Point(166, 71);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(555, 20);
            this.textBox6.TabIndex = 10;
            this.textBox6.Text = "unit0004630a96ec";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Model:";
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Location = new System.Drawing.Point(166, 45);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(555, 20);
            this.textBox5.TabIndex = 8;
            this.textBox5.Text = "Dinion";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Brand:";
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Location = new System.Drawing.Point(166, 19);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(555, 20);
            this.textBox4.TabIndex = 6;
            this.textBox4.Text = "Bosch";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(808, 103);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application Information";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(166, 72);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(555, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "ONVIF Test Tool version 1.0.1.16";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(166, 46);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(555, 20);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "ONVIF Test Specification version 1.0, December 2008";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "ONVIF Core Specification:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(166, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(555, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "ONVIF Core Specification version 1.0, November 2008";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ONVIF Test Specification:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ONVIF Test Tool Version:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Controls.Add(this.label39);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.listView1);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(824, 461);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Discovery";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Local Area Connection",
            "Local Area Connection 2",
            "Local Area Connection 3"});
            this.comboBox1.Location = new System.Drawing.Point(44, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(294, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "Local Area Connection";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(10, 10);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(28, 13);
            this.label39.TabIndex = 4;
            this.label39.Text = "NIC:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label77);
            this.groupBox4.Controls.Add(this.textBox25);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.Controls.Add(this.textBox24);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.textBox23);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.textBox22);
            this.groupBox4.Controls.Add(this.button51);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.textBox18);
            this.groupBox4.Controls.Add(this.comboBox10);
            this.groupBox4.Controls.Add(this.button8);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.textBox17);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.textBox19);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.textBox20);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.textBox21);
            this.groupBox4.Location = new System.Drawing.Point(344, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(474, 447);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Device Under Test Information";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter_1);
            // 
            // label77
            // 
            this.label77.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(6, 395);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(90, 13);
            this.label77.TabIndex = 35;
            this.label77.Text = "Firmware Version:";
            // 
            // textBox25
            // 
            this.textBox25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox25.Location = new System.Drawing.Point(143, 392);
            this.textBox25.Name = "textBox25";
            this.textBox25.ReadOnly = true;
            this.textBox25.Size = new System.Drawing.Size(325, 20);
            this.textBox25.TabIndex = 34;
            this.textBox25.Text = "<Press Check to get information>";
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(6, 369);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(80, 13);
            this.label25.TabIndex = 33;
            this.label25.Text = "Sereal Number:";
            // 
            // textBox24
            // 
            this.textBox24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox24.Location = new System.Drawing.Point(143, 366);
            this.textBox24.Name = "textBox24";
            this.textBox24.ReadOnly = true;
            this.textBox24.Size = new System.Drawing.Size(325, 20);
            this.textBox24.TabIndex = 32;
            this.textBox24.Text = "<Press Check to get information>";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 343);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(39, 13);
            this.label24.TabIndex = 31;
            this.label24.Text = "Model:";
            // 
            // textBox23
            // 
            this.textBox23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox23.Location = new System.Drawing.Point(143, 340);
            this.textBox23.Name = "textBox23";
            this.textBox23.ReadOnly = true;
            this.textBox23.Size = new System.Drawing.Size(325, 20);
            this.textBox23.TabIndex = 30;
            this.textBox23.Text = "<Press Check to get information>";
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 317);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(38, 13);
            this.label23.TabIndex = 29;
            this.label23.Text = "Brand:";
            // 
            // textBox22
            // 
            this.textBox22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox22.Location = new System.Drawing.Point(143, 314);
            this.textBox22.Name = "textBox22";
            this.textBox22.ReadOnly = true;
            this.textBox22.Size = new System.Drawing.Size(325, 20);
            this.textBox22.TabIndex = 28;
            this.textBox22.Text = "<Press Check to get information>";
            // 
            // button51
            // 
            this.button51.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button51.Location = new System.Drawing.Point(410, 95);
            this.button51.Name = "button51";
            this.button51.Size = new System.Drawing.Size(58, 23);
            this.button51.TabIndex = 27;
            this.button51.Text = "Check";
            this.button51.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 291);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(93, 13);
            this.label22.TabIndex = 26;
            this.label22.Text = "Metadata Version:";
            // 
            // textBox18
            // 
            this.textBox18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox18.Location = new System.Drawing.Point(143, 288);
            this.textBox18.Name = "textBox18";
            this.textBox18.ReadOnly = true;
            this.textBox18.Size = new System.Drawing.Size(325, 20);
            this.textBox18.TabIndex = 25;
            this.textBox18.Text = "1";
            // 
            // comboBox10
            // 
            this.comboBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox10.FormattingEnabled = true;
            this.comboBox10.Location = new System.Drawing.Point(143, 96);
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new System.Drawing.Size(261, 21);
            this.comboBox10.TabIndex = 24;
            this.comboBox10.Text = "http://169.254.136.46/onvif/device_service";
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(408, 418);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(60, 23);
            this.button8.TabIndex = 4;
            this.button8.Text = "Clear";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(410, 68);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(58, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "Check";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 126);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(46, 13);
            this.label17.TabIndex = 15;
            this.label17.Text = "Scopes:";
            // 
            // textBox17
            // 
            this.textBox17.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox17.Location = new System.Drawing.Point(143, 123);
            this.textBox17.Multiline = true;
            this.textBox17.Name = "textBox17";
            this.textBox17.ReadOnly = true;
            this.textBox17.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox17.Size = new System.Drawing.Size(325, 159);
            this.textBox17.TabIndex = 14;
            this.textBox17.Text = resources.GetString("textBox17.Text");
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 100);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(124, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Device Service Address:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 74);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(57, 13);
            this.label19.TabIndex = 11;
            this.label19.Text = "Device IP:";
            // 
            // textBox19
            // 
            this.textBox19.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox19.Location = new System.Drawing.Point(143, 71);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(261, 20);
            this.textBox19.TabIndex = 10;
            this.textBox19.Text = "169.254.136.46";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 13);
            this.label20.TabIndex = 9;
            this.label20.Text = "Type:";
            // 
            // textBox20
            // 
            this.textBox20.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox20.Location = new System.Drawing.Point(143, 45);
            this.textBox20.Name = "textBox20";
            this.textBox20.ReadOnly = true;
            this.textBox20.Size = new System.Drawing.Size(325, 20);
            this.textBox20.TabIndex = 8;
            this.textBox20.Text = "dn:NetworkVideoTransmitter";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 22);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 13);
            this.label21.TabIndex = 7;
            this.label21.Text = "EP Adress:";
            // 
            // textBox21
            // 
            this.textBox21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox21.Location = new System.Drawing.Point(143, 19);
            this.textBox21.Name = "textBox21";
            this.textBox21.ReadOnly = true;
            this.textBox21.Size = new System.Drawing.Size(325, 20);
            this.textBox21.TabIndex = 6;
            this.textBox21.Text = "uuid:8e555be0-668f-44e9-88c3-4c965f572ab2";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem17,
            listViewItem18});
            this.listView1.Location = new System.Drawing.Point(6, 34);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(332, 390);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "IP";
            this.columnHeader1.Width = 107;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "UUID";
            this.columnHeader2.Width = 220;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(6, 430);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(332, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Discover Devices";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox9);
            this.tabPage5.Controls.Add(this.groupBox7);
            this.tabPage5.Controls.Add(this.groupBox6);
            this.tabPage5.Controls.Add(this.groupBox5);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(824, 461);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Management";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox9.Controls.Add(this.checkBox5);
            this.groupBox9.Controls.Add(this.checkBox4);
            this.groupBox9.Controls.Add(this.checkBox3);
            this.groupBox9.Controls.Add(this.checkBox2);
            this.groupBox9.Location = new System.Drawing.Point(9, 198);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(351, 255);
            this.groupBox9.TabIndex = 3;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Product Features";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(169, 44);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(55, 17);
            this.checkBox5.TabIndex = 3;
            this.checkBox5.Text = "H.264";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(169, 20);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(66, 17);
            this.checkBox4.TabIndex = 2;
            this.checkBox4.Text = "MPEG-4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(7, 44);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(53, 17);
            this.checkBox3.TabIndex = 1;
            this.checkBox3.Text = "Audio";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(7, 20);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(48, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "IPv6";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.button50);
            this.groupBox7.Controls.Add(this.textBox36);
            this.groupBox7.Controls.Add(this.label40);
            this.groupBox7.Controls.Add(this.button9);
            this.groupBox7.Controls.Add(this.label30);
            this.groupBox7.Controls.Add(this.listBox1);
            this.groupBox7.Controls.Add(this.radioButton2);
            this.groupBox7.Controls.Add(this.radioButton1);
            this.groupBox7.Location = new System.Drawing.Point(366, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(455, 450);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Profiles";
            // 
            // button50
            // 
            this.button50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button50.Enabled = false;
            this.button50.Location = new System.Drawing.Point(338, 418);
            this.button50.Name = "button50";
            this.button50.Size = new System.Drawing.Size(110, 23);
            this.button50.TabIndex = 7;
            this.button50.Text = "Apply Profile";
            this.button50.UseVisualStyleBackColor = true;
            // 
            // textBox36
            // 
            this.textBox36.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox36.Enabled = false;
            this.textBox36.Location = new System.Drawing.Point(83, 421);
            this.textBox36.Name = "textBox36";
            this.textBox36.Size = new System.Drawing.Size(123, 20);
            this.textBox36.TabIndex = 6;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label40.AutoSize = true;
            this.label40.Enabled = false;
            this.label40.Location = new System.Drawing.Point(7, 424);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(70, 13);
            this.label40.TabIndex = 5;
            this.label40.Text = "Profile Name:";
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.Enabled = false;
            this.button9.Location = new System.Drawing.Point(212, 418);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(120, 23);
            this.button9.TabIndex = 4;
            this.button9.Text = "Save Current Options";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Enabled = false;
            this.label30.Location = new System.Drawing.Point(6, 59);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(58, 13);
            this.label30.TabIndex = 3;
            this.label30.Text = "Profile List:";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Enabled = false;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Embeded Profile",
            "Must Be Profile",
            "Custom Profile 1",
            "Custom Profile 2"});
            this.listBox1.Location = new System.Drawing.Point(6, 75);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(443, 329);
            this.listBox1.TabIndex = 2;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 39);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(114, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Use Custom Profile";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 16);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(242, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Use Embeded Profile (required for sertification)";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label31);
            this.groupBox6.Controls.Add(this.textBox31);
            this.groupBox6.Controls.Add(this.label28);
            this.groupBox6.Controls.Add(this.textBox28);
            this.groupBox6.Controls.Add(this.label29);
            this.groupBox6.Controls.Add(this.textBox29);
            this.groupBox6.Location = new System.Drawing.Point(8, 90);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(352, 101);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Timeouts";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 73);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(129, 13);
            this.label31.TabIndex = 19;
            this.label31.Text = "Time Between Tests (ms):";
            // 
            // textBox31
            // 
            this.textBox31.Location = new System.Drawing.Point(140, 70);
            this.textBox31.Name = "textBox31";
            this.textBox31.Size = new System.Drawing.Size(206, 20);
            this.textBox31.TabIndex = 18;
            this.textBox31.Text = "1000";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 48);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(108, 13);
            this.label28.TabIndex = 17;
            this.label28.Text = "Reboot Timeout (ms):";
            // 
            // textBox28
            // 
            this.textBox28.Location = new System.Drawing.Point(140, 44);
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new System.Drawing.Size(206, 20);
            this.textBox28.TabIndex = 16;
            this.textBox28.Text = "30000";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 22);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(116, 13);
            this.label29.TabIndex = 15;
            this.label29.Text = "Message Timeout (ms):";
            // 
            // textBox29
            // 
            this.textBox29.Location = new System.Drawing.Point(140, 19);
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new System.Drawing.Size(206, 20);
            this.textBox29.TabIndex = 14;
            this.textBox29.Text = "5000";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label26);
            this.groupBox5.Controls.Add(this.textBox26);
            this.groupBox5.Controls.Add(this.label27);
            this.groupBox5.Controls.Add(this.textBox27);
            this.groupBox5.Location = new System.Drawing.Point(8, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(352, 81);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Camera Admin Account";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 46);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(56, 13);
            this.label26.TabIndex = 13;
            this.label26.Text = "Password:";
            // 
            // textBox26
            // 
            this.textBox26.Location = new System.Drawing.Point(79, 42);
            this.textBox26.Name = "textBox26";
            this.textBox26.Size = new System.Drawing.Size(267, 20);
            this.textBox26.TabIndex = 12;
            this.textBox26.Text = "password";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 20);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(63, 13);
            this.label27.TabIndex = 11;
            this.label27.Text = "User Name:";
            // 
            // textBox27
            // 
            this.textBox27.Location = new System.Drawing.Point(79, 17);
            this.textBox27.Name = "textBox27";
            this.textBox27.Size = new System.Drawing.Size(267, 20);
            this.textBox27.TabIndex = 10;
            this.textBox27.Text = "Admin";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer1);
            this.tabPage3.Controls.Add(this.toolStrip1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(824, 461);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Test";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox8);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(824, 436);
            this.splitContainer1.SplitterDistance = 322;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.treeView1);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(322, 436);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Test Cases";
            this.groupBox8.Enter += new System.EventHandler(this.groupBox8_Enter);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.FullRowSelect = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Indent = 19;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            treeNode19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            treeNode19.ImageIndex = 1;
            treeNode19.Name = "Node1";
            treeNode19.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode19.SelectedImageIndex = 1;
            treeNode19.Text = "4.1.1 NVT IPV4 STATIC IP";
            treeNode19.ToolTipText = "4.1.1 NVT IPV4 STATIC IP\r\nRequirement Level: MUST\r\nState: FAILED";
            treeNode20.ForeColor = System.Drawing.Color.Gray;
            treeNode20.ImageIndex = 3;
            treeNode20.Name = "Node0";
            treeNode20.SelectedImageIndex = 3;
            treeNode20.Text = "4.1.2 NVT IPV4 LINK LOCAL ADDRESS";
            treeNode20.ToolTipText = "4.1.2 NVT IPV4 LINK LOCAL ADDRESS\r\nRequirement Level: SHOULD\r\nState: SKIPPED";
            treeNode21.ImageIndex = 6;
            treeNode21.Name = "Node0";
            treeNode21.SelectedImageIndex = 6;
            treeNode21.Text = "4.1.4 NVT IPV6 STATIC IP";
            treeNode21.ToolTipText = "4.1.4 NVT IPV6 STATIC IP\r\nRequirement Level: MUST IF IMPLEMENTED (IPv6)\r\nState: N" +
                "OT PERFORMED";
            treeNode22.ImageIndex = 0;
            treeNode22.Name = "Node0";
            treeNode22.SelectedImageIndex = 0;
            treeNode22.Text = "IP Configuration";
            treeNode23.ImageIndex = 5;
            treeNode23.Name = "Node0";
            treeNode23.SelectedImageIndex = 5;
            treeNode23.Text = "5.6 NVT SEARCH USING UNICAST PROBE MESSAGE";
            treeNode23.ToolTipText = "5.6 NVT SEARCH USING UNICAST PROBE MESSAGE\r\nRequirement Level: OPTIONAL\r\nState: N" +
                "OT PERFORMED";
            treeNode24.ImageIndex = 0;
            treeNode24.Name = "Node3";
            treeNode24.SelectedImageIndex = 0;
            treeNode24.Text = "Device Discovery";
            treeNode25.ForeColor = System.Drawing.Color.Green;
            treeNode25.ImageIndex = 2;
            treeNode25.Name = "Node2";
            treeNode25.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode25.SelectedImageIndex = 2;
            treeNode25.Text = "6.2.11 NVT NETWORK COMMAND NTP CONFIGURATION GETNTP";
            treeNode25.ToolTipText = "6.2.11 NVT NETWORK COMMAND NTP CONFIGURATION GETNTP\r\nRequirement Level: MUST IF S" +
                "UPPORTED (NTP)\r\nState: PASSED";
            treeNode26.ImageIndex = 4;
            treeNode26.Name = "Node0";
            treeNode26.SelectedImageIndex = 4;
            treeNode26.Text = "6.2.15 NVT NETWORK COMMAND NTP CONFIGURATION SETNTP NTPMANUAL INVALID IPV4";
            treeNode26.ToolTipText = "6.2.15 NVT NETWORK COMMAND NTP CONFIGURATION SETNTP NTPMANUAL INVALID IPV4\r\nRequi" +
                "rement Level: SHOULD IF SUPPORTED (NTP)\r\nState: NOT PERFORMED";
            treeNode27.ImageIndex = 0;
            treeNode27.Name = "Node1";
            treeNode27.SelectedImageIndex = 0;
            treeNode27.Text = "Capabilities";
            treeNode28.ImageIndex = 7;
            treeNode28.Name = "Node0";
            treeNode28.SelectedImageIndex = 7;
            treeNode28.Text = "6.2.13 NVT NETWORK COMMAND NTP CONFIGURATION SETNTP NTPMANUAL IPV6";
            treeNode28.ToolTipText = "6.2.13 NVT NETWORK COMMAND NTP CONFIGURATION SETNTP NTPMANUAL IPV6\r\nRequirement L" +
                "evel: MUST IF SUPPORTED (NTP) & IMPLEMENTED (IPv6)\r\nState: NOT PERFORMED";
            treeNode29.ImageIndex = 0;
            treeNode29.Name = "Node2";
            treeNode29.SelectedImageIndex = 0;
            treeNode29.Text = "Network";
            treeNode30.ImageIndex = 0;
            treeNode30.Name = "Node3";
            treeNode30.SelectedImageIndex = 0;
            treeNode30.Text = "System";
            treeNode31.ImageIndex = 0;
            treeNode31.Name = "Node4";
            treeNode31.SelectedImageIndex = 0;
            treeNode31.Text = "Security";
            treeNode32.ImageIndex = 0;
            treeNode32.Name = "Node0";
            treeNode32.SelectedImageIndex = 0;
            treeNode32.Text = "Device Management";
            treeNode33.Name = "Node17";
            treeNode33.Text = "Media Profile";
            treeNode34.Name = "Node18";
            treeNode34.Text = "Video Configuration";
            treeNode35.Name = "Node19";
            treeNode35.Text = "Audio Configuration";
            treeNode36.Name = "Node20";
            treeNode36.Text = "PTZ Configuration";
            treeNode37.Name = "Node21";
            treeNode37.Text = "Metadata Configuration";
            treeNode84.Name = "Node22";
            treeNode84.Text = "Media Streaming";
            treeNode85.Name = "Node23";
            treeNode85.Text = "Error Handling";
            treeNode86.ImageIndex = 0;
            treeNode86.Name = "Node5";
            treeNode86.SelectedImageIndex = 0;
            treeNode86.Text = "Media Configuration";
            treeNode87.Name = "Node15";
            treeNode87.Text = "Video Streaming";
            treeNode88.Name = "Node16";
            treeNode88.Text = "Audio & Video Streamong";
            treeNode89.Name = "Node6";
            treeNode89.Text = "Real Time Streaming";
            treeNode90.Name = "Node7";
            treeNode90.Text = "Event Handling";
            treeNode91.Name = "Node14";
            treeNode91.Text = "Preset Operations";
            treeNode92.Name = "Node13";
            treeNode92.Text = "Home Position Operations";
            treeNode102.Name = "Node12";
            treeNode102.Text = "Auxiliary Operations";
            treeNode103.Name = "Node0";
            treeNode103.Text = "Absolute Position Spaces";
            treeNode104.Name = "Node1";
            treeNode104.Text = "Relative Translation Spaces";
            treeNode105.Name = "Node3";
            treeNode105.Text = "Continuous Velocity Spaces";
            treeNode106.Name = "Node4";
            treeNode106.Text = "Speed Spaces";
            treeNode107.Name = "Node11";
            treeNode107.Text = "Predefined PTZ spaces";
            treeNode108.Name = "Node8";
            treeNode108.Text = "PTZ Control";
            treeNode109.ImageIndex = 2;
            treeNode109.Name = "Node10";
            treeNode109.SelectedImageIndex = 2;
            treeNode109.Text = "11.1 NVT USER TOKEN PROFILE";
            treeNode110.Name = "Node9";
            treeNode110.Text = "Security";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode22,
            treeNode24,
            treeNode32,
            treeNode86,
            treeNode89,
            treeNode90,
            treeNode108,
            treeNode110});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(316, 416);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "None.ico");
            this.imageList1.Images.SetKeyName(1, "MUST.ico");
            this.imageList1.Images.SetKeyName(2, "MUSTIFSUP.ico");
            this.imageList1.Images.SetKeyName(3, "SHOULD.ico");
            this.imageList1.Images.SetKeyName(4, "SHOULDIFSUP.ico");
            this.imageList1.Images.SetKeyName(5, "OPTIONAL.ico");
            this.imageList1.Images.SetKeyName(6, "MUSTIFIML.ico");
            this.imageList1.Images.SetKeyName(7, "MUSTIFSUPIMPL.ico");
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(498, 436);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.textBox30);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(490, 410);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "Test Result";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // textBox30
            // 
            this.textBox30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox30.Location = new System.Drawing.Point(3, 3);
            this.textBox30.Multiline = true;
            this.textBox30.Name = "textBox30";
            this.textBox30.ReadOnly = true;
            this.textBox30.Size = new System.Drawing.Size(484, 404);
            this.textBox30.TabIndex = 0;
            this.textBox30.Text = "TEST TITLE\r\nTEST DESCRIPTION\r\n\r\nSTEP1 - STEP TITLE\r\nSTEP RESULT\r\n\r\nSTEP2 - STEP T" +
                "ITLE\r\nSTEP RESULT\r\n\r\nTEST RESULT\r\n";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.splitContainer2);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(490, 410);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "Step Details";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(484, 404);
            this.splitContainer2.SplitterDistance = 133;
            this.splitContainer2.TabIndex = 0;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem19,
            listViewItem20});
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(484, 133);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Step";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Result";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Details";
            this.columnHeader5.Width = 374;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.textBox32);
            this.splitContainer3.Panel1.Controls.Add(this.label32);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.textBox33);
            this.splitContainer3.Panel2.Controls.Add(this.label33);
            this.splitContainer3.Size = new System.Drawing.Size(484, 267);
            this.splitContainer3.SplitterDistance = 249;
            this.splitContainer3.TabIndex = 0;
            // 
            // textBox32
            // 
            this.textBox32.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox32.Location = new System.Drawing.Point(3, 16);
            this.textBox32.Multiline = true;
            this.textBox32.Name = "textBox32";
            this.textBox32.ReadOnly = true;
            this.textBox32.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox32.Size = new System.Drawing.Size(243, 248);
            this.textBox32.TabIndex = 1;
            this.textBox32.Text = resources.GetString("textBox32.Text");
            this.textBox32.WordWrap = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(3, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(50, 13);
            this.label32.TabIndex = 0;
            this.label32.Text = "Request:";
            // 
            // textBox33
            // 
            this.textBox33.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox33.Location = new System.Drawing.Point(3, 17);
            this.textBox33.Multiline = true;
            this.textBox33.Name = "textBox33";
            this.textBox33.ReadOnly = true;
            this.textBox33.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox33.Size = new System.Drawing.Size(225, 247);
            this.textBox33.TabIndex = 3;
            this.textBox33.Text = resources.GetString("textBox33.Text");
            this.textBox33.WordWrap = false;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(3, 1);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(58, 13);
            this.label33.TabIndex = 2;
            this.label33.Text = "Response:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton6,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripButton5,
            this.toolStripSeparator3,
            this.toolStripButton7});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(824, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.RunAll;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton1.Text = "Run";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.RunSelected;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(86, 22);
            this.toolStripButton2.Text = "Run Current";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.Pause;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(56, 22);
            this.toolStripButton6.Text = "Pause";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.Stop;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton3.Text = "Stop";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.Halt;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton4.Text = "Halt";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.Clear;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton5.Text = "Clear";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.Checked = true;
            this.toolStripButton7.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton7.Image = global::ONVIF_Test_Tool_GUI.Properties.Resources.OK;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(142, 22);
            this.toolStripButton7.Text = "Interactive tests at first";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button15);
            this.tabPage4.Controls.Add(this.button14);
            this.tabPage4.Controls.Add(this.textBox35);
            this.tabPage4.Controls.Add(this.textBox34);
            this.tabPage4.Controls.Add(this.label34);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(824, 461);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Report";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button15.Location = new System.Drawing.Point(741, 430);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 4;
            this.button15.Text = "Save";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button14.Location = new System.Drawing.Point(708, 431);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(27, 23);
            this.button14.TabIndex = 3;
            this.button14.Text = "...";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // textBox35
            // 
            this.textBox35.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox35.Location = new System.Drawing.Point(3, 432);
            this.textBox35.Name = "textBox35";
            this.textBox35.Size = new System.Drawing.Size(699, 20);
            this.textBox35.TabIndex = 2;
            // 
            // textBox34
            // 
            this.textBox34.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox34.Location = new System.Drawing.Point(3, 20);
            this.textBox34.Multiline = true;
            this.textBox34.Name = "textBox34";
            this.textBox34.ReadOnly = true;
            this.textBox34.Size = new System.Drawing.Size(813, 404);
            this.textBox34.TabIndex = 1;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(8, 4);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(77, 13);
            this.label34.TabIndex = 0;
            this.label34.Text = "Test Summary:";
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.tabControl3);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(824, 461);
            this.tabPage9.TabIndex = 6;
            this.tabPage9.Text = "Device";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage10);
            this.tabControl3.Controls.Add(this.tabPage13);
            this.tabControl3.Controls.Add(this.tabPage11);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(824, 461);
            this.tabControl3.TabIndex = 0;
            this.tabControl3.SelectedIndexChanged += new System.EventHandler(this.tabControl3_SelectedIndexChanged);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.button17);
            this.tabPage10.Controls.Add(this.button18);
            this.tabPage10.Controls.Add(this.button19);
            this.tabPage10.Controls.Add(this.button13);
            this.tabPage10.Controls.Add(this.button12);
            this.tabPage10.Controls.Add(this.button11);
            this.tabPage10.Controls.Add(this.button10);
            this.tabPage10.Controls.Add(this.label47);
            this.tabPage10.Controls.Add(this.textBox46);
            this.tabPage10.Controls.Add(this.label43);
            this.tabPage10.Controls.Add(this.textBox42);
            this.tabPage10.Controls.Add(this.label44);
            this.tabPage10.Controls.Add(this.textBox43);
            this.tabPage10.Controls.Add(this.label45);
            this.tabPage10.Controls.Add(this.textBox44);
            this.tabPage10.Controls.Add(this.label46);
            this.tabPage10.Controls.Add(this.textBox45);
            this.tabPage10.Controls.Add(this.label42);
            this.tabPage10.Controls.Add(this.textBox41);
            this.tabPage10.Controls.Add(this.textBox38);
            this.tabPage10.Controls.Add(this.textBox37);
            this.tabPage10.Controls.Add(this.label41);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(816, 435);
            this.tabPage10.TabIndex = 0;
            this.tabPage10.Text = "Device Management";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(198, 218);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(90, 23);
            this.button17.TabIndex = 40;
            this.button17.Text = "Set IP Address";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(102, 218);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(90, 23);
            this.button18.TabIndex = 39;
            this.button18.Text = "Get Hostname";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(6, 218);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(90, 23);
            this.button19.TabIndex = 38;
            this.button19.Text = "Get Interfaces";
            this.button19.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(294, 189);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(90, 23);
            this.button13.TabIndex = 37;
            this.button13.Text = "Reboot";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(198, 189);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(90, 23);
            this.button12.TabIndex = 36;
            this.button12.Text = "Hard Reset";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(102, 189);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(90, 23);
            this.button11.TabIndex = 35;
            this.button11.Text = "Probe";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(6, 189);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(90, 23);
            this.button10.TabIndex = 34;
            this.button10.Text = "Device Info";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(6, 137);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(56, 13);
            this.label47.TabIndex = 33;
            this.label47.Text = "Hardware:";
            // 
            // textBox46
            // 
            this.textBox46.Location = new System.Drawing.Point(142, 137);
            this.textBox46.Name = "textBox46";
            this.textBox46.ReadOnly = true;
            this.textBox46.Size = new System.Drawing.Size(269, 20);
            this.textBox46.TabIndex = 32;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(6, 166);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(52, 13);
            this.label43.TabIndex = 31;
            this.label43.Text = "Firmware:";
            // 
            // textBox42
            // 
            this.textBox42.Location = new System.Drawing.Point(142, 163);
            this.textBox42.Name = "textBox42";
            this.textBox42.ReadOnly = true;
            this.textBox42.Size = new System.Drawing.Size(269, 20);
            this.textBox42.TabIndex = 30;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(6, 111);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(76, 13);
            this.label44.TabIndex = 29;
            this.label44.Text = "Serial Number:";
            // 
            // textBox43
            // 
            this.textBox43.Location = new System.Drawing.Point(142, 111);
            this.textBox43.Name = "textBox43";
            this.textBox43.ReadOnly = true;
            this.textBox43.Size = new System.Drawing.Size(269, 20);
            this.textBox43.TabIndex = 28;
            this.textBox43.TextChanged += new System.EventHandler(this.textBox43_TextChanged);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(6, 88);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(39, 13);
            this.label45.TabIndex = 27;
            this.label45.Text = "Model:";
            // 
            // textBox44
            // 
            this.textBox44.Location = new System.Drawing.Point(142, 85);
            this.textBox44.Name = "textBox44";
            this.textBox44.ReadOnly = true;
            this.textBox44.Size = new System.Drawing.Size(269, 20);
            this.textBox44.TabIndex = 26;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(6, 62);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(73, 13);
            this.label46.TabIndex = 25;
            this.label46.Text = "Manufacturer:";
            // 
            // textBox45
            // 
            this.textBox45.Location = new System.Drawing.Point(142, 59);
            this.textBox45.Name = "textBox45";
            this.textBox45.ReadOnly = true;
            this.textBox45.Size = new System.Drawing.Size(269, 20);
            this.textBox45.TabIndex = 24;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(6, 36);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(20, 13);
            this.label42.TabIndex = 5;
            this.label42.Text = "IP:";
            // 
            // textBox41
            // 
            this.textBox41.Location = new System.Drawing.Point(142, 33);
            this.textBox41.Name = "textBox41";
            this.textBox41.ReadOnly = true;
            this.textBox41.Size = new System.Drawing.Size(269, 20);
            this.textBox41.TabIndex = 4;
            // 
            // textBox38
            // 
            this.textBox38.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox38.Location = new System.Drawing.Point(417, 7);
            this.textBox38.Multiline = true;
            this.textBox38.Name = "textBox38";
            this.textBox38.ReadOnly = true;
            this.textBox38.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox38.Size = new System.Drawing.Size(396, 422);
            this.textBox38.TabIndex = 3;
            this.textBox38.TextChanged += new System.EventHandler(this.textBox38_TextChanged);
            // 
            // textBox37
            // 
            this.textBox37.Location = new System.Drawing.Point(142, 7);
            this.textBox37.Name = "textBox37";
            this.textBox37.ReadOnly = true;
            this.textBox37.Size = new System.Drawing.Size(269, 20);
            this.textBox37.TabIndex = 2;
            this.textBox37.Text = "http://169.254.136.46/onvif/device_service";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(6, 10);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(134, 13);
            this.label41.TabIndex = 1;
            this.label41.Text = "Device Management URL:";
            // 
            // tabPage13
            // 
            this.tabPage13.Controls.Add(this.groupBox11);
            this.tabPage13.Controls.Add(this.groupBox10);
            this.tabPage13.Controls.Add(this.button46);
            this.tabPage13.Controls.Add(this.button29);
            this.tabPage13.Controls.Add(this.button28);
            this.tabPage13.Controls.Add(this.textBox39);
            this.tabPage13.Controls.Add(this.textBox40);
            this.tabPage13.Controls.Add(this.label36);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Size = new System.Drawing.Size(816, 435);
            this.tabPage13.TabIndex = 3;
            this.tabPage13.Text = "Media";
            this.tabPage13.UseVisualStyleBackColor = true;
            this.tabPage13.Click += new System.EventHandler(this.tabPage13_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.comboBox7);
            this.groupBox11.Controls.Add(this.label68);
            this.groupBox11.Controls.Add(this.comboBox8);
            this.groupBox11.Controls.Add(this.label69);
            this.groupBox11.Controls.Add(this.comboBox9);
            this.groupBox11.Controls.Add(this.label70);
            this.groupBox11.Location = new System.Drawing.Point(5, 205);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(368, 105);
            this.groupBox11.TabIndex = 51;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Audio";
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(86, 73);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(121, 21);
            this.comboBox7.TabIndex = 53;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(6, 76);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(41, 13);
            this.label68.TabIndex = 52;
            this.label68.Text = "Codec:";
            // 
            // comboBox8
            // 
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Location = new System.Drawing.Point(86, 46);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(121, 21);
            this.comboBox8.TabIndex = 51;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(6, 49);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(50, 13);
            this.label69.TabIndex = 50;
            this.label69.Text = "Encoder:";
            // 
            // comboBox9
            // 
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Location = new System.Drawing.Point(86, 19);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(121, 21);
            this.comboBox9.TabIndex = 49;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(6, 22);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(44, 13);
            this.label70.TabIndex = 48;
            this.label70.Text = "Source:";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.comboBox5);
            this.groupBox10.Controls.Add(this.label66);
            this.groupBox10.Controls.Add(this.comboBox4);
            this.groupBox10.Controls.Add(this.label65);
            this.groupBox10.Controls.Add(this.comboBox3);
            this.groupBox10.Controls.Add(this.label64);
            this.groupBox10.Controls.Add(this.comboBox2);
            this.groupBox10.Controls.Add(this.label38);
            this.groupBox10.Location = new System.Drawing.Point(5, 63);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(368, 136);
            this.groupBox10.TabIndex = 50;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Video";
            this.groupBox10.Enter += new System.EventHandler(this.groupBox10_Enter);
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(86, 100);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 21);
            this.comboBox5.TabIndex = 55;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(6, 103);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(60, 13);
            this.label66.TabIndex = 54;
            this.label66.Text = "Resolution:";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(86, 73);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 53;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(6, 76);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(41, 13);
            this.label65.TabIndex = 52;
            this.label65.Text = "Codec:";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(86, 46);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 51;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(6, 49);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(50, 13);
            this.label64.TabIndex = 50;
            this.label64.Text = "Encoder:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(86, 19);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 49;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 22);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(44, 13);
            this.label38.TabIndex = 48;
            this.label38.Text = "Source:";
            // 
            // button46
            // 
            this.button46.Location = new System.Drawing.Point(4, 316);
            this.button46.Name = "button46";
            this.button46.Size = new System.Drawing.Size(161, 23);
            this.button46.TabIndex = 47;
            this.button46.Text = "Get Video and Audio Sream";
            this.button46.UseVisualStyleBackColor = true;
            this.button46.Click += new System.EventHandler(this.button46_Click);
            // 
            // button29
            // 
            this.button29.Location = new System.Drawing.Point(4, 33);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(90, 23);
            this.button29.TabIndex = 8;
            this.button29.Text = "Get Media URL";
            this.button29.UseVisualStyleBackColor = true;
            // 
            // button28
            // 
            this.button28.Location = new System.Drawing.Point(100, 33);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(90, 23);
            this.button28.TabIndex = 7;
            this.button28.Text = "Get Profiles";
            this.button28.UseVisualStyleBackColor = true;
            // 
            // textBox39
            // 
            this.textBox39.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox39.Location = new System.Drawing.Point(382, 0);
            this.textBox39.Multiline = true;
            this.textBox39.Name = "textBox39";
            this.textBox39.ReadOnly = true;
            this.textBox39.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox39.Size = new System.Drawing.Size(430, 409);
            this.textBox39.TabIndex = 6;
            // 
            // textBox40
            // 
            this.textBox40.Location = new System.Drawing.Point(138, 7);
            this.textBox40.Name = "textBox40";
            this.textBox40.ReadOnly = true;
            this.textBox40.Size = new System.Drawing.Size(235, 20);
            this.textBox40.TabIndex = 5;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(4, 10);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(64, 13);
            this.label36.TabIndex = 4;
            this.label36.Text = "Media URL:";
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.button45);
            this.tabPage11.Controls.Add(this.textBox59);
            this.tabPage11.Controls.Add(this.checkBox1);
            this.tabPage11.Controls.Add(this.button43);
            this.tabPage11.Controls.Add(this.button44);
            this.tabPage11.Controls.Add(this.button30);
            this.tabPage11.Controls.Add(this.button37);
            this.tabPage11.Controls.Add(this.label61);
            this.tabPage11.Controls.Add(this.numericUpDown4);
            this.tabPage11.Controls.Add(this.label62);
            this.tabPage11.Controls.Add(this.numericUpDown5);
            this.tabPage11.Controls.Add(this.button41);
            this.tabPage11.Controls.Add(this.button42);
            this.tabPage11.Controls.Add(this.label63);
            this.tabPage11.Controls.Add(this.numericUpDown6);
            this.tabPage11.Controls.Add(this.radioButton4);
            this.tabPage11.Controls.Add(this.button38);
            this.tabPage11.Controls.Add(this.button39);
            this.tabPage11.Controls.Add(this.button40);
            this.tabPage11.Controls.Add(this.label60);
            this.tabPage11.Controls.Add(this.numericUpDown3);
            this.tabPage11.Controls.Add(this.button34);
            this.tabPage11.Controls.Add(this.button35);
            this.tabPage11.Controls.Add(this.button36);
            this.tabPage11.Controls.Add(this.label59);
            this.tabPage11.Controls.Add(this.numericUpDown2);
            this.tabPage11.Controls.Add(this.button33);
            this.tabPage11.Controls.Add(this.button32);
            this.tabPage11.Controls.Add(this.button31);
            this.tabPage11.Controls.Add(this.label58);
            this.tabPage11.Controls.Add(this.numericUpDown1);
            this.tabPage11.Controls.Add(this.radioButton3);
            this.tabPage11.Controls.Add(this.button16);
            this.tabPage11.Controls.Add(this.textBox58);
            this.tabPage11.Controls.Add(this.label37);
            this.tabPage11.Controls.Add(this.textBox57);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(816, 435);
            this.tabPage11.TabIndex = 1;
            this.tabPage11.Text = "PTZ";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // button45
            // 
            this.button45.Location = new System.Drawing.Point(6, 337);
            this.button45.Name = "button45";
            this.button45.Size = new System.Drawing.Size(75, 23);
            this.button45.TabIndex = 46;
            this.button45.Text = "Video";
            this.button45.UseVisualStyleBackColor = true;
            this.button45.Click += new System.EventHandler(this.button45_Click);
            // 
            // textBox59
            // 
            this.textBox59.Location = new System.Drawing.Point(137, 311);
            this.textBox59.Name = "textBox59";
            this.textBox59.Size = new System.Drawing.Size(49, 20);
            this.textBox59.TabIndex = 45;
            this.textBox59.Text = "10";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(23, 314);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 44;
            this.checkBox1.Text = "Use Timeout (ms)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button43
            // 
            this.button43.Location = new System.Drawing.Point(214, 279);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(68, 23);
            this.button43.TabIndex = 43;
            this.button43.Text = "Stop All";
            this.button43.UseVisualStyleBackColor = true;
            // 
            // button44
            // 
            this.button44.Location = new System.Drawing.Point(140, 279);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(68, 23);
            this.button44.TabIndex = 42;
            this.button44.Text = "Start All";
            this.button44.UseVisualStyleBackColor = true;
            // 
            // button30
            // 
            this.button30.Location = new System.Drawing.Point(214, 250);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(68, 23);
            this.button30.TabIndex = 41;
            this.button30.Text = "Stop Zoom";
            this.button30.UseVisualStyleBackColor = true;
            // 
            // button37
            // 
            this.button37.Location = new System.Drawing.Point(140, 250);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(68, 23);
            this.button37.TabIndex = 40;
            this.button37.Text = "Start Zoom";
            this.button37.UseVisualStyleBackColor = true;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(25, 255);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(44, 13);
            this.label61.TabIndex = 39;
            this.label61.Text = "VZoom:";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.DecimalPlaces = 2;
            this.numericUpDown4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown4.Location = new System.Drawing.Point(70, 253);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown4.TabIndex = 38;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(25, 226);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(24, 13);
            this.label62.TabIndex = 37;
            this.label62.Text = "VY:";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.DecimalPlaces = 2;
            this.numericUpDown5.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown5.Location = new System.Drawing.Point(70, 224);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown5.TabIndex = 36;
            // 
            // button41
            // 
            this.button41.Location = new System.Drawing.Point(214, 193);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(68, 23);
            this.button41.TabIndex = 35;
            this.button41.Text = "Stop Move";
            this.button41.UseVisualStyleBackColor = true;
            // 
            // button42
            // 
            this.button42.Location = new System.Drawing.Point(140, 193);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(68, 23);
            this.button42.TabIndex = 34;
            this.button42.Text = "Start Move";
            this.button42.UseVisualStyleBackColor = true;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(24, 197);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(24, 13);
            this.label63.TabIndex = 33;
            this.label63.Text = "VX:";
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.DecimalPlaces = 2;
            this.numericUpDown6.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown6.Location = new System.Drawing.Point(70, 195);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown6.TabIndex = 32;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(6, 64);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(96, 17);
            this.radioButton4.TabIndex = 31;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Absolute Move";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // button38
            // 
            this.button38.Location = new System.Drawing.Point(256, 143);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(103, 23);
            this.button38.TabIndex = 30;
            this.button38.Text = "From Min To Max";
            this.button38.UseVisualStyleBackColor = true;
            // 
            // button39
            // 
            this.button39.Location = new System.Drawing.Point(198, 144);
            this.button39.Name = "button39";
            this.button39.Size = new System.Drawing.Size(52, 23);
            this.button39.TabIndex = 29;
            this.button39.Text = "Max";
            this.button39.UseVisualStyleBackColor = true;
            // 
            // button40
            // 
            this.button40.Location = new System.Drawing.Point(140, 144);
            this.button40.Name = "button40";
            this.button40.Size = new System.Drawing.Size(52, 23);
            this.button40.TabIndex = 28;
            this.button40.Text = "Min";
            this.button40.UseVisualStyleBackColor = true;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(25, 148);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(37, 13);
            this.label60.TabIndex = 26;
            this.label60.Text = "Zoom:";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown3.Location = new System.Drawing.Point(70, 146);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown3.TabIndex = 25;
            // 
            // button34
            // 
            this.button34.Location = new System.Drawing.Point(256, 114);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(103, 23);
            this.button34.TabIndex = 24;
            this.button34.Text = "From Min To Max";
            this.button34.UseVisualStyleBackColor = true;
            // 
            // button35
            // 
            this.button35.Location = new System.Drawing.Point(198, 114);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(52, 23);
            this.button35.TabIndex = 23;
            this.button35.Text = "Max";
            this.button35.UseVisualStyleBackColor = true;
            // 
            // button36
            // 
            this.button36.Location = new System.Drawing.Point(140, 114);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(52, 23);
            this.button36.TabIndex = 22;
            this.button36.Text = "Min";
            this.button36.UseVisualStyleBackColor = true;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(25, 119);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(17, 13);
            this.label59.TabIndex = 20;
            this.label59.Text = "Y:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown2.Location = new System.Drawing.Point(70, 117);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown2.TabIndex = 19;
            // 
            // button33
            // 
            this.button33.Location = new System.Drawing.Point(256, 86);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(103, 23);
            this.button33.TabIndex = 18;
            this.button33.Text = "From Min To Max";
            this.button33.UseVisualStyleBackColor = true;
            // 
            // button32
            // 
            this.button32.Location = new System.Drawing.Point(198, 86);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(52, 23);
            this.button32.TabIndex = 17;
            this.button32.Text = "Max";
            this.button32.UseVisualStyleBackColor = true;
            // 
            // button31
            // 
            this.button31.Location = new System.Drawing.Point(140, 86);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(52, 23);
            this.button31.TabIndex = 16;
            this.button31.Text = "Min";
            this.button31.UseVisualStyleBackColor = true;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(24, 90);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(17, 13);
            this.label58.TabIndex = 14;
            this.label58.Text = "X:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(70, 88);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown1.TabIndex = 13;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 172);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(98, 17);
            this.radioButton3.TabIndex = 12;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Continius Move";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(6, 35);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(90, 23);
            this.button16.TabIndex = 11;
            this.button16.Text = "Get PTZ URL";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // textBox58
            // 
            this.textBox58.Location = new System.Drawing.Point(105, 6);
            this.textBox58.Name = "textBox58";
            this.textBox58.ReadOnly = true;
            this.textBox58.Size = new System.Drawing.Size(252, 20);
            this.textBox58.TabIndex = 10;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(8, 9);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(56, 13);
            this.label37.TabIndex = 9;
            this.label37.Text = "PTZ URL:";
            // 
            // textBox57
            // 
            this.textBox57.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox57.Location = new System.Drawing.Point(376, 3);
            this.textBox57.Multiline = true;
            this.textBox57.Name = "textBox57";
            this.textBox57.ReadOnly = true;
            this.textBox57.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox57.Size = new System.Drawing.Size(434, 415);
            this.textBox57.TabIndex = 7;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.splitContainer4);
            this.tabPage6.Controls.Add(this.groupBox12);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(824, 461);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Requests";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer4.Location = new System.Drawing.Point(8, 80);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.label74);
            this.splitContainer4.Panel1.Controls.Add(this.treeView2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Panel2.Controls.Add(this.comboBox6);
            this.splitContainer4.Panel2.Controls.Add(this.button49);
            this.splitContainer4.Panel2.Controls.Add(this.label71);
            this.splitContainer4.Panel2.Controls.Add(this.textBox62);
            this.splitContainer4.Panel2.Controls.Add(this.label72);
            this.splitContainer4.Panel2.Controls.Add(this.button48);
            this.splitContainer4.Panel2.Controls.Add(this.textBox63);
            this.splitContainer4.Panel2.Controls.Add(this.label73);
            this.splitContainer4.Panel2.Controls.Add(this.button47);
            this.splitContainer4.Size = new System.Drawing.Size(808, 373);
            this.splitContainer4.SplitterDistance = 269;
            this.splitContainer4.TabIndex = 3;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(3, 6);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(59, 13);
            this.label74.TabIndex = 1;
            this.label74.Text = "Templates:";
            // 
            // treeView2
            // 
            this.treeView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView2.Location = new System.Drawing.Point(3, 22);
            this.treeView2.Name = "treeView2";
            treeNode56.Name = "Node2";
            treeNode56.Text = "Request1";
            treeNode57.Name = "Node4";
            treeNode57.Text = "Request2";
            treeNode58.Name = "Node1";
            treeNode58.Text = "GetWsdlUrl";
            treeNode59.Name = "Node6";
            treeNode59.Text = "Request1";
            treeNode60.Name = "Node5";
            treeNode60.Text = "GetCapabilities";
            treeNode61.Name = "Node0";
            treeNode61.Text = "Device Managment";
            treeNode62.Name = "Node9";
            treeNode62.Text = "Request1";
            treeNode63.Name = "Node8";
            treeNode63.Text = "GetNodes";
            treeNode64.Name = "Node7";
            treeNode64.Text = "PTZ";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode61,
            treeNode64});
            this.treeView2.Size = new System.Drawing.Size(263, 348);
            this.treeView2.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer5.Location = new System.Drawing.Point(6, 114);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.textBox64);
            this.splitContainer5.Panel1.Controls.Add(this.label75);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.textBox65);
            this.splitContainer5.Panel2.Controls.Add(this.label76);
            this.splitContainer5.Size = new System.Drawing.Size(523, 259);
            this.splitContainer5.SplitterDistance = 260;
            this.splitContainer5.TabIndex = 19;
            // 
            // textBox64
            // 
            this.textBox64.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox64.Location = new System.Drawing.Point(6, 17);
            this.textBox64.Multiline = true;
            this.textBox64.Name = "textBox64";
            this.textBox64.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox64.Size = new System.Drawing.Size(251, 239);
            this.textBox64.TabIndex = 3;
            this.textBox64.Text = resources.GetString("textBox64.Text");
            this.textBox64.WordWrap = false;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(3, 0);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(50, 13);
            this.label75.TabIndex = 2;
            this.label75.Text = "Request:";
            // 
            // textBox65
            // 
            this.textBox65.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox65.Location = new System.Drawing.Point(5, 17);
            this.textBox65.Multiline = true;
            this.textBox65.Name = "textBox65";
            this.textBox65.ReadOnly = true;
            this.textBox65.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox65.Size = new System.Drawing.Size(253, 239);
            this.textBox65.TabIndex = 5;
            this.textBox65.Text = resources.GetString("textBox65.Text");
            this.textBox65.WordWrap = false;
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(3, 0);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(58, 13);
            this.label76.TabIndex = 4;
            this.label76.Text = "Response:";
            // 
            // comboBox6
            // 
            this.comboBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "Device Managment",
            "Media",
            "PTZ",
            "Event"});
            this.comboBox6.Location = new System.Drawing.Point(96, 6);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(433, 21);
            this.comboBox6.TabIndex = 18;
            this.comboBox6.Text = "Device Managment";
            // 
            // button49
            // 
            this.button49.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button49.Location = new System.Drawing.Point(436, 85);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(93, 23);
            this.button49.TabIndex = 4;
            this.button49.Text = "Send Request";
            this.button49.UseVisualStyleBackColor = true;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(3, 37);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(87, 13);
            this.label71.TabIndex = 17;
            this.label71.Text = "Service Address:";
            // 
            // textBox62
            // 
            this.textBox62.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox62.Location = new System.Drawing.Point(96, 33);
            this.textBox62.Name = "textBox62";
            this.textBox62.Size = new System.Drawing.Size(433, 20);
            this.textBox62.TabIndex = 16;
            this.textBox62.Text = "/onvif/device_service";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(3, 11);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(46, 13);
            this.label72.TabIndex = 15;
            this.label72.Text = "Service:";
            // 
            // button48
            // 
            this.button48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button48.Location = new System.Drawing.Point(267, 85);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(163, 23);
            this.button48.TabIndex = 3;
            this.button48.Text = "Add Request To Templates";
            this.button48.UseVisualStyleBackColor = true;
            // 
            // textBox63
            // 
            this.textBox63.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox63.Enabled = false;
            this.textBox63.Location = new System.Drawing.Point(96, 59);
            this.textBox63.Name = "textBox63";
            this.textBox63.Size = new System.Drawing.Size(399, 20);
            this.textBox63.TabIndex = 0;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(3, 61);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(69, 13);
            this.label73.TabIndex = 2;
            this.label73.Text = "Request File:";
            // 
            // button47
            // 
            this.button47.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button47.Location = new System.Drawing.Point(501, 56);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(28, 23);
            this.button47.TabIndex = 1;
            this.button47.Text = "...";
            this.button47.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Controls.Add(this.label35);
            this.groupBox12.Controls.Add(this.textBox60);
            this.groupBox12.Controls.Add(this.label67);
            this.groupBox12.Controls.Add(this.textBox61);
            this.groupBox12.Location = new System.Drawing.Point(8, 3);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(808, 71);
            this.groupBox12.TabIndex = 1;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Camera Account";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(6, 46);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(56, 13);
            this.label35.TabIndex = 13;
            this.label35.Text = "Password:";
            // 
            // textBox60
            // 
            this.textBox60.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox60.Location = new System.Drawing.Point(99, 42);
            this.textBox60.Name = "textBox60";
            this.textBox60.Size = new System.Drawing.Size(703, 20);
            this.textBox60.TabIndex = 12;
            this.textBox60.Text = "password";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(6, 20);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(63, 13);
            this.label67.TabIndex = 11;
            this.label67.Text = "User Name:";
            // 
            // textBox61
            // 
            this.textBox61.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox61.Location = new System.Drawing.Point(99, 17);
            this.textBox61.Name = "textBox61";
            this.textBox61.Size = new System.Drawing.Size(703, 20);
            this.textBox61.TabIndex = 10;
            this.textBox61.Text = "Admin";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Other Information:";
            // 
            // textBox10
            // 
            this.textBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox10.Location = new System.Drawing.Point(143, 123);
            this.textBox10.Multiline = true;
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(578, 62);
            this.textBox10.TabIndex = 14;
            this.textBox10.Text = "XXXXXXXXXX";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Firmware:";
            // 
            // textBox11
            // 
            this.textBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox11.Location = new System.Drawing.Point(143, 97);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(578, 20);
            this.textBox11.TabIndex = 12;
            this.textBox11.Text = "XXXXXXXXXX";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Sereal Number:";
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(727, 162);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 17;
            this.button6.Text = "Clear";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Location = new System.Drawing.Point(727, 19);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 98);
            this.button7.TabIndex = 16;
            this.button7.Text = "Get From Device";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // textBox14
            // 
            this.textBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox14.Location = new System.Drawing.Point(143, 71);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(578, 20);
            this.textBox14.TabIndex = 10;
            this.textBox14.Text = "XXXXXXXXXX";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(39, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "Model:";
            // 
            // textBox15
            // 
            this.textBox15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox15.Location = new System.Drawing.Point(143, 45);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(578, 20);
            this.textBox15.TabIndex = 8;
            this.textBox15.Text = "XXXXXXXXXX";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Brand:";
            // 
            // textBox16
            // 
            this.textBox16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox16.Location = new System.Drawing.Point(143, 19);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(578, 20);
            this.textBox16.TabIndex = 6;
            this.textBox16.Text = "XXXXXXXXXX";
            // 
            // tabPage15
            // 
            this.tabPage15.Controls.Add(this.button20);
            this.tabPage15.Controls.Add(this.button21);
            this.tabPage15.Controls.Add(this.button22);
            this.tabPage15.Controls.Add(this.button23);
            this.tabPage15.Controls.Add(this.button24);
            this.tabPage15.Controls.Add(this.button25);
            this.tabPage15.Controls.Add(this.button26);
            this.tabPage15.Controls.Add(this.button27);
            this.tabPage15.Controls.Add(this.label48);
            this.tabPage15.Controls.Add(this.textBox47);
            this.tabPage15.Controls.Add(this.label49);
            this.tabPage15.Controls.Add(this.textBox48);
            this.tabPage15.Controls.Add(this.label50);
            this.tabPage15.Controls.Add(this.textBox49);
            this.tabPage15.Controls.Add(this.label51);
            this.tabPage15.Controls.Add(this.textBox50);
            this.tabPage15.Controls.Add(this.label52);
            this.tabPage15.Controls.Add(this.textBox51);
            this.tabPage15.Controls.Add(this.label53);
            this.tabPage15.Controls.Add(this.textBox52);
            this.tabPage15.Controls.Add(this.textBox53);
            this.tabPage15.Controls.Add(this.textBox54);
            this.tabPage15.Controls.Add(this.label54);
            this.tabPage15.Location = new System.Drawing.Point(4, 22);
            this.tabPage15.Name = "tabPage15";
            this.tabPage15.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage15.Size = new System.Drawing.Size(816, 439);
            this.tabPage15.TabIndex = 0;
            this.tabPage15.Text = "Device Managment";
            this.tabPage15.UseVisualStyleBackColor = true;
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(249, 218);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(75, 23);
            this.button20.TabIndex = 41;
            this.button20.Text = "Reboot";
            this.button20.UseVisualStyleBackColor = true;
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(168, 218);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(75, 23);
            this.button21.TabIndex = 40;
            this.button21.Text = "Set IP Address";
            this.button21.UseVisualStyleBackColor = true;
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(87, 218);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(75, 23);
            this.button22.TabIndex = 39;
            this.button22.Text = "Get Hostname";
            this.button22.UseVisualStyleBackColor = true;
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(6, 218);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(75, 23);
            this.button23.TabIndex = 38;
            this.button23.Text = "Get Interfaces";
            this.button23.UseVisualStyleBackColor = true;
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(249, 189);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(75, 23);
            this.button24.TabIndex = 37;
            this.button24.Text = "Reboot";
            this.button24.UseVisualStyleBackColor = true;
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(168, 189);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(75, 23);
            this.button25.TabIndex = 36;
            this.button25.Text = "Hard Reset";
            this.button25.UseVisualStyleBackColor = true;
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(87, 189);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(75, 23);
            this.button26.TabIndex = 35;
            this.button26.Text = "Probe";
            this.button26.UseVisualStyleBackColor = true;
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(6, 189);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(75, 23);
            this.button27.TabIndex = 34;
            this.button27.Text = "Device Info";
            this.button27.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(5, 140);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(56, 13);
            this.label48.TabIndex = 33;
            this.label48.Text = "Hardware:";
            // 
            // textBox47
            // 
            this.textBox47.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox47.Location = new System.Drawing.Point(142, 137);
            this.textBox47.Name = "textBox47";
            this.textBox47.ReadOnly = true;
            this.textBox47.Size = new System.Drawing.Size(235, 20);
            this.textBox47.TabIndex = 32;
            this.textBox47.Text = "XXXXXXXXXX";
            // 
            // label49
            // 
            this.label49.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(3, 166);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(52, 13);
            this.label49.TabIndex = 31;
            this.label49.Text = "Firmware:";
            // 
            // textBox48
            // 
            this.textBox48.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox48.Location = new System.Drawing.Point(142, 163);
            this.textBox48.Name = "textBox48";
            this.textBox48.ReadOnly = true;
            this.textBox48.Size = new System.Drawing.Size(235, 20);
            this.textBox48.TabIndex = 30;
            this.textBox48.Text = "XXXXXXXXXX";
            // 
            // label50
            // 
            this.label50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(5, 114);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(76, 13);
            this.label50.TabIndex = 29;
            this.label50.Text = "Serial Number:";
            // 
            // textBox49
            // 
            this.textBox49.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox49.Location = new System.Drawing.Point(142, 111);
            this.textBox49.Name = "textBox49";
            this.textBox49.ReadOnly = true;
            this.textBox49.Size = new System.Drawing.Size(235, 20);
            this.textBox49.TabIndex = 28;
            this.textBox49.Text = "XXXXXXXXXX";
            // 
            // label51
            // 
            this.label51.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(5, 88);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(39, 13);
            this.label51.TabIndex = 27;
            this.label51.Text = "Model:";
            // 
            // textBox50
            // 
            this.textBox50.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox50.Location = new System.Drawing.Point(142, 85);
            this.textBox50.Name = "textBox50";
            this.textBox50.ReadOnly = true;
            this.textBox50.Size = new System.Drawing.Size(235, 20);
            this.textBox50.TabIndex = 26;
            this.textBox50.Text = "XXXXXXXXXX";
            // 
            // label52
            // 
            this.label52.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(5, 62);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(73, 13);
            this.label52.TabIndex = 25;
            this.label52.Text = "Manufacturer:";
            // 
            // textBox51
            // 
            this.textBox51.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox51.Location = new System.Drawing.Point(142, 59);
            this.textBox51.Name = "textBox51";
            this.textBox51.ReadOnly = true;
            this.textBox51.Size = new System.Drawing.Size(235, 20);
            this.textBox51.TabIndex = 24;
            this.textBox51.Text = "XXXXXXXXXX";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(8, 36);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(20, 13);
            this.label53.TabIndex = 5;
            this.label53.Text = "IP:";
            // 
            // textBox52
            // 
            this.textBox52.Location = new System.Drawing.Point(142, 33);
            this.textBox52.Name = "textBox52";
            this.textBox52.ReadOnly = true;
            this.textBox52.Size = new System.Drawing.Size(235, 20);
            this.textBox52.TabIndex = 4;
            // 
            // textBox53
            // 
            this.textBox53.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBox53.Location = new System.Drawing.Point(383, 3);
            this.textBox53.Multiline = true;
            this.textBox53.Name = "textBox53";
            this.textBox53.ReadOnly = true;
            this.textBox53.Size = new System.Drawing.Size(430, 433);
            this.textBox53.TabIndex = 3;
            // 
            // textBox54
            // 
            this.textBox54.Location = new System.Drawing.Point(142, 7);
            this.textBox54.Name = "textBox54";
            this.textBox54.ReadOnly = true;
            this.textBox54.Size = new System.Drawing.Size(235, 20);
            this.textBox54.TabIndex = 2;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(8, 10);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(128, 13);
            this.label54.TabIndex = 1;
            this.label54.Text = "Device Managment URL:";
            // 
            // tabPage16
            // 
            this.tabPage16.Controls.Add(this.textBox55);
            this.tabPage16.Controls.Add(this.textBox56);
            this.tabPage16.Controls.Add(this.label55);
            this.tabPage16.Location = new System.Drawing.Point(4, 22);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Size = new System.Drawing.Size(816, 439);
            this.tabPage16.TabIndex = 3;
            this.tabPage16.Text = "Media";
            this.tabPage16.UseVisualStyleBackColor = true;
            // 
            // textBox55
            // 
            this.textBox55.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBox55.Location = new System.Drawing.Point(386, 0);
            this.textBox55.Multiline = true;
            this.textBox55.Name = "textBox55";
            this.textBox55.ReadOnly = true;
            this.textBox55.Size = new System.Drawing.Size(430, 439);
            this.textBox55.TabIndex = 6;
            // 
            // textBox56
            // 
            this.textBox56.Location = new System.Drawing.Point(138, 7);
            this.textBox56.Name = "textBox56";
            this.textBox56.ReadOnly = true;
            this.textBox56.Size = new System.Drawing.Size(235, 20);
            this.textBox56.TabIndex = 5;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(4, 10);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(64, 13);
            this.label55.TabIndex = 4;
            this.label55.Text = "Media URL:";
            // 
            // tabPage17
            // 
            this.tabPage17.Controls.Add(this.label56);
            this.tabPage17.Location = new System.Drawing.Point(4, 22);
            this.tabPage17.Name = "tabPage17";
            this.tabPage17.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage17.Size = new System.Drawing.Size(816, 439);
            this.tabPage17.TabIndex = 1;
            this.tabPage17.Text = "PTZ";
            this.tabPage17.UseVisualStyleBackColor = true;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(294, 147);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(186, 13);
            this.label56.TabIndex = 1;
            this.label56.Text = "КОЕ-КАКИЕ КОНТРОЛЫ ДЛЯ PTZ";
            // 
            // tabPage18
            // 
            this.tabPage18.Controls.Add(this.label57);
            this.tabPage18.Location = new System.Drawing.Point(4, 22);
            this.tabPage18.Name = "tabPage18";
            this.tabPage18.Size = new System.Drawing.Size(816, 439);
            this.tabPage18.TabIndex = 2;
            this.tabPage18.Text = "Real Time Streaming";
            this.tabPage18.UseVisualStyleBackColor = true;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(329, 168);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(310, 13);
            this.label57.TabIndex = 0;
            this.label57.Text = "СОБСТВЕННО ПЛЕЕР И НАСТРОЙКИ (ПОТОМ ДОРИСУЮ)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(832, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howDoIToolStripMenuItem,
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howDoIToolStripMenuItem
            // 
            this.howDoIToolStripMenuItem.Name = "howDoIToolStripMenuItem";
            this.howDoIToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.howDoIToolStripMenuItem.Text = "How Do I";
            this.howDoIToolStripMenuItem.Click += new System.EventHandler(this.howDoIToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "None.ico");
            this.imageList2.Images.SetKeyName(1, "MUST2.ico");
            this.imageList2.Images.SetKeyName(2, "MUSTIFSUP2.ico");
            this.imageList2.Images.SetKeyName(3, "SHOULD2.ico");
            this.imageList2.Images.SetKeyName(4, "SHOULDIFSUP2.ico");
            this.imageList2.Images.SetKeyName(5, "OPTIONAL2.ico");
            this.imageList2.Images.SetKeyName(6, "MUSTIFIML2.ico");
            this.imageList2.Images.SetKeyName(7, "MUSTIFSUPIMPL2.ico");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 511);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(840, 545);
            this.Name = "Form1";
            this.Text = "ONVIF Conformance Test - 169.254.136.46";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage10.PerformLayout();
            this.tabPage13.ResumeLayout(false);
            this.tabPage13.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            this.splitContainer5.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.tabPage15.ResumeLayout(false);
            this.tabPage15.PerformLayout();
            this.tabPage16.ResumeLayout(false);
            this.tabPage16.PerformLayout();
            this.tabPage17.ResumeLayout(false);
            this.tabPage17.PerformLayout();
            this.tabPage18.ResumeLayout(false);
            this.tabPage18.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBox26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBox27;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox textBox31;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBox28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox textBox29;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TextBox textBox30;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox textBox32;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox textBox33;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TextBox textBox35;
        private System.Windows.Forms.TextBox textBox34;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox textBox36;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox textBox38;
        private System.Windows.Forms.TextBox textBox37;
        private System.Windows.Forms.TextBox textBox41;
        private System.Windows.Forms.TabPage tabPage13;
        private System.Windows.Forms.TextBox textBox39;
        private System.Windows.Forms.TextBox textBox40;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox textBox46;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textBox42;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox textBox43;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox textBox44;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox textBox45;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TabPage tabPage15;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox textBox47;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textBox48;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox textBox49;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox textBox50;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox textBox51;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox textBox52;
        private System.Windows.Forms.TextBox textBox53;
        private System.Windows.Forms.TextBox textBox54;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.TabPage tabPage16;
        private System.Windows.Forms.TextBox textBox55;
        private System.Windows.Forms.TextBox textBox56;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TabPage tabPage17;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.TabPage tabPage18;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.Button button28;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.TextBox textBox58;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox textBox57;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button38;
        private System.Windows.Forms.Button button39;
        private System.Windows.Forms.Button button40;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Button button34;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.Button button36;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button button33;
        private System.Windows.Forms.Button button32;
        private System.Windows.Forms.Button button31;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Button button43;
        private System.Windows.Forms.Button button44;
        private System.Windows.Forms.Button button30;
        private System.Windows.Forms.Button button37;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.Button button41;
        private System.Windows.Forms.Button button42;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.TextBox textBox59;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.Button button45;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Button button46;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox textBox60;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.TextBox textBox61;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.TextBox textBox62;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button button49;
        private System.Windows.Forms.Button button48;
        private System.Windows.Forms.TextBox textBox63;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Button button47;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TextBox textBox64;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.TextBox textBox65;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.ComboBox comboBox10;
        private System.Windows.Forms.Button button50;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howDoIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textBox24;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBox22;
        private System.Windows.Forms.Button button51;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBox18;
    }
}

