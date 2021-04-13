using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Collections.Generic;

namespace PreviewDemo
{
	/// <summary>
	/// Form1 ��ժҪ˵����
	/// </summary>
	public class Preview : System.Windows.Forms.Form
	{
        private uint iLastErr = 0;
		private Int32 m_lUserID = -1;
		private bool m_bInitSDK = false;
        /*private bool m_bRecord = false;
        private bool m_bTalk = false;*/
		private Int32 m_lRealHandle = -1;
       /* private int lVoiceComHandle = -1;*/
        private string str;

        CHCNetSDK.REALDATACALLBACK RealData = null;
        CHCNetSDK.LOGINRESULTCALLBACK LoginCallBack = null;
        public CHCNetSDK.NET_DVR_PTZPOS m_struPtzCfg;
        public CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;

        public delegate void UpdateTextStatusCallback(string strLogStatus, IntPtr lpDeviceInfo);

        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.PictureBox RealPlayWnd;
        private TextBox textBoxIP;
        private TextBox textBoxPort;
        private TextBox textBoxUserName;
        private TextBox textBoxPassword;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label10;
        private Label label13;
        private TextBox textBoxChannel;
        private Label label17;
        private TextBox textBoxID;
        /*private Button PtzGet;
        private Button PtzSet;*/
        private Label label19;
        /*private ComboBox comboBox1;
        private TextBox textBoxPanPos;
        private TextBox textBoxTiltPos;
        private TextBox textBoxZoomPos;*/
        private Label label20;
        private Label label21;
        private Label label22;
        private TextBox textBox1;
        private Label label15;
        private Button button1;
        private Label label11;
        private TextBox textBox2;
        private Button button2;
        private Label label16;
        private TextBox textBox3;
        private PictureBox pictureBox1;
        private Label label18;
        private TextBox textBox4;
        private CheckBox checkBox1;
        private Label label23;
        private CheckBox checkBox2;
        private Label label9;
        private TextBox textBox5;
        private Button button3;
        private Label label12;
        private CheckBox checkBox3;

        //private GroupBox groupBox1;

        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;

		public Preview()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();
            
