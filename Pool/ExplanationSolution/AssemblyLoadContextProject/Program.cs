using System.Runtime.Loader;

// V tomto projektu musim mit nareferencovanou dll. Viz cesta nize:
var context = new AssemblyLoadContext("MyDomain", true); 
var assembly = context.LoadFromAssemblyPath("d:\\AsusNotebookRepository\\Pool\\ExplanationSolution\\ClassLibraryAsDll\\bin\\Debug\\net6.0\\ClassLibraryAsDll.dll");

// Nasledujici kod neizoluje context pro nactene tridy a jejich zavislosti. Pouziti typeOf navic predpoklada, ze je nareferencovana dll na kterou je videt.
// var assembly = Assembly.LoadFile("d:\\AsusNotebookRepository\\Pool\\ExplanationSolution\\ClassLibraryAsDll\\bin\\Debug\\net6.0\\ClassLibraryAsDll.dll");
// var assembly = Assembly.GetAssembly(typeof(RsEval));

var programType = assembly?.GetType("ClassLibraryAsDll.RsEval");

if (programType == null) return;

// Metoda bez parametru
var eval = Activator.CreateInstance(programType);
var methodInfo = programType.GetMethod("Foo");
methodInfo?.Invoke(eval, null);

// Metoda s parametrem, ktera vraci integer:
var multipleInfo = programType.GetMethod("Multiple");
var result = multipleInfo?.Invoke(eval, new object?[] { 3 });
Console.WriteLine("Result metody multiple(3) je: " + result);