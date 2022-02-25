using CodeLibrary.Core;
using System.Collections.Generic;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class EditorHelperFactory
    {
        private static EditorHelperFactory _Instance;
        private Dictionary<CodeType, EditorClipboardHelperBase> _Instances = new Dictionary<CodeType, EditorClipboardHelperBase>();

        private EditorHelperFactory()
        {
        }

        public void Register(CodeType codetype, EditorClipboardHelperBase editorClipboardhelper)
        {
            if (_Instances.ContainsKey(codetype))
                return;

            _Instances.Add(codetype, editorClipboardhelper);
        }

        public static EditorHelperFactory Instance => _Instance ?? (_Instance = new EditorHelperFactory());

        public EditorClipboardHelperBase GetInstance(CodeType codetype) => _Instances[codetype];
    }
}