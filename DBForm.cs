using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace P6FullHistory
{
    public partial class DBForm : Form
    {
        public DBForm()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            string DBString = string.Format("Server={0};Database={1};User ID={2};Password={3};Trusted_Connection=False;",new string[]{ textServer.Text, textDatabase.Text, textUser.Text, textPassword.Text});
            DBCredential dbc=new DBCredential();
            Properties.Settings.Default.DBString = dbc.GenerateDBCredential(DBString);
            Properties.Settings.Default.Save();
            this.Close();

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