            this.Text = "ץ�ĳ���";
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
			{
				MessageBox.Show("NET_DVR_Init error!");
				return;
			}
			else
			{
                //����SDK��־ To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
			}
            //
            // TODO: �� InitializeComponent ���ú�����κι��캯������
            //
        }

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (m_lRealHandle >= 0)
			{
				CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
			}
			if (m_lUserID >= 0)
			{
				CHCNetSDK.NET_DVR_Logout(m_lUserID);
			}
			if (m_bInitSDK == true)
			{
				CHCNetSDK.NET_DVR_Cleanup();
			}
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxChannel = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device IP";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "User Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(312, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(312, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 22);
            this.label4.TabIndex = 0;
            this.label4.Text = "Device Port";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(580, 49);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(104, 64);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackColor = System.Drawing.SystemColors.WindowText;
            this.RealPlayWnd.Location = new System.Drawing.Point(24, 168);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(660, 486);
            this.RealPlayWnd.TabIndex = 4;
            this.RealPlayWnd.TabStop = false;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(104, 31);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(152, 25);
            this.textBoxIP.TabIndex = 2;
            this.textBoxIP.Text = "192.168.3.4";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(411, 31);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(149, 25);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.Text = "8000";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(104, 90);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(152, 25);
            this.textBoxUserName.TabIndex = 4;
            this.textBoxUserName.Text = "admin";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxPassword.Location = new System.Drawing.Point(411, 90);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(149, 25);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.Text = "zym20000216";
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "�豸IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(312, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "�豸�˿�";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "�û���";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(315, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 15);
            this.label8.TabIndex = 12;
            this.label8.Text = "����";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(589, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 15);
            this.label10.TabIndex = 14;
            this.label10.Text = "��¼";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 670);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 15);
            this.label13.TabIndex = 19;
            this.label13.Text = "Ԥ��/ץͼͨ��";
            // 
            // textBoxChannel
            // 
            this.textBoxChannel.Location = new System.Drawing.Point(143, 665);
            this.textBoxChannel.Name = "textBoxChannel";
            this.textBoxChannel.Size = new System.Drawing.Size(133, 25);
            this.textBoxChannel.TabIndex = 6;
            this.textBoxChannel.Text = "1";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(317, 669);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 15);
            this.label17.TabIndex = 27;
            this.label17.Text = "��ID";
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(369, 663);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(300, 25);
            this.textBoxID.TabIndex = 28;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(823, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(383, 25);
            this.textBox1.TabIndex = 34;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(724, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(97, 15);
            this.label15.TabIndex = 35;
            this.label15.Text = "�洢���ļ���";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1229, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 36;
            this.button1.Text = "ѡ��";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ChooseFile);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(967, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(165, 15);
            this.label11.TabIndex = 37;
            this.label11.Text = "��ȡͼƬʱ������s��";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1137, 64);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(69, 25);
            this.textBox2.TabIndex = 38;
            this.textBox2.Text = "0.5";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1179, 117);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 31);
            this.button2.TabIndex = 39;
            this.button2.Text = "��ʼ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1226, 69);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(97, 15);
            this.label16.TabIndex = 40;
            this.label16.Text = "��ȡͼƬ����";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(1329, 64);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(45, 25);
            this.textBox3.TabIndex = 41;
            this.textBox3.Text = "0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(727, 168);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(660, 486);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(724, 122);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(127, 15);
            this.label18.TabIndex = 43;
            this.label18.Text = "�趨��ȡͼƬ����";
            this.label18.Click += new System.EventHandler(this.label18_Click_1);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(881, 117);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(85, 25);
            this.textBox4.TabIndex = 44;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(857, 122);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(18, 17);
            this.checkBox1.TabIndex = 45;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(983, 122);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(37, 15);
            this.label23.TabIndex = 46;
            this.label23.Text = "¼��";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(1026, 122);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(18, 17);
            this.checkBox2.TabIndex = 47;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(724, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 15);
            this.label9.TabIndex = 48;
            this.label9.Text = "���ļ�������";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(823, 66);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(125, 25);
            this.textBox5.TabIndex = 49;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1288, 117);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 31);
            this.button3.TabIndex = 50;
            this.button3.Text = "�ϳ�";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1065, 122);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 15);
            this.label12.TabIndex = 51;
            this.label12.Text = "�Զ��ϳ�";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(1137, 122);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(18, 17);
            this.checkBox3.TabIndex = 52;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // Preview
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.ClientSize = new System.Drawing.Size(1495, 714);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBoxID);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBoxChannel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.RealPlayWnd);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Name = "Preview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview";
            this.Resize += new System.EventHandler(this.Preview_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Preview());
		}

        public void UpdateClientList(string strLogStatus, IntPtr lpDeviceInfo)
        {
        }

        public void cbLoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {
            string strLoginCallBack = "��¼�豸��lUserID��" + lUserID + "��dwResult��" + dwResult;

            if (dwResult==0)
            {
                uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                strLoginCallBack = strLoginCallBack + "�������:" + iErrCode;
            }

            //�������ע�͵�Ҳ�����
            if (InvokeRequired)
            {
                object[] paras = new object[2];
                paras[0] = strLoginCallBack;
                paras[1] = lpDeviceInfo;
            }
            else
            {
                //�����ÿؼ������߳�ֱ�Ӹ�����Ϣ�б� 
                UpdateClientList(strLoginCallBack, lpDeviceInfo);
            }

        }

		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			if (textBoxIP.Text == "" || textBoxPort.Text == "" ||
				textBoxUserName.Text == "" || textBoxPassword.Text == "")
			{
				MessageBox.Show("Please input IP, Port, User name and Password!");
				return;
			}
            if (m_lUserID < 0)
            {

                struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();
                
                //�豸IP��ַ��������
                byte[] byIP = System.Text.Encoding.Default.GetBytes(textBoxIP.Text);
                struLogInfo.sDeviceAddress = new byte[129];
                byIP.CopyTo(struLogInfo.sDeviceAddress, 0);

                //�豸�û���
                byte[] byUserName = System.Text.Encoding.Default.GetBytes(textBoxUserName.Text);
                struLogInfo.sUserName = new byte[64];
                byUserName.CopyTo(struLogInfo.sUserName, 0);
                
                //�豸����
                byte[] byPassword = System.Text.Encoding.Default.GetBytes(textBoxPassword.Text);
                struLogInfo.sPassword = new byte[64];
                byPassword.CopyTo(struLogInfo.sPassword, 0);

                struLogInfo.wPort = ushort.Parse(textBoxPort.Text);//�豸����˿ں�

                if (LoginCallBack == null)
                {
                    LoginCallBack = new CHCNetSDK.LOGINRESULTCALLBACK(cbLoginCallBack);//ע��ص�����                    
                }
                struLogInfo.cbLoginResult = LoginCallBack;
                struLogInfo.bUseAsynLogin = false; //�Ƿ��첽��¼��0- ��1- �� 
                DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();
                //��¼�豸 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLogInfo, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V40 failed, error code= " + iLastErr; //��¼ʧ�ܣ���������
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //��¼�ɹ�
                    MessageBox.Show("Login Success!");
                    btnLogin.Text = "Logout";
                }

            }
            else
            {
                //ע����¼ Logout the device
                if (m_lRealHandle >= 0)
                {
                    MessageBox.Show("Please stop live view firstly");
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;           
                }
                m_lUserID = -1;
                btnLogin.Text = "Login";
            }
            return;
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{

        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
		{
            if (dwBufSize > 0)
            {
                byte[] sData = new byte[dwBufSize];
                Marshal.Copy(pBuffer, sData, 0, (Int32)dwBufSize);

                string str = "ʵʱ������.ps";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sData, 0, iLen);
                fs.Close();            
            }
		}

        private void btnBMP_Click(object sender, EventArgs e)
        {

        }

        private void btnJPEG_Click(object sender, EventArgs e)
        {

        }

        private void btnRecord_Click(object sender, EventArgs e)
        {

        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {

        }

        public void VoiceDataCallBack(int lVoiceComHandle, IntPtr pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, System.IntPtr pUser)
        {
            byte[] sString = new byte[dwBufSize];
            Marshal.Copy(pRecvDataBuffer, sString, 0, (Int32)dwBufSize);

            if (byAudioFlag ==0)
            {
                //�������������Ƶ����д���ļ� save the data into a file
                string str = "PC�ɼ���Ƶ�ļ�.pcm";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sString, 0, iLen);
                fs.Close();
            }
            if (byAudioFlag == 1)
            {
                //�������������Ƶ����д���ļ� save the data into a file
                string str = "�豸��Ƶ�ļ�.pcm";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sString, 0, iLen);
                fs.Close();
            }

        }

        private void btnVioceTalk_Click(object sender, EventArgs e)
        {

        }



        private void Preview_Resize(object sender, EventArgs e)
        {
            
        }
       






        private void Ptz_Set_Click(object sender, EventArgs e)
        {

        }

        private void PreSet_Click(object sender, EventArgs e)
        {
            PreSet dlg = new PreSet();
            dlg.m_lUserID = m_lUserID;
            dlg.m_lChannel = 1;
            dlg.m_lRealHandle = m_lRealHandle;
            dlg.ShowDialog();
            
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void ChooseFile(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "ѡ��Ŀ¼"; //��ʾ����
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string strSaveFolder = dialog.SelectedPath;
                textBox1.Text = strSaveFolder;
                num = GetFileNum(strSaveFolder);
                num++;
                textBox5.Text = num.ToString();
            }
        }

        private void label18_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void takephoto(String filename)
        {
            string sJpegPicFileName;
            //ͼƬ����·�����ļ��� the path and file name to save
            //sJpegPicFileName = "JPEG_test.jpg";
            sJpegPicFileName = filename;
            int lChannel = Int16.Parse(textBoxChannel.Text); //ͨ���� Channel number
            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //ͼ������ Image quality
            lpJpegPara.wPicSize = 0xff; //ץͼ�ֱ��� Picture size: 2- 4CIF��0xff- Auto(ʹ�õ�ǰ�����ֱ���)��ץͼ�ֱ�����Ҫ�豸֧�֣�����ȡֵ��ο�SDK�ĵ�

            //JPEGץͼ Capture a JPEG picture
            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, lChannel, ref lpJpegPara, sJpegPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
               /* str = "Successful to capture the JPEG file and the saved file is " + sJpegPicFileName;
                MessageBox.Show(str);*/
            }
            return;
        }

        private void startvideo(String filename)
        {
            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//Ԥ������
            lpPreviewInfo.lChannel = Int16.Parse(textBoxChannel.Text);//Ԥte�����豸ͨ��
            lpPreviewInfo.dwStreamType = 0;//�������ͣ�0-��������1-��������2-����3��3-����4���Դ�����
            lpPreviewInfo.dwLinkMode = 0;//���ӷ�ʽ��0- TCP��ʽ��1- UDP��ʽ��2- �ಥ��ʽ��3- RTP��ʽ��4-RTP/RTSP��5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = true; //0- ������ȡ����1- ����ȡ��
            lpPreviewInfo.dwDisplayBufNum = 1; //���ſⲥ�Ż�������󻺳�֡��
            lpPreviewInfo.byProtoType = 0;
            lpPreviewInfo.byPreviewMode = 0;
            if (RealData == null)
            {
                RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//Ԥ��ʵʱ���ص�����
            }
            IntPtr pUser = new IntPtr();//�û�����
            //��Ԥ�� Start live view 
            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
            if (m_lRealHandle < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //Ԥ��ʧ�ܣ���������
                MessageBox.Show(str);
                return;
            }
            else
            {

            }
            //¼�񱣴�·�����ļ��� the path and file name to save
            string sVideoFileName;
           /* sVideoFileName = "Record_test.mp4";*/
            sVideoFileName = filename;
            //ǿ��I֡ Make a I frame
            int lChannel = Int16.Parse(textBoxChannel.Text); //ͨ���� Channel number
            CHCNetSDK.NET_DVR_MakeKeyFrame(m_lUserID, lChannel);
            //��ʼ¼�� Start recording
            if (!CHCNetSDK.NET_DVR_SaveRealData(m_lRealHandle, sVideoFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_SaveRealData failed, error code= " + iLastErr;
                return;
            }
            else
            {
                /*MessageBox.Show("��ʼ¼��");*/
            }
            return;
        }

        private void endvideo()
        {
            //ֹͣ¼�� Stop recording
            if (!CHCNetSDK.NET_DVR_StopSaveRealData(m_lRealHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_StopSaveRealData failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                str = "¼��ɹ�";
                MessageBox.Show(str);
            }
            if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            m_lRealHandle = -1;
        }
        public int GetFileNum(string srcPath)
        {
            try
            {
                // �õ�ԴĿ¼���ļ��б��������ǰ����ļ��Լ�Ŀ¼·����һ������
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                return fileList.Length;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return 0;
        }

        private bool start = false;
        private bool circle = true;
        private Task task;
        private int num = 1;
        private int x = 1513;
        private int y = 761;

        private void readpicture(String dir,float time,bool controlnum,int num,bool controlvideo)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            int i = 0;
            circle = true;
            while (circle)
            {
                i++;
                String picturedir = dir + "//" + i + ".jpg";
                takephoto(picturedir);
                textBox3.Text = i.ToString();
                try
                {
                    pictureBox1.Image = Image.FromFile(picturedir);
                }catch(Exception e)
                {
                    
                }
                System.Threading.Thread.Sleep((int)(1000 * time));
                if (controlnum)
                {
                    if (i == num)
                    {
                        circle = false;
                    }
                }
            }
            if (start == true)
            {
                MessageBox.Show("����");
                start = false;
                if (checkBox3.Checked)
                {
                    readImage(dir);
                    stitch(dir);
                }
                string text = textBox5.Text;
                try
                {
                    int number = int.Parse(text);
                    textBox5.Text = (++number).ToString();
                }
                catch (Exception)
                {

                }
                button2.Text = "��ʼ";
                if (controlvideo)
                {
                    endvideo();
                }
                circle = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            float time;
            bool a = float.TryParse(textBox2.Text, out time);
            String dir = textBox1.Text;
            if(dir == "")
            {
                MessageBox.Show("����ѡ���ļ���");
                return;
            }
            String name = textBox5.Text;
            if (name == "")
            {
                MessageBox.Show("����д�ļ�������");
                return;
            }
            dir = dir + "//" + name;
            if (File.Exists(dir))
            {
                DialogResult MsgBoxResult;//���öԻ���ķ���ֵ  
                MsgBoxResult = MessageBox.Show("���ļ����Ѵ���ͬ���ļ��У��Ƿ񸲸Ǹ��ļ���",//�Ի������ʾ����   
                "��ʾ",//�Ի���ı���   
                MessageBoxButtons.YesNo,//����Ի���İ�ť�����ﶨ����YSE��NO������ť   
                MessageBoxIcon.Exclamation,//����Ի����ڵ�ͼ��ʽ����������һ����ɫ�������ڼ�һ����̾��   
                MessageBoxDefaultButton.Button2);//����Ի���İ�ťʽ��  
                if (MsgBoxResult == DialogResult.Yes)//����Ի���ķ���ֵ��YES����"Y"��ť��  
                {
                    DeleteDir(dir);
                }
                if (MsgBoxResult == DialogResult.No)//����Ի���ķ���ֵ��NO����"N"��ť��  
                {
                    return;
                }
            }
            bool controlnum = false;
            bool controlvideo = checkBox2.Checked;
            if (checkBox1.Checked)
            {
                bool b = int.TryParse(textBox4.Text, out num);
                if (b)
                {
                    controlnum = true;
                }
                else
                {
                    controlnum = false;
                }
            }
            if (start == false)
            {
                MessageBox.Show("��ʼ");
                System.IO.Directory.CreateDirectory(dir);
                start = true;
                button2.Text = "����";
                if (a)
                {
                    if (time == 0.0)
                    {
                        MessageBox.Show("ʱ��������Ϊ0");
                        return;
                    }
                    if (controlvideo)
                    {
                        String videopath=dir+"//video.mp4";
                        startvideo(videopath);
                    }
                    Task.Run(() => readpicture(dir,time,controlnum,num,controlvideo));

                }
                else
                {
                    MessageBox.Show("ʱ��������Ϊ������");
                }
            }
            else
            {
                MessageBox.Show("����");
                start = false;
                if (checkBox3.Checked)
                {
                    readImage(dir);
                    stitch(dir);
                }
                string num = textBox5.Text;
                try
                {
                    int number = int.Parse(num);
                    textBox5.Text = (++number).ToString();
                }catch(Exception)
                {

                }
                button2.Text = "��ʼ";
                if (controlvideo)
                {
                    String videopath = dir + "//video.mp4";
                    endvideo();
                }
                circle = false; 
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void dealimage(String path,String savepath)
        {
            Mat result = Cv2.ImRead(path);
            Scalar color = new Scalar(0, 0, 0);
            Cv2.CopyMakeBorder(result, result, 10, 10, 10, 10, BorderTypes.Constant, color);

            Mat outp = new Mat();
            Cv2.CvtColor(result, outp, ColorConversionCodes.BGR2GRAY);

            Mat thresh = new Mat();
            Cv2.Threshold(outp, thresh, 0, 255, ThresholdTypes.Binary);
            /* Cv2.ImShow("2", thresh);
             Cv2.WaitKey(-1);*/
            OpenCvSharp.Point[][] counts;
            HierarchyIndex[] hierarchyIndices;
            Cv2.FindContours(thresh.Clone(), out counts, out hierarchyIndices, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            double max = 0;
            OpenCvSharp.Point[] point = null;
            foreach (var count in counts)
            {
                if (max < Cv2.ContourArea(count))
                {
                    point = count;
                    max = Cv2.ContourArea(count);
                }

            }
            Console.WriteLine(thresh.Rows);
            Console.WriteLine(thresh.Cols);
            /*int** mask = new int[][];*/
            Rect rect = Cv2.BoundingRect(point);
            Mat mat = Mat.Zeros(thresh.Rows, thresh.Cols, thresh.Type());
            Cv2.Rectangle(mat, rect.TopLeft, rect.BottomRight, 255, -1);
            Mat minRect = mat.Clone();
            Mat sub = mat.Clone();
            while (Cv2.CountNonZero(sub) > 0)
            {
                Cv2.Erode(minRect, minRect, null);
                Cv2.Subtract(minRect, thresh, sub);
            }
            Cv2.FindContours(minRect.Clone(), out counts, out hierarchyIndices, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            max = 0;
            foreach (var count in counts)
            {
                if (max < Cv2.ContourArea(count))
                {
                    point = count;
                    max = Cv2.ContourArea(count);
                }
            }
            rect = Cv2.BoundingRect(point);
            result = new Mat(result, rect);
            savepath = savepath + "/" + "result.jpg";
            Cv2.ImWrite(savepath, result);
            try
            {
                pictureBox1.Image = Image.FromFile(savepath);
            }
            catch (Exception e)
            {

            }
            MessageBox.Show("ƴ�ӳɹ�");
        }
        void DeleteDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //����Ŀ¼�������ļ�����Ŀ¼
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //�ж��Ƿ��ļ���
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //ɾ����Ŀ¼���ļ�
                    }
                    else
                    {
                        File.Delete(i.FullName);      //ɾ��ָ���ļ�
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        List<Mat> images = new List<Mat>();
        void readImage(string path)
        {
            images = new List<Mat>();
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            foreach (var file in files)
            {
                if (file.FullName.Substring(file.FullName.Length - 4).Equals(".mp4"))
                {
                    continue;
                }
                Mat image = new Mat(file.FullName);
                images.Add(image);
            }
        }
        void stitch(String path)
        {
            Stitcher stitcher = Stitcher.Create(Stitcher.Mode.Scans);
            Mat result = new Mat();
            try
            {
                var status = stitcher.Stitch(images, result);
                if (status != Stitcher.Status.OK)
                {
                    MessageBox.Show("ƴ��ʧ��");
                    return;
                }
                else
                {
                    String savepath = path + "/" + "stitch.jpg";
                    Cv2.ImWrite(savepath, result);
                    dealimage(savepath, path);
                }
            }catch(Exception e)
            {
                MessageBox.Show("ƴ��ʧ��");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            String dir = textBox1.Text;
            if (dir == "")
            {
                MessageBox.Show("����ѡ���ļ���");
                return;
            }
            String name = textBox5.Text;
            if (name == "")
            {
                MessageBox.Show("����д�ļ�������");
                return;
            }
            dir = dir + "//" + name;
            if (File.Exists(dir))
            {
                MessageBox.Show("δ�ҵ����ļ���");
                return;
            }
            Console.WriteLine(dir);
            readImage(dir);
            stitch(dir);
        }


       
    }
}
