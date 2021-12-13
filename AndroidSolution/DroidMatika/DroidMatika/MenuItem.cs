using System;
using Xamarin.Forms;

namespace DroidMatika
{
    public class MenuItem : BindableObject
    {
        private readonly ParamsSource m_paramsSource;
        private bool m_isChecked;
        private readonly bool? m_negativeNumbersAllowed;
        private readonly bool? m_decimalNumbersAllowed;

        // getExampleFunc = tuto funkci dostane menuItem pri svem vzniku napr d => new SumExample(d), kde d je paramsSource
        public MenuItem(string name, ParamsSource paramsSource, Func<ParamsSource , ExampleBase> getExample = null, bool? negativeNumbersAllowed = null,
             bool? decimalNumbersAllowed = null, bool isChecked = false)
        {
            GetExampleFunc = getExample;
            IsChecked = isChecked;
            m_paramsSource = paramsSource;
            m_negativeNumbersAllowed = negativeNumbersAllowed;
            m_decimalNumbersAllowed = decimalNumbersAllowed;

            Name = name;
        }

        public string Name { get; }
        private Func<ParamsSource, ExampleBase> GetExampleFunc { get; }

        public bool IsChecked
        {
            get => m_isChecked;
            set
            {
                m_isChecked = value;
                
                if (m_paramsSource != null && m_negativeNumbersAllowed == true)
                {
                    m_paramsSource.NegativeNumbersAllowed = m_isChecked;
                }
                if (m_paramsSource != null && m_decimalNumbersAllowed == true)
                {
                    m_paramsSource.DecimalNumbersAllowed = m_isChecked;
                }
            }
        }


        public ExampleBase GetExample()
        {
            return GetExampleFunc.Invoke(m_paramsSource);
        }
    }
}