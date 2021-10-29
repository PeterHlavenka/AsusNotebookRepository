using System;

namespace DroidMatika
{
    public class MenuItem
    {
        private int m_difficulty;

        public MenuItem(string name, Func<int, ExampleBase> getExample = null, bool isChecked = false)
        {
            Name = name;
            GetExampleFunc = getExample;
            IsChecked = isChecked;
            m_difficulty = 1;
        }

        public string Name { get; }
        private Func<int, ExampleBase> GetExampleFunc { get; }
        public bool IsChecked { get; set; }

        public void SetDifficulty(int difficulty)
        {
            m_difficulty = difficulty;
        }
        
        
        public ExampleBase GetExample()
        {
            return GetExampleFunc.Invoke(m_difficulty);
        }
    }
}