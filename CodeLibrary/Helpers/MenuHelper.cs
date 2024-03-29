﻿using CodeLibrary.Core;
using CodeLibrary.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class MenuHelper
    {
        private readonly FormCodeLibrary _mainform;
        private FavoriteHelper _FavoriteHelper;
        private TreeviewHelper _treeviewHelper;

        public MenuHelper(FormCodeLibrary mainform, TreeviewHelper treeviewHelper, FavoriteHelper favoriteHelper)
        {
            _mainform = mainform;
            _FavoriteHelper = favoriteHelper;
            _treeviewHelper = treeviewHelper;
            _mainform.mncPasteFilelist.Click += mnuPasteFilelist_Click;
            _mainform.mnuPasteFilelist.Click += mnuPasteFilelist_Click;

            _mainform.mncPasteImage.Click += mnuPasteImage_Click;
            _mainform.mnuPasteImage.Click += mnuPasteImage_Click;

            _mainform.mncPasteImageNoCompression.Click += mnuPasteImageNoCompression_Click;
            _mainform.mnuPasteImageNoCompression.Click += mnuPasteImageNoCompression_Click;

            _mainform.mncPasteTextPerLine.Click += mnuPasteTextPerLine_Click;
            _mainform.mnuPasteTextPerLine.Click += mnuPasteTextPerLine_Click;

            _mainform.mncPasteText.Click += mnuPasteText_Click;
            _mainform.mnuPasteText.Click += mnuPasteText_Click;

            _mainform.mncCopyImage.Click += mnuCopyImage_Click;
            _mainform.mncSaveImage.Click += mnuSaveImage_Click;
            _mainform.mncCopyImageAsBase64String.Click += mnuCopyImageAsBase64String_Click;
            _mainform.mncCopyImageAsMarkDownImage.Click += mnuCopyImageAsMarkDownImage_Click;
            _mainform.mncCopyImageAsHTMLIMG.Click += mnuCopyImageAsHTMLIMG_Click;
            _mainform.mncResizeimageToCurrentZoom.Click += MncResizeimageToCurrentZoom_Click;
            _mainform.mnuRotateImageLeft.Click += MnuRotateImageLeft_Click;
            _mainform.mnuRotateImageRight.Click += MnuRotateImageRight_Click;


            _mainform.mnuManageFavorites.Click += MnuManageFavorites_Click;

            _mainform.mnuExportNoteToPdf.Click += MnuExportNoteToPdf_Click;
            _mainform.mnuExportNoteToFileAs.Click += MnuExportNoteToFileAs_Click;
            _mainform.mnuExportNoteToFile.Click += MnuExportNoteToFile_Click;
            _mainform.mnuImportKnownFile.Click += MnuImportKnownFile_Click;

            _mainform.mncCopyAsHtml.Click += MncCopyAsHtml_Click;
            _mainform.mnuCopyAsHtml.Click += MncCopyAsHtml_Click;
        }



        private void MncCopyAsHtml_Click(object sender, EventArgs e)
        {
            _treeviewHelper.TextBoxHelper.CopyHtml();
        }

        private void MnuExportNoteToFileAs_Click(object sender, EventArgs e)
        {
            _treeviewHelper.TextBoxHelper.ExportToFile(true);
        }

        private void MnuExportNoteToFile_Click(object sender, EventArgs e)
        {
            _treeviewHelper.TextBoxHelper.ExportToFile(false);
        }

        private void MnuImportKnownFile_Click(object sender, EventArgs e)
        {
            _treeviewHelper.TextBoxHelper.ExportToFile(false);
        }

        private void MnuExportNoteToPdf_Click(object sender, EventArgs e)
        {
            _treeviewHelper.TextBoxHelper.ExportToPdfFile();
        }

        private void mnuCopyImage_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(_mainform.imageViewer.Image);
        }

        private void mnuCopyImageAsBase64String_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(_base64);
        }

        private void MncResizeimageToCurrentZoom_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
            Image _image = ImageExtensions.ConvertByteArrayToImage(_snippet.Blob);
            Size _size = _mainform.imageViewer.ImageSize;
            Image _imageResized = _image.ResizeImage(_size.Width, _size.Height);
            _snippet.Blob = _imageResized.ConvertImageToByteArray();
        }

        private void MnuRotateImageRight_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
            Image _image = ImageExtensions.ConvertByteArrayToImage(_snippet.Blob);
            _image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            _snippet.Blob = _image.ConvertImageToByteArray();
            _mainform.imageViewer.setImage(_snippet.Blob);
        }

        private void MnuRotateImageLeft_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
            Image _image = ImageExtensions.ConvertByteArrayToImage(_snippet.Blob);
            _image.RotateFlip(RotateFlipType.Rotate90FlipXY);
            _snippet.Blob = _image.ConvertImageToByteArray();
            _mainform.imageViewer.setImage(_snippet.Blob);
        }

        private void mnuCopyImageAsHTMLIMG_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
        }

        private void mnuCopyImageAsMarkDownImage_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(string.Format(@"![{0}](data:image/png;base64,{1})", _snippet.Title(), _base64));
        }

        private void MnuManageFavorites_Click(object sender, EventArgs e)
        {
            FormFavorites f = new FormFavorites();
            DialogResult _dr = f.ShowDialog();
            if (_dr == DialogResult.OK)
            {
                _FavoriteHelper.BuildMenu();
            }
        }

        private void mnuPasteFilelist_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsFileDropList())
            {
                _treeviewHelper.PasteClipBoardFileList();
            }
        }

        private void mnuPasteImage_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                _treeviewHelper.AddImageNode(_mainform.treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
            }
        }

        private void mnuPasteImageNoCompression_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                _treeviewHelper.AddImageNodeNoCompression(_mainform.treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
            }
        }

        private void mnuPasteText_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                TreeNode node = _treeviewHelper.CreateNewNode(_mainform.treeViewLibrary.SelectedNode.Nodes, CodeType.None, "New Note", Clipboard.GetText(), "");
            }
        }

        private void mnuPasteTextPerLine_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                _treeviewHelper.PasteClipBoardEachLine();
            }
        }

        private void mnuSaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "JPEG Image|*.jpg";
            DialogResult _result = _dialog.ShowDialog();
            if (_result == DialogResult.OK)
            {
                string _filename = _dialog.FileName;

                CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.Get(_treeviewHelper.SelectedId);
                File.WriteAllBytes(_filename, _snippet.Blob);
            }
        }
    }
}