﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace P6FullHistory.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("d:\\temp\\out.csv")]
        public string OutputPath {
            get {
                return ((string)(this["OutputPath"]));
            }
            set {
                this["OutputPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SELECT *  from task where delete_date is null and proj_id in (select proj_id from" +
            " project where proj_short_name in ({0}))")]
        public string Tasks {
            get {
                return ((string)(this["Tasks"]));
            }
            set {
                this["Tasks"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SELECT distinct task_id,proj_id FROM  taskpred where  proj_id in (select proj_id " +
            "from project where proj_short_name in ({0}))")]
        public string TaskUnique {
            get {
                return ((string)(this["TaskUnique"]));
            }
            set {
                this["TaskUnique"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("select a.task_id ,                                                               " +
            "                       \r\na.proj_id ,                                            " +
            "                                                 \r\na.task_code ,                " +
            "                                                                           \r\na.t" +
            "ask_name,\r\na.status_code,                                                       " +
            "                                     \r\na.early_start_date ,                     " +
            "                                                               \r\na.early_end_dat" +
            "e,                                                                              " +
            "         \r\na.late_start_date ,                                                  " +
            "                                   \r\na.late_end_date,                           " +
            "                                                             \r\na.total_float_hr_" +
            "cnt,\r\na.remain_drtn_hr_cnt,                                                     " +
            "                              \r\nb.task_id as pred_task_id ,                     " +
            "                                                        \r\nb.proj_id as pred_proj" +
            "_id ,                                                                           " +
            "  \r\nc.pred_type,                                                                " +
            "                            \r\nc.lag_hr_cnt,                                     " +
            "                                                      \r\nb.task_code as pred_task" +
            "_code ,                                                                         " +
            "\r\nb.task_name as pred_task_name ,                                               " +
            "                          \r\nb.status_code as pred_status_code,\r\nb.early_start_da" +
            "te as pred_early_start_date ,                                                   " +
            "        \r\nb.early_end_date as pred_early_end_date ,                             " +
            "                                  \r\nb.late_start_date as pred_late_start_date , " +
            "                                                            \r\nb.late_end_date as" +
            " pred_late_end_date ,                                                           " +
            "      \r\nb.total_float_hr_cnt as pred_total_float_hr_cnt, \r\nb.remain_drtn_hr_cnt " +
            "as pred_remain_drtn_hr_cnt,   \r\nc.pred_type as pred_relation_type,              " +
            "                                       \r\ncase                                   " +
            "                                                                 \r\nwhen c.pred_t" +
            "ype=\'PR_SS\' then DATEDIFF ( day , a.early_start_date , b.early_start_date )     " +
            "           \r\nwhen c.pred_type=\'PR_FF\' then DATEDIFF ( day , a.early_end_date , b" +
            ".early_end_date )                    \r\nwhen c.pred_type=\'PR_SF\' then DATEDIFF ( " +
            "day , a.early_end_date , b.early_start_date )                  \r\nelse DATEDIFF (" +
            " day , a.early_start_date , b.early_end_date )                                  " +
            "         \r\nend + (c.lag_hr_cnt/8) as delta                                      " +
            "                                   \r\nfrom task as a,                            " +
            "                                                             \r\ntaskpred as c,   " +
            "                                                                                " +
            "       \r\ntask as b where (a.delete_date is null and b.delete_date is null and c." +
            "delete_date is null) and a.task_id=c.task_id and b.task_id=c.pred_task_id and a." +
            "proj_id  in (select proj_id from project where proj_short_name in ({0}))")]
        public string TaskPredecessor {
            get {
                return ((string)(this["TaskPredecessor"]));
            }
            set {
                this["TaskPredecessor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("select clndr_id,clndr_name,clndr_data from calendar where clndr_id in (select dis" +
            "tinct clndr_id from task where proj_id  in (select proj_id from project where pr" +
            "oj_short_name in ({0})))")]
        public string Calendar {
            get {
                return ((string)(this["Calendar"]));
            }
            set {
                this["Calendar"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ActivityList {
            get {
                return ((string)(this["ActivityList"]));
            }
            set {
                this["ActivityList"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ProjectCode {
            get {
                return ((string)(this["ProjectCode"]));
            }
            set {
                this["ProjectCode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DisregardActivity {
            get {
                return ((string)(this["DisregardActivity"]));
            }
            set {
                this["DisregardActivity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public long MaxLevel {
            get {
                return ((long)(this["MaxLevel"]));
            }
            set {
                this["MaxLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DBString {
            get {
                return ((string)(this["DBString"]));
            }
            set {
                this["DBString"] = value;
            }
        }
    }
}