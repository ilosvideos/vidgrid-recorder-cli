﻿using obs_cli.Objects;
using obs_cli.Services.Recording.Abstract;
using obs_cli.Services.Recording.Objects;
using System;

namespace obs_cli.Services.Recording
{
    public class MainRecordingService : BaseRecordingService
    {
        public int CropTop { get; set; }
        public int CropRight { get; set; }
        public int CropLeft { get; set; }
        public int CropBottom { get; set; }
        public double OutputWidth { get; set; }
        public double OutputHeight { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }

        public MainRecordingService(BaseRecordingParameters parameters)
            :base(parameters)
        {
            CropTop = parameters.CropTop;
            CropRight = parameters.CropRight;
            CropLeft = parameters.CropLeft;
            CropBottom = parameters.CropBottom;
            OutputWidth = parameters.OutputWidth;
            OutputHeight = parameters.OutputHeight;
            CanvasWidth = parameters.CanvasWidth;
            CanvasHeight = parameters.CanvasHeight;
            ScreenX = parameters.ScreenX;
            ScreenY = parameters.ScreenY;
        }

        public override void Setup()
        {
            Loggers.CliLogger.Trace("In MainRecordingService Setup");
            VideoService.ResetVideoInfo(new ResetVideoInfoParameters
            {
                CropTop = CropTop,
                CropRight = CropRight,
                CropLeft = CropLeft,
                CropBottom = CropBottom,
                FrameRate = FrameRate,
                OutputWidth = OutputWidth,
                OutputHeight = OutputHeight,
                CanvasWidth = CanvasWidth,
                CanvasHeight = CanvasHeight,
                ScreenX = ScreenX,
                ScreenY = ScreenY
            });
        }
    }
}
