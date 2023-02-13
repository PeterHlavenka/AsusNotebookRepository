namespace CustomerInfo;

public static class CustomerLoaderProvider
{
    private static readonly CustomerLoader m_instance = new();
    
    public static CustomerLoader GetCustomerLoader()
    {
        return m_instance;
    }
}