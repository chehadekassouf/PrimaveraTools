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
    public partial class Form1 : Form
    {
        private BackgroundWorker _bw;

        private System.Threading.Thread th;
        public delegate void ProgressCallback(int currentvalue);
        public delegate void TextCallback(string currentvalue);
        public Form1()
        {
            InitializeComponent();
            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
           _bw.ProgressChanged+=new ProgressChangedEventHandler(_bw_ProgressChanged);
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 100;

        }

        void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            this.SetProgressValue( e.ProgressPercentage);
            this.SetProgressValue(e.ProgressPercentage.ToString());
            
           
        }

        private void SetProgressValue(string currentvalue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label1.InvokeRequired)
            {

                TextCallback cb = new TextCallback(SetProgressValue);
                this.Invoke(cb, new object[] { currentvalue });
            }
            else
            {
                this.label1.Text = currentvalue+"/"+this.label1.Tag.ToString();
            }
        }
        private void SetProgressValue(int currentvalue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressBar1.InvokeRequired)
            {

                ProgressCallback cb = new ProgressCallback(SetProgressValue);
                this.Invoke(cb, new object[] { currentvalue });
            }
            else
            {
              
                    this.progressBar1.Value = currentvalue;
                
            }
        }
        private void SetProgressMax(int currentvalue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.progressBar1.InvokeRequired)
            {

                ProgressCallback cb = new ProgressCallback(SetProgressMax);
                this.Invoke(cb, new object[] { currentvalue });
            }
            else
            {
                this.progressBar1.Maximum = currentvalue;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            this.saveUserChanges();
            th=new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(runTaskMain));

            th.Start(_bw);
         
        }
        private void runTaskMain(object param)
        {
            TaskMain tm= new TaskMain(param as BackgroundWorker);
            this.label1.Tag = tm.totalrows.ToString();
            this.SetProgressMax(tm.totalrows);
            tm.StartProcess();
            System.Windows.Forms.MessageBox.Show("DONE");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null)
            {
                th.Abort();
            }
            this.saveUserChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DBForm().ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textOutPath.Text = Properties.Settings.Default.OutputPath;
            this.textTask.Text = Properties.Settings.Default.Tasks;
            this.textTaskRelation.Text = Properties.Settings.Default.TaskUnique;
            this.textTaskPredecessor.Text = Properties.Settings.Default.TaskPredecessor;
            this.textBoxCalendar.Text= Properties.Settings.Default.Calendar;
            this.textBoxActivityList.Text = Properties.Settings.Default.ActivityList;
            this.textBoxProject.Text = Properties.Settings.Default.ProjectCode;
            this.textBoxDisredardActivities.Text = Properties.Settings.Default.DisregardActivity;
            this.textBoxLevel.Text = Properties.Settings.Default.MaxLevel.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.saveUserChanges();
            th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(runResourceLoading));

            th.Start(_bw);
            
        }
        private void runResourceLoading(object param)
        {
            ResourceLoading rl = new ResourceLoading(param as BackgroundWorker);
            this.label1.Tag = rl.totalrows.ToString();
            this.SetProgressMax(rl.totalrows);
            rl.StartProcess();
            System.Windows.Forms.MessageBox.Show("DONE");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.saveUserChanges();
            th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(runTasPath));

            th.Start(_bw);



        }
        private void runTasPath(object param)
        {

            System.Drawing.Color cl = this.button4.BackColor;
            this.button4.BackColor = System.Drawing.Color.Yellow;
            TaskPath tp = new TaskPath(this.textBoxProject.Text,this.checkBox1.Checked, param as BackgroundWorker);
            
            
            this.label1.Tag = tp.totalrows.ToString();
            this.SetProgressMax(tp.totalrows);
            tp.startProcess();
            new TreeForm(tp.tn,tp.nl).ShowDialog();
            this.button4.BackColor = cl;
        }
        private void saveUserChanges()
        {
            Properties.Settings.Default.OutputPath = this.textOutPath.Text;
            Properties.Settings.Default.Tasks= this.textTask.Text ;
            Properties.Settings.Default.TaskUnique= this.textTaskRelation.Text ;
            Properties.Settings.Default.TaskPredecessor=this.textTaskPredecessor.Text ;
            Properties.Settings.Default.Calendar = this.textBoxCalendar.Text;
            Properties.Settings.Default.ActivityList = this.textBoxActivityList.Text;
            Properties.Settings.Default.ProjectCode = this.textBoxProject.Text;
            Properties.Settings.Default.DisregardActivity = this.textBoxDisredardActivities.Text;
            Properties.Settings.Default.MaxLevel = long.Parse(this.textBoxLevel.Text);
            Properties.Settings.Default.Save();
        }

    }
}
