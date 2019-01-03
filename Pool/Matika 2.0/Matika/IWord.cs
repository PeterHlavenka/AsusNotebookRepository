namespace Matika
{
    public interface IWord
    {
        string Name { get; set; }
        string CoveredName { get; set; }
        bool IsEnumerated { get; set; }
        string Help { get; set; }
    }
}