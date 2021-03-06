// 
// Main website for TVRename is http://tvrename.com
// 
// Source code available at https://github.com/TV-Rename/tvrename
// 
// Copyright (c) TV Rename. This code is released under GPLv3 https://github.com/TV-Rename/tvrename/blob/master/LICENSE.md
// 

using System;
using System.Windows.Forms;
using Alphaleonis.Win32.Filesystem;

namespace TVRename.Forms.ShowPreferences
{
    public partial class QuickLocateForm : Form
    {
        public string DirectoryFullPath;

        public QuickLocateForm(string hint)
        {
            InitializeComponent();

            cbDirectory.SuspendLayout();
            cbDirectory.Items.Clear();
            foreach (string folder in TVSettings.Instance.LibraryFolders)
            {
                cbDirectory.Items.Add(folder.TrimEnd(Path.DirectorySeparatorChar.ToString()));
            }
            cbDirectory.SelectedIndex = 0;
            cbDirectory.ResumeLayout();

            txtShowFolder.Text = Path.DirectorySeparatorChar + TVSettings.Instance.FilenameFriendly(FileHelper.MakeValidPath(hint));
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
              SetDirectory();
                DialogResult = DialogResult.OK;
                Close();
        }

        private void SetDirectory()
        {
            DirectoryFullPath = cbDirectory.Text + txtShowFolder.Text;
        }
    }
}
