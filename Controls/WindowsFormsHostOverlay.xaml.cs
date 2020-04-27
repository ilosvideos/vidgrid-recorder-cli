﻿using System;
using System.Windows;
using System.Windows.Controls;
using Control = System.Windows.Forms.Control;

namespace obs_cli.Controls
{
    /// <summary>
    /// Interaction logic for WindowsFormsHostOverlay.xaml
    /// </summary>
    public partial class WindowsFormsHostOverlay : Window
    {
        Border t;

        public WindowsFormsHostOverlay(Border target, Control child)
        {
            InitializeComponent();

            t = target;
            wfh.Child = child;

            Owner = Window.GetWindow(t);

            Owner.LocationChanged += new EventHandler(EventHandler);
            t.SizeChanged += new SizeChangedEventHandler(EventHandler);
            PositionAndResize();

            if (Owner.IsVisible)
                Show();
            else
                Owner.IsVisibleChanged += delegate
                {
                    if (Owner.IsVisible)
                        Show();
                };
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Owner.LocationChanged -= new EventHandler(EventHandler);
            t.SizeChanged -= new SizeChangedEventHandler(EventHandler);
        }

        private void EventHandler(object sender, EventArgs e)
        {
            PositionAndResize();
        }

        public void PositionAndResize()
        {
            // todo: set padding to correct size. bring DpiUtil over
            //double padding = DpiUtil.ConvertSizeSystemDpiToMonitorDpi(Owner, ((Border)(t.Parent)).Padding.Left);
            double padding = 4.0;

            Left = Owner.Left + padding;
            Top = Owner.Top + padding;

            // Don't allow negatives
            Width = Math.Max((Owner.Width - 2 * padding), 0);
            Height = Math.Max((Owner.Height - 2 * padding), 0);
        }
    };

}