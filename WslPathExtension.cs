using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace WslPathShellExt
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class WslPathExtension : SharpContextMenu
    {
        [DllImport("Kernel32.Dll", EntryPoint="Wow64EnableWow64FsRedirection")]
        public static extern bool EnableWow64FSRedirection(bool enable);

        protected override bool CanShowMenu()
        {
            //  We always show the menu
            return true;
        }
        
        protected override ContextMenuStrip CreateMenu()
        {
            //  Create the menu strip
            var menu = new ContextMenuStrip();
            var menuStrip = new ToolStripMenuItem
            {
                Text = "Copy WSL Path",
            };
        
            menuStrip.Click += (sender, args) => CopyWslPathToClipboard();
            //  Add the item to the context menu.
            menu.Items.Add(menuStrip);
 
            //  Return the menu
            return menu;
        }

        private void CopyWslPathToClipboard()
        {
            if (!SelectedItemPaths.Any()) return;

            try
            {
                Clipboard.SetText(GetWslPath(SelectedItemPaths.First()));
            }
            catch
            {
                // die silently
            }
        }
    
        private string GetWslPath(string path)
        { EnableWow64FSRedirection(false);
            try
            {
                //Create process
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = @"C:\Windows\System32\wsl.exe",
                        Arguments = $"wslpath \"{path}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Path.GetDirectoryName(path) ?? @"C:\Windows\System32"
                    }
                };

                proc.Start();

                string strOutput = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();

                return strOutput;
            }
            finally
            {
                EnableWow64FSRedirection(true);
            }
        }
    }
} 