﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaScout.Interfaces;
using MediaScoutGUI.Views;
using System.Windows;

namespace MediaScoutGUI.ViewModels
{
    public class MoviesViewModel : ITab
    {
        private UIElement _view = new MoviesView();

        public string Name
        {
            get { return "Movies"; }
        }


        public System.Windows.UIElement View { get { return _view; } }
    }
}