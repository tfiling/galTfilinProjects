using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DW.CodedUI;
using DW.CodedUI.Utilities;
using DW.CodedUI.BasicElements;

namespace DW.CodedUI.Example
{
    /*
     * In this class I just want to show some basic actions how you can work with the DW.CodedUI.
     * The UI to search for elements, the WindowFinder and all other objects has many more features I cannot show here.
     * But you should get enough overview to start UI testing.
     * */
    [TestClass]
    public class GettingStarted
    {
        [TestMethod]
        public void TypeSomethingInNotepad()
        {
            // Launch the application and wait a a second to be sure its loaded properly.
            Do.Launch(@"C:\Windows\System32\Notepad.exe").And.Wait(1000);

            // Find the notepad window by its process name.
            var notepad = WindowFinder.Search(Use.Process("notepad"));

            // Type something into notepad a little slower like you may do it.
            KeyboardEx.TypeText(notepad, "Have fun using the DW.CodedUI", 100);

            // Try close the notepad.
            MouseEx.Click(notepad.CloseButton);

            // Because of the typed text it asks if I want to save it, so we need to get the messagebox.
            var messageBox = WindowFinder.Search(Use.Title("Editor"));

            // Using the ElementFinder (See my-libraries.com) we know the automation ID of the "don't save" button is "CommandButton_7", so search for it
            var doNotSaveButton = UI.GetChild(By.AutomationId("CommandButton_7"), From.Element(messageBox));

            // Click don't save and the application is closed now.
            MouseEx.Click(doNotSaveButton);
        }

        [TestMethod]
        public void ToggleStatusBarStatesInNotepad()
        {
            Do.Launch(@"C:\Windows\System32\Notepad.exe").And.Wait(1000);
            var notepad = WindowFinder.Search(Use.Process("notepad"));

            // Get the "View" menu item, with the ElementFinder I see the itme has no AutomationId, so I need to use my german Text to find it.
            var viewMenuItem = UI.GetChild<BasicMenuItem>(By.Name("Ansicht"), From.Element(notepad));

            // Click the view menu item and wait a little that the subentries can be created.
            MouseEx.Click(viewMenuItem).And.Wait(1000);

            // Click it, so the child elements will be created and I can search for them. (Popups are in a different visual treee, so start searching from the notepad window)
            var toggleStatusBarMenuItem = UI.GetChild<BasicMenuItem>(By.AutomationId("27"), From.Element(notepad));

            // Click it and wait a second so you can see it happen.
            MouseEx.Click(toggleStatusBarMenuItem).And.Wait(1000);

            // Click the view menu item again with the waiting time.
            MouseEx.Click(viewMenuItem).And.Wait(1000);

            // Search again for the sub menu item and click it with waiting time. (The list might be recreated and the UI element is a new one so we cannot recycle the old one.)
            toggleStatusBarMenuItem = UI.GetChild<BasicMenuItem>(By.AutomationId("27"), From.Element(notepad));
            MouseEx.Click(toggleStatusBarMenuItem).And.Wait(1000);

            // Just close notpad hard without using the mouse.
            notepad.Unsafe.Close();
        }
    }
}
