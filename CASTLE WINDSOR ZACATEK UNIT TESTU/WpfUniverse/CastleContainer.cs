using Castle.Windsor;

namespace WpfUniverse
{
    public class CastleContainer
    {
        private static IWindsorContainer m_conainer;
        private static readonly object Lock = new object();

        public static IWindsorContainer Container
        {
            get
            {
                lock (Lock)
                {
                    if (m_conainer == null)
                    {
                        m_conainer = new WindsorContainer();
                    }
                }

                return m_conainer;
            }
        }
    }
}
