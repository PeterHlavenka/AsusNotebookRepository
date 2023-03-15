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
        
        // OnChangedEvent();
        // ChangedEvent += MyHandler;
        
        Console.WriteLine(sb.ToString());
    }

    public int Multiple(int source)
    {
        
        return source * 2;
        
    }

    // public void SetDelegate(Action? action)
    // {
    //     MyAction = action;
    // }

    // public Action? MyAction { get; set; }

    public event EventHandler? ChangedEvent;

    public virtual void OnChangedEvent()
    {
        ChangedEvent?.Invoke(this, EventArgs.Empty);
    }
    
    // Aby bylo mozne 
    // public static void MyHandler(object sender, EventArgs args)
    // {
    //     throw new InvalidOperationException();
    // }
}