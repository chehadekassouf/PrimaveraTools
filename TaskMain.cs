using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P6FullHistory
{
    class TaskMain
    {

        private DatabaseLinks dl;
        private System.IO.StreamWriter sw;
        private System.Data.DataTable  processedTasks;
        private System.ComponentModel.BackgroundWorker _bw;
        public int totalrows;
        public TaskMain(System.ComponentModel.BackgroundWorker _bw)
        {

           
            this.dl = new DatabaseLinks();
            processedTasks = new System.Data.DataTable();
            processedTasks.Columns.Add("task_id");
            processedTasks.Columns.Add("proj_id");
            processedTasks.Constraints.Add("PK", new System.Data.DataColumn[] { processedTasks.Columns[0], processedTasks.Columns[1] }, true);
            sw = new System.IO.StreamWriter(Properties.Settings.Default.OutputPath,false);
            this._bw=_bw;
            this.totalrows = dl.TaskTable.Rows.Count;
           
          


        }
        public void StartProcess()
        {
            getHistory();
            sw.Close();
        }
        private void getHistory()
        {
            int i = 0;
            foreach(System.Data.DataRow intaskrow in dl.TaskTable.Rows)
            {
                i++;
               _bw.ReportProgress(i);
               if (this.processedTasks.Rows.Find(new string[] { intaskrow["task_id"].ToString(), intaskrow["proj_id"].ToString() }) == null)
               {
                   this.processedTasks.Rows.Add(new string[] { intaskrow["task_id"].ToString(), intaskrow["proj_id"].ToString() });
                    getHistory(intaskrow["task_id"].ToString(), intaskrow["proj_id"].ToString(), intaskrow["task_id"].ToString(), intaskrow["proj_id"].ToString(),"1");
               }
            }
 
        }
        private void getHistory(string task_id,string proj_id,string parent_task_id,string parent_proj_id,string degree)
        {

            System.Data.DataRow relrow = dl.TaskRelation.Rows.Find(new string[]{task_id,proj_id});
            
           if(relrow!=null)
            {
                foreach (System.Data.DataRow prdrow in relrow.GetChildRows("PCRel"))
                {
                    string pred_task_id = prdrow["pred_task_id"].ToString();
                    string pred_proj_id = prdrow["pred_proj_id"].ToString();
                    this.exportToCsv(parent_task_id + "," + parent_proj_id + "," + pred_task_id + "," + pred_proj_id+","+degree);
                    if (this.processedTasks.Rows.Find(new string[]{pred_task_id , parent_task_id})==null)
                    {
                        this.processedTasks.Rows.Add(new string[]{pred_task_id , parent_task_id});
                        getHistory(pred_task_id, pred_proj_id, parent_task_id, parent_proj_id,degree+".1");
                    }
                }
                
            }
        }
        private void exportToCsv(string datastring)
        {
            sw.WriteLine(datastring);
 
        }
    }
}
