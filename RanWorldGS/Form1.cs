// PSGG.64 Exfarm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// anrsk.Form1
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;


public class Form1 : Form
{
    public class ComboBoxItem
    {
        public string DisplayText { get; set; }

        public string Value { get; set; }

        public ComboBoxItem(string displayText, string value)
        {
            DisplayText = displayText;
            Value = value;
        }

        public override string ToString()
        {
            return DisplayText;
        }
    }
    private GameMemoryModifier modifier;

    private Thread modificationThread;

    private Dictionary<string, string> itemDescriptions2 = new Dictionary<string, string>
    {
        { "1", "Speed Box" },
        { "2", "Crazy Time Box" },
        { "3", "Power Up Box" },
        { "4", "Exp Box" },
        { "6", "Lucky Box" },
        { "9,", "Speed Up Max Box" },
        { "10,", "Madness Time Box" },
        { "11", "Power Up Max Box" }
    };

    private Mem m = new Mem();

    private Dictionary<string, string> itemDescriptions = new Dictionary<string, string>
    {
        { "1", "Hp+Sp+MP" },
        { "2", "Power" },
        { "3", "Defender" },
        { "4", "Auto Pot" },
        { "5", "Protect Item" },
        { "6", "Collect All" },
        { "7", "Collect Rare" },
        { "8", "Collect Potion" },
        { "9", "Collect Gold" },
        { "10", "Collect Ore" },
        { "32", "Collect Box" }
    };

    private int selectedPercentage = 50;

    private Thread buff;

    private System.Windows.Forms.Timer timerHPMPSP;

    private string selectedFilePath;

    private BackgroundWorker updateWorker = new BackgroundWorker();

    private int previousMemoryDead;

    private int sleepDuration;

    private IContainer components;

    private ComboBox comboBox1;

    private Button button3;

    private Button button1;

    private Button button2;

    private Button button4;

    private CheckBox checkBox1;

    private BackgroundWorker backgroundWorker1;

    private ComboBox comboBox3;

    private Button button7;

    private Label label1;

    private CheckBox checkBox2;

    private Button button6;

    private ProgressBar progressBarHP;

    private ProgressBar progressBarMP;

    private ProgressBar progressBarSP;

    private BackgroundWorker backgroundWorkerHPMPSP;

    private ComboBox comboBoxPercentage;

    private System.Windows.Forms.Timer timer1;

    private BackgroundWorker backgroundWorker2;

    private GroupBox groupBox1;

    private ComboBox Boxdrop;

    private ComboBox comboBox2;

    private GroupBox groupBox2;

    private GroupBox groupBox3;

    private GroupBox groupBox4;

    private TabControl tabControl1;

    private TabPage tabPage1;

    private TabPage tabPage2;

    private GroupBox groupBox5;

    private ListBox listBox1;

    private Button bypassbut;

    private Button refreshbut;

    private CheckBox checkBox3;

    private BackgroundWorker autorv;

    private TextBox selectedPathTextBox;

    private Button BrowseButton;

    private FolderBrowserDialog folderBrowserDialog1;

    private Button Startbut;

    private GroupBox groupBox6;

    private Button button8;

    private CheckBox checkBox4;

    private CheckBox typeB;

    private CheckBox typeA;

    private GroupBox groupBox7;

    private Button ModifyMemoryButton;

    private Button StopButton;

    private GroupBox groupBox9;

    private GroupBox groupBox8;

    private TabPage tabPage3;

    private ListBox listBox2;

    private BackgroundWorker Exfarm;

    private GroupBox groupBox11;

    private Button button5;

    private ListBox Savepoint;

    private GroupBox groupBox10;

    private CheckBox checkBox5;

    private TextBox textBox1;

    private CheckBox checkBox6;

    private GroupBox groupBox12;

    private CheckBox checkBox7;

    private Label label2;

    private TextBox textBox2;

    private CheckBox F3tray;

    private CheckBox F2tray;

    private BackgroundWorker Autoskillworker;

