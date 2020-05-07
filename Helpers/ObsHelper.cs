﻿using OBS;
using obs_cli.Objects;
using obs_cli.Objects.Obs;
using obs_cli.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static OBS.libobs;

namespace obs_cli.Helpers
{
    public static class ObsHelper
    {
        /** 
		 * Get the active display from the window's position.
		 * We have to do it this way because the OBS index (or display value) is different than that of Screen.AllScreens
		 */
        public static int GetObsDisplayValueFromScreen(Source displaySource, Screen screen)
        {
            // Get a list of OBS properties (names and values) from the display source
            ObsProperty[] displayCaptureProperties = displaySource.GetProperties().GetPropertyList();
            List<string> displayNames = new List<string>();
            List<object> displayValues = new List<object>();
            for (int i = 0; i < displayCaptureProperties.Length; i++)
            {
                if (displayCaptureProperties[i].Name.Equals("monitor"))
                {
                    displayNames = displayCaptureProperties[i].GetListItemNames().ToList();
                    displayValues = displayCaptureProperties[i].GetListItemValues().ToList();
                    break;
                }
            }

            // Find the OBS display that matches the bounds of our active screen. OBS display names are in the format of "Display {value}: WidthxHeight @ X,Y"
            string searchForString = $"@ {screen.Bounds.X},{screen.Bounds.Y}";
            int targetDisplayIndex = displayNames.FindIndex(x => x.Contains(searchForString));
            int targetDisplayValue = int.Parse(displayValues[targetDisplayIndex].ToString());

            return targetDisplayValue;
        }

        /// <summary>
        /// Generates a new ObsVideoInfo object.
        /// </summary>
        /// <param name="baseWidth"></param>
        /// <param name="baseHeight"></param>
        /// <param name="outputWidth"></param>
        /// <param name="outputHeight"></param>
        /// <returns></returns>
        public static obs_video_info GenerateObsVideoInfoObject(uint baseWidth, uint baseHeight, uint outputWidth, uint outputHeight)
        {
            return new obs_video_info
            {
                adapter = 0,
                base_width = baseWidth,
                base_height = baseHeight,
                output_width = outputWidth,
                output_height = outputHeight,
                fps_den = Constants.Video.FPS_DEN,
                graphics_module = "libobs-d3d11.dll",
                output_format = video_format.VIDEO_FORMAT_NV12,
                scale_type = obs_scale_type.OBS_SCALE_BICUBIC,
                colorspace = video_colorspace.VIDEO_CS_601,
                range = video_range_type.VIDEO_RANGE_PARTIAL,
                gpu_conversion = true,
            };
        }

        /// <summary>
        /// Generates a new ObsVideoInfo object with the designated frame rate.
        /// </summary>
        /// <param name="baseWidth"></param>
        /// <param name="baseHeight"></param>
        /// <param name="outputWidth"></param>
        /// <param name="outputHeight"></param>
        /// <param name="frameRate"></param>
        /// <returns></returns>
        public static obs_video_info GenerateObsVideoInfoObject(uint baseWidth, uint baseHeight, uint outputWidth, uint outputHeight, uint frameRate)
        {
            var obsVideoInfo = GenerateObsVideoInfoObject(baseWidth, baseHeight, outputWidth, outputHeight);
            obsVideoInfo.fps_num = frameRate;
            return obsVideoInfo;
        }
    }
}
