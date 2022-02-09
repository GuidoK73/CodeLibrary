﻿using CodeLibrary.Controls;
using CodeLibrary.Core;
using System;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAbout : Form
    {
        private readonly string _Content = @"
# Version 3.3
- Markdown, using CodeLibrary references in text will auto create codeblocks for specific language on Preview and Copy Contents and Merge.
    - When Markdown is merged in to HTML document then it's converted to HTML.
- Export to PDF.
- Export to File.(Both merge Content)
    - MD can be exported to HTML, by changing the file extension.
    - when exporting MD to HTML, a stylesheet is used, this stylesheet can be configured in settings.
- Clipboard => Copy as Html (Markdown only).
- Export to File (Rtf).

### Image viewer
- Resize image, in the Image viewer you can no resize to current zoom.
- Rotate left, right

### New Paste features:
- CTRL-V
    - Paste Images directly into text editor, an Id will occur and the image is placed in the folder structure directly under the current Note.
    - Paste File lists directly into text editor and references are added and inserted into the text.
- CTRL-SHIFT-V
    - Paste Images directly into text editor directly converted to base64 and formatted for Html or Markdown specific.
    - Paste File lists direcly into the text editor, depending on source type and target type data will be included in the text.

- Shortcut Template Keys can now be set for a specific language.

### Bugfixes
-   Circular Reference when merge content points to circular notes.
-   Wrong selection after insert

# Version 3.2
- Markdown, Added SynraxHighlighting extensions.\
    github syntax highlighting blocks like ~~~sql, ~~~cs, now supported.
- Markdown DarkMode, chahged font.
- Moved source to new location:\
    new: https://github.com/Guido73/CodeLibrary \
    old: https://github.com/Guido1234/CodeLibrary
- Bug Fix: Moving snippets did not refresh index.
- Clipboard menu item Copy Id.\
    Copies the Id as #[497e1b9c-2adf-4183-ba23-c7b5bcb3557a] which can be used as mergeable tags in text. same as the Copy Path.
- Searchbox now supports searching on Id.
- New MarkDown parser, now based on MarkDig\
    features like tables and Fenced code also work.
- Improved path merging, path tags located in nested Snippets are parsed as well.
- Copy and Merge content either html or Markdown.
- Html / Markdown preview zoom
- New Template Function: GetUrlParam, retrieves url parameter by name.\
~~~
    Data:
    http://www.test.com?code=1245&id=5678

    template:
    {0:GetUrlParam(code)}

    result:
    1245
~~~

# Version 3.1:

- Bug fix: Paste image in treeview wrong ID / Name.
- Bug fix: Key icon not showing when using usbKey.
- UsbKey Change:
    -   Allowed to have multiple UsbKeys inserted as long you are not creating one.
    -   Key for current document is auto synced to all Inserted UsbKeys (Usb flash drives marked as UsbKey).

# Version 3.0:

-   New file format
    -  Uses binaryformatter
    -  Save and Encryption within one FileStream
    -  All string conversions are removed when saving.
-   Export
    -   Tools => Export: exports entire library to directory/file structure.
-   CodeSnippet
    -   exetended Code, Rtf, Path with Base64 en Compression
-   Restore Backup
    -   Configure settings to backup on a different location then file location.
-   UsbKEY correct message when no UsbKEY is present for the document.
-   Improved NewDocument message flow.
-   Bug: Exit without saving changes showed Save as dialog.
-   Bug: Exit without saving changes still saved current document.
-   Config Save Bugfix
-   Backups stored in specified location got their own directory based on DocumentId.
-   Changed shortcut Ctrl DragDrop for reference link to Ctrl-Shift DragDrop
-   New: shortcut Ctrl DragDrop duplicates Note + tree to new location.

# Version 2.3
-   Improved TreeView behavior
    - changed behavior of the short cut keys - they now respond to KeyDown instead of the KeyUp event for better response
    - key down is now marked das handled, when it was handled
-   New Template Function: SplitUpperCase, splits 'ThisIsAName' to 'This Is A Name'
-   Reference Notes, add a link to another note anywhere in the the tree.
    -   Reference will only be to the note specified, child notes will not be available.
-   Note names are now required to be unique. (in most cases)
-   New Note name default with a number.
-   Bugfix: CodeSnippet Path was not directly updated on treeview Node change.
-   Merging now supports pattern matching.\
    #[Markdown\Merge Example\*]#\
    See Help => Demo Library => Markdown\
-   New Tool: Select Is Copy\
    When checked, any selection you make is directly available on clipboard. this can be usefull is you need to single pick and copy large amount of values.\
    This function does not work in RTF!
-   Extensions
    -   Database Schema Reader
-   Ctrl-DragDrop creates referrence link

# Version 2.2
-   Added Setting Screen
    -   Sort Mode
    -   Theme
-   New: Manage Favorites
-   State Icons (right corner)
    -   Debug Mode
    -   Document Changed
    -   Password / USB locked
    -   Clipboard monitor on
-   Exit without saving warning dialog.
-   Browse file in Restore Backup
-   USBKey.
    Use a usb flash drive as a passkey container instead of using a password.
    -   There can be only one usb flash drive marked as a USBKey inserted in the usb ports.
    -   When creating an usb key drive, there can only be one usb drive in the ports.
    -   When an USBKey is created multiple files can be created using this usb flash drive.
    -   Each new file protected with a USBKey has an unique identifier which matches the keyfiles in: [Drive]:\EncrKeys
    -   You will never be prompted for a password, only to insert the USBKey flash drive when it's not presents or the required key does not exist.
    -   The USBKey is required for Saving and for loading, keys are not kept in memory and are read from the drive when needed.
-   Improved TreeView Load.
-   Improved Sort ASC, Important sorted first, Folders sorted second then other notes.
-   New database field 'CodeLastModificationDate'.\
    This field is set when the 'Code' field changes.
    This field is currently only shown in the search results.
-   Added new commands to Library management (treeview):
        - Sort children ascending (A-Z).\
            This command sorts the children of the selected node in ascending order.
        - MoveLeft\
            This moves the selected item to the left.\
            This meaning it will be decoupled from to parent and it will be put just after parent (as a Next node)
        - MoveRight\
            This moves the selected item to the right.\
            This meains it will be put as a child under the previous node and the last item.
-   Added some shortcuts to Library management (in treeview) and dialogs
        - Ctrl + Enter in TreeView
            This will open the properties dialog for the selected node
        - Esc key in Property dialog
            This will close the form

### FIXED:
-   Paste text per line misses last one
-   Treeview paste text applies text to current note.
-   AddImageNode ChangeType
-   HTML Preview
-   CreationDate contained incorrect formatting for seconds containing ':nn'.
    The is changed in the getter changed to ':00:00'.
    Please note that the field is still incorrect in the contents of file (Json)
    until the the Code Snippet is changed and saved again.
-   Removed Shortcut keys from Library Ordering Menu.
-   In treeview shortcutkeys:
    -   Ctrl-Up         Move up
    -   Ctrl-Down       Move Down
    -   Ctrl-Shft-Up    Move to Top
    -   Ctrl-Shft-Down  Move to Bottom
    -   Ctrl-Left       Move left
    -   Ctrl-Right      Move right

# Version 2.1
-   New file container.
-   Ask for password on encrypted file container. (only for new container or saved container).\
    When a file without password is opened, the password will be reset.
-   Encrypted files are reminded as well, password dialog will show on opening.
-   Move to Top / Move to Bottom
-   Bugfixes

# Version 2.0:

### NEW:
-   Rtf document type + Rtf Editor
-   MarkDowm document type
    -   Html Preview will translate markdown script.
-   Add New Dialog box
    -   Add New always shows dialog.
    -   Assign defaults in note properties to supress this behaviour.
-   Backup Manager
-   Plugin: Markdown to HTML
-   [Copy Contents and Merge] option in clipboard context menu. (merges all #[Note Path]# tags)

### CHANGES:
-   Clipboard monitor now has an own Note, clipboard monitor can be activated and deactivated in it's context menu.\
    the clipboard monitor will only paste changes within this note.
-   Removed Default Note Settings
-   Removed Bookmark functionality
-   Re-designed Note properties window.
-   Reorganized Library Menu
-   Reorganized Edit Menu
-   Reorganized File Menu
-   Removed themes from dialog windows (only main screen uses themes).
-   Note links #[Note Path]# will be merged in Html Preview. (links created by [Copy Path] )

BUG FIXES:
-   Syntax Highlight not working on first note.
-   HtmlPreview window not properly initialized.
-   Wordwrap not applied.
-   Syntax Highlight not applied directly on Change Type
-   Template insight dropdown unusable small.
-   Tree icon does not change when changing type.
-   Errors / Situations in Empty project while.
-   Error while switching to deleted favorite.
-   Drag drop system notes should not be allowed.
-   Save Image => GDI+ Error message
-   Error, open file with last selection an Image
-   Error, Default child code containing wrong format
-   Error, Default child name containing wrong format
-   Error, Access Denied
-   New Document did not clear the editor.
-   Html Preview not shown on first open.
-   Disabled Javascript errors in webbrowser.
-   Default OK button for Properties window and Addnew dialog

----------------------------------------------------------------------
Version 1.9:
-   Added Clipboard menu to Note context menu item.
    -   Menu items depending on clipboard content.
    -   Added paste image without compression.
-   Image context menu (rightclick on image itself)
    -   Added 'copy as Base64 String'
    -   Added 'copy as Html IMG'
-   Image can be moved around.
-   Adding notes beneath images.
-   Images default orginal size.
-   Html Preview window for Html / Xml

----------------------------------------------------------------------
Version 1.8:
-   Paste in Treeviewer (Ctrl-V)
    -   FileList: inserts all files below current note.
    -   Text: Note contents contains clipboard contents.
    -   Image: Paste image into Treeviewer, Image is displayed instead of code editor.
-   Paste in Treeviewer (Ctrl-Shift-V): Splits text in lines and creates a note for every line.
-   Drag / Drop into treeviewer supports images (jpg / png / bmp)

----------------------------------------------------------------------
Version 1.7:
-   Fixed Version number bug.
-   Library Search (Search global in all documents)
-   Assign shortcut keys to note, Note can be used as a template (Paste Special).\
    Shortcut keys can be set in the Note properties window.
-   Added Note properties window.
-   Removed fontsize / added zoom control.
-   F12 swtiches between last two documents
-   Added 2 plugins: Remove Lines Containing, Remove Lines Not Containing
-   Added KeepQuoted to C# plugins: remove all text except for text enclosed within double qoutes.
-   Added a Template function KeepQouted.

----------------------------------------------------------------------
Version 1.6:

-   Clipboard monitor\
    (Auto paste clipboard contents in current document when the clipboard contents changes.)
-   Option to remove current from Favorite Library.
-   Favorite Library shortcuts automatically have a shortcut key (Ctrl-F1 - Ctrl-F12)
-   Bookmarks overview.
-   Drop files into browser.
-   Drop file into editor.
-   In memmory password uses SecureString.
-   Keep selection after clearing search.
-   Message when opening non library file (or wrong password) instead of C# error message.
-   Remind Last line on note.
-   New Plugin: Encoding\Import as base64

----------------------------------------------------------------------
Version 1.5:

-   Favorite Libraries Menu\
    (Switching Favorite Library always saves implicit.)
-   Note Defaults.\
    create default text / title for Childs.
-   Template Type.\
    Templates are used for Paste Special.\
    (Template Type not required for Paste Special)
-   Bookmarks
-   High Contrast Mode
-   Paste Special
~~~
    --------------------------------------------
    Use formatting on clipboard to merge with selected Text.

    Exampe 1
    --------

    Clipboard contents:
    ""{0}"",

    Selected Text:
    Id
    Name

    CTRL-T Result:
    ""Id"",""Name"",

    Exampe 2
    --------

    Clipboard content:
    private {0} {1:CamelCaseLower};

    public {0} {1:CamelCaseUpper}
    {
        get
        {
            return {0} {1:CamelCaseLower};
        }
        set
        {
            {1:CamelCaseLower} = value;
        }
    }

    Selected text:
    int;Id
    string;Name

    Result:
    int;Id
    string;Name

    Note 1: Each line in selected text will be seen as a record!
    Note 2: Formatting is not the same as C# string formatting.

    0:MethodAsciiValue()
~~~

----------------------------------------------------------------------
General Public Licence

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

Programming: Guido Kleijer
";

        public FormAbout()
        {
            InitializeComponent();
            CancelButton = dialogButton.buttonOk;
            AcceptButton = dialogButton.buttonOk;
            Load += FormAbout_Load;
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/u/guidok915");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/PavelTorgashov/FastColoredTextBox");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Guido1234/CodeLibrary");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/StackExchange/MarkdownSharp");
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            lbTitle.Text = $"Code Library";
            lblVersion.Text = $"V{Config.CurrentVersion().ToString() }";
            ShowText();
        }

        public void ShowText()
        {
            webBrowser.AllowNavigation = true;

            MarkDigWrapper _markdown = new MarkDigWrapper();
            string _text = _Content;
            _text = _markdown.Transform(_text);
            StringBuilder _sb = new StringBuilder();
            _sb.Append("<body style =\"background-color:#333333;color:#cccccc;font-family:Arial\"></body>\r\n");
            _sb.Append("<style>");
            _sb.Append("a:link { color: green; background-color: transparent; text-decoration: none; }");
            _sb.Append("a:visited { color: lightgreen; background-color: transparent; text-decoration: none; }");
            _sb.Append("a:hover { color: lightgreen; background-color: transparent; text-decoration: underline; }");
            _sb.Append("a:active { color: yellow; background-color: transparent; text-decoration: underline; }");
            _sb.Append("table, th, td { border: 1px solid lightgray; border-collapse: collapse; padding:4px; }");
            _sb.Append("</style>");
            _sb.Append(_text);
            _text = _sb.ToString();
            _text = _text.Replace("style=\"color:Black;background-color:White;\"", "style=\"color:White;background-color:Black;\"");
            webBrowser.DocumentText = _text;
        }
    }
}