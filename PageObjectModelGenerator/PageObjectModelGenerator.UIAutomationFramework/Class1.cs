using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace PageObjectModelGenerator.UIAutomationFramework
{
    // TODO rename the class
    public class Class1
    {
        private Stack<string> allChildren = new Stack<string>();

        // AutomationID: ; Name: ; Type: ; OffScreen: ;
        public List<string> GetAllControls(string processName)
        {
            var mainWindow = this.GetApplicationMainWindow(processName);
            this.FindAllChildren(mainWindow, string.Empty);

            while (allChildren.Count > 0)
            {
                Console.WriteLine(allChildren.Pop());
            }

            return null;
        }

        private AutomationElement GetApplicationMainWindow(string processName)
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName == processName.Replace(".exe", "")).ToList();

            if (processes.Count > 0)
            {
                return AutomationElement.FromHandle((IntPtr)processes[0].MainWindowHandle);                
            }

            var process = Process.Start(processName);
            process.WaitForInputIdle();
            while (process.MainWindowHandle == IntPtr.Zero)
            {
                // wait for loading
            }

            return AutomationElement.FromHandle((IntPtr)process.MainWindowHandle);       
        }

        private void FindAllChildren(AutomationElement parent, string space)
        {
            var children = parent.FindAll(TreeScope.Children, Condition.TrueCondition);       
            foreach (AutomationElement ch in children)
            {
                this.FindAllChildren(ch, "    " + space);
            }

            allChildren.Push(string.Format("{0}AutomationID: {1}; Name: {2}; Type: {3}; IsOffscrean: {4}", space, parent.Current.AutomationId, parent.Current.Name, parent.Current.ControlType.ProgrammaticName, parent.Current.IsOffscreen));
        }
    }
}
