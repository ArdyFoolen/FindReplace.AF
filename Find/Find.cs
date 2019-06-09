using FindReplace;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Find
{
    public partial class Find : Form
    {
        CancellationTokenSource cancellationTokenSource;
        Task task;
        bool canceling { get; set; }

        public Find()
        {
            InitializeComponent();
            folderBrowser.Description = "Select the folder that you want to use";
            folderBrowser.ShowNewFolderButton = false;
            folderBrowser.SelectedPath = string.IsNullOrWhiteSpace(Settings.Default.RecentFolder) ? @"C:\" : Settings.Default.RecentFolder;
            txtPath.Text = folderBrowser.SelectedPath;

            if (IsAdministrator())
                this.Text += " (Administrator)";
        }

        private bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowser.SelectedPath;
                Settings.Default.RecentFolder = folderBrowser.SelectedPath;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFind.Text))
            {
                MessageBox.Show("Cannot find: Find text is empty", "Find", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Cannot find: Path is not an existing Directory", "Find", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            this.btnCancel.Visible = true;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.Register(this.canceled);
            this.canceling = false;

            FileActions actions = new FileActions();
            actions.RootDirectoryPath = txtPath.Text;
            actions.FilePattern = txtPattern.Text;
            actions.IncludingSubdirectories = chkInclude.Checked;
            actions.CaseSensitive = chkCase.Checked;
            actions.OnSkipEvent += this.skipped;
            actions.CancellationToken = cancellationTokenSource.Token;

            txtResult.Text = "";
            ToggleEnable();

            task = Task.Factory.StartNew(async () => setResultFind(await actions.FindAsync(txtFind.Text), actions), cancellationTokenSource.Token);
        }

        private void ToggleEnable()
        {
            InvokeControl(txtFind, (c) => c.Enabled = !c.Enabled);
            InvokeControl(txtReplace, (c) => c.Enabled = !c.Enabled);
            InvokeControl(txtPath, (c) => c.Enabled = !c.Enabled);
            InvokeControl(btnPath, (c) => c.Enabled = !c.Enabled);
            InvokeControl(txtPattern, (c) => c.Enabled = !c.Enabled);
            InvokeControl(chkInclude, (c) => c.Enabled = !c.Enabled);
            InvokeControl(chkCase, (c) => c.Enabled = !c.Enabled);
            InvokeControl(btnFind, (c) => c.Enabled = !c.Enabled);
            InvokeControl(btnReplace, (c) => c.Enabled = !c.Enabled);
        }

        private void setResultFind(List<string> result, FileActions actions)
        {
            if (!canceling)
            {
                if (string.IsNullOrWhiteSpace(txtResult.Text))
                    InvokeControl(txtResult, (c) => c.Text = $"Find {txtFind.Text} in path {actions.RootDirectoryPath}{Environment.NewLine}");
                else
                    InvokeControl(txtResult, (c) => c.Text = $"{c.Text}{Environment.NewLine}{Environment.NewLine}Find {txtFind.Text} in path {actions.RootDirectoryPath}{Environment.NewLine}");
                InvokeControl(txtResult, (c) => c.Text = $"{c.Text}{Environment.NewLine}{string.Join(Environment.NewLine, result)}");
            }
            ToggleEnable();
            InvokeControl(btnCancel, (c) => c.Visible = false);
        }

        private void setResultReplace(List<string> result, FileActions actions)
        {
            if (!canceling)
            {
                if (string.IsNullOrWhiteSpace(txtResult.Text))
                    InvokeControl(txtResult, (c) => c.Text = $"Replace {txtFind.Text} with {txtReplace.Text} in path {actions.RootDirectoryPath}{Environment.NewLine}");
                else
                    InvokeControl(txtResult, (c) => c.Text = $"{c.Text}{Environment.NewLine}{Environment.NewLine}Replace {txtFind.Text} with {txtReplace.Text} in path {actions.RootDirectoryPath}{Environment.NewLine}");
                InvokeControl(txtResult, (c) => c.Text = $"{c.Text}{Environment.NewLine}{string.Join(Environment.NewLine, result)}");
            }
            ToggleEnable();
            InvokeControl(btnCancel, (c) => c.Visible = false);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFind.Text))
            {
                MessageBox.Show("Cannot replace: Find text is empty", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtReplace.Text))
            {
                MessageBox.Show("Cannot replace: Replace text is empty", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Cannot replace: Path is not an existing Directory", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            this.btnCancel.Visible = true;
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.Register(this.canceled);
            this.canceling = false;

            FileActions actions = new FileActions();
            actions.RootDirectoryPath = txtPath.Text;
            actions.FilePattern = txtPattern.Text;
            actions.IncludingSubdirectories = chkInclude.Checked;
            actions.CaseSensitive = chkCase.Checked;
            actions.OnSkipEvent += this.skipped;
            actions.CancellationToken = cancellationTokenSource.Token;

            txtResult.Text = "";
            ToggleEnable();
            task = Task.Factory.StartNew(async () => setResultReplace(await actions.ReplaceAsync(txtFind.Text, txtReplace.Text), actions), cancellationTokenSource.Token);
        }

        private void skipped(string file)
        {
            if (!canceling)
            {
                if (string.IsNullOrWhiteSpace(txtResult.Text))
                    InvokeControl(txtResult, (c) => c.Text = $"{file} skipped");
                else
                    InvokeControl(txtResult, (c) => c.Text = $"{txtResult.Text}{Environment.NewLine}{file} skipped");
            }
        }

        private void canceled()
        {
            this.canceling = true;
            cancellationTokenSource.Dispose();
            ToggleEnable();
            if (string.IsNullOrWhiteSpace(txtResult.Text))
                InvokeControl(txtResult, (c) => c.Text = $"{Environment.NewLine}Find & Replace Task has been Canceled!");
            else
                InvokeControl(txtResult, (c) => c.Text = $"{txtResult.Text}{Environment.NewLine}{Environment.NewLine}Find & Replace Task has been Canceled!");
        }

        private void InvokeControl(Control control, Action<Control> action)
        {
            if (control.InvokeRequired)
                control.Invoke(new Action<Control, Action<Control>>((c, a) => InvokeControl(c, a)), new object[] { control, action });
            else
                action(control);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.cancellationTokenSource.Cancel();
            this.btnCancel.Visible = false;
        }
    }
}
