using System;

namespace Arrowgene.Launcher.Http
{
    public class HttpRequestErrorArgs : EventArgs
    {
        public HttpRequestErrorArgs(Exception exception)
        {
            Exception = exception;
        }
        public Exception Exception { get; private set; }
    }
}