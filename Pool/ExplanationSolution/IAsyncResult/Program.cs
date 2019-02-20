using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace IAsyncResult
{
    //NA CALLBACK SE PROSTE NEDA POCKAT POKUD JE NA THREADU Z THREADPOOLU. DA SE ALE VYHODIT EVENT POMOCI AGGREGATORA A PORESIT DAL. VIZ ZACHYTAVADLO 
    public class AsyncDemo : IDisposable
    {
        // The method to be executed asynchronously.
        public static string TestMethod(int count)
        {
            Console.WriteLine("V novem vlakne se spustila testovaci metoda.");
            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i);
            }          
            return $"My call time was {count} seconds.";
        }

        public System.IAsyncResult BeginWaitForConnection(AsyncCallback callback)
        {
            var delegat = new AsyncMethodCallerDelegate(TestMethod);
            var asyncResult = delegat.BeginInvoke(3, null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            callback.BeginInvoke(asyncResult, null, null);
            return asyncResult;
        }

        // The delegate must have the same signature as the method it will call asynchronously.
        public delegate string AsyncMethodCallerDelegate(int callDuration);


        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
    }




    public class AsyncMain
    {
        // StartPoint
        public static void Main()
        {
            Console.WriteLine($"Vyber moznost: 1 = pomoci delegat.BeginInvoke(int, callback), 2 = pomoci AsyncResult.AsyncWaitHandle.WaitOne()");
            if (Console.ReadLine() == "1")
            {
                DelegateMain();
            }
            if (Console.ReadLine() == "2")
            {
                PomociWaitOne();
            }
            if (Console.ReadLine() == "3")
            {
                PomociManualResetEventu();
            }
            else
            {
                PomociTasku();
            }
        }


        #region DelegateRegion
        /// <summary>
        /// 1. zpusob pomoci delegata
        /// Vytvorim delegata, spustim ho v jinem vlakne a pomoci EndInvoke ziskam vysledky. EndInvoke musi mit nejakou moznost jak zjistit pro kterou asynchronni operaci ma vrati vysledky.
        /// Predame ji proto objekt kt. jsem ziskali z metody BeginInvoke
        /// </summary>
        private static void DelegateMain()
        {
            AsyncDemo.AsyncMethodCallerDelegate delegat = AsyncDemo.TestMethod;             // Vytvorim instanci delegata ktery ukazuje na metodu z tridy AsyncDemo

            var asyncResult = delegat.BeginInvoke(2, ar =>
            {
                bool pipedisposed = false;

                try
                {
                    Console.WriteLine($"Callback Zacal");
                    throw new ObjectDisposedException("dis");
                    Console.WriteLine($"Callback skoncil");
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e);
                    pipedisposed = true;
                }
            }, null);     // Spustim tuto metodu na novem vlakne.  Parametry jsou int = pocet sekund, callback ktery se ma zavolat po skonceni metody

            Console.WriteLine("Main thread pokracuje dal");


            var returnValue = delegat.EndInvoke(asyncResult);       // Pockej na ukonceni asynchronniho volani a dostan vysledky, coz je vypis My call time was...  
                                                                    //var returnValue =  asyncResult.AsyncWaitHandle.WaitOne();  // Stejny vysledek jako radek vyse. Udela se prace, uvolni main a ten pokracuje soucasne s callbackem.

            //Main thread uz pokracuje. Metoda na kterou delegat ukazoval je hotova, ale callback nikoli. Ten se ted provadi soucasne s main threadem. 
            //Po tomto vypise proved metodu kterou jsme predali vlaknu jako callback - tj. vypis "Callback skoncil"  

            Console.WriteLine("Pockali jsme na dokonceni delegata.    Ted se provede Callback.    The call return's value \"{0}\".", returnValue);
            Console.ReadLine();
        }
        #endregion

        private static void PomociWaitOne()
        {
            var pipedisposed = false;

            var ad = new AsyncDemo();
            var asyncResult = ad.BeginWaitForConnection(ar =>
            {
                //toto je callback


                try
                {
                    Console.WriteLine($"Callback Zacal");
                    //Thread.Sleep(5000000);
                    Console.WriteLine($"disposuju objekt");
                    throw new ObjectDisposedException("dis");
                    Console.WriteLine($"Callback skoncil");
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e);
                    pipedisposed = true;
                }
            });

            var neco = asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3), true);

            Console.WriteLine($"jsem za WaitOne() a pipeDisposed je {pipedisposed}");

            if (neco)       // WaitOne s casem vraci true pokud byl WaitHandle signalizovany, false pokud uplynul dany cas.
            {
                Console.WriteLine($"AsyncWaitHandle byl signalizovany a muzeme pokracovat");
            }
            else
            {
                Console.WriteLine($"AsyncWaitHandle nebyl signalizovany, ale uplynul nastaveny cas tak jedeme dal");
            }


            if (pipedisposed)
            {
                Console.WriteLine($" pipe byla disposovana v callbacku a ja o to za volanim WaitOne() vim. ");
            }
            else
            {
                Console.WriteLine($" pipe nebyla disposovana v callbacku a vse probehlo v poradku. ");
            }

            Console.ReadLine();
        }

        private static void PomociManualResetEventu()
        {           
            var pipedisposed = false;

            ManualResetEvent manual = new ManualResetEvent(false);

            var add = new AsyncDemo();
            add = null;


            try
            {
                add.BeginWaitForConnection(ar =>
                {
                    //toto je callback
                    try
                    {
                        Console.WriteLine($"Callback Zacal");
                        //Thread.Sleep(5000000);
                        Console.WriteLine($"disposuju objekt");
                        throw new ObjectDisposedException("dis");
                        Console.WriteLine($"Callback skoncil");
                    }
                    catch (ObjectDisposedException e)
                    {
                        Console.WriteLine(e);
                       // pipedisposed = true;
                    }
                });
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine($"chytnuto ve vnejsim try");
                pipedisposed = true;
            }
            catch (NullReferenceException)
            {
                pipedisposed = true;
            }
            finally
            {
                manual.Set();
            }

            

            var neco = manual.WaitOne(TimeSpan.FromSeconds(10), true);

            Console.WriteLine($"jsem za WaitOne() a pipeDisposed je {pipedisposed}");

            if (neco)       // WaitOne s casem vraci true pokud byl WaitHandle signalizovany, false pokud uplynul dany cas.
            {
                Console.WriteLine($"AsyncWaitHandle byl signalizovany a muzeme pokracovat");
            }
            else
            {
                Console.WriteLine($"AsyncWaitHandle nebyl signalizovany, ale uplynul nastaveny cas tak jedeme dal");
            }


            if (pipedisposed)
            {
                Console.WriteLine($" pipe byla disposovana v callbacku a ja o to za volanim WaitOne() vim. ");
            }
            else
            {
                Console.WriteLine($" pipe nebyla disposovana v callbacku a vse probehlo v poradku. ");
            }

            Console.ReadLine();
        }

        private static void PomociTasku()
        {
            var pipedisposed = false;

            var ad = new AsyncDemo();

            Task t = new Task(() =>
            {
                ad.BeginWaitForConnection(ar =>
                {
                    //toto je callback


                    try
                    {
                        Console.WriteLine($"Callback Zacal");
                        //Thread.Sleep(5000000);
                        Console.WriteLine($"disposuju objekt");
                        throw new ObjectDisposedException("dis");
                        Console.WriteLine($"Callback skoncil");
                    }
                    catch (ObjectDisposedException e)
                    {
                        Console.WriteLine(e);
                        pipedisposed = true;
                    }
                });
                Console.WriteLine("koncim s praci tasku. Mam hotovy Callback?");
            });

            t.Start();


            t.Wait();

            Console.WriteLine($"jsem za WaitOne() a pipeDisposed je {pipedisposed}");

            if (pipedisposed)
            {
                Console.WriteLine($" pipe byla disposovana v callbacku a ja o to za volanim WaitOne() vim. ");
            }
            else
            {
                Console.WriteLine($" pipe nebyla disposovana v callbacku a vse probehlo v poradku. ");
            }

            Console.ReadLine();
        }
    }
}