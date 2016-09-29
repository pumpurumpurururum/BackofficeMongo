using System;

namespace BaseCms.Events
{
    public class UserLoginEventArgs : EventArgs
    {
        public Guid UserId { get; set; }
    }
}
