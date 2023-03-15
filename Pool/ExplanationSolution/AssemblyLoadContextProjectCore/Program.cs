using System.IO.Pipes;
using System.Runtime.Loader;

// V tomto projektu musim mit nareferencovanou dll. Viz cesta nize:
var context = new AssemblyLoadContext("IsolatedPlace", true); 
var assembly = context.LoadFromAssemblyPath("d:\\AsusNotebookRepository\\Pool\\ExplanationSolution\\ClassLibraryAsDllCore\\bin\\Debug\\net6.0\\ClassLibraryAsDllCore.dll");

// Nasledujici kod neizoluje context pro nactene tridy a jejich zavislosti. Pouziti typeOf navic predpoklada, ze je nareferencovana dll na kterou je videt.
// var assembly = Assembly.LoadFile("d:\\AsusNotebookRepository\\Pool\\ExplanationSolution\\ClassLibraryAsDllCore\\bin\\Debug\\net6.0\\ClassLibraryAsDllCore.dll");
// var assembly = Assembly.GetAssembly(typeof(RsEval));

var programType = assembly.GetType("ClassLibraryAsDll.RsEval");

if (programType == null) return;

// Metoda bez parametru
var eval = Activator.CreateInstance(programType);
var methodInfo = programType.GetMethod("Foo");
methodInfo?.Invoke(eval, null);

// Metoda s parametrem, ktera vraci integer:
var multipleInfo = programType.GetMethod("Multiple");
var result = multipleInfo?.Invoke(eval, new object?[] { 3 });
Console.WriteLine("Result metody multiple(3) je: " + result);

// Event
var eventInfo = programType.GetEvent("ChangedEvent");
var handlerType = eventInfo?.EventHandlerType;
var handler = typeof(Test).GetMethod("MyHandler");

if (handlerType is not null && handler is not null)
{
    Delegate del = Delegate.CreateDelegate(handlerType, null, handler);
    eventInfo?.AddEventHandler(eval, del);

    var methodInfoInvoker = programType.GetMethod("OnChangedEvent");
    methodInfoInvoker?.Invoke(eval, null);  // Event tridy RsEval muzu tady vyhodit a dokonce mu zde priradit posluchace, ale nemuzu poslouchat event vyhozeny z RsEval..
    // methodInfoInvoker?.Invoke(eval, null);
    // methodInfoInvoker?.Invoke(eval, null);
    // methodInfoInvoker?.Invoke(eval, null);
}



// Ziskani parametru eventu. (sender, eventArgs)
var invokeMethod = handlerType?.GetMethod("Invoke");
var parameters = invokeMethod?.GetParameters();
if (parameters != null)
{
    var parameterTypes = new Type[parameters.Length];
    for (var i = 0; i < parameters.Length; i++)
    {
        parameterTypes[i] = parameters[i].ParameterType;
    }
}

// Spusteni okna v tezkem .netu
var wpfWindowContext = new AssemblyLoadContext("IsolatedPlace1", true); 
var wpfWindowAssembly = wpfWindowContext.LoadFromAssemblyPath(@"d:\AsusNotebookRepository\Pool\ExplanationSolution\ApplicationLibrary48\bin\Debug\ApplicationLibrary48.dll");
var wpfWindowStarter = wpfWindowAssembly.GetType("ApplicationLibrary48.Starter");
if (wpfWindowStarter == null) return;
// Metoda bez parametru
var starter = Activator.CreateInstance(wpfWindowStarter);
var starterMethodInfo = wpfWindowStarter.GetMethod("Start");
starterMethodInfo?.Invoke(starter, null);


// Tato trida, resp. jeji metoda se pouzije jako handler pro ChangedEvent ze tridy RSEval, jejiz instanci jsme si vyse vytvorili pomoci Activatoru.
class Test
{
    public static void MyHandler(object sender, EventArgs args)
    {
        Console.WriteLine("Hi z testu");
    }
}
