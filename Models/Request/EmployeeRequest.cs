using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectASP01.Models.Request
{
    public class EmployeeRequest
    {
        public int id_332 { get; set; }
        public string Name_332 { get; set; }
        public string email_332 { get; set; }
        public string phone_332 { get; set; }
        public string position_332 { get; set; }
        public int departmentId_332 { get; set; }

        public EmployeeRequest() { }

        public EmployeeRequest(string Name_332, string email_332, string phone_332, string position_332, int departmentId_332)
        {
            this.Name_332 = Name_332;
            this.email_332 = email_332;
            this.phone_332 = phone_332;
            this.position_332 = position_332;
            this.departmentId_332 = departmentId_332;
        }
    }
}