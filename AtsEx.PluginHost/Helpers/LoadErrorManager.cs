﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Automatic9045.AtsEx.PluginHost.ClassWrappers;

namespace Automatic9045.AtsEx.PluginHost.Helpers
{
    public static partial class LoadErrorManager
    {
        static LoadErrorManager()
        {
            if (InstanceStore.IsInitialized)
            {
                Initialize();
            }
            else
            {
                InstanceStore.Initialized += e => Initialize();
            }
        }

        private static void Initialize()
        {
            Errors = new LoadErrorList();
        }


        public static LoadErrorList Errors { get; private set; }


        public static void Throw(string text, string senderFileName, int lineIndex, int charIndex)
        {
            InstanceStore.BveHacker.LoadingProgressForm.ThrowError(text, senderFileName, lineIndex, charIndex);
        }

        public static void Throw(string text, string senderFileName, int lineIndex) => Throw(text, senderFileName, lineIndex, 0);

        public static void Throw(string text, string senderFileName) => Throw(text, senderFileName, 0);

        public static void Throw(string text) => Throw(text, "");

        public static void Throw(LoadError error)
        {
            InstanceStore.BveHacker.LoadingProgressForm.ThrowError(error);
        }
    }
}