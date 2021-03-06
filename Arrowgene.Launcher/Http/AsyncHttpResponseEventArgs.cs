﻿using System;

namespace Arrowgene.Launcher.Http
{
    public class AsyncHttpResponseEventArgs : EventArgs
    {
        public AsyncHttpResponseEventArgs(byte[] response)
        {
            Response = response;
        }
        public byte[] Response { get; private set; }
    }
}