//By SkyStudio.
//25.4.12
//for LanpinPE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanpinPEISO
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

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "数据无价！ 数据无价！ 数据无价！ \n 请确保您的USB设备内的文件已备份 如已备份 请点击确定按钮 否则请关闭此窗口";
            string title = "警告";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string message1 = "数据无价！ 数据无价！ 数据无价！\n 清除文件后本软件的制作方将不负任何责任 请再次确认！！！";
                string title1 = "警告";
                MessageBoxButtons buttons1 = MessageBoxButtons.YesNo;
                DialogResult result1 = MessageBox.Show(message1, title1, buttons1, MessageBoxIcon.Warning);

                if (result1 == DialogResult.Yes)
                {
                    // 获取当前选择的盘符（假设 panfu 是 ComboBox）
                    string selectedDrive = panfu.SelectedItem?.ToString();

                    if (!string.IsNullOrEmpty(selectedDrive))
                    {
                        // 获取当前应用程序所在目录
                        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string ventoyDirectory = Path.Combine(appDirectory, "ventoy");
                        string panfuFilePath = Path.Combine(ventoyDirectory, "panfu.txt");

                        try
                        {
                            // 确保 ventoy 文件夹存在
                            if (!Directory.Exists(ventoyDirectory))
                            {
                                Directory.CreateDirectory(ventoyDirectory);
                            }

                            // 将盘符写入 panfu.txt
                            File.WriteAllText(panfuFilePath, selectedDrive);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"写入盘符失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // 如果写入失败，不再继续
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先选择盘符！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 如果没有选择盘符，不再继续
                    }

                    // 写入成功后再打开 Form2
                    Form2 form2 = new Form2();
                    form2.ShowDialog();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUSBDriveLetters();
        }


        private void LoadUSBDriveLetters()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                // 将所有盘符添加到ComboBox中
                panfu.Items.Add(drive.Name);
            }
        }

        private void panfu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
