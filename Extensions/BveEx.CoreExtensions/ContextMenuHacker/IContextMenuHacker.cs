using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Extensions.ContextMenuHacker
{
    /// <summary>
    /// メインフォームの右クリックメニューを編集するための機能を提供します。
    /// </summary>
    public interface IContextMenuHacker : IExtension
    {
        /// <summary>
        /// メニューに新しい項目を追加します。
        /// </summary>
        /// <param name="item">追加する項目。</param>
        /// <param name="itemType">項目の種類。</param>
        /// <returns>追加した項目。</returns>
        ToolStripItem AddItem(ToolStripItem item, ContextMenuItemType itemType);

        /// <summary>
        /// メニューに新しいクリック可能な <see cref="ToolStripMenuItem"/> を追加します。
        /// </summary>
        /// <param name="text">メニュー項目に表示するテキスト。</param>
        /// <param name="click">メニュー項目がクリックされたときに発生する <see cref="EventHandler"/>。</param>
        /// <param name="itemType">項目の種類。</param>
        /// <returns>追加した項目。</returns>
        ToolStripMenuItem AddClickableMenuItem(string text, EventHandler click, ContextMenuItemType itemType);

        /// <summary>
        /// メニューに新しいチェック可能な <see cref="ToolStripMenuItem"/> を追加します。
        /// </summary>
        /// <param name="text">メニュー項目に表示するテキスト。</param>
        /// <param name="checkedChanged"><see cref="ToolStripMenuItem.Checked"/> プロパティの値が変化したときに発生する <see cref="EventHandler"/>。</param>
        /// <param name="itemType">項目の種類。</param>
        /// <returns>追加した項目。</returns>
        ToolStripMenuItem AddCheckableMenuItem(string text, EventHandler checkedChanged, ContextMenuItemType itemType);

        /// <summary>
        /// メニューに新しいチェック可能な <see cref="ToolStripMenuItem"/> を追加します。
        /// </summary>
        /// <param name="text">メニュー項目に表示するテキスト。</param>
        /// <param name="itemType">項目の種類。</param>
        /// <returns>追加した項目。</returns>
        ToolStripMenuItem AddCheckableMenuItem(string text, ContextMenuItemType itemType);

        /// <summary>
        /// メニューに新しい区切り線を表す <see cref="ToolStripSeparator"/> を追加します。
        /// </summary>
        /// <param name="itemType">項目の種類。</param>
        /// <returns>追加した項目。</returns>
        ToolStripSeparator AddSeparator(ContextMenuItemType itemType);
    }
}
