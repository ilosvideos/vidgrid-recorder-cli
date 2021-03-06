﻿using obs_cli.Data;
using obs_cli.Objects.Obs;
using obs_cli.Services.Recording.Objects;
using System;

namespace obs_cli.Services.Recording.Abstract
{
    public abstract class BaseRecordingService : IBaseRecordingService
    {
        public uint FrameRate { get; set; }

        public string VideoOutputFolder { get; set; }

        public BaseRecordingService(BaseRecordingParameters parameters)
        {
            FrameRate = parameters.FrameRate;
            VideoOutputFolder = parameters.VideoOutputFolder;
        }

        public abstract void Setup();

        public bool StartRecording()
        {
            Store.Data.Record.VideoOutputFolder = VideoOutputFolder;

            Setup();

            try
            {
                ObsOutputAndEncoders outputAndEncoders = ObsService.CreateNewObsOutput();
                Store.Data.Record.OutputAndEncoders = outputAndEncoders;
                var isStarted = Store.Data.Record.OutputAndEncoders.obsOutput.Start();

                Store.Data.Record.IsPaused = !isStarted;

                return isStarted;
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }
        }
    }
}
