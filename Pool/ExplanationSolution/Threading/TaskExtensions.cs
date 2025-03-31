namespace Threading
{
    public static class TaskExtensions
    {
        public static void ThrowIsFaulted(this Task task)
        {
            if (task.IsFaulted)
                throw task.Exception.GetBaseException();
        }
        
        /// <summary>
        /// Umoznuje volat asynchronni metodu v synchronnim kodu.
        /// POZOR: Kod v synchronni metode dale pokracuje a neceka na dokonceni asynchronni metody!
        /// </summary>
        /// <param name="task">Asynchronni metoda, ktera bude awaitnuta v try/catch bloku</param>
        /// <param name="exceptionAction">Co se ma provest s pripadnou vyjimkou z asynchronni metody (Mela by se zalogovat. Pripadne je mozne setnout null a vyjimku sezrat.)</param>
        /// <param name="configureAwait">Default true (.NET default), ale v naproste vetsine pripadu je spravne false :)</param>
        public static async void FireAndForgetSafeAsync(this Task task, Action<Exception> exceptionAction, bool configureAwait = true)
        {
            try
            {
                await task.ConfigureAwait(configureAwait);
            }
            catch (Exception e)
            {
                exceptionAction?.Invoke(e);
            }
        }
        
        /// <summary>
        /// Umoznuje volat asynchronni metodu v synchronnim kodu.
        /// POZOR: Kod v synchronni metode dale pokracuje a neceka na dokonceni asynchronni metody!
        /// </summary>
        /// <param name="task">Asynchronni metoda, ktera bude awaitnuta v try/catch bloku</param>
        /// <param name="exceptionAction">Co se ma provest s pripadnou vyjimkou z asynchronni metody (Mela by se zalogovat. Pripadne je mozne setnout null a vyjimku sezrat.)</param>
        /// <param name="configureAwait">Default true (.NET default), ale v naproste vetsine pripadu je spravne false :)</param>
        public static async void FireAndForgetSafeAsync(this Task task, Action<Exception, string, object[]> exceptionAction, bool configureAwait = true)
        {
            try
            {
                await task.ConfigureAwait(configureAwait);
            }
            catch (Exception e)
            {
                exceptionAction?.Invoke(e, e.Message, Array.Empty<object>());
            }
        }
        
        /// <summary>
        /// Umoznuje volat asynchronni metodu v synchronnim kodu.
        /// POZOR: Kod v synchronni metode dale pokracuje a neceka na dokonceni asynchronni metody!
        /// </summary>
        /// <param name="task">Asynchronni metoda, ktera bude awaitnuta v try/catch bloku</param>
        /// <param name="errorHandler">Co se ma provest s pripadnou vyjimkou z asynchronni metody (Mela by se zalogovat. Pripadne je mozne setnout null a vyjimku sezrat.)</param>
        /// <param name="configureAwait">Default true (.NET default), ale v naproste vetsine pripadu je spravne false :)</param>
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler errorHandler, bool configureAwait = true)
        {
            try
            {
                await task.ConfigureAwait(configureAwait);
            }
            catch (Exception e)
            {
                errorHandler?.HandleError(e);
            }
        }
        
        public static Task IgnoreUnobservedExceptions(this Task task)
        {
            Ignore(task);
            return task;
        }

        public static Task<T> IgnoreUnobservedExceptions<T>(this Task<T> task)
        {
            Ignore(task);
            return task;
        }

        private static async void Ignore(this Task task)
        {
            try { await task.ConfigureAwait(false); }
            catch
            {
                // ignored
            }
        }
    }
    
    public interface IErrorHandler
    {
        void HandleError(Exception error);
    }
}
