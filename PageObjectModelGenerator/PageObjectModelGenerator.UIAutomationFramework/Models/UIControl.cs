using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectModelGenerator.UIAutomationFramework.Models
{
    internal class UIControl
    {
        internal int Id { get; set; }
        internal string AutomationId { get; set; }
        internal string Name { get; set; }
        internal string ControlType { get; set; }
        internal bool IsOffscreen { get; set; }
        internal int ParentId { get; set; }
    }
}
