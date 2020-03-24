using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using Entities;
using Mediaresearch.Framework.DataAccess.BLToolkit.Dao;
using Action = System.Action;

namespace Matika.Gui
{
    public class EnumeratedWordsViewModel : Screen

    {
        private static readonly Random Random = new Random();

        private string m_displayedName;
        private string[] m_enumChars;
        private string m_help;
        private IWord m_item;
        private readonly IBWordDao m_bWordDao;
        private readonly ILWordDao m_lWordDao;
        private readonly IMWordDao m_mWordDao;
        private readonly IPWordDao m_pWordDao;


        public EnumeratedWordsViewModel(IDaoSource daoSource)
        {
            m_bWordDao = daoSource.GetDaoByEntityType<IBWordDao, BWord, int>();
            m_lWordDao = daoSource.GetDaoByEntityType<ILWordDao, LWord, int>();
            m_mWordDao = daoSource.GetDaoByEntityType<IMWordDao, MWord, int>();
            m_pWordDao = daoSource.GetDaoByEntityType<IPWordDao, PWord, int>();

            EnumChars = new[] {"B", "L", "M", "P"};
            GetQueue(null);
            ChangeItem(Queue);
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
            string first = string.IsNullOrEmpty(parameter) ? EnumChars.Shuffle().First() : parameter;

            IWord[] test = null;

            switch (first)
            {
                case "B":

                    test = m_bWordDao.GetWords().ToArray();
                    break;
                case "L":
                    test = m_lWordDao.GetWords().ToArray();
                    break;
                case "M":
                    test = m_mWordDao.GetWords().ToArray();
                    break;
                case "P":
                    test = m_pWordDao.GetWords().ToArray();
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

        public void SettingsButtonClicked()
        {
            Console.WriteLine(@"Not implemented");
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
            for (int i = 0; i < instance.Length; ++i)
            {
                int j = Random.Next(i, instance.Length); // select a random j such that i <= j < instance.Length

                // swap instance[i] and instance[j]
                var x = instance[j];
                instance[j] = instance[i];
                instance[i] = x;
            }

            return instance;
        }
    }
}