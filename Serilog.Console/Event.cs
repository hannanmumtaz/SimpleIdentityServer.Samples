namespace Serilog.Console
{
    public class Event
    {
        /// <summary>
        /// Identity the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set the category
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Message displayed
        /// </summary>
        public string Message { get; set; }
    }
}
