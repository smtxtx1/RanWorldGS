// PSGG.64 Exfarm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// ranall.Login
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using KeyAuth;


public class Login : Form
{
    public static api KeyAuthApp = new api("smbotenglish", "zbtz7lqHV9", "bc923a5105f16fd9b12ae6a1ceee8660f1116f8af26061a1b75e9dae2c590e22", "1.4.6"); private IContainer components;

    private TextBox key;

    private Button button1;

    public Login()
    {
        KeyAuthApp.init();
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        KeyAuthApp.license(key.Text);
        if (KeyAuthApp.response.success)
        {
            SaveKeyToFile(key.Text);
            Form1 form = new Form1();
            Hide();
            form.Show();
            foreach (Control control in base.Controls)
            {
                control.Enabled = true;
            }
            MessageBox.Show("Date Expire: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.subscriptions[0].expiry)).ToString());
        }
        else
        {
            MessageBox.Show("Invalid Key!!");
        }
    }

    private void SaveKeyToFile(string key)
    {
        string path = "key.txt";
        try
        {
            File.WriteAllText(path, key);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error saving the key: " + ex.Message);
        }
    }

    public string expirydaysleft()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(long.Parse(KeyAuthApp.user_data.subscriptions[0].expiry)).ToLocalTime();
        TimeSpan timeSpan = dateTime - DateTime.Now;
        return Convert.ToString(timeSpan.Days + " Days " + timeSpan.Hours + " Hours Left");
    }

    public DateTime UnixTimeToDateTime(long unixtime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
        try
        {
            return dateTime.AddSeconds(unixtime).ToLocalTime();
        }
        catch
        {
            return DateTime.MaxValue;
        }
    }

    private void Login_Load(object sender, EventArgs e)
    {
        string value = ReadKeyFromFile();
        if (!string.IsNullOrEmpty(value))
        {
            key.Text = value;
        }
    }

    private string ReadKeyFromFile()
    {
        string path = "key.txt";
        if (File.Exists(path))
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading the key: " + ex.Message);
            }
        }
        return null;
    }

    private void Login_FormClosing(object sender, FormClosingEventArgs e)
    {
        Application.Exit();
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
        this.key = new System.Windows.Forms.TextBox();
        this.button1 = new System.Windows.Forms.Button();
        base.SuspendLayout();
        this.key.Location = new System.Drawing.Point(12, 12);
        this.key.Name = "key";
        this.key.Size = new System.Drawing.Size(205, 20);
        this.key.TabIndex = 0;
        this.button1.Location = new System.Drawing.Point(234, 12);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(75, 21);
        this.button1.TabIndex = 1;
        this.button1.Text = "Login";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(button1_Click);
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(324, 50);
        base.Controls.Add(this.button1);
        base.Controls.Add(this.key);
        base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
        base.MaximizeBox = false;
        base.Name = "Login";
        this.Text = "Insert Your Key";
        base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Login_FormClosing);
        base.Load += new System.EventHandler(Login_Load);
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}
