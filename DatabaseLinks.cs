using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P6FullHistory
{
    class DatabaseLinks
    {
        
        private System.Data.DataSet dataset;
        private System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
        public System.Data.DataTable TaskTable { get { return this.dataset.Tables["TaskTable"]; } }
        public System.Data.DataTable TaskRelation { get { return this.dataset.Tables["TaskRelation"]; } }
        public System.Data.DataTable TaskPredcessor { get { return this.dataset.Tables["TaskPredcessor"]; } }
        public System.Data.DataTable Calendar { get { return this.dataset.Tables["Calendar"]; } }
        public DatabaseLinks()
        {
            try
            {
                DBCredential dbc = new DBCredential();
                string csting = dbc.Read(Properties.Settings.Default.DBString);
                dataset = new System.Data.DataSet();
                this.dataset.Tables.Add("TaskTable");
                this.dataset.Tables.Add("TaskRelation");
                this.dataset.Tables.Add("TaskPredcessor");
                this.dataset.Tables.Add("Calendar");
                connection.ConnectionString = csting;
                string projcode = "'" + Properties.Settings.Default.ProjectCode.Replace(",", "','") + "'";
                getSqlTable(this.dataset.Tables["TaskTable"], string.Format(Properties.Settings.Default.Tasks, projcode));
                getSqlTable(this.dataset.Tables["TaskRelation"], string.Format(Properties.Settings.Default.TaskUnique, projcode));
                getSqlTable(this.dataset.Tables["TaskPredcessor"], string.Format(Properties.Settings.Default.TaskPredecessor, projcode));
                getSqlTable(this.dataset.Tables["Calendar"], string.Format(Properties.Settings.Default.Calendar, projcode));

                try
                {
                    this.TaskRelation.Constraints.Add("PK", new System.Data.DataColumn[] { this.TaskRelation.Columns["task_id"], this.TaskRelation.Columns["proj_id"] }, true);
                    this.dataset.Relations.Add("PCrel", new System.Data.DataColumn[] { this.TaskRelation.Columns["task_id"], this.TaskRelation.Columns["proj_id"] }, new System.Data.DataColumn[] { this.TaskPredcessor.Columns["task_id"], this.TaskPredcessor.Columns["proj_id"] });

                }
                catch (Exception exp)
                {
                    System.Windows.Forms.MessageBox.Show(exp.Message);
                }
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
           
        }
        public void  getSqlTable(System.Data.DataTable dt,string selectstatement)
        {
            new System.Data.SqlClient.SqlDataAdapter(selectstatement, connection).Fill(dt);
        }
        public System.Data.DataTable getSqlTable(string selectstatement)
        {

            System.Data.DataTable dt = new System.Data.DataTable();
            new System.Data.SqlClient.SqlDataAdapter(selectstatement, connection).Fill(dt);
            return dt;
        }
        
    }
}
