﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJ.App.Common
{
    [Serializable]
    public class SerializableListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public string ColorCss { get; set; }
    }
}
