﻿using System;
using System.Collections.Generic;

namespace Client.Model
{
    public class GameInfo
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Length { get; set; }
        public int Place { get; set; }
        public List<GamePlayer> Enemies { get; set; }
    }
}
