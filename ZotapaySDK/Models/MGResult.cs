namespace ZotapaySDK.Models
{
    /// <summary>
    /// Wrapper class with http and validation results
    /// </summary>
    public class MGResult
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        internal MGResult() { }

        /// <summary>
        /// Represents the Zotapay API response
        /// </summary>
        public MGResponse MGResponse { get; internal set; }

        /// <summary>
        /// Indicates wether the request object was valid and an actual http request was send
        /// </summary>
        public bool IsSuccess { get; internal set; }

        /// <summary>
        /// Error message(s) indicating reason of unsuccessful API request  
        /// </summary>
        public string ErrorList { get; internal set; }
    }
}
