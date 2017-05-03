﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Services
{
    public interface IGameJudgementService
    {
        bool Check(GameMove gameMove, GameMove[] occupiedPisition);
    }
}