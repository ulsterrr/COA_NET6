using Application.Wrappers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers.Concrete
{
    public class SuccessResponse : ISuccessResponse
    {
        public bool Success { get; } = true;
        private string _message;
        public string Message
        {
            get => _message ?? string.Empty;
            set => _message = value;
        }
        public int StatusCode { get; }

        public SuccessResponse()
        {

        }

        public SuccessResponse(int statuscode, string message)
        {
            StatusCode = statuscode;
            Message = message;
        }
    }
}
