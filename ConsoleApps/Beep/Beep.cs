using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.Beep
{
    public class Beep
    {
        readonly Dictionary<int, int> _sounds = new Dictionary<int, int>
        {
            { 1, 300},
            { 2, 400},
            { 3, 500},
            { 4, 600},
            { 5, 700 },
            { 6, 800 },
            { 7, 900 },
            { 8, 1000 },
            { 9, 1100 },
            { 0, 1200},
        };
        public void Start()
        {
            while (true)
            {
                var input = Console.ReadKey();
                if(!int.TryParse(input.KeyChar.ToString(), out int key) || key > 9) continue;
                Console.Beep(_sounds[key] * 2, 100);
            }
        }
    }
}
