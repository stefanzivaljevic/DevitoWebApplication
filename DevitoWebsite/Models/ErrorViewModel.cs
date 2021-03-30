using System;

namespace DevitoWebsite.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
