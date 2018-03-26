using PageObjectModelGenerator.UIAutomationFramework.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectModelGenerator.UIAutomationFramework.PageObjectModelTemplate
{
    public partial class PageObjectModelTemplate
    {
        internal string className;
        internal List<UIControl> allPomControls = new List<UIControl>();
        internal Dictionary<int, string> allPomControlsNamesDict;

        internal PageObjectModelTemplate(string className, List<UIControl> controls)
        {
            this.className = className;
            this.allPomControls = controls;
            this.allPomControlsNamesDict = this.GenerateControlNames();
        }

        internal string GetFindingMethod(UIControl control)
        {
            string parent = control.ParentId != 0 ? this.allPomControlsNamesDict[control.ParentId] : "null";
            if (!string.IsNullOrEmpty(control.AutomationId))
            {
                return $"base.GetControlById(\"{ control.AutomationId }\", {parent})";
            }

            return "TODO";
        }

        private Dictionary<int, string> GenerateControlNames()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var control in this.allPomControls.OrderBy(c => c.Id))
            {
                string controlName, orginalName;
                controlName = orginalName = this.CreateControlName(control);
                var i = 1;                
                while (result.ContainsValue(controlName))
                {
                    controlName = $"{orginalName}_{i++}";                    
                }
                result.Add(control.Id, controlName);
            }

            return result;
        }

        private string CreateControlName(UIControl control)
        {
            var controlType = control.ControlType.Replace("ControlType.", string.Empty);
            var name = string.Empty;

            if (!string.IsNullOrEmpty(control.Name) || !string.IsNullOrEmpty(control.AutomationId))
            {
                name = this.GetCamelCaseName(!string.IsNullOrEmpty(control.Name) ? control.Name : control.AutomationId);
                return string.Format($"{controlType}{name}");
            }

            return controlType;
        }

        public string GetCamelCaseName(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name).Replace(" ", "");
        }
    }
}
