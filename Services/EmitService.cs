﻿using obs_cli.Enums;
using obs_cli.Helpers;
using obs_cli.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace obs_cli.Services
{
    // todo: maybe break this into two separate services - EmitOutputService and EmitSerializedOutputService
    public static class EmitService
    {
        /// <summary>
        /// Emits the audio input magnitude level to standard output.
        /// </summary>
        /// <param name="magnitude"></param>
        public static void EmitInputMagnitude(AudioMagnitudeParameters parameters)
        {
            EmitOutput(AvailableCommand.AudioInputMagnitude, parameters.ToDictionary());
        }

        /// <summary>
        /// Emits the audio output magnitude level to standard output.
        /// </summary>
        /// <param name="magnitude"></param>
        public static void EmitOutputMagnitude(AudioMagnitudeParameters parameters)
        {
            EmitOutput(AvailableCommand.AudioOutputMagnitude, parameters.ToDictionary());
        }

        /// <summary>
        /// Emits the stop recording status to standard output.
        /// </summary>
        /// <param name="parameters"></param>
        public static void EmitStopRecordingStatus(StopRecordingStatusParameters parameters)
        {
            EmitOutput(AvailableCommand.StopRecording, parameters.ToDictionary());
        }

        /// <summary>
        /// Emits the list of all available audio input devices.
        /// </summary>
        /// <param name="audioDeviceList"></param>
        public static void EmitAudioInputDevices(AudioDeviceList audioDeviceList)
        {
            EmitSerializedOutput(AvailableCommand.GetAudioInputDevices, audioDeviceList);
        }

        /// <summary>
        /// Emits the list of all available audio output devices.
        /// </summary>
        /// <param name="audioDeviceList"></param>
        public static void EmitAudioOutputDevices(AudioDeviceList audioDeviceList)
        {
            EmitSerializedOutput(AvailableCommand.GetAudioOutputDevices, audioDeviceList);
        }

        /// <summary>
        /// Emits the list of all available webcam devices.
        /// </summary>
        /// <param name="audioDeviceList"></param>
        public static void EmitWebcamDevices(WebcamDeviceList webcamDeviceList)
        {
            EmitSerializedOutput(AvailableCommand.GetWebcamDevices, webcamDeviceList);
        }
        
        /// Emits a true/false status response with an optional message.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public static void EmitStatusResponse(AvailableCommand command, bool status, string message = null)
        {
            var statusResponse = new StatusResponse
            { 
                IsSuccessful = status,
                Message = message
            };

            EmitSerializedOutput(command, statusResponse);
        }

        /// <summary>
        /// Emits the message type with the given data serialized as JSON.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="dataToSerialize"></param>
        private static void EmitSerializedOutput(AvailableCommand messageType, object dataToSerialize)
        {
            var serializedString = new JavaScriptSerializer().Serialize(dataToSerialize);
            Console.WriteLine($"{ messageType.GetDescription() } --response={ serializedString }");
        }

        /// <summary>
        /// Emits the message type with the parameters formatted in the form of CLI arguments.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="additionalParameters"></param>
        private static void EmitOutput(AvailableCommand messageType, IDictionary<string, string> additionalParameters = null)
        {
            var commandToExecute = new StringBuilder(messageType.GetDescription());

            if (additionalParameters != null)
            {
                StringBuilder parameterString = additionalParameters.Aggregate(new StringBuilder(), (stringBuilder, x) => stringBuilder.Append($" --{x.Key}={x.Value}"));
                commandToExecute.Append(parameterString);
            }

            Console.WriteLine(commandToExecute.ToString());
        }
    }
}
