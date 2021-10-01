using System;

namespace DroidMatika
{
    public class MenuItem
    {
        public MenuItem(string name, Func<ExampleBase> getExample = null, bool isChecked = false)
        {
            Name = name;
            GetExampleFunc = getExample;
            IsChecked = isChecked;
        }

        public string Name { get; }
        private Func<ExampleBase> GetExampleFunc { get; }
        public bool IsChecked { get; set; }

        public ExampleBase GetExample()
        {
            return GetExampleFunc.Invoke();
        }
    }
}