using baUHInia.Playground.Model;

namespace baUHInia.Playground.Logic.Utils
{
    public class Config
    {
        public string Name { get; }
        public string Group { get; }
        public bool IsElement { get; }
        public Offset[] Offsets { get; }
        
        public Config(string name, string group, bool isElement, Offset[] offsets)
        {
            Name = name;
            Group = group;
            IsElement = isElement;
            Offsets = offsets;
        }
    }
}