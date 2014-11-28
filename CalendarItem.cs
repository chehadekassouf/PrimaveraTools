using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P6FullHistory
{
    class CalendarItem
    {
        private System.Collections.Generic.List<CalendarItem> children = new List<CalendarItem> ();
        private System.Collections.Generic.List<CalendarItem> adam = new List<CalendarItem>();
       
        private string name;
        private long uniqueid;
        private CalendarItem parent;
        public string Name { get { return name; } }
        public CalendarItem Parent { get { return parent; } }
        public System.Collections.Generic.List<CalendarItem>  Children { get { return children; } }
        public System.Collections.Generic.List<CalendarItem> Adam { get { return adam; } }
        public CalendarItem(long uniqueid,string name,CalendarItem parent)
        {
            this.name = name;
            this.uniqueid = uniqueid;
            this.parent = parent;
        }
        public void addItem(CalendarItem item)
        {
            children.Add(item);
            
        }
        public override string ToString()
        {
            return this.Name + ","+this.children.Count+","+this.uniqueid; 
        }
        public string FullFamily()
        {
            string returnvalue = "";
            CalendarItem ci=this;
            while (ci != null)
            {
                returnvalue = ci.Name + "," + returnvalue;
                ci=ci.Parent;
            }
            return returnvalue;
        }

    }
}
