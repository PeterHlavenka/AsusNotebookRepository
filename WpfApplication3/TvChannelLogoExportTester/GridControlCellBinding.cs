using System.Windows;

namespace WpfApplication3
{
    /// <summary>
    /// Slouží k bindění v buňce syncfusního gridu. 
    /// Bez INotifyPropertyChanged by vznikal memory-leak.
    /// </summary>
    public class GridControlCellBinding //: PropertyChangedBase
    {
        private readonly UIElement[] m_items;
        private readonly HorizontalAlignment m_horizontalAlignment;
        private readonly VerticalAlignment m_verticalAlignment;

        public const string CellType = "DataBoundTemplate";

        public const string CellTemplate = "ImageTemplate";

        public GridControlCellBinding(UIElement item, 
                                        HorizontalAlignment hAlign = HorizontalAlignment.Center, 
                                        VerticalAlignment vAlign = VerticalAlignment.Top)
        {
            m_items = new [] {item };
            m_horizontalAlignment = hAlign;
            m_verticalAlignment = vAlign;
        }


        public UIElement[] Items
        {
            get { return m_items; }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return m_horizontalAlignment; }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return m_verticalAlignment; }
        }
    }
}
