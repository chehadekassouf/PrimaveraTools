using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P6FullHistory
{
    class ResourceLoading
    {
        private System.Data.DataTable calendarDays;
        private System.Data.DataTable activityLoading;
        private System.Data.DataTable calendar;
        private System.Data.DataTable activity;
        private System.IO.StreamWriter sw;
        public int totalrows;
        private System.ComponentModel.BackgroundWorker _bw;
        public ResourceLoading(System.ComponentModel.BackgroundWorker _bw)
        {
            this.sw = new System.IO.StreamWriter(Properties.Settings.Default.OutputPath);
            DatabaseLinks db = new DatabaseLinks();
            this.calendar = db.Calendar;
            this.activity = db.TaskTable;

 
            this._bw = _bw;
            this.totalrows = this.activity.Rows.Count;
           
        }
        public void StartProcess()
        {
            this.sw.WriteLine("\"" +
                              "task_id"+ "\",\"" +
                               "task_code" + "\",\"" +
                               "task_name" + "\",\"" +
                               "total_float_hr_cnt" + "\",\"" +
                               "late_start_date" + "\",\"" +
                               "late_end_date" + "\",\"" +
                               "early_start_date" + "\",\"" +
                               "early_end_date" + "\",\"" +
                               "target_start_date" + "\",\"" +
                               "target_end_date" + "\",\"" +
                               "completeday" + "\",\"" +
                               "day" + "\",\"" +
                               "month" + "\",\"" +
                               "year" + "\",\"" +
                               "caltype" + "\",\"" +
                               "numberhours" + "\""
                               );
            this.populateCalendarDays();
            this.populateActivityLoading();
            sw.Close();
        }
        private void populateActivityLoading()
        {
            activityLoading = new System.Data.DataTable();
            activityLoading.Columns.Add("task_id");
            activityLoading.Columns.Add("task_code");
            activityLoading.Columns.Add("task_name");
            activityLoading.Columns.Add("total_float_hr_cnt");
            activityLoading.Columns.Add("late_start_date");
            activityLoading.Columns.Add("late_end_date");
            activityLoading.Columns.Add("early_start_date");
            activityLoading.Columns.Add("early_end_date");
            activityLoading.Columns.Add("target_start_date");
            activityLoading.Columns.Add("target_end_date");
            activityLoading.Columns.Add("latehours");
            activityLoading.Columns.Add("earlyhours");
            activityLoading.Columns.Add("targethours");
            DateTime earliest = DateTime.Parse(activity.Select(null, "early_start_date")[0]["early_start_date"].ToString());
            DateTime latest = DateTime.Parse(activity.Select(null, "late_end_date asc")[0]["late_end_date"].ToString());
            int i = 1;
            foreach (System.Data.DataRow dr in this.activity.Rows)
            {
                _bw.ReportProgress(i++);
                double whl=getWorkingHoursBetween(dr["late_start_date"].ToString(), dr["late_end_date"].ToString(), dr["clndr_id"].ToString(),dr,"LATE");
                double whe = getWorkingHoursBetween(dr["early_start_date"].ToString(), dr["early_end_date"].ToString(), dr["clndr_id"].ToString(),dr,"EARLY");
                double wht = getWorkingHoursBetween(dr["target_start_date"].ToString(), dr["target_end_date"].ToString(), dr["clndr_id"].ToString(),dr,"TARGET");
                activityLoading.Rows.Add(new string[]{
                    dr["task_id"].ToString(),
                    dr["task_code"].ToString(),
                    dr["task_name"].ToString(),
                    dr["total_float_hr_cnt"].ToString(),
                    dr["late_start_date"].ToString(),
                    dr["late_end_date"].ToString(),
                    dr["early_start_date"].ToString(),
                    dr["early_end_date"].ToString(),
                    dr["target_start_date"].ToString(),
                    dr["target_end_date"].ToString(),
                    whl.ToString(),
                    whe.ToString(),
                    wht.ToString()
                });
            }
        }
        private void populateCalendarDays()
        {
            calendarDays = new System.Data.DataTable();
            calendarDays.Columns.Add("ID");
            calendarDays.Columns.Add("CalendarName");
            calendarDays.Columns.Add("year");
            calendarDays.Columns.Add("month");
            calendarDays.Columns.Add("day");
            calendarDays.Columns.Add("workinghours");
            calendarDays.Columns.Add("exception");
            foreach (System.Data.DataRow dr in calendar.Rows)
            {
                this.populateCalendarDays(dr["clndr_id"].ToString(),dr["clndr_name"].ToString(), dr["clndr_data"].ToString());
            }
        }
        private long getWorkingDaysCount(DateTime start, DateTime finish, string clndr_data)
        {
            long returnvalue = 0;
            for (long i = 0; start.AddDays(i) <= finish; i++)
            {

                if (isWorkingDay(start.AddDays(i), clndr_data))
                {
                    returnvalue += 1;
                }
            }
            return returnvalue;
        }
        private double getWorkingHoursBetween(string startin, string finishin, string clndr_id,System.Data.DataRow dr,string datetype)
        {
            double returnvalue = 0;
            DateTime start = DateTime.Parse(startin);
            DateTime finish = DateTime.Parse(finishin);

            for (long i = 0; start.AddDays(i) <= finish; i++)
            {
                string ndfilter = "ID='" + clndr_id + "' and exception='NO' and day='" + ((int)start.DayOfWeek).ToString() + "'";
                string edfilter = "ID='" + clndr_id + "' and exception='YES' and day='" + start.Day.ToString() + "' and month='"+start.Month.ToString()+"' and year='"+start.Year.ToString()+"'";
                System.Data.DataRow[] nd = calendarDays.Select(ndfilter);
                System.Data.DataRow[] ed = calendarDays.Select(edfilter);
                if (ed.Length == 0 && nd.Length > 0)
                {
                    double dailyhours = double.Parse(nd[0]["workinghours"].ToString());
                    returnvalue += dailyhours;

                    if (nd.Length > 1)
                    {
                        dailyhours += double.Parse(nd[1]["workinghours"].ToString());
                        returnvalue += double.Parse(nd[1]["workinghours"].ToString());
                    }
                    this.sw.WriteLine("\""+
                                 dr["task_id"].ToString() + "\",\"" +
                                 dr["task_code"].ToString() + "\",\"" +
                                 dr["task_name"].ToString() + "\",\"" +
                                 dr["total_float_hr_cnt"].ToString() + "\",\"" +
                                 dr["late_start_date"].ToString() + "\",\"" +
                                 dr["late_end_date"].ToString() + "\",\"" +
                                 dr["early_start_date"].ToString() + "\",\"" +
                                 dr["early_end_date"].ToString() + "\",\"" +
                                 dr["target_start_date"].ToString() + "\",\"" +
                                 dr["target_end_date"].ToString() + "\",\"" +
                                 start.ToShortDateString() + "\",\"" +
                                 start.Day.ToString() + "\",\"" +
                                 start.Month.ToString() + "\",\"" +
                                 start.Year.ToString() + "\",\"" +
                                 datetype + "\",\"" +
                                 dailyhours.ToString()+ "\""
                                 );
                }
                
                
            }
            return returnvalue;
        }
        private bool isWorkingDay(DateTime day, string clndr_data)
        {
            return true;
        }
        private void populateCalendarDays(string calendarid,string calendarname,string clndr_data)
        {
            CalendarItem maincalendar = this.getCalendarItem(clndr_data);



            foreach(CalendarItem ci in maincalendar.Adam)
            {
                if(ci.FullFamily().Contains("Exceptions") && ci.Name.Contains("d|"))
                {
                    if (ci.Name.Split(new string[] { "|" },StringSplitOptions.None).Length > 0)
                    { 
                        DateTime dt= DateTime.FromOADate(double.Parse( ci.Name.Split(new string[] { "|" }, StringSplitOptions.None)[1]));

                        calendarDays.Rows.Add(new string[] { calendarid,calendarname, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), "0", "YES" });
                            
                    }
                   
                }
                if (ci.FullFamily().Contains("DaysOfWeek") && ci.Name.Contains("s|"))
                {
                    string[] dayno=ci.Parent.Parent.Parent.Name.Split(new string[] { "||" }, StringSplitOptions.None);
                    if (dayno.Length > 0)
                    {
                        string[] stime = ci.Name.Split(new string[] { "|", ":" }, StringSplitOptions.None);
                        double d=(double.Parse(stime[4])+(double.Parse(stime[5])/60))-(double.Parse(stime[1])+(double.Parse(stime[2])/60));
                        calendarDays.Rows.Add(new string[] { calendarid,calendarname, "0", "0", dayno[1], d.ToString(), "NO" });

                    }

                }
                
            }


        }
        
        private CalendarItem getCalendarItem(string clndr_data)
        {
            CalendarItem returnvalue;

            System.Collections.Generic.Stack<CalendarItem> stack1 = new System.Collections.Generic.Stack<CalendarItem>();


            long level=0;
            long[] currentlevel = new long[20];
            System.IO.StringReader sr = new System.IO.StringReader(clndr_data.Replace("(", "\n(").Replace(")", "\n)").Replace(" ", ""));
            string aline=sr.ReadLine();
            aline = sr.ReadLine();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Collections.Generic.List<CalendarItem> adam = new List<CalendarItem>();
            while (aline != null)
            {
               

                    if (aline.StartsWith("("))
                    {
                        string name = aline.Replace("(","");
                        CalendarItem pci = stack1.Count>0? stack1.Peek() as CalendarItem:null;
                        CalendarItem ci = new CalendarItem(level++, name, pci);
                        adam.Add(ci);
                        if (stack1.Count > 0)
                        {
                           stack1.Peek().addItem(ci);
                        }
                        stack1.Push(ci);
                        sw.WriteLine(ci.Name + "," + level.ToString() + "," + aline);
                        
                    }
                    else if (aline == ")")
                    {
                        if (stack1.Count > 1)
                        {
                            stack1.Pop();
                        }
                    }
                    
                
                aline = sr.ReadLine();
            }
            returnvalue = stack1.Pop();
            returnvalue.Adam.AddRange(adam);
            return returnvalue;
        }
        private void checkLevel(string level, string instring)
        { 
        }
        private string longArrayToString(long[] inarray)
        {
            string returnvalue = "";
            for (int i = 0; i < inarray.Length; i++)
            {
                returnvalue = returnvalue + inarray[i].ToString()+".";
            }

            return returnvalue;
        }

    }
}
