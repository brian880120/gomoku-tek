﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tek.Gomoku.Service.Services
{
    public class WebSocketMessage
    {
        public string Type { get; set; }

        public object Payload { get; set; }
    }
}