    public Form1()
    {
        InitializeComponent();
        backgroundWorker2.WorkerSupportsCancellation = true;
        autorv.WorkerSupportsCancellation = true;
        Exfarm.WorkerSupportsCancellation = true;
        Autoskillworker.WorkerSupportsCancellation = true;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        label1.Text = "Please select a window.";
        comboBox1.Items.Clear();
        comboBox2.Items.Clear();
        Process[] processesByName = Process.GetProcessesByName("Game");
        foreach (Process process in processesByName)
        {
            comboBox1.Items.Add(process.Id);
        }
        comboBox3.Items.Clear();
        foreach (KeyValuePair<string, string> itemDescription in itemDescriptions)
        {
            comboBox3.Items.Add(new ComboBoxItem(itemDescription.Value, itemDescription.Key));
            comboBox2.Items.Add(new ComboBoxItem(itemDescription.Value, itemDescription.Key));
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        button1.Enabled = false;
        button2.Enabled = true;
        m.FreezeValue("01F321A0", "int", "1");
        if (typeB.Checked && F2tray.Checked && !typeA.Checked && !button1.Enabled && !Autoskillworker.IsBusy)
        {
            Autoskillworker.RunWorkerAsync();
        }
        else if (typeA.Checked && F2tray.Checked)
        {
            MessageBox.Show("This feature only working for typeB\nCheck TypeB then try again please ");
        }
        else if (Autoskillworker.IsBusy)
        {
            Autoskillworker.CancelAsync();
        }
    }

    private void Autoskillworker_DoWork(object sender, DoWorkEventArgs e)
    {
        while (true)
        {
            if (typeB.Checked && !button1.Enabled && F2tray.Checked)
            {
                PerformAutoSkill("02B0424E", "02B04252");
                PerformAutoSkill("02B0425C", "02B04260");
                if (F2tray.Checked)
                {
                    PerformSingleWrite("02B04288");
                    if (F3tray.Checked)
                    {
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(500);
                PerformAutoSkill("02B0424E", "02B04252");
                PerformAutoSkill("02B0425C", "02B04260");
                if (F3tray.Checked)
                {
                    PerformSingleWrite("02B04289");
                    if (F2tray.Checked)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            Thread.Sleep(500);
        }
    }

    private void PerformAutoSkill(string startAddress, string endAddress)
    {
        for (int i = Convert.ToInt32(startAddress, 16); i <= Convert.ToInt32(endAddress, 16); i++)
        {
            PerformSingleWrite("02B0425B");
            PerformSingleWrite(i.ToString("X"));
            Thread.Sleep(300);
            PerformSingleWrite(i.ToString("X"), "00");
            PerformSingleWrite("02B0425B", "00");
        }
    }

    private void PerformSingleWrite(string address, string value = "63")
    {
        m.WriteMemory(address, "byte", value);
        Thread.Sleep(300);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        button1.Enabled = true;
        button2.Enabled = false;
        m.UnfreezeValue("01F321A0");
        m.WriteMemory("01F321A0", "int", "0");
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        m.OpenProcess(int.Parse(comboBox1.Text));
        label1.Text = m.ReadString("Game.exe+13E2574", "", 10, zeroTerminated: false);
    }

    private void UpdateUI(int item)
    {
        if (Thread.CurrentThread.IsBackground)
        {
            listBox1.Invoke((Action)delegate
            {
                if (!listBox1.Items.Contains(item))
                {
                    listBox1.Items.Add(item);
                }
            });
        }
        else if (!listBox1.Items.Contains(item))
        {
            listBox1.Items.Add(item);
        }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        for (int i = 10; i <= 90; i += 10)
        {
            comboBoxPercentage.Items.Add(i);
        }
        foreach (KeyValuePair<string, string> item in itemDescriptions2)
        {
            Boxdrop.Items.Add(new ComboBoxItem(item.Value, item.Key));
        }
        comboBoxPercentage.SelectedIndex = 8;
        Task.Factory.StartNew(delegate
        {
            while (true)
            {
                Process[] processesByName = Process.GetProcessesByName("Game");
                for (int j = 0; j < processesByName.Length; j++)
                {
                    UpdateUI(processesByName[j].Id);
                }
                Thread.Sleep(1000);
            }
        });
    }

    private void button4_Click(object sender, EventArgs e)
    {
        DisableButtonAndStartTimer();
    }

    private void comboBoxPercentage_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSelectedPercentage();
    }

    private void UpdateHPMPSP(object sender, EventArgs e)
    {
        int num = m.Read2Byte("Game.exe+1B32160,144");
        int num2 = m.Read2Byte("Game.exe+1B32160,140");
        int num3 = m.Read2Byte("Game.exe+1B32160,14C");
        int num4 = m.Read2Byte("Game.exe+1B32160,154");
        int num5 = m.Read2Byte("Game.exe+1B32160,148");
        int num6 = m.Read2Byte("Game.exe+1B32160,150");
        int threshold = CalculatePercentage(num, selectedPercentage);
        int threshold2 = CalculatePercentage(num3, selectedPercentage);
        int threshold3 = CalculatePercentage(num4, selectedPercentage);
        int value = NormalizeValue(num2, num);
        int value2 = NormalizeValue(num5, num3);
        int value3 = NormalizeValue(num6, num4);
        UpdateProgressBar(progressBarHP, value);
        UpdateProgressBar(progressBarMP, value2);
        UpdateProgressBar(progressBarSP, value3);
        if (typeB.Checked && !typeA.Checked)
        {
            WriteMemoryIfBelowThreshold(num2, threshold, "0x02B04253");
            WriteMemoryIfBelowThreshold(num5, threshold2, "0x02B04254");
            WriteMemoryIfBelowThreshold(num6, threshold3, "0x02B04255");
        }
        else if (typeA.Checked && !typeB.Checked)
        {
            WriteMemoryIfBelowThreshold(num2, threshold, "0x02B0425C");
            WriteMemoryIfBelowThreshold(num5, threshold2, "0x02B0425D");
            WriteMemoryIfBelowThreshold(num6, threshold3, "0x02B0425E");
        }
        else if (!typeA.Checked && !typeB.Checked)
        {
            MessageBox.Show("Please Select Your Type First");
        }
    }

    private void backgroundWorkerHPMPSP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        StopTimerAndEnableButton();
    }

    private void DisableButtonAndStartTimer()
    {
        button4.Enabled = false;
        CreateAndStartTimer();
    }

    private void CreateAndStartTimer()
    {
        timerHPMPSP = new System.Windows.Forms.Timer
        {
            Interval = 10
        };
        timerHPMPSP.Tick += UpdateHPMPSP;
        timerHPMPSP.Start();
    }

    private void UpdateSelectedPercentage()
    {
        selectedPercentage = (int)comboBoxPercentage.SelectedItem;
    }

    private int CalculatePercentage(int value, int percentage)
    {
        return value * percentage / 100;
    }

    private int NormalizeValue(int value, int maxValue)
    {
        return (int)((float)value / (float)maxValue * 100f);
    }

    private void UpdateProgressBar(ProgressBar progressBar, int value)
    {
        progressBar.Invoke((Action)delegate
        {
            progressBar.Value = Math.Max(progressBar.Minimum, Math.Min(value, progressBar.Maximum));
        });
    }

    private void WriteMemoryIfBelowThreshold(int currentValue, int threshold, string address)
    {
        if (currentValue <= threshold)
        {
            m.WriteMemory(address, "byte", "63");
            Thread.Sleep(10);
            m.WriteMemory(address, "byte", "0");
        }
    }

    private void StopTimerAndEnableButton()
    {
        if (timerHPMPSP != null)
        {
            timerHPMPSP.Stop();
            timerHPMPSP.Dispose();
            timerHPMPSP = null;
        }
        button4.Enabled = true;
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
    {
        if (checkBox1.Checked)
        {
            m.WriteMemory("004EABEB", "bytes", "90 90 90");
            m.WriteMemory("004EBAC2", "bytes", "90 00 FD 00");
            m.WriteMemory("00fd0090", "int", "99999");
        }
        else
        {
            m.WriteMemory("004EABEB", "bytes", "D9 45 10");
            m.WriteMemory("00fd0090", "int", "4");
        }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox2.Checked)
        {
            Boxdrop.Enabled = false;
            if (!backgroundWorker2.IsBusy)
            {
                backgroundWorker2.RunWorkerAsync();
            }
        }
        else
        {
            Boxdrop.Enabled = true;
            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
            }
        }
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
    {
        button7.Enabled = true;
        m.UnfreezeValue("Game.exe+10D7640,13c,28");
        m.UnfreezeValue("Game.exe+10D7640,13c,2A");
        m.UnfreezeValue("Game.exe+10D7640,13c,2C");
        m.UnfreezeValue("Game.exe+10D7640,13c,2E");
        m.WriteMemory("Game.exe+10D7640,13c,26", "2bytes", "0");
    }

    private void button7_Click(object sender, EventArgs e)
    {
        button7.Enabled = false;
        int.Parse(((ComboBoxItem)comboBox3.SelectedItem).Value);
        int num = int.Parse(((ComboBoxItem)comboBox2.SelectedItem).Value);
        m.FreezeValue("Game.exe+10D7640,13c,28", "2bytes", "26");
        m.FreezeValue("Game.exe+10D7640,13c,2C", "2bytes", "26");
        m.FreezeValue("Game.exe+10D7640,13c,2A", "2bytes", num.ToString());
        updateWorker.WorkerSupportsCancellation = true;
        updateWorker.DoWork += UpdateWorker_DoWork;
        updateWorker.RunWorkerAsync();
    }

    private void UpdateWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        while (!updateWorker.CancellationPending)
        {
            if (comboBox3.InvokeRequired || comboBox2.InvokeRequired)
            {
                comboBox3.Invoke((Action)delegate
                {
                    int num2 = int.Parse(((ComboBoxItem)comboBox3.SelectedItem).Value);
                    m.WriteMemory("Game.exe+10D7640,13c,26", "2bytes", num2.ToString());
                });
                Thread.Sleep(500);
                comboBox2.Invoke((Action)delegate
                {
                    int num = int.Parse(((ComboBoxItem)comboBox2.SelectedItem).Value);
                    m.WriteMemory("Game.exe+10D7640,13c,2A", "2bytes", num.ToString());
                });
            }
            Thread.Sleep(500);
        }
        button7.Invoke((Action)delegate
        {
            button7.Enabled = true;
        });
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void button6_Click_1(object sender, EventArgs e)
    {
        try
        {
            Process.Start(Application.ExecutablePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to run another instance of the program: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
    {
        while (!backgroundWorker2.CancellationPending)
        {
            if (checkBox2.Checked)
            {
                int num = m.Read2Byte("base+013E255C,9C,6c,10,7C,9c,7c,5c,380");
                int num2 = m.Read2Byte("game.exe+111C230,3b08");
                if (Boxdrop.SelectedItem is ComboBoxItem { Value: var value })
                {
                    if (num2 != int.Parse(value) && num == 0)
                    {
                        m.WriteMemory("02B04285", "byte", "63");
                        Thread.Sleep(200);
                        m.WriteMemory("02B04285", "byte", "01");
                    }
                    if (num2 == int.Parse(value) && num == 0)
                    {
                        m.WriteMemory("02B04264", "byte", "63");
                    }
                    else if (num2 != int.Parse(value) && num2 == 0 && num == 1)
                    {
                        m.WriteMemory("02B04264", "byte", "63");
                    }
                    if (num2 != int.Parse(value) && num == 1)
                    {
                        m.WriteMemory("02B04264", "byte", "63");
                    }
                }
            }
            Thread.Sleep(200);
        }
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5 && !e.Shift)
        {
            if (m.ReadInt("game.exe+2700B90") >= 0)
            {
                m.WriteMemory("game.exe+2700B90", "int", "1");
            }
            else
            {
                m.WriteMemory("game.exe+2700B90", "int", "0");
            }
        }
    }

    private void refreshbut_Click_1(object sender, EventArgs e)
    {
        listBox1.DataSource = null;
        listBox1.Items.Clear();
    }

    private void bypassbut_Click(object sender, EventArgs e)
    {
    }


    private void checkBox3_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox3.Checked)
        {
            if (!autorv.IsBusy)
            {
                autorv.RunWorkerAsync();
            }
        }
        else if (autorv.IsBusy)
        {
            autorv.CancelAsync();
        }
    }

    private void autorv_DoWork(object sender, DoWorkEventArgs e)
    {
        string code = "Game.exe+1118E94,20,10,6C,10,45C,DC,FE0";
        try
        {
            previousMemoryDead = m.ReadInt("game.exe+111C230,471C");
            while (!autorv.CancellationPending)
            {
                double num = m.ReadDouble(code);
                int num2 = m.ReadInt("game.exe+111C230,471C");
                int num3 = m.ReadInt("Game.exe+10D7640,13c,3bc");
                if (num <= 595.0 && num != 0.0 && num != -1.0)
                {
                    m.WriteMemory(code, "double", "-1");
                }
                if (DateTime.Now.Second % 5 == 0)
                {
                    num3 = m.ReadInt("Game.exe+10D7640,13c,3bc");
                }
                if (num2 == previousMemoryDead)
                {
                    continue;
                }
                string logMessage = $"Memory: , Time: {DateTime.Now}";
                listBox2.Invoke((MethodInvoker)delegate
                {
                    listBox2.Items.Add(logMessage);
                });
                previousMemoryDead = num2;
                Thread.Sleep(1000);
                if (num3 != 1)
                {
                    continue;
                }
                if (typeA.Checked)
                {
                    m.WriteMemory("02B0426C", "byte", "63");
                    Thread.Sleep(300);
                    m.WriteMemory("02B0426C", "byte", "00");
                    Thread.Sleep(1000);
                    if (m.ReadInt("Game.exe+10D7640,13c,3bc") == 0)
                    {
                        m.WriteMemory("02B0426C", "byte", "63");
                        Thread.Sleep(300);
                        m.WriteMemory("02B0426C", "byte", "00");
                    }
                }
                else if (typeB.Checked)
                {
                    m.WriteMemory("02B04258", "byte", "63");
                    Thread.Sleep(300);
                    m.WriteMemory("02B04258", "byte", "00");
                    Thread.Sleep(1000);
                    if (m.ReadInt("Game.exe+10D7640,13c,3bc") == 0)
                    {
                        m.WriteMemory("02B04258", "byte", "63");
                        Thread.Sleep(300);
                        m.WriteMemory("02B04258", "byte", "00");
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }

    private void BrowseButton_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            selectedFilePath = openFileDialog.FileName;
            selectedPathTextBox.Text = selectedFilePath;
        }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (modificationThread != null && modificationThread.IsAlive)
        {
            try
            {
                modificationThread.Join();
            }
            catch (ThreadInterruptedException)
            {
            }
        }
        Application.Exit();
        e.Cancel = false;
    }

    private void RunExecutable(string exePath)
    {
        try
        {
            Process.Start(exePath, "/gsrun_run");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error running the executable: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private void Startbut_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            RunExecutable(selectedFilePath);
        }
        else
        {
            MessageBox.Show("Please select an executable file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private void button8_Click(object sender, EventArgs e)
    {
        MessageBox.Show("1.Press 'browse' button then browse to your game folder \n2.Press Start Game Button");
    }

    private void checkBox4_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox4.Checked)
        {
            m.WriteMemory("0076EAB3", "bytes", "31 d0");
        }
        else if (!checkBox4.Checked)
        {
            m.WriteMemory("0076EAB3", "bytes", "33 c0");
        }
    }

    private void typeA_CheckedChanged(object sender, EventArgs e)
    {
        if (typeA.Checked)
        {
            typeB.Enabled = true;
            typeA.Enabled = false;
            typeB.Checked = false;
        }
    }

    private void typeB_CheckedChanged(object sender, EventArgs e)
    {
        if (typeB.Checked)
        {
            typeA.Enabled = true;
            typeB.Enabled = false;
            typeA.Checked = false;
        }
    }

    private void ModifyMemoryButton_Click(object sender, EventArgs e)
    {
        string processName = "game";
        int baseAddress = 0x40169E;
        byte[] bytesToWrite = new byte[4] { 233, 103, 11, 0 };
        modifier = new GameMemoryModifier(processName, baseAddress, bytesToWrite);
        new Thread(modifier.MonitorAndModifyMemory).Start();
        ModifyMemoryButton.Enabled = false;
    }

    private void StopButton_Click(object sender, EventArgs e)
    {
        if (modificationThread != null && modificationThread.IsAlive)
        {
            modificationThread.Join();
        }
        ModifyMemoryButton.Enabled = true;
    }

    private void button5_Click(object sender, EventArgs e)
    {
        float num = m.ReadFloat("game.exe+2701510");
        float num2 = m.ReadFloat("game.exe+2701518");
        int num3 = (int)num;
        int num4 = (int)num2;
        string result = $"{num3} {num4}";
        if (Savepoint.InvokeRequired)
        {
            Savepoint.Invoke((Action)delegate
            {
                Savepoint.Items.Add(result);
            });
        }
        else
        {
            Savepoint.Items.Add(result);
        }
    }

    private void Exfarm_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            while (!Exfarm.CancellationPending)
            {
                DateTime now = DateTime.Now;
                float num = m.ReadFloat("game.exe+2701510");
                float num2 = m.ReadFloat("game.exe+2701518");
                if (typeB.Checked)
                {
                    string selectedPoint = null;
                    Savepoint.Invoke((Action)delegate
                    {
                        selectedPoint = Savepoint.SelectedItem as string;
                    });
                    if (selectedPoint != null)
                    {
                        string[] array = selectedPoint.Split(' ');
                        if (array.Length == 2)
                        {
                            float num3 = float.Parse(array[0]);
                            float num4 = float.Parse(array[1]);
                            int num5 = (int)num;
                            int num6 = (int)num2;
                            while (num5 == (int)num3 && num6 == (int)num4 && !Exfarm.CancellationPending)
                            {
                                m.WriteMemory("02B04256", "byte", "63");
                                Thread.Sleep(500);
                                m.WriteMemory("02B04256", "byte", "00");
                                Thread.Sleep(1000);
                                num = m.ReadFloat("game.exe+2701510");
                                num2 = m.ReadFloat("game.exe+2701518");
                                num5 = (int)num;
                                num6 = (int)num2;
                                if (num5 != (int)num3 || num6 != (int)num4)
                                {
                                    num = m.ReadFloat("game.exe+2701510");
                                    num2 = m.ReadFloat("game.exe+2701518");
                                    m.FreezeValue("01F321A0", "int", "0");
                                    Thread.Sleep(500);
                                }
                                m.WriteMemory("02B0424F", "byte", "63");
                                Thread.Sleep(500);
                                m.WriteMemory("02B0424F", "byte", "00");
                                m.WriteMemory("02B0424E", "byte", "63");
                                Thread.Sleep(500);
                                m.WriteMemory("02B0424E", "byte", "00");
                                Thread.Sleep(1000);
                                num = m.ReadFloat("game.exe+2701510");
                                num2 = m.ReadFloat("game.exe+2701518");
                                num5 = (int)num;
                                num6 = (int)num2;
                            }
                            while ((num5 != (int)num3 || num6 != (int)num4) && !Exfarm.CancellationPending)
                            {
                                m.WriteMemory("02B04257", "byte", "63");
                                Thread.Sleep(500);
                                m.WriteMemory("02B04257", "byte", "00");
                                num = m.ReadFloat("game.exe+2701510");
                                num2 = m.ReadFloat("game.exe+2701518");
                                num5 = (int)num;
                                num6 = (int)num2;
                                if (num5 == (int)num3 && num6 == (int)num4)
                                {
                                    m.FreezeValue("01F321A0", "int", "1");
                                    Thread.Sleep(2000);
                                    num = m.ReadFloat("game.exe+2701510");
                                    num2 = m.ReadFloat("game.exe+2701518");
                                    num5 = (int)num;
                                    num6 = (int)num2;
                                }
                                num = m.ReadFloat("game.exe+2701510");
                                num2 = m.ReadFloat("game.exe+2701518");
                                num5 = (int)num;
                                num6 = (int)num2;
                            }
                        }
                    }
                }
                if (int.TryParse(textBox1.Text, out sleepDuration))
                {
                    int num7 = (int)(DateTime.Now - now).TotalMilliseconds;
                    Thread.Sleep(Math.Max(0, sleepDuration - num7));
                }
            }
        }
        catch (Exception)
        {
        }
    }

    private void checkBox5_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox5.Checked)
        {
            if (!Exfarm.IsBusy)
            {
                Exfarm.RunWorkerAsync();
            }
        }
        else if (Exfarm.IsBusy)
        {
            Exfarm.CancelAsync();
            Thread.Sleep(200);
        }
    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox6.Checked)
        {
            m.WriteMemory("02506B84", "float", "9999999");
        }
        else
        {
            m.WriteMemory("02506B84", "float", "60");
        }
    }

    private void checkBox7_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox7.Checked && textBox2 != null)
        {
            m.FreezeValue("game.exe+111C230,49e0", "float", textBox2.Text);
        }
        else
        {
            m.UnfreezeValue("game.exe+111C230,49e0");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.comboBox1 = new System.Windows.Forms.ComboBox();
        this.button3 = new System.Windows.Forms.Button();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.button4 = new System.Windows.Forms.Button();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        this.comboBox3 = new System.Windows.Forms.ComboBox();
        this.button7 = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.checkBox2 = new System.Windows.Forms.CheckBox();
        this.button6 = new System.Windows.Forms.Button();
        this.progressBarHP = new System.Windows.Forms.ProgressBar();
        this.progressBarMP = new System.Windows.Forms.ProgressBar();
        this.progressBarSP = new System.Windows.Forms.ProgressBar();
        this.backgroundWorkerHPMPSP = new System.ComponentModel.BackgroundWorker();
        this.comboBoxPercentage = new System.Windows.Forms.ComboBox();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.Boxdrop = new System.Windows.Forms.ComboBox();
        this.comboBox2 = new System.Windows.Forms.ComboBox();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.checkBox6 = new System.Windows.Forms.CheckBox();
        this.checkBox4 = new System.Windows.Forms.CheckBox();
        this.checkBox3 = new System.Windows.Forms.CheckBox();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.typeB = new System.Windows.Forms.CheckBox();
        this.typeA = new System.Windows.Forms.CheckBox();
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.groupBox12 = new System.Windows.Forms.GroupBox();
        this.checkBox7 = new System.Windows.Forms.CheckBox();
        this.label2 = new System.Windows.Forms.Label();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.groupBox9 = new System.Windows.Forms.GroupBox();
        this.F3tray = new System.Windows.Forms.CheckBox();
        this.F2tray = new System.Windows.Forms.CheckBox();
        this.groupBox8 = new System.Windows.Forms.GroupBox();
        this.tabPage2 = new System.Windows.Forms.TabPage();
        this.groupBox7 = new System.Windows.Forms.GroupBox();
        this.ModifyMemoryButton = new System.Windows.Forms.Button();
        this.StopButton = new System.Windows.Forms.Button();
        this.groupBox6 = new System.Windows.Forms.GroupBox();
        this.button8 = new System.Windows.Forms.Button();
        this.selectedPathTextBox = new System.Windows.Forms.TextBox();
        this.Startbut = new System.Windows.Forms.Button();
        this.BrowseButton = new System.Windows.Forms.Button();
        this.groupBox5 = new System.Windows.Forms.GroupBox();
        this.listBox1 = new System.Windows.Forms.ListBox();
        this.bypassbut = new System.Windows.Forms.Button();
        this.refreshbut = new System.Windows.Forms.Button();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.groupBox11 = new System.Windows.Forms.GroupBox();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.checkBox5 = new System.Windows.Forms.CheckBox();
        this.button5 = new System.Windows.Forms.Button();
        this.Savepoint = new System.Windows.Forms.ListBox();
        this.groupBox10 = new System.Windows.Forms.GroupBox();
        this.listBox2 = new System.Windows.Forms.ListBox();
        this.autorv = new System.ComponentModel.BackgroundWorker();
        this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        this.Exfarm = new System.ComponentModel.BackgroundWorker();
        this.Autoskillworker = new System.ComponentModel.BackgroundWorker();
        this.groupBox1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.groupBox4.SuspendLayout();
        this.tabControl1.SuspendLayout();
        this.tabPage1.SuspendLayout();
        this.groupBox12.SuspendLayout();
        this.groupBox9.SuspendLayout();
        this.groupBox8.SuspendLayout();
        this.tabPage2.SuspendLayout();
        this.groupBox7.SuspendLayout();
        this.groupBox6.SuspendLayout();
        this.groupBox5.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.groupBox11.SuspendLayout();
        this.groupBox10.SuspendLayout();
        base.SuspendLayout();
        this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.comboBox1.FormattingEnabled = true;
        this.comboBox1.Location = new System.Drawing.Point(8, 39);
        this.comboBox1.Name = "comboBox1";
        this.comboBox1.Size = new System.Drawing.Size(83, 21);
        this.comboBox1.TabIndex = 2;
        this.comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
        this.button3.Location = new System.Drawing.Point(95, 39);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(69, 23);
        this.button3.TabIndex = 3;
        this.button3.Text = "Refresh";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(button3_Click);
        this.button1.Location = new System.Drawing.Point(6, 14);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(71, 23);
        this.button1.TabIndex = 4;
        this.button1.Text = "Start";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(button1_Click);
        this.button2.Location = new System.Drawing.Point(6, 38);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(71, 23);
        this.button2.TabIndex = 5;
        this.button2.Text = "Stop";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(button2_Click);
        this.button4.Location = new System.Drawing.Point(135, 65);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(42, 23);
        this.button4.TabIndex = 6;
        this.button4.Text = "POT";
        this.button4.UseVisualStyleBackColor = true;
        this.button4.Click += new System.EventHandler(button4_Click);
        this.checkBox1.AutoSize = true;
        this.checkBox1.Location = new System.Drawing.Point(4, 18);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(73, 17);
        this.checkBox1.TabIndex = 8;
        this.checkBox1.Text = "Active LR";
        this.checkBox1.UseVisualStyleBackColor = true;
        this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged_1);
        this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.comboBox3.FormattingEnabled = true;
        this.comboBox3.Location = new System.Drawing.Point(6, 19);
        this.comboBox3.Name = "comboBox3";
        this.comboBox3.Size = new System.Drawing.Size(59, 21);
        this.comboBox3.TabIndex = 13;
        this.comboBox3.SelectedIndexChanged += new System.EventHandler(comboBox3_SelectedIndexChanged);
        this.button7.Enabled = false;
        this.button7.Location = new System.Drawing.Point(136, 19);
        this.button7.Name = "button7";
        this.button7.Size = new System.Drawing.Size(59, 21);
        this.button7.TabIndex = 14;
        this.button7.Text = "Execute";
        this.button7.UseCompatibleTextRendering = true;
        this.button7.UseVisualStyleBackColor = true;
        this.button7.Click += new System.EventHandler(button7_Click);
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft PhagsPa", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label1.Location = new System.Drawing.Point(7, 16);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(172, 20);
        this.label1.TabIndex = 15;
        this.label1.Text = "Please select a window.";
        this.label1.Click += new System.EventHandler(label1_Click);
        this.checkBox2.AutoSize = true;
        this.checkBox2.Location = new System.Drawing.Point(6, 46);
        this.checkBox2.Name = "checkBox2";
        this.checkBox2.Size = new System.Drawing.Size(65, 17);
        this.checkBox2.TabIndex = 17;
        this.checkBox2.Text = "Activate";
        this.checkBox2.UseVisualStyleBackColor = true;
        this.checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);
        this.button6.Location = new System.Drawing.Point(6, 249);
        this.button6.Name = "button6";
        this.button6.Size = new System.Drawing.Size(83, 21);
        this.button6.TabIndex = 19;
        this.button6.Text = "More";
        this.button6.UseVisualStyleBackColor = true;
        this.button6.Click += new System.EventHandler(button6_Click_1);
        this.progressBarHP.ForeColor = System.Drawing.Color.Red;
        this.progressBarHP.Location = new System.Drawing.Point(6, 22);
        this.progressBarHP.Name = "progressBarHP";
        this.progressBarHP.Size = new System.Drawing.Size(123, 18);
        this.progressBarHP.TabIndex = 22;
        this.progressBarMP.Location = new System.Drawing.Point(6, 46);
        this.progressBarMP.Name = "progressBarMP";
        this.progressBarMP.Size = new System.Drawing.Size(123, 18);
        this.progressBarMP.TabIndex = 23;
        this.progressBarSP.Location = new System.Drawing.Point(6, 70);
        this.progressBarSP.Name = "progressBarSP";
        this.progressBarSP.Size = new System.Drawing.Size(123, 18);
        this.progressBarSP.TabIndex = 24;
        this.comboBoxPercentage.FormattingEnabled = true;
        this.comboBoxPercentage.Location = new System.Drawing.Point(135, 43);
        this.comboBoxPercentage.Name = "comboBoxPercentage";
        this.comboBoxPercentage.Size = new System.Drawing.Size(42, 21);
        this.comboBoxPercentage.TabIndex = 25;
        this.comboBoxPercentage.SelectedIndexChanged += new System.EventHandler(comboBoxPercentage_SelectedIndexChanged);
        this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker2_DoWork);
        this.groupBox1.Controls.Add(this.Boxdrop);
        this.groupBox1.Controls.Add(this.checkBox2);
        this.groupBox1.Location = new System.Drawing.Point(6, 173);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(83, 73);
        this.groupBox1.TabIndex = 26;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Box Section";
        this.Boxdrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.Boxdrop.FormattingEnabled = true;
        this.Boxdrop.Location = new System.Drawing.Point(6, 19);
        this.Boxdrop.Name = "Boxdrop";
        this.Boxdrop.Size = new System.Drawing.Size(67, 21);
        this.Boxdrop.TabIndex = 27;
        this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.comboBox2.FormattingEnabled = true;
        this.comboBox2.Location = new System.Drawing.Point(71, 19);
        this.comboBox2.Name = "comboBox2";
        this.comboBox2.Size = new System.Drawing.Size(59, 21);
        this.comboBox2.TabIndex = 20;
        this.groupBox2.Controls.Add(this.comboBox3);
        this.groupBox2.Controls.Add(this.comboBox2);
        this.groupBox2.Controls.Add(this.button7);
        this.groupBox2.Location = new System.Drawing.Point(94, 168);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(206, 53);
        this.groupBox2.TabIndex = 27;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Pet Skills";
        this.groupBox3.Controls.Add(this.checkBox6);
        this.groupBox3.Controls.Add(this.checkBox4);
        this.groupBox3.Controls.Add(this.checkBox3);
        this.groupBox3.Controls.Add(this.checkBox1);
        this.groupBox3.Location = new System.Drawing.Point(211, 75);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(89, 92);
        this.groupBox3.TabIndex = 28;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Misc";
        this.checkBox6.AutoSize = true;
        this.checkBox6.Location = new System.Drawing.Point(4, 69);
        this.checkBox6.Name = "checkBox6";
        this.checkBox6.Size = new System.Drawing.Size(89, 17);
        this.checkBox6.TabIndex = 22;
        this.checkBox6.Text = "Anti Kick TW";
        this.checkBox6.UseVisualStyleBackColor = true;
        this.checkBox6.CheckedChanged += new System.EventHandler(checkBox6_CheckedChanged);
        this.checkBox4.AutoSize = true;
        this.checkBox4.Location = new System.Drawing.Point(4, 34);
        this.checkBox4.Name = "checkBox4";
        this.checkBox4.Size = new System.Drawing.Size(81, 17);
        this.checkBox4.TabIndex = 21;
        this.checkBox4.Text = "Active AOE";
        this.checkBox4.UseVisualStyleBackColor = true;
        this.checkBox4.CheckedChanged += new System.EventHandler(checkBox4_CheckedChanged);
        this.checkBox3.AutoSize = true;
        this.checkBox3.Location = new System.Drawing.Point(4, 51);
        this.checkBox3.Name = "checkBox3";
        this.checkBox3.Size = new System.Drawing.Size(85, 17);
        this.checkBox3.TabIndex = 20;
        this.checkBox3.Text = "Auto Revive";
        this.checkBox3.UseVisualStyleBackColor = true;
        this.checkBox3.CheckedChanged += new System.EventHandler(checkBox3_CheckedChanged);
        this.groupBox4.Controls.Add(this.typeB);
        this.groupBox4.Controls.Add(this.typeA);
        this.groupBox4.Controls.Add(this.progressBarHP);
        this.groupBox4.Controls.Add(this.progressBarMP);
        this.groupBox4.Controls.Add(this.progressBarSP);
        this.groupBox4.Controls.Add(this.comboBoxPercentage);
        this.groupBox4.Controls.Add(this.button4);
        this.groupBox4.Location = new System.Drawing.Point(6, 74);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.Size = new System.Drawing.Size(199, 93);
        this.groupBox4.TabIndex = 29;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "Auto Pot";
        this.typeB.AutoSize = true;
        this.typeB.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.typeB.Checked = true;
        this.typeB.CheckState = System.Windows.Forms.CheckState.Checked;
        this.typeB.Enabled = false;
        this.typeB.Location = new System.Drawing.Point(159, 9);
        this.typeB.Name = "typeB";
        this.typeB.Size = new System.Drawing.Size(18, 31);
        this.typeB.TabIndex = 26;
        this.typeB.Text = "B";
        this.typeB.UseVisualStyleBackColor = true;
        this.typeB.CheckedChanged += new System.EventHandler(typeB_CheckedChanged);
        this.typeA.AutoSize = true;
        this.typeA.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.typeA.Location = new System.Drawing.Point(135, 9);
        this.typeA.Name = "typeA";
        this.typeA.Size = new System.Drawing.Size(18, 31);
        this.typeA.TabIndex = 22;
        this.typeA.Text = "A";
        this.typeA.UseVisualStyleBackColor = true;
        this.typeA.CheckedChanged += new System.EventHandler(typeA_CheckedChanged);
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Controls.Add(this.tabPage2);
        this.tabControl1.Controls.Add(this.tabPage3);
        this.tabControl1.Location = new System.Drawing.Point(3, 1);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(349, 301);
        this.tabControl1.TabIndex = 30;
        this.tabPage1.Controls.Add(this.groupBox12);
        this.tabPage1.Controls.Add(this.groupBox9);
        this.tabPage1.Controls.Add(this.groupBox8);
        this.tabPage1.Controls.Add(this.groupBox4);
        this.tabPage1.Controls.Add(this.button6);
        this.tabPage1.Controls.Add(this.groupBox3);
        this.tabPage1.Controls.Add(this.groupBox2);
        this.tabPage1.Controls.Add(this.groupBox1);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(341, 275);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "Main";
        this.tabPage1.UseVisualStyleBackColor = true;
        this.groupBox12.Controls.Add(this.checkBox7);
        this.groupBox12.Controls.Add(this.label2);
        this.groupBox12.Controls.Add(this.textBox2);
        this.groupBox12.Location = new System.Drawing.Point(94, 225);
        this.groupBox12.Name = "groupBox12";
        this.groupBox12.Size = new System.Drawing.Size(206, 44);
        this.groupBox12.TabIndex = 32;
        this.groupBox12.TabStop = false;
        this.groupBox12.Text = "Adjustment";
        this.checkBox7.AutoSize = true;
        this.checkBox7.Location = new System.Drawing.Point(150, 19);
        this.checkBox7.Name = "checkBox7";
        this.checkBox7.Size = new System.Drawing.Size(56, 17);
        this.checkBox7.TabIndex = 2;
        this.checkBox7.Text = "Active";
        this.checkBox7.UseVisualStyleBackColor = true;
        this.checkBox7.CheckedChanged += new System.EventHandler(checkBox7_CheckedChanged);
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(6, 21);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(72, 13);
        this.label2.TabIndex = 1;
        this.label2.Text = "Attack Speed";
        this.textBox2.Location = new System.Drawing.Point(80, 18);
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(64, 20);
        this.textBox2.TabIndex = 0;
        this.groupBox9.Controls.Add(this.F3tray);
        this.groupBox9.Controls.Add(this.F2tray);
        this.groupBox9.Controls.Add(this.button1);
        this.groupBox9.Controls.Add(this.button2);
        this.groupBox9.Location = new System.Drawing.Point(211, 6);
        this.groupBox9.Name = "groupBox9";
        this.groupBox9.Size = new System.Drawing.Size(124, 68);
        this.groupBox9.TabIndex = 31;
        this.groupBox9.TabStop = false;
        this.groupBox9.Text = "Auto Skill";
        this.F3tray.AutoSize = true;
        this.F3tray.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.F3tray.Location = new System.Drawing.Point(95, 13);
        this.F3tray.Name = "F3tray";
        this.F3tray.Size = new System.Drawing.Size(23, 31);
        this.F3tray.TabIndex = 24;
        this.F3tray.Text = "F3";
        this.F3tray.UseVisualStyleBackColor = true;
        this.F2tray.AutoSize = true;
        this.F2tray.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
        this.F2tray.Location = new System.Drawing.Point(77, 13);
        this.F2tray.Name = "F2tray";
        this.F2tray.Size = new System.Drawing.Size(23, 31);
        this.F2tray.TabIndex = 23;
        this.F2tray.Text = "F2";
        this.F2tray.UseVisualStyleBackColor = true;
        this.groupBox8.Controls.Add(this.label1);
        this.groupBox8.Controls.Add(this.comboBox1);
        this.groupBox8.Controls.Add(this.button3);
        this.groupBox8.Location = new System.Drawing.Point(6, 6);
        this.groupBox8.Name = "groupBox8";
        this.groupBox8.Size = new System.Drawing.Size(199, 68);
        this.groupBox8.TabIndex = 30;
        this.groupBox8.TabStop = false;
        this.groupBox8.Text = "Selection";
        this.tabPage2.Controls.Add(this.groupBox7);
        this.tabPage2.Controls.Add(this.groupBox6);
        this.tabPage2.Controls.Add(this.groupBox5);
        this.tabPage2.Location = new System.Drawing.Point(4, 22);
        this.tabPage2.Name = "tabPage2";
        this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage2.Size = new System.Drawing.Size(341, 275);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text = "Bypass";
        this.tabPage2.UseVisualStyleBackColor = true;
        this.groupBox7.Controls.Add(this.ModifyMemoryButton);
        this.groupBox7.Controls.Add(this.StopButton);
        this.groupBox7.Location = new System.Drawing.Point(204, 96);
        this.groupBox7.Name = "groupBox7";
        this.groupBox7.Size = new System.Drawing.Size(96, 100);
        this.groupBox7.TabIndex = 34;
        this.groupBox7.TabStop = false;
        this.groupBox7.Text = "Bypass Limit";
        this.ModifyMemoryButton.Location = new System.Drawing.Point(9, 19);
        this.ModifyMemoryButton.Name = "ModifyMemoryButton";
        this.ModifyMemoryButton.Size = new System.Drawing.Size(75, 27);
        this.ModifyMemoryButton.TabIndex = 32;
        this.ModifyMemoryButton.Text = "Bypass Limit";
        this.ModifyMemoryButton.UseVisualStyleBackColor = true;
        this.ModifyMemoryButton.Click += new System.EventHandler(ModifyMemoryButton_Click);
        this.StopButton.Location = new System.Drawing.Point(9, 52);
        this.StopButton.Name = "StopButton";
        this.StopButton.Size = new System.Drawing.Size(75, 27);
        this.StopButton.TabIndex = 33;
        this.StopButton.Text = "Stop Bypass";
        this.StopButton.UseVisualStyleBackColor = true;
        this.StopButton.Click += new System.EventHandler(StopButton_Click);
        this.groupBox6.Controls.Add(this.button8);
        this.groupBox6.Controls.Add(this.selectedPathTextBox);
        this.groupBox6.Controls.Add(this.Startbut);
        this.groupBox6.Controls.Add(this.BrowseButton);
        this.groupBox6.Location = new System.Drawing.Point(9, 96);
        this.groupBox6.Name = "groupBox6";
        this.groupBox6.Size = new System.Drawing.Size(189, 100);
        this.groupBox6.TabIndex = 6;
        this.groupBox6.TabStop = false;
        this.groupBox6.Text = "Game Launch";
        this.button8.Location = new System.Drawing.Point(96, 46);
        this.button8.Name = "button8";
        this.button8.Size = new System.Drawing.Size(84, 23);
        this.button8.TabIndex = 6;
        this.button8.Text = "Read Me";
        this.button8.UseVisualStyleBackColor = true;
        this.button8.Click += new System.EventHandler(button8_Click);
        this.selectedPathTextBox.Enabled = false;
        this.selectedPathTextBox.Location = new System.Drawing.Point(6, 20);
        this.selectedPathTextBox.Name = "selectedPathTextBox";
        this.selectedPathTextBox.Size = new System.Drawing.Size(174, 20);
        this.selectedPathTextBox.TabIndex = 4;
        this.Startbut.Location = new System.Drawing.Point(6, 71);
        this.Startbut.Name = "Startbut";
        this.Startbut.Size = new System.Drawing.Size(84, 23);
        this.Startbut.TabIndex = 5;
        this.Startbut.Text = "Start Game";
        this.Startbut.UseVisualStyleBackColor = true;
        this.Startbut.Click += new System.EventHandler(Startbut_Click);
        this.BrowseButton.Location = new System.Drawing.Point(6, 46);
        this.BrowseButton.Name = "BrowseButton";
        this.BrowseButton.Size = new System.Drawing.Size(84, 23);
        this.BrowseButton.TabIndex = 3;
        this.BrowseButton.Text = "Browse";
        this.BrowseButton.UseVisualStyleBackColor = true;
        this.BrowseButton.Click += new System.EventHandler(BrowseButton_Click);
        this.groupBox5.Controls.Add(this.listBox1);
        this.groupBox5.Controls.Add(this.bypassbut);
        this.groupBox5.Controls.Add(this.refreshbut);
        this.groupBox5.Location = new System.Drawing.Point(9, 6);
        this.groupBox5.Name = "groupBox5";
        this.groupBox5.Size = new System.Drawing.Size(258, 84);
        this.groupBox5.TabIndex = 3;
        this.groupBox5.TabStop = false;
        this.groupBox5.Text = "PID Bypass";
        this.listBox1.FormattingEnabled = true;
        this.listBox1.Location = new System.Drawing.Point(6, 19);
        this.listBox1.Name = "listBox1";
        this.listBox1.Size = new System.Drawing.Size(165, 56);
        this.listBox1.TabIndex = 0;
        this.bypassbut.Location = new System.Drawing.Point(177, 52);
        this.bypassbut.Name = "bypassbut";
        this.bypassbut.Size = new System.Drawing.Size(75, 23);
        this.bypassbut.TabIndex = 2;
        this.bypassbut.Text = "Bypass";
        this.bypassbut.UseVisualStyleBackColor = true;
        this.bypassbut.Click += new System.EventHandler(bypassbut_Click);
        this.refreshbut.Location = new System.Drawing.Point(177, 19);
        this.refreshbut.Name = "refreshbut";
        this.refreshbut.Size = new System.Drawing.Size(75, 23);
        this.refreshbut.TabIndex = 1;
        this.refreshbut.Text = "Refresh";
        this.refreshbut.UseVisualStyleBackColor = true;
        this.refreshbut.Click += new System.EventHandler(refreshbut_Click_1);
        this.tabPage3.Controls.Add(this.groupBox11);
        this.tabPage3.Controls.Add(this.groupBox10);
        this.tabPage3.Location = new System.Drawing.Point(4, 22);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Size = new System.Drawing.Size(341, 275);
        this.tabPage3.TabIndex = 2;
        this.tabPage3.Text = "Dead Log";
        this.tabPage3.UseVisualStyleBackColor = true;
        this.groupBox11.Controls.Add(this.textBox1);
        this.groupBox11.Controls.Add(this.checkBox5);
        this.groupBox11.Controls.Add(this.button5);
        this.groupBox11.Controls.Add(this.Savepoint);
        this.groupBox11.Location = new System.Drawing.Point(4, 139);
        this.groupBox11.Name = "groupBox11";
        this.groupBox11.Size = new System.Drawing.Size(228, 93);
        this.groupBox11.TabIndex = 2;
        this.groupBox11.TabStop = false;
        this.groupBox11.Text = "Save Point";
        this.groupBox11.Visible = false;
        this.textBox1.Location = new System.Drawing.Point(174, 19);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(48, 20);
        this.textBox1.TabIndex = 6;
        this.checkBox5.AutoSize = true;
        this.checkBox5.Location = new System.Drawing.Point(105, 48);
        this.checkBox5.Name = "checkBox5";
        this.checkBox5.Size = new System.Drawing.Size(56, 17);
        this.checkBox5.TabIndex = 5;
        this.checkBox5.Text = "Active";
        this.checkBox5.UseVisualStyleBackColor = true;
        this.checkBox5.CheckedChanged += new System.EventHandler(checkBox5_CheckedChanged);
        this.button5.Location = new System.Drawing.Point(105, 19);
        this.button5.Name = "button5";
        this.button5.Size = new System.Drawing.Size(63, 23);
        this.button5.TabIndex = 4;
        this.button5.Text = "Get point";
        this.button5.UseVisualStyleBackColor = true;
        this.button5.Click += new System.EventHandler(button5_Click);
        this.Savepoint.FormattingEnabled = true;
        this.Savepoint.Location = new System.Drawing.Point(6, 19);
        this.Savepoint.Name = "Savepoint";
        this.Savepoint.Size = new System.Drawing.Size(93, 56);
        this.Savepoint.TabIndex = 0;
        this.groupBox10.Controls.Add(this.listBox2);
        this.groupBox10.Location = new System.Drawing.Point(4, 3);
        this.groupBox10.Name = "groupBox10";
        this.groupBox10.Size = new System.Drawing.Size(297, 105);
        this.groupBox10.TabIndex = 1;
        this.groupBox10.TabStop = false;
        this.groupBox10.Text = "groupBox10";
        this.listBox2.FormattingEnabled = true;
        this.listBox2.Location = new System.Drawing.Point(6, 19);
        this.listBox2.Name = "listBox2";
        this.listBox2.Size = new System.Drawing.Size(285, 69);
        this.listBox2.TabIndex = 0;
        this.autorv.DoWork += new System.ComponentModel.DoWorkEventHandler(autorv_DoWork);
        this.Exfarm.DoWork += new System.ComponentModel.DoWorkEventHandler(Exfarm_DoWork);
        this.Autoskillworker.DoWork += new System.ComponentModel.DoWorkEventHandler(Autoskillworker_DoWork);
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(354, 305);
        base.Controls.Add(this.tabControl1);
        base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        base.KeyPreview = true;
        base.MaximizeBox = false;
        base.Name = "Form1";
        base.ShowIcon = false;
        this.Text = "SMT-Botting Ran Online";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
        base.Load += new System.EventHandler(Form1_Load);
        base.KeyDown += new System.Windows.Forms.KeyEventHandler(Form1_KeyDown);
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox3.ResumeLayout(false);
        this.groupBox3.PerformLayout();
        this.groupBox4.ResumeLayout(false);
        this.groupBox4.PerformLayout();
        this.tabControl1.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.groupBox12.ResumeLayout(false);
        this.groupBox12.PerformLayout();
        this.groupBox9.ResumeLayout(false);
        this.groupBox9.PerformLayout();
        this.groupBox8.ResumeLayout(false);
        this.groupBox8.PerformLayout();
        this.tabPage2.ResumeLayout(false);
        this.groupBox7.ResumeLayout(false);
        this.groupBox6.ResumeLayout(false);
        this.groupBox6.PerformLayout();
        this.groupBox5.ResumeLayout(false);
        this.tabPage3.ResumeLayout(false);
        this.groupBox11.ResumeLayout(false);
        this.groupBox11.PerformLayout();
        this.groupBox10.ResumeLayout(false);
        base.ResumeLayout(false);
    }
}
