using PageObjectModelGenerator.UIAutomationFramework.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;

namespace PageObjectModelGenerator.Engine
{    
    public class PomGenerator
    {
        private int currentId;
        private Stack<string> childrenTree = new Stack<string>();
        private List<UIControl> allControls = new List<UIControl>();
       
        public List<string> GetAllControls(string processName)
        {
            this.currentId = 1;
            var mainWindow = this.GetApplicationMainWindow(processName);
            allControls.Add(ConvertUIControl(mainWindow, this.currentId, 0));
            this.FindAllChildren(mainWindow, string.Empty);
            this.PrintChildrenTree();
            
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
            var parentId = this.currentId;
            foreach (AutomationElement ch in children)
            {
                allControls.Add(ConvertUIControl(ch, ++this.currentId, parentId));
                this.FindAllChildren(ch, "    " + space);
            }

            if (space.Length > 0)
            {
                space = space.Substring(0, space.Length - 4) + "└───";
            }

            childrenTree.Push(string.Format("{0}ID: {1} AutomationID: {2}; Name: {3}; Type: {4}; IsOffscrean: {5}", space, parentId, parent.Current.AutomationId, parent.Current.Name, parent.Current.ControlType.ProgrammaticName, parent.Current.IsOffscreen));
        }

        private UIControl ConvertUIControl(AutomationElement automationElement, int id, int parentId)
        {
            return new UIControl() { Id = id, AutomationId = automationElement.Current.AutomationId, Name = automationElement.Current.Name, ControlType = automationElement.Current.ControlType.ProgrammaticName, IsOffscreen = automationElement.Current.IsOffscreen, ParentId = parentId };
        }

        private void PrintChildrenTree()
        {
            while (childrenTree.Count > 0)
            {
                Console.WriteLine(childrenTree.Pop());
            }
        }
    }
}
