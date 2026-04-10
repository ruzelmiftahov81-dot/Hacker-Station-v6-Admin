using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Principal;

public class UltimateAdminHub : Form {
    TextBox edit;
    Point lastPoint; // Для движения окна
    string logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HACK_BASE", "log.txt");

    [STAThread] static void Main() {
        if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) {
            Process.Start(new ProcessStartInfo { FileName = Application.ExecutablePath, Verb = "runas", UseShellExecute = true });
            return;
        }
        Application.Run(new UltimateAdminHub());
    }

    public UltimateAdminHub() {
        if (!Directory.Exists(Path.GetDirectoryName(logFile))) Directory.CreateDirectory(Path.GetDirectoryName(logFile));
        this.BackColor = Color.Black; this.Size = new Size(1100, 750);
        this.StartPosition = FormStartPosition.CenterScreen; this.FormBorderStyle = FormBorderStyle.None;

        // ВЕРХНЯЯ ПАНЕЛЬ (Теперь за неё можно двигать!)
        Panel top = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(40, 40, 40) };
        top.MouseDown += (s, e) => lastPoint = new Point(e.X, e.Y);
        top.MouseMove += (s, e) => {
            if (e.Button == MouseButtons.Left) {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        };

        // КНОПКИ УПРАВЛЕНИЯ
        Button btnX = new Button { Text = "X", Dock = DockStyle.Right, Width = 45, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
        btnX.Click += (s, e) => Application.Exit();

        Button btnMax = new Button { Text = "□", Dock = DockStyle.Right, Width = 45, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
        btnMax.Click += (s, e) => this.WindowState = (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;

        Button btnMin = new Button { Text = "_", Dock = DockStyle.Right, Width = 45, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
        btnMin.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

        top.Controls.Add(btnMin); top.Controls.Add(btnMax); top.Controls.Add(btnX);

        MenuStrip ms = new MenuStrip { BackColor = Color.FromArgb(20, 20, 20), ForeColor = Color.Lime };
        ToolStripMenuItem fileMenu = new ToolStripMenuItem("ФАЙЛ");
        fileMenu.DropDownItems.Add("ОЧИСТИТЬ", null, (s, e) => edit.Clear());

        ToolStripMenuItem runMenu = new ToolStripMenuItem("ЗАПУСК (ROOT)");
        runMenu.DropDownItems.Add("CMD", null, (s, e) => Process.Start("cmd.exe"));
        runMenu.DropDownItems.Add("РЕЕСТР", null, (s, e) => Process.Start("regedit.exe"));
        runMenu.DropDownItems.Add("ДИСПЕТЧЕР", null, (s, e) => Process.Start("taskmgr.exe"));

        ms.Items.Add(fileMenu); ms.Items.Add(runMenu);
        ms.Items.Add(new ToolStripMenuItem("БАЗА", null, (s, e) => Process.Start("explorer.exe", Path.GetDirectoryName(logFile))));

        edit = new TextBox { Multiline = true, Dock = DockStyle.Fill, BackColor = Color.Black, ForeColor = Color.Lime, BorderStyle = BorderStyle.None, Font = new Font("Consolas", 14), ScrollBars = ScrollBars.Vertical };
        edit.AppendText("> SYSTEM ACTIVE v6.0 ADMIN\r\n> READY FOR PRIVILEGED COMMANDS\r\n> ");

        this.Controls.Add(edit); this.Controls.Add(ms); this.Controls.Add(top);
    }
}
