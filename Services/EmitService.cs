﻿using NamedPipeWrapper;
using obs_cli.Data;
using obs_cli.Enums;
using obs_cli.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using vidgrid_recorder_data;

namespace obs_cli.Services
{
    public static class EmitService
    {
        /// <summary>
        /// Emits the thrown exception.
        /// </summary>
        /// <param name="exceptionMessage"></param>
        /// <param name="stackTrace"></param>
        public static void EmitException(string commandName, string exceptionMessage, string stackTrace)
        {
            var exceptionThrownParameters = new ExceptionThrownParameters
            {
                CommandName = commandName,
                Message = exceptionMessage,
                StackTrace = stackTrace
            };

            ThrowException(AvailableCommand.ExceptionThrown, exceptionThrownParameters.ToDictionary());
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
        /// Emits status response for enabling webcam.
        /// </summary>
        /// <param name="webcamValue"></param>
        /// <param name="command"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public static void EmitEnableWebcamResponse(AvailableCommand command, string webcamValue, bool status, string message = null)
        {
            var statusResponse = new EnableWebcamOnlyResponse
            {
                IsSuccessful = status,
                Message = message,
                WebcamEnabledValue = webcamValue
            };

            EmitSerializedOutput(command, statusResponse);
        }

        /// <summary>
        /// Emits serialized status response for stop recording command.
        /// </summary>
        /// <param name="videoFilePath"></param>
        /// <param name="lastVideoName"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public static void EmitStopRecordingStatusResponse(string videoFilePath, string lastVideoName, bool status, string message = null)
        {
            var stopRecordingResponse = new StopRecordingResponse()
            {
                VideoFilePath = videoFilePath,
                LastVideoName = lastVideoName,
                IsSuccessful = status,
                Message = message
            };

            EmitSerializedOutput(AvailableCommand.StopRecording, stopRecordingResponse);
        }

        /// <summary>
        /// Emits a status response for deleting the last section of the video.
        /// </summary>
        /// <param name="numberOfPartsLeft"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public static void EmitDeleteLastSectionResponse(int numberOfPartsLeft, bool status, string message = null)
        {
            var statusResponse = new DeleteLastSectionResponse
            {
                IsSuccessful = status,
                Message = message,
                NumberOfPartsLeft = numberOfPartsLeft
            };

            EmitSerializedOutput(AvailableCommand.DeleteLastSection, statusResponse);
        }

        /// <summary>
        /// Emits a response with webcam window properties.
        /// </summary>
        /// <param name="webcamWindowProperties"></param>
        public static void EmitWebcamWindowProperties(WebcamWindowProperties webcamWindowProperties)
        {
            EmitSerializedOutput(AvailableCommand.GetWebcamWindowProperties, webcamWindowProperties);
        }

        /// <summary>
        /// Emits a response with initialization status.
        /// </summary>
        /// <param name="response"></param>
        public static void EmitInitializeResponse(InitializeResponse response)
        {
            EmitSerializedOutput(AvailableCommand.Initialize, response);
        }

        /// <summary>
        /// Emits the message type with the given data serialized as JSON.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="dataToSerialize"></param>
        private static void EmitSerializedOutput(AvailableCommand messageType, object dataToSerialize)
        {
            var serializedString = new JavaScriptSerializer().Serialize(dataToSerialize);
            Store.Data.Pipe.Main.PushMessage(new Message() { Text = $"{ messageType.GetDescription() } --response={ serializedString }" });
            Loggers.CliLogger.Trace($"Emitting response for {messageType.GetDescription()}");
        }

        /// <summary>
        /// Emits the message type with the parameters to the standard error output.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="additionalParameters"></param>
        private static void ThrowException(AvailableCommand messageType, IDictionary<string, string> additionalParameters = null)
        {
            Console.Error.WriteLine(BuildParameterizedOutput(messageType, additionalParameters));
        }

        /// <summary>
        /// Builds and formats the output response.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static string BuildParameterizedOutput(AvailableCommand messageType, IDictionary<string, string> parameters = null)
        {
            var commandToExecute = new StringBuilder(messageType.GetDescription());

            if (parameters != null)
            {
                StringBuilder parameterString = parameters.Aggregate(new StringBuilder(), (stringBuilder, x) => stringBuilder.Append($" --{x.Key}={x.Value}"));
                commandToExecute.Append(parameterString);
            }

            return commandToExecute.ToString();
        }
    }
}
