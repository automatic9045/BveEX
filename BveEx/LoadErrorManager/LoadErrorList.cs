using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveTypes.ClassWrappers;

namespace BveEx.LoadErrorManager
{
    /// <summary>
    /// <see cref="LoadError"/> のリストを表します。
    /// </summary>
    internal sealed class LoadErrorList : IList<LoadError>
    {
        private readonly LoadingProgressForm Form;
        private readonly Form FormSource;

        internal LoadErrorList(LoadingProgressForm loadingProgressForm)
        {
            Form = loadingProgressForm;
            FormSource = loadingProgressForm.Src as Form;
        }

        private static ListViewItem ToListViewItem(LoadError loadError, int index)
        {
            ListViewItem item = new ListViewItem(new string[] {
                    index.ToString(),
                    loadError.Text,
                    loadError.SenderFileName,
                    loadError.LineIndex > 0 ? loadError.LineIndex.ToString() : string.Empty,
                    loadError.CharIndex > 0 ? loadError.CharIndex.ToString() : string.Empty,
                });
            return item;
        }

        private static LoadError ToLoadError(ListViewItem item)
        {
            string text = item.SubItems[1].Text;
            string senderFileName = item.SubItems[2].Text;

            string lineIndexText = item.SubItems[3].Text;
            int.TryParse(lineIndexText, out int lineIndex);

            string charIndexText = item.SubItems[4].Text;
            int.TryParse(charIndexText, out int charIndex);

            LoadError loadError = new LoadError(text, senderFileName, lineIndex, charIndex);
            return loadError;
        }

        private void Update()
        {
            Form.ErrorCount = Count;

            if (Count == 0)
            {
                Form.IsAborted = false;
            }

            FormSource.ClientSize = new Size(800, Count == 0 ? Form.Panel.Height : 500);
            FormSource.Update();

            Form.ErrorListView.Update();
            Application.DoEvents();
        }

        /// <inheritdoc/>
        public LoadError this[int index]
        {
            get => ToLoadError(Form.ErrorListView.Items[index]);
            set => Form.ErrorListView.Items[index] = ToListViewItem(value, index + 1);
        }

        /// <inheritdoc/>
        public int Count => Form.ErrorListView.Items.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(LoadError item)
        {
            Form.ErrorListView.Items.Add(ToListViewItem(item, Count + 1));
            Update();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            Form.ErrorListView.Items.Clear();
            Update();
        }

        /// <inheritdoc/>
        public bool Contains(LoadError item) => IndexOf(item) >= 0;

        /// <inheritdoc/>
        public void CopyTo(LoadError[] array, int arrayIndex)
        {
            LoadError[] items = new LoadError[Count];
            for (int i = 0; i < Count; i++)
            {
                LoadError item = ToLoadError(Form.ErrorListView.Items[i]);
                items[i] = item;
            }

            items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<LoadError> GetEnumerator() => new Enumerator(Form);

        /// <inheritdoc/>
        public int IndexOf(LoadError item)
        {
            for (int i = 0; i < Form.ErrorListView.Items.Count; i++)
            {
                ListViewItem listViewItem = Form.ErrorListView.Items[i];

                string text = listViewItem.SubItems[1].Text;
                string senderFileName = listViewItem.SubItems[2].Text;
                string lineIndexText = listViewItem.SubItems[3].Text;
                string charIndexText = listViewItem.SubItems[4].Text;

                if (item.Text != text || item.SenderFileName != senderFileName) continue;

                if (item.LineIndex > 0)
                {
                    if (item.LineIndex.ToString() != lineIndexText) continue;
                }
                else
                {
                    if (lineIndexText != string.Empty) continue;
                }

                if (item.CharIndex > 0)
                {
                    if (item.CharIndex.ToString() != charIndexText) continue;
                }
                else
                {
                    if (charIndexText != string.Empty) continue;
                }

                return i;
            }

            return -1;
        }

        /// <inheritdoc/>
        public void Insert(int index, LoadError item)
        {
            ListViewItem listViewItem = ToListViewItem(item, index);
            Form.ErrorListView.Items.Insert(index, listViewItem);

            for (int i = index + 1; i < Form.ErrorListView.Items.Count; i++)
            {
                Form.ErrorListView.Items[i].SubItems[0].Text = (i + 1).ToString();
            }

            Update();
        }

        /// <inheritdoc/>
        public bool Remove(LoadError item)
        {
            int targetIndex = IndexOf(item);
            if (targetIndex == -1)
            {
                return false;
            }
            else
            {
                RemoveAt(targetIndex);
                return true;
            }
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            Form.ErrorListView.Items.RemoveAt(index);
            Update();
        }

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Form);


        private class Enumerator : IEnumerator<LoadError>
        {
            protected IEnumerator ListViewItemEnumerator;

            public Enumerator(LoadingProgressForm form)
            {
                ListViewItemEnumerator = form.ErrorListView.Items.GetEnumerator();
            }


            public LoadError Current
            {
                get
                {
                    ListViewItem source = ListViewItemEnumerator.Current as ListViewItem;
                    LoadError item = ToLoadError(source);
                    return item;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext() => ListViewItemEnumerator.MoveNext();

            public void Reset() => ListViewItemEnumerator.Reset();
        }
    }
}
