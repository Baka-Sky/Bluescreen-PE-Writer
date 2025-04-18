using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanpinPEISO
{
    public partial class Form2 : Form
    {
        private Process cmdProcess;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string batFilePath = System.IO.Path.Combine(Application.StartupPath, "ventoy", "run.bat");
            string workingDirectory = System.IO.Path.GetDirectoryName(batFilePath);
            ExecuteCommand(batFilePath, workingDirectory);
        }

        private void ExecuteCommand(string command, string workingDirectory)
        {
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.Arguments = "/C " + command; // /C 表示执行完命令后终止CMD
            cmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向输出流
            cmdProcess.StartInfo.UseShellExecute = false;      // 不使用操作系统外壳启动进程
            cmdProcess.StartInfo.CreateNoWindow = true;       // 不创建窗口
            cmdProcess.StartInfo.WorkingDirectory = workingDirectory; // 设置工作目录
            cmdProcess.StartInfo.Verb = "runas"; // 以管理员权限运行

            cmdProcess.OutputDataReceived += CmdProcess_OutputDataReceived; // 添加事件处理器
            cmdProcess.Start(); // 启动进程
            cmdProcess.BeginOutputReadLine(); // 开始异步读取输出
        }
        private void CmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                AppendTextToTextBox(e.Data);
            }
        }
        private void AppendTextToTextBox(string text)
        {
            if (textBoxOutput.InvokeRequired)
            {
                textBoxOutput.Invoke(new Action<string>(AppendTextToTextBox), text);
                return;
            }

            textBoxOutput.AppendText(text + Environment.NewLine);
            textBoxOutput.ScrollToCaret(); // 滚动到文本框底部以查看最新输出
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (cmdProcess != null && !cmdProcess.HasExited)
            {
                var result = MessageBox.Show("正在执行CMD命令，确认要关闭吗？", "确认", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        cmdProcess.Kill();  // 终止进程
                        cmdProcess.Dispose(); // 释放资源
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"无法终止进程: {ex.Message}");
                    }
                }
                else
                {
                    e.Cancel = true; // 取消关闭操作
                }
            }
        }
    }
}

