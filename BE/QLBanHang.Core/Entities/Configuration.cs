using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Entities
{
    public class Configuration
    {
        public Guid ConfigurationId { get; set; }

        public string? Screen {  get; set; }

        /// <summary>
        /// Độ phân giải
        /// </summary>
        public string? Resolution { get; set; }

        /// <summary>
        /// Công nghệ màn hình
        /// </summary>
        public string? ScreenTechnology { get; set; }

        public string? RAM { get; set; }

        public string? Memory { get; set; }

        public string? Material { get; set; }

        public string? Size { get; set; }

        public double? Weight { get; set; }

        public string? Camera { get; set; }

        public string? Pin { get; set; }

        public string? CPU { get; set; }

        public string? ImageCPU { get; set; }

        public string? ImageRAM { get; set; }

        public string? ImageCamera { get; set; }

        public string? ImagePin { get; set; }

        public string? ContentCPU { get; set; }

        public string? ContentRAM { get; set; }

        public string? ContentCamera { get; set; }

        public string? ContentPin { get; set; }

        public string? Chip { get; set; }

        public Guid ProductId { get; set; }
    }
}
