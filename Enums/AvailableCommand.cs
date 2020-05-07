﻿using System.ComponentModel;

namespace obs_cli.Enums
{
    public enum AvailableCommand
    {
        [Description("audio-input-magnitude")]
        AudioInputMagnitude,
        [Description("audio-output-magnitude")]
        AudioOutputMagnitude,
        [Description("cancel-recording")]
        CancelRecording,
        [Description("change-webcam")]
        ChangeWebcam,
        [Description("delete-last-section")]
        DeleteLastSection,
        [Description("disable-webcam")]
        DisableWebcam,
        [Description("enable-webcam")]
        EnableWebcam,
        [Description("enable-webcam-only")]
        EnableWebcamOnly,
        [Description("get-audio-input-devices")]
        GetAudioInputDevices,
        [Description("get-audio-output-devices")]
        GetAudioOutputDevices,
        [Description("get-webcam-devices")]
        GetWebcamDevices,
        [Description("initialize")]
        Initialize,
        [Description("pause-recording")]
        PauseRecording,
        [Description("resume-recording")]
        ResumeRecording,
        [Description("start-recording")]
        StartRecording,
        [Description("stop-recording")]
        StopRecording,
        [Description("terminate")]
        Terminate,
        [Description("translate-webcam-window")]
        TranslateWebcamWindow,
        [Description("update-audio-input-device")]
        UpdateAudioInputDevice,
        [Description("update-audio-output-device")]
        UpdateAudioOutputDevice,
    }
}
