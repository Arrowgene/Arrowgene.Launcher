using System;

namespace Arrowgene.Launcher.Http
{
    public class HttpRequestProgressArgs : EventArgs
    {
        public HttpRequestProgressArgs(long current, long total)
        {
            Total = total;
            Current = current;
        }
        public long Total { get; private set; }
        public long Current { get; private set; }
    }
}