namespace Serilog.Console
{
    public class EventSource
    {
        private static class Tasks
        {
            public const string Authorization = "Authorization";
        }

        private const string MessagePattern = "{Id} : {Task}, {Message}";

        #region Constructor
        

        #endregion

        #region Public methods

        public void StartAuthorization(
            string clientId,
            string responseType,
            string scope,
            string individualClaims)
        {
            var evt = new Event
            {
                Id = 1,
                Task = Tasks.Authorization,
                Message = $"Start the authorization process for the client : {clientId}, response type : {responseType}, scope : {scope} and claims : {individualClaims}"
            };
            LogInformation(evt);
        }

        #endregion

        #region Private methods

        private void LogInformation(Event evt)
        {
            Log.Information(MessagePattern, evt.Id, evt.Task, evt.Message);
        }

        #endregion
    }
}
