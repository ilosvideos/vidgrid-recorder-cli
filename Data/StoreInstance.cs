﻿using obs_cli.Data.Modules;

namespace obs_cli.Data
{
    public class StoreInstance
    {
        public StoreInstance()
        {
            App = new App();
            Audio = new Audio();
            Display = new Display();
            Obs = new Obs();
            Pipe = new Pipe();
            Record = new Record();
            SelectionWindow = new SelectionWindow();
            Webcam = new Webcam();
        }

        public App App { get; set; }
        public Audio Audio { get; set; }
        public Display Display { get; set; }
        public Obs Obs { get; set; }
        public Pipe Pipe { get; set; }
        public Record Record { get; set; }
        public SelectionWindow SelectionWindow { get; set; }
        public Webcam Webcam { get; set; }

        /// <summary>
        /// Resets the Record store.
        /// </summary>
        public void ResetRecordModule()
        {
            var previousActiveScreen = Record.ActiveScreen;
            Record = new Record
            {
                ActiveScreen = previousActiveScreen
            };
        }
    }
}
