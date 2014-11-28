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
    public partial class TreeForm : Form
    {

        private System.Collections.ArrayList nodelist;
        public TreeForm(System.Windows.Forms.TreeNode tn, System.Collections.ArrayList nodelist)
        {
            InitializeComponent();
            this.nodelist = nodelist;
            treeView1.Nodes.Add(tn);
            this.Text = "Total Activities Found: " + this.nodelist.Count;

        }

        private void printViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Codebasement.PrintControl.PrintDocumentForm form = new Codebasement.PrintControl.PrintDocumentForm(treeView1);
            Cursor = Cursors.Default;
            form.Show();
            form.Activate();
            form.BringToFront();
        }

        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            System.Drawing.Color cl = this.buttonFindNext.BackColor;
            this.buttonFindNext.BackColor = System.Drawing.Color.Yellow;
            this.ExpandNodes(this.textBox1.Text.Trim());
            this.buttonFindNext.BackColor = cl;
        }
        private void ExpandNodes(string node_text)
        {
            foreach (object obj in nodelist)
            {
                System.Windows.Forms.TreeNode tn = (System.Windows.Forms.TreeNode)obj;
                if (tn.Text.ToUpper().Contains(node_text.ToUpper()))
                {
                    tn.BackColor = System.Drawing.Color.Yellow;
                    System.Windows.Forms.TreeNode ptn = tn.Parent;
                    while (ptn.Text!="Top Node")
                    {
                        ptn.Expand();
                        ptn = ptn.Parent;
                    }
                }
            }
            treeView1.Nodes[0].Expand();
            
        }
        private void ResetTree()
        {
            foreach (object obj in nodelist)
            {
                System.Windows.Forms.TreeNode tn = (System.Windows.Forms.TreeNode)obj;
                tn.BackColor = System.Drawing.Color.White;
                if (tn.IsExpanded)
                {
                   
                    tn.Collapse();
                }
                   
                
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            System.Drawing.Color cl = this.buttonReset.BackColor;
            this.buttonReset.BackColor = System.Drawing.Color.Yellow;
            ResetTree();
            this.buttonReset.BackColor = cl;
           

        }

        
    }
}
