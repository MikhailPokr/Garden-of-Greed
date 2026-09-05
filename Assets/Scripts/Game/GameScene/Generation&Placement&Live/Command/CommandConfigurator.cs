using System.Collections.Generic;
using System.Linq;

namespace Garden
{
    public class CommandConfigurator
    {
        private readonly Dictionary<int, List<ICommand>> _commandList = new Dictionary<int, List<ICommand>>();
        public void AddInRange(int start, int end, params ICommand[] commands)
        {
            for (int i = start; i < end; i++)
            {
                AddInPosition(i, commands);
            }
        }
        public void AddInPosition(int position, params ICommand[] commands)
        {
            if (!_commandList.ContainsKey(position))
                _commandList.Add(position, new List<ICommand>());
            _commandList[position].AddRange(commands);
        }

        public Queue<ICommand[]> GetCommands()
        {
            int start = _commandList.Keys.Min();
            int fin = _commandList.Keys.Max();
            Queue<ICommand[]> commands = new Queue<ICommand[]>();
            for (int i = start; i <= fin; i++)
            {
                if (!_commandList.ContainsKey(i))
                    _commandList.Add(i, new List<ICommand>());
                commands.Enqueue(_commandList[i].ToArray());
            }

            return commands;
        }
    }
}