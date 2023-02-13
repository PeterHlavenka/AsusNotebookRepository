using System.Text;

namespace ClassLibraryAsDll;

public class RsEval : MyAbstractClass
{
    public override void Foo()
    {
        var domain = AppDomain.CurrentDomain.FriendlyName;

        var sb = new StringBuilder();
        sb.Append("jsem v metode Foo, nazev domeny je : ");
        sb.Append(domain);
        
        Console.WriteLine(sb.ToString());
    }

    public int Multiple(int source)
    {
        return source * 2;
    }
}