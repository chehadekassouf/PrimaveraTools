using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P6FullHistory
{
    class TaskPath
    {
        private DatabaseLinks dl;
        private System.IO.StreamWriter sw;
        private System.Data.DataTable  processedTasks;
        public System.Windows.Forms.TreeNode tn;
        public System.Collections.ArrayList  nl=new System.Collections.ArrayList();
        private bool driving;
        private System.ComponentModel.BackgroundWorker _bw;
        public int totalrows;

        public TaskPath(string projectname,bool driving, System.ComponentModel.BackgroundWorker _bw)
        {
            this.driving = driving;                                       
            this.dl = new DatabaseLinks();
            processedTasks = new System.Data.DataTable();
            processedTasks.Columns.Add("task_code");
            processedTasks.Columns.Add("level");
            processedTasks.Constraints.Add("PK", new System.Data.DataColumn[] { processedTasks.Columns[0] }, true);
            sw = new System.IO.StreamWriter(Properties.Settings.Default.OutputPath,false);
            //sw.WriteLine("\"  task_code  \",\"  task_name  \",\"  status_code  \",\"  pred_task_code  \",\"  pred_task_name  \",\"  pred_status_code  \",\"  total_float_hr_cnt  \",\" task_id  \",\"  proj_id  \",\"  pred_task_id  \",\"  pred_proj_id  \",\"pred_total_float_hr_cnt  \",\" level \"");
            //sw.WriteLine("\"level01\",\"level02\",\"level03\",\"level04\",\"level05\",\"level06\",\"level07\",\"level08\",\"level09\",\"level10\",\"level11\",\"level12\",\"level13\",\"level14\",\"level15\",\"level16\",\"level17\",\"level18\",\"level19\",\"level20\",\"level21\",\"level22\",\"level23\",\"level24\",\"level25\",\"level26\",\"level27\",\"level28\",\"level29\",\"level30\"");
            tn = new System.Windows.Forms.TreeNode(projectname);
            this._bw = _bw;
            this.totalrows = this.getListedTasks().Rows.Count;
            
            
        }
        public void startProcess()
        {
            int i = 1;
            
            foreach (System.Data.DataRow dr in this.getListedTasks().Rows)
            {
                double flt = 0;
                double.TryParse(dr["total_float_hr_cnt"].ToString(), out flt);
                System.Windows.Forms.TreeNode ctn = new System.Windows.Forms.TreeNode("(" + i.ToString() + ")" + dr["task_code"].ToString() + "-(" + ((int)(flt / 8)).ToString() + ")-" + dr["task_name"].ToString() );
                tn.Nodes.Add(ctn);
                nl.Add(ctn);
                tn.Tag = "Top Node";
                ctn.Tag = dr["task_code"].ToString();
                if (flt < 0)
                {
                    ctn.ForeColor = System.Drawing.Color.Red;
                }
                
                getNextLevel(dr["task_id"].ToString(), dr["proj_id"].ToString(), i.ToString(), "(" + i.ToString() + ")" + dr["task_code"].ToString() + "-(" + ((int)(flt / 8)).ToString() + ")-" + dr["task_name"].ToString(), ctn);
                _bw.ReportProgress(i);
                i++;
            }
            sw.Close();
            sw = new System.IO.StreamWriter(Properties.Settings.Default.OutputPath + "1", false);
            sw.WriteLine("\"  task_code  \",\"  level  \"");
            foreach (System.Data.DataRow dr1 in processedTasks.Rows)
            {
                sw.WriteLine("\""+dr1["task_code"].ToString() + "\",\"'" + dr1["level"].ToString()+"\"");

            }
            sw.Close();
        }
        private System.Windows.Forms.TreeNode getNode(string task_code)
        {
            foreach (object obj in nl)
            {
                System.Windows.Forms.TreeNode tn = (System.Windows.Forms.TreeNode)obj;
                if (tn.Tag.ToString() == task_code)
                {
                    return tn;
                }
            }
            return null;
        }
        private System.Windows.Forms.TreeNode getNode(System.Windows.Forms.TreeNodeCollection tnc, string task_code)
        {
            foreach (System.Windows.Forms.TreeNode tn in tnc)
            {
                
                if (tn.Tag.ToString() == task_code)
                {
                    return tn;
                }
            }
            return null;
        }
        private void getNextLevel(string task_id,string proj_id,string level,string level_task,System.Windows.Forms.TreeNode ctn)
        {

            int i = 0;
            string s = "GO";
            if (Properties.Settings.Default.MaxLevel != 0 && level.Replace(".", "").Length > Properties.Settings.Default.MaxLevel)
            {
                return;
            }
            foreach (System.Data.DataRow dr in this.dl.TaskPredcessor.Select("task_id=" + task_id + " and proj_id=" + proj_id,"delta desc"))
            {
                i++;
                double flt = 1000000;
                double.TryParse(dr["pred_total_float_hr_cnt"].ToString(), out flt);
                string levelcode = getLevelCode(level + "." + i.ToString());
                string floatdays= ((int)(flt / 8)).ToString() ;
                double duration = 0;
                double.TryParse(dr["pred_remain_drtn_hr_cnt"].ToString(),out duration);
                string durationdays=((int)(duration/ 8)).ToString();
                string dts = dr["pred_early_start_date"].ToString().Length > 0 ? DateTime.Parse(dr["pred_early_start_date"].ToString()).ToString("dd-MMM-yyyy") : "";
                string dtf = dr["pred_early_end_date"].ToString().Length > 0 ? DateTime.Parse(dr["pred_early_end_date"].ToString()).ToString("dd-MMM-yyyy") : "";
                string nodename = "(" + levelcode + ")" + dr["pred_task_code"].ToString() + "-(" + floatdays + ")-" + dr["pred_task_name"].ToString() + "(" + durationdays + "," + dts + "," + dtf + ")" + " (" + dr["pred_relation_type"].ToString() + "," + dr["pred_status_code"].ToString() + ")";
                 System.Windows.Forms.TreeNode cctn = new System.Windows.Forms.TreeNode(nodename);

                cctn.Tag = dr["pred_task_code"].ToString();

                if (dr["pred_status_code"].ToString() == "TK_Complete")
                {
                    cctn.ForeColor = System.Drawing.Color.Green;
                }
                else if (flt < 0)
                {
                    cctn.ForeColor = System.Drawing.Color.Red;
                }
                else if (flt == 0)
                {
                    cctn.ForeColor = System.Drawing.Color.Blue;
                }

                //System.Windows.Forms.TreeNode cctn = this.getNode(dr["pred_task_code"].ToString());
                //if (cctn == null)
                //{
                //    cctn = new System.Windows.Forms.TreeNode(dr["pred_task_code"].ToString() + "-(" + (flt / 8).ToString("#") + ")-" + dr["pred_task_name"].ToString() + " (" + dr["pred_status_code"].ToString() + ")");
                //    cctn.Tag = dr["pred_task_code"].ToString();
                //   al.Add(cctn);
                //}


                
                
                /*
                  sw.WriteLine("\"" + dr["task_code"].ToString() + "\",\"" + dr["task_name"].ToString() + "\",\"" + dr["status_code"].ToString() + "\",\"" + dr["pred_task_code"].ToString() + "\",\"" + dr["pred_task_name"].ToString() + "\",\"" + dr["pred_status_code"].ToString() + "\",\"" + dr["total_float_hr_cnt"].ToString() + "\",\"" 
                    + dr["task_id"].ToString() + "\",\"" + dr["proj_id"].ToString() + "\",\"" + dr["pred_task_id"].ToString() + "\",\"" + dr["pred_proj_id"].ToString() + "\",\""
                    + dr["pred_total_float_hr_cnt"].ToString() + "\",\"'" + levelcode + "\",\"" + (level.Length - level.Replace(".", "").Length).ToString() + "\"");
                 * */
                //sw.WriteLine("\""+level_task.Replace("|","\",\"")+"\"");
                sw.WriteLine("\"" + "".PadLeft(level.Length - level.Replace(".", "").Length).Replace(" ", "\",\"") + nodename + "\"");
                string[] lvlarray=levelcode.Split(',');
                for(int lvli=1;lvli<lvlarray.Length;lvli++)
                {
                    System.Data.DataRow dr1 = processedTasks.Rows.Find(new object[] { dr["task_code"].ToString() });
                    if (dr1 == null)
                    {
                        processedTasks.Rows.Add(new string[] { dr["task_code"].ToString(), lvlarray[0]+"."+lvlarray[lvli] });
                    }
                    else if (!("," + dr1["level"].ToString() + ",").Contains("," + lvlarray[0] + "." + lvlarray[lvli] + ","))
                    {
                        dr1["level"] = dr1["level"].ToString() + "," + lvlarray[0] + "." + lvlarray[lvli];
                    }
                }
                

                    if (s != "GO")
                    {
                        double prevdelta = 1000000;
                        double currdelta = 10000000;
                        double.TryParse(s, out prevdelta);
                        double.TryParse(dr["delta"].ToString(), out currdelta);
                        if (Math.Abs(prevdelta - currdelta) > 10 && driving)
                        {
                            break;
                        }
                    }

                    if (flt > 0 && driving)
                    {
                        break;
                    }
                
               
                
                s = dr["delta"].ToString();
                if (level.Length < 20 || dr["status_code"].ToString() != "TK_Complete")
                {

                    if (this.getNode(ctn.Nodes, dr["pred_task_code"].ToString()) == null && !IsActivityDisregarded(dr["pred_task_code"].ToString()))
                    {
                        getNextLevel(dr["pred_task_id"].ToString(), dr["pred_proj_id"].ToString(), level + "." + i.ToString(), level_task + "|" + nodename, cctn);
                    }
                }
                if (this.getNode(ctn.Nodes, cctn.Tag.ToString()) == null)
                {
                    ctn.Nodes.Add(cctn);
                    nl.Add(cctn);
                }
            }
        }
        private bool IsActivityDisregarded(string task_code)
        {
            if (Properties.Settings.Default.DisregardActivity.Trim().Length == 0)
            {
                return false;
            }
            foreach (string taskstart in Properties.Settings.Default.DisregardActivity.Split(','))
            {
                if (task_code.StartsWith(taskstart))
                {
                    return true;
                }
            }
            return false;
        }
        private string getLevelCode(string level)
        {
            
            string[] levelsplit = level.Split(new string[] { "." }, StringSplitOptions.None);
            string returnvalue = levelsplit[0];
            for (int i = 1; i < levelsplit.Length; i++)
            {
                if (levelsplit[i] != "1")
                {
                    returnvalue = returnvalue + "," + levelsplit[i] + i.ToString();
                }
            }
            
            return  returnvalue;
        }
        private System.Data.DataTable getTopTasks()
        {
            /*
             select a.task_id ,a.proj_id ,a.task_code ,a.task_name,a.early_start_date ,a.early_end_date,a.late_start_date ,a.late_end_date,a.total_float_hr_cnt,b.task_id            pred_task_id ,b.proj_id            pred_proj_id ,c.pred_type,c.lag_hr_cnt,b.task_code          pred_task_code ,b.task_name          pred_task_name ,b.early_start_date   pred_early_start_date ,b.early_end_date     pred_early_end_date ,b.late_start_date    pred_late_start_date ,b.late_end_date      pred_late_end_date ,b.total_float_hr_cnt pred_total_float_hr_cnt from task as a,taskpred as c,task as b where a.task_id=c.task_id and b.task_id=c.pred_task_id and a.proj_id=2082
             */
            System.Data.DataTable returntable = new System.Data.DataTable();
            returntable.Columns.Add("task_id");
            returntable.Columns.Add("task_code");
            returntable.Columns.Add("task_name");
            returntable.Columns.Add("proj_id");
            returntable.Columns.Add("total_float_hr_cnt");
            foreach (System.Data.DataRow dr in this.dl.TaskTable.Select("status_code<>'TK_Complete'"))
            {
                if (this.dl.TaskPredcessor.Select("pred_task_id=" + dr["task_id"].ToString() + " and pred_proj_id=" + dr["proj_id"].ToString()).Length == 0)
                {
                    returntable.Rows.Add(new string[] { dr["task_id"].ToString(), dr["task_code"].ToString(), dr["task_name"].ToString(), dr["proj_id"].ToString(), dr["total_float_hr_cnt"].ToString() });
                }
            }
            return returntable;
        }
        private System.Data.DataTable getListedTasks()
        {
            /*
             select a.task_id ,a.proj_id ,a.task_code ,a.task_name,a.early_start_date ,a.early_end_date,a.late_start_date ,a.late_end_date,a.total_float_hr_cnt,b.task_id            pred_task_id ,b.proj_id            pred_proj_id ,c.pred_type,c.lag_hr_cnt,b.task_code          pred_task_code ,b.task_name          pred_task_name ,b.early_start_date   pred_early_start_date ,b.early_end_date     pred_early_end_date ,b.late_start_date    pred_late_start_date ,b.late_end_date      pred_late_end_date ,b.total_float_hr_cnt pred_total_float_hr_cnt from task as a,taskpred as c,task as b where a.task_id=c.task_id and b.task_id=c.pred_task_id and a.proj_id=2082
             */
            System.Data.DataTable returntable = new System.Data.DataTable();
            returntable.Columns.Add("task_id");
            returntable.Columns.Add("task_code");
            returntable.Columns.Add("task_name");
            returntable.Columns.Add("proj_id");
            returntable.Columns.Add("total_float_hr_cnt");
            foreach (string activity in Properties.Settings.Default.ActivityList.Split(new string[] { "," }, StringSplitOptions.None))
            {
                foreach (System.Data.DataRow dr in this.dl.TaskTable.Select("task_code='" + activity + "'"))
                {

                    returntable.Rows.Add(new string[] { dr["task_id"].ToString(), dr["task_code"].ToString(), dr["task_name"].ToString(), dr["proj_id"].ToString(), dr["total_float_hr_cnt"].ToString() });

                }
            }
            return returntable;
        }
        
    }
}
