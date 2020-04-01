﻿using OBS;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace obs_cli.Commands.Implementations
{
    public class StartRecording : ICommand
    {
        public static string Name
        {
            get
            {
                return "start-recording";
            }
        }

        public string VideoOutputFolder { get; set; }

        public string lastVideoName;
        public List<FileInfo> recordedFiles = new List<FileInfo>();

        public StartRecording(IDictionary<string, string> arguments)
        {
            this.VideoOutputFolder = arguments["videoOutputFolder"];
        }

        public void Execute()
        {
            FileWriteService.WriteToFile("in start recording execute");

            try 
            {
                ObsOutputAndEncoders outputAndEncoders = CreateNewObsOutput();
                outputAndEncoders.obsOutput.Start();

                Store.Data.Obs.OutputAndEncoders = outputAndEncoders;

                FileWriteService.WriteToFile("recording started");
            }
            catch(Exception ex)
            {
                FileWriteService.WriteToFile(ex.Message);
            }
        }

        private ObsOutputAndEncoders CreateNewObsOutput()
        {
            ObsOutputAndEncoders outputAndEncoders = new ObsOutputAndEncoders();

            outputAndEncoders.obsVideoEncoder = createVideoEncoder();
            outputAndEncoders.obsAudioEncoder = createAudioEncoder();
            outputAndEncoders.obsOutput = createOutput();

            outputAndEncoders.obsOutput.SetVideoEncoder(outputAndEncoders.obsVideoEncoder);
            outputAndEncoders.obsOutput.SetAudioEncoder(outputAndEncoders.obsAudioEncoder);

            return outputAndEncoders;
        }

        private ObsEncoder createVideoEncoder()
        {
            ObsEncoder obsVideoEncoder = new ObsEncoder(ObsEncoderType.Video, "obs_x264", "simple_h264_stream");
            //obsVideoEncoder.Dispose();
            obsVideoEncoder.SetVideo(Obs.GetVideo());

            ObsData vEncoderSettings = new ObsData();
            vEncoderSettings.SetInt("bitrate", Constants.Video.ENCODER_BITRATE);
            vEncoderSettings.SetString("rate_control", Constants.Video.RATE_CONTROL);
            obsVideoEncoder.Update(vEncoderSettings);
            vEncoderSettings.Dispose();

            return obsVideoEncoder;
        }

        private ObsEncoder createAudioEncoder()
        {
            // mf_aac for W8 and later, ffmpeg_aac for W7
            string encoderId = "mf_aac";

            // Windows 7 is Version 6.1. Check if version 6.1 and below. We don't support anything below Windows 7. 
            if (System.Environment.OSVersion.Platform == PlatformID.Win32NT &&
                ((System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor <= 1) ||
                 System.Environment.OSVersion.Version.Major < 6)
            ) encoderId = "ffmpeg_aac";

            ObsEncoder obsAudioEncoder = new ObsEncoder(ObsEncoderType.Audio, encoderId, "simple_aac");
            obsAudioEncoder.SetAudio(Obs.GetAudio());

            ObsData aEncoderSettings = new ObsData();
            aEncoderSettings.SetInt("bitrate", Constants.Audio.ENCODER_BITRATE);
            aEncoderSettings.SetString("rate_control", Constants.Audio.RATE_CONTROL);
            aEncoderSettings.SetInt("samplerate", Constants.Audio.SAMPLES_PER_SEC);
            aEncoderSettings.SetBoolDefault("allow he-aac", true);
            obsAudioEncoder.Update(aEncoderSettings);
            aEncoderSettings.Dispose();

            return obsAudioEncoder;
        }

        private ObsOutput createOutput()
        {
            // Output
            string videoDirectory = $"{FolderService.GetPath(KnownFolder.Videos)}\\{VideoOutputFolder}";
            if (recordedFiles.Count == 0)
            {
                lastVideoName = $"ScreenRecording {DateTime.Now:yyyy-MM-dd HH.mm.ss}";
            }
            string videoFileName = lastVideoName + "_part " + (recordedFiles.Count + 1) + ".mp4";
            string videoFilePath = $"{videoDirectory}\\{videoFileName}";
            recordedFiles.Add(new FileInfo(videoFilePath));

            Directory.CreateDirectory(videoDirectory);
            videoFilePath = videoFilePath.Replace("\\", "/"); // OBS uses forward slashes

            ObsOutput obsOutput = new ObsOutput(ObsOutputType.Dummy, "ffmpeg_muxer", "simple_file_output");
            //obsOutput.Dispose();

            ObsData outputSettings = new ObsData();
            outputSettings.SetString("path", videoFilePath);
            outputSettings.SetString("muxer_settings", "movflags=faststart");
            obsOutput.Update(outputSettings);
            outputSettings.Dispose();

            return obsOutput;
        }
    }
}
