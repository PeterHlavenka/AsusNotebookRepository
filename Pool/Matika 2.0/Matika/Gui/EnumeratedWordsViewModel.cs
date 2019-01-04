using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using Matika.Data;
using Action = System.Action;

namespace Matika.Gui
{
    public class EnumeratedWordsViewModel : Screen

    {
        private static readonly Random Random = new Random();
        private int m_counter;

        private string m_displayedName;
        private string[] m_enumChars;
        private string m_help;
        private IWord m_item;

        public EnumeratedWordsViewModel()
        {
            EnumChars = new[] {"B", "L", "M"};
            GetQueue(null);
            ChangeItem(Queue);
        }

        public int Counter
        {
            get => m_counter;
            set
            {
                m_counter = value;
                NotifyOfPropertyChange();
            }
        }

        public string[] EnumChars
        {
            get => m_enumChars;
            set
            {
                m_enumChars = value;
                NotifyOfPropertyChange();
            }
        }

        private Queue<IWord> Queue { get; set; }

        public IWord Item
        {
            get => m_item;
            set
            {
                m_item = value;
                Counter++;
                NotifyOfPropertyChange();
            }
        }

        public string DisplayedName
        {
            get => m_displayedName;
            set
            {
                m_displayedName = value.Trim();
                NotifyOfPropertyChange();
            }
        }

        public string Help
        {
            get => m_help;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    m_help = "(" + value.Trim() + ")";
                }
                else
                {
                    m_help = value;
                }

                NotifyOfPropertyChange();
            }
        }

        public void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetQueue((string) e.AddedItems[0]);
            ChangeItem(Queue);
        }

        private void GetQueue(string parameter)
        {
            var dc = new EnumeratedWordsDBDataContext();

            var first = string.IsNullOrEmpty(parameter) ? EnumChars.Shuffle().First() : parameter;

            IWord[] test = null;

            switch (first)
            {
                case "B":
                    test = dc.B_Words.Select(d => d).ToArray();
                    break;
                case "L":
                    test = dc.L_Words.Select(d => d).ToArray();
                    break;
                case "M":
                    test = dc.M_Words.Select(d => d).ToArray();
                    break;
            }


            test = test.Shuffle();

            Queue = new Queue<IWord>(test);
        }

        private void ChangeItem(Queue<IWord> queue)
        {
            if (queue.Any())
            {
                Item = queue.Dequeue();
                DisplayedName = Item.CoveredName;

                if (Item.Help != null)
                {
                    Help = Item.Help;
                }
                else
                {
                    Help = string.Empty;
                }
            }
        }

        public async void LeftButtonClicked()
        {
            if (Item.IsEnumerated)
            {
                DisplayedName = Item.Name;

                var action = new Action(() =>
                {
                    Thread.Sleep(1500);
                    ChangeItem(Queue);
                });
                await Task.Run(action);
            }
        }

        public async void RightButtonClicked()
        {
            if (Item.IsEnumerated)
            {
                return;
            }
            DisplayedName = Item.Name;

            var action = new Action(() =>
            {
                Thread.Sleep(1500);
                ChangeItem(Queue);
            });
            await Task.Run(action);
        }
    }

    internal static class ExtensionMethods
    {
        private static readonly Random Random = new Random();

        /// <summary>
        ///     Performs an in-place shuffle of an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T[] Shuffle<T>(this T[] instance)
        {
            for (var i = 0; i < instance.Length; ++i)
            {
                var j = Random.Next(i, instance.Length); // select a random j such that i <= j < instance.Length

                // swap instance[i] and instance[j]
                var x = instance[j];
                instance[j] = instance[i];
                instance[i] = x;
            }

            return instance;
        }
    }
}