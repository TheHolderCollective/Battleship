﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipGame.Objects.Ships
{
    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;
            OccupationType = OccupationType.Submarine;
            IsPlaced = false;
            ShipType = ShipType.Submarine;
        }
    }
}
