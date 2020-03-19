using RDPCOMAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class RDPServer
    {
        static RDPSession currentSession = null;
        public void CreateSession()
        {
            currentSession = new RDPSession();
            Connect();
        }

        public RDPServer()
        {
            CreateSession();
        }

        public void Connect()
        {
            currentSession.OnAttendeeConnected += Incoming;
            currentSession.OnAttendeeDisconnected += Disconnect;
            currentSession.Open();
        }

        private static void Disconnect(object Guest)
        {
            currentSession.Pause();
        }

        public string GetConnectionString(String authString,
            string group, string password, int clientLimit)
        {
            IRDPSRAPIInvitation invitation =
                currentSession.Invitations.CreateInvitation
                (authString, group, password, clientLimit);
            return invitation.ConnectionString;
        }

        private static void Incoming(object Guest)
        {
            currentSession.Resume();
            IRDPSRAPIAttendee MyGuest = (IRDPSRAPIAttendee)Guest;
            MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_MAX;
        }
    }
}
