using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Lab2
{
    public class AppSettings : INotifyPropertyChanged
    {
		private double appHeight = 350;

		public double AppHeight
		{
			get { return appHeight; }
			set { appHeight = value; NotifyProperty(); }
		}

		private double appWidth = 800;

		public double AppWidth
		{
			get { return appWidth; }
			set { appWidth = value; NotifyProperty(); }
		}

		private double appTop = 250;

		public double AppTop
		{
			get { return appTop; }
			set { appTop = value; NotifyProperty(); }
		}

		private double appLeft = 250;

		public double AppLeft
		{
			get { return appLeft; }
			set { appLeft = value; NotifyProperty(); }
		}

        private const string appSettingsPath = "C:/Users/shant/source/repos/Labs/Lab2/settingsDefault.json";

        public string AppSettingsPath
		{
			get { return appSettingsPath; }
			// set { appSettingsPath = value; NotifyProperty(); }
		}


		private string lastSavedPath;

		public string LastSavedPath
		{
			get { return lastSavedPath; }
			set { lastSavedPath = value; NotifyProperty(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyProperty([CallerMemberName]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

    }
}
