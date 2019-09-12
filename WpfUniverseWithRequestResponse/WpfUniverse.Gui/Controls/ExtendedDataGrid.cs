using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfUniverse.Gui.Controls
{
    public class ExtendedDataGrid : DataGrid
    {
        private List<object> mojeKolekce;

        public static readonly DependencyProperty BindableSelectedItemsProperty = DependencyProperty.Register(
            "BindableSelectedItems", typeof(IList), typeof(ExtendedDataGrid), new PropertyMetadata(default(IList), SelectedItemsChanged));

        private static bool m_selectionChanging;

        public ExtendedDataGrid()
        {
            
        }

        private static void SelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs d)      // Sem proleze z metody OnSelectionChanged
        {
            if (m_selectionChanging)                                                                                           // Tady potrebujeme false abychom mohli dal
            {
                return;                                                                                                        // Pri prvnim spusteni tady koncime a jdeme znovu do metody OnSelectionChanged
            }                                                                                                                  // Prolezeme i set v kolekci Bindable a v OnSel prohodime ve finaly boolean.

            ExtendedDataGrid dataGrid = dependencyObject as ExtendedDataGrid;                                                  // Nacastujeme si nas dependency objekt jako dataGrid 

            if (dataGrid != null)
            {
                try
                {
                    m_selectionChanging = true;
                    
                    IList selectedItems = (IList) d.NewValue;                                  // Nova hodnota  predana v argumentu metody bude IList   selectedItems 
                   

                    dataGrid.SelectedItems.Clear();                                            // Tim ze nas dependency objekt ma predka , ma i kolekci SelectedItems . Vyprazdnime ji.

                    foreach (var item in selectedItems)                                        // Pro kazdou polozku z Listu ( neboli z d.NewValue )
                    {
                        dataGrid.SelectedItems.Add(item);                                      // Pridej polozku do kolekce SelectedItems na datagridu.   Ty budou vybrane.
                        /*BindableSelectedItems.Add(item)*/;       // STATIC
                    }

                    
                }
                finally
                {
                    m_selectionChanging = false;                                               // Nezapomenout na boolean.
                }
            }
        }

        // Metoda vyvolana kliknutim na polozku datagridu  event
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)                       // Kliknutim do gridu vlezeme sem
        {
            base.OnSelectionChanged(e);                                                               // Zavolame event rodice 

            if (m_selectionChanging)                                                                  // Pokud je boolean true vratime se 
            {
                return;
            }

            try                                                                                        // Jinak
            {                                                 
                m_selectionChanging = true;                                                            // Boolean zmenime na true

                ArrayList items = new ArrayList();

                foreach (var selectedItem in SelectedItems)                                            // Projdeme kolekci na rodicovske tride  SelectedItems . 
                {
                    items.Add(selectedItem);                                                           // Do prazdne kolekce si pridame obsah rodicovske kolekce .
                    //BindableSelectedItems.Add(selectedItem);
                    
                }

                BindableSelectedItems = items;     //TOHLE UZ JE NULL     (reference ??)               // Rekneme , ze kolekce BindableSelectedItems (na kterou je navazana kolekce
                Console.WriteLine($@"Pocet : {BindableSelectedItems?.Count}");                                                                                      // v GalaxyViewModelu (na kterou binduje xaml))  bude nase  pozbirana kolekce
            }
            finally
            {
                m_selectionChanging = false;                                                            // Nakonec prohodime boolean aby se kolekce porad neprepisovala
            }
        }


        // Tato kolekce je stejna jako ta v GalaxyViewModelu (SelectedGalaxies)
        public  IList BindableSelectedItems                                                             // 
        {
            get { return (IList) GetValue(BindableSelectedItemsProperty); }                             // Vraci hodnotu nasi dependency property v podobe listu
            set
            {
                SetValue(BindableSelectedItemsProperty, value);                                         // Muze dostat list ktery ulozi do dependency propr

            }                    
        }
    }
}