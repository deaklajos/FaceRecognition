using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Enum describing the contents of the menu.
    /// </summary>
    public enum MenuItemType
    {
        Browse,
        Recognition,
        About
    }

    /// <summary>
    /// Class for pairing MenuItemType with a string title.
    /// </summary>
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
