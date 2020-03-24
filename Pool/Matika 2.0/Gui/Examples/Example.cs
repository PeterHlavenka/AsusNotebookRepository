﻿using System;
using System.Collections.Generic;
using Matika.Gui;
using Matika.Settings;

namespace Matika.Examples
{
    public class Example
    {
        public int Result { get; protected set; }
        public string TaskString { get; set; }
        protected static string EqualSign => " =  ";
        private static int Diff { get; set; }


        public Example Generate(MatikaSettingsViewModel settings)
        {
            Diff = settings.Difficulty;

            List<int> list = new List<int>();

            for (int i = 0; i < settings.AddCount; i++)
            {
                list.Add(0);
            }

            for (int i = 0; i < settings.DifferenceCount; i++)
            {
                list.Add(1);
            }

            for (int i = 0; i < settings.ProductCount; i++)
            {
                list.Add(2);
            }

            for (int i = 0; i < settings.DivideCount; i++)
            {
                list.Add(3);
            }

            int rand = 0;

            if (list.Count > 0)
            {
                int r = new Random().Next(list.Count);
                rand = list[r];
            }


            switch (rand)
            {
                case 0:
                    return new Addition(Diff);
                case 1:
                    return new Difference(Diff);
                case 2:
                    return new Product(Diff);
                case 3:
                    return new Share(Diff);
                default:
                    throw new ArgumentOutOfRangeException(nameof(rand));
            }
        }
    }
}