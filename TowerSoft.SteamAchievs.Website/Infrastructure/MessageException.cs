using System;

namespace TowerSoft.SteamAchievs.Website.Infrastructure {
    /// <summary>
    /// Exception type designed to just return a message to the user and not trigger any additional exception logic
    /// </summary>
    public class MessageException : Exception {
        public MessageException(string message) : base(message) { }
    }
}
