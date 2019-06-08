using FindReplace;
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

namespace Find
{
    public partial class Find : Form
    {
        public Find()
        {
            InitializeComponent();
            folderBrowser.Description = "Select the folder that you want to use";
            folderBrowser.ShowNewFolderButton = false;
            txtPath.Text = @"C:\";
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowser.SelectedPath;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text))
            {
                MessageBox.Show("Cannot find: Find text is empty", "Find", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Cannot find: Path is not an existing Directory", "Find", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            FileActions actions = new FileActions();
            actions.RootDirectoryPath = txtPath.Text;
            actions.FilePattern = txtPattern.Text;
            actions.IncludingSubdirectories = chkInclude.Checked;
            actions.CaseSensitive = chkCase.Checked;
            actions.OnSkipEvent += this.skipped;

            txtResult.Text = "";
            Task.Run(async () => setResult(await actions.FindAsync(txtFind.Text), actions));
        }

        private void setResult(List<string> result, FileActions actions)
        {
            if (string.IsNullOrEmpty(txtResult.Text))
                InvokeControl(txtResult, (c) => c.Text = string.Format("Find {1} in path {2}{0}", Environment.NewLine, txtFind.Text, actions.RootDirectoryPath));
            else
                InvokeControl(txtResult, (c) => c.Text = string.Format("{0}{1}{1}Find {2} in path {3}{1}", c.Text, Environment.NewLine, txtFind.Text, actions.RootDirectoryPath));
            InvokeControl(txtResult, (c) => c.Text = string.Format("{0}{1}{2}", c.Text, Environment.NewLine, string.Join(Environment.NewLine, result)));
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text))
            {
                MessageBox.Show("Cannot replace: Find text is empty", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (string.IsNullOrEmpty(txtReplace.Text))
            {
                MessageBox.Show("Cannot replace: Replace text is empty", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!Directory.Exists(txtPath.Text))
            {
                MessageBox.Show("Cannot replace: Path is not an existing Directory", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            FileActions actions = new FileActions();
            actions.RootDirectoryPath = txtPath.Text;
            actions.FilePattern = txtPattern.Text;
            actions.IncludingSubdirectories = chkInclude.Checked;
            actions.CaseSensitive = chkCase.Checked;
            actions.OnSkipEvent += this.skipped;

            txtResult.Text = "";
            List<string> result = actions.Replace(txtFind.Text, txtReplace.Text);
            if (string.IsNullOrEmpty(txtResult.Text))
                txtResult.Text = string.Format("Replace {1} with {2} in path {3}{0}", Environment.NewLine, txtFind.Text, txtReplace.Text, actions.RootDirectoryPath);
            else
                txtResult.Text = string.Format("{0}{1}{1}Replace {2} with {3} in path {4}{1}", txtResult.Text, Environment.NewLine, txtFind.Text, txtReplace.Text, actions.RootDirectoryPath);
            txtResult.Text = string.Format("{0}{1}{2}", txtResult.Text, Environment.NewLine, string.Join(Environment.NewLine, result));
        }

        private void skipped(string file)
        {
            if (string.IsNullOrEmpty(txtResult.Text))
                InvokeControl(txtResult, (c) => c.Text = string.Format("{0} skipped", file));
            else
                InvokeControl(txtResult, (c) => c.Text = string.Format("{0}{1}{2} skipped", txtResult.Text, Environment.NewLine, file));
        }

        private void InvokeControl(Control control, Action<Control> action)
        {
            if (control.InvokeRequired)
                control.Invoke(new Action<Control, Action<Control>>((c, a) => InvokeControl(c, a)), new object[] { control, action });
            else
                action(control);
        }
    }
}
