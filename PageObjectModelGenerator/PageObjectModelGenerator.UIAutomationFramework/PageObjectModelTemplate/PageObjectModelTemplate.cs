using Newtonsoft.Json;
using PageObjectModelGenerator.UIAutomationFramework.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectModelGenerator.UIAutomationFramework.PageObjectModelTemplate
{
    /* 
     * todo 

     * * snippets - every control in partial class 
     * * different type of methods like getControlByTypeName
     *  * if there's not possible to create methods - wrte a commetn
     *  * get only partycilar POM and all controils under
     */

    public partial class PageObjectModelTemplate
    {
        private Dictionary<string, string> controlFindingMethods;
        internal string className;
        internal List<UIControl> allPomControls = new List<UIControl>();
        internal Dictionary<int, string> allPomControlsNamesDict;

        internal PageObjectModelTemplate(string className, List<UIControl> controls)
        {
            this.className = className;
            this.allPomControls = controls;
            this.allPomControlsNamesDict = this.GenerateControlNames();
            this.controlFindingMethods = this.GetControlFindingMethods();
        }

        internal string GetFindingMethod(UIControl control)
        {
            string parent = control.ParentId != 0 ? this.allPomControlsNamesDict[control.ParentId] : "null";
            if (!string.IsNullOrEmpty(control.AutomationId))
            {
                return string.Format(this.controlFindingMethods["GetControlByAutomationId"], control.ControlType, control.AutomationId, parent);
            }

            if (!string.IsNullOrEmpty(control.Name))
            {
                return string.Format(this.controlFindingMethods["GetControlByName"], control.ControlType, control.Name, parent);
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

        private string GetCamelCaseName(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name).Replace(" ", "");
        }

        private Dictionary<string, string> GetControlFindingMethods()
        {
            var result = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(@"PageObjectModelTemplate\Config\methods.json"))
            {
                var methods = sr.ReadToEnd();
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(methods);
            }

            return result;
        }
    }
}
