using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    public enum MenuItemType
    {
        Browse,
        Recognition,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
