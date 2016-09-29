using System;

namespace BaseCms.Events
{
    public class EventSource
    {
        public delegate void LoginEventHandler(object sender, UserLoginEventArgs e);

        public event LoginEventHandler LoginEvent;

        protected internal virtual void OnLoginEvent(Guid userId)
        {
            var handler = LoginEvent;
            if (handler != null) handler(this, new UserLoginEventArgs
            {
                UserId = userId
            });
        }

        public delegate void LogoutEventHandler(object sender, EventArgs e);

        public event LogoutEventHandler LogoutEvent;

        protected internal virtual void OnLogoutEvent()
        {
            var handler = LogoutEvent;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
