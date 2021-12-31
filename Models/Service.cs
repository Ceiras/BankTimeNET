using BankTimeNET.Models;
using System;

namespace BankTimeNET
{
    public class Service
    {
        public Service()
        {
        }

        public Service(DateTime date, string description, int requestTime, int doneTime, ServiceState state, User requestUser, User doneUser, Bank bank)
        {
            Date = date;
            Description = description;
            RequestTime = requestTime;
            DoneTime = doneTime;
            State = state;
            RequestUser = requestUser;
            DoneUser = doneUser;
            Bank = bank;
        }

        public int Id{ get; set; }
        public DateTime Date { get; set; }
        public String Description{ get; set; }
        public int RequestTime{ get; set; }
        public int DoneTime{ get; set; }
        public ServiceState State{ get; set; }
        public virtual User RequestUser{ get; set; }
        public virtual User? DoneUser { get; set; }
        public virtual Bank Bank { get; set; }
    }
}