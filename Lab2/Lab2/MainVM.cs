﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class MainVM : INotifyPropertyChanged
    {

        private AppSettings settings = new AppSettings();

        public AppSettings Settings
        {
            get { return settings; }
            set { settings = value; NotifyProperty(); }
        }

        private FontList fonts = new FontList();

        public FontList Fonts
        {
            get { return fonts; }
            set { fonts = value; NotifyProperty(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyProperty([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
