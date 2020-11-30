using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Ai
{
    public class Snake
    {
        public List<Circle> Body { get; set; } = new List<Circle>();
        public Direction Direction { get; set; }


        public Snake Clone()
        {
            return new Snake { Body = Body.ToList(), Direction = Direction };
        }
    }
}