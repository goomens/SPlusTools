using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data
{
    public abstract partial class SPlusObject
    {
        internal dynamic Object;

        public College College { get; protected set; }

        public string ObjectId { get { return Object.ObjectId; } }
        public string HostKey { get { return Object.HostKey; } set { Object.HostKey = value; } }
        public string Name { get { return Object.Name; } set { Object.Name = value; } }
        public string Description { get { return Object.Description; } set { Object.Description = value; } }

        public string UserText1 { get { return Object.UserText1; } set { Object.UserText1 = value; } }
        public string UserText2 { get { return Object.UserText2; } set { Object.UserText2 = value; } }
        public string UserText3 { get { return Object.UserText3; } set { Object.UserText3 = value; } }
        public string UserText4 { get { return Object.UserText4; } set { Object.UserText4 = value; } }
        public string UserText5 { get { return Object.UserText5; } set { Object.UserText5 = value; } }

        public SPlusObject()
        {
            
        }
    }
}
