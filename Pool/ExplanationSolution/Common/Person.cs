namespace Common
{
    public class Person
    {
        public Person(string name, string surName)
        {
            Name = name;
            SurName = surName;
        }

        public string Name { get; set; }
        public string SurName { get; set; }
    }
}