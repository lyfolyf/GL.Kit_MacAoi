using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static class TreeViewSettings
    {
        const int TVIF_STATE = 0x8;
        const int TVIS_STATEIMAGEMASK = 0xF000;
        const int TV_FIRST = 0x1100;
        const int TVM_SETITEM = TV_FIRST + 63;

        /// <summary>
        /// 当条件满足时，不显示 CheckBox
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="predicate">条件</param>
        public static void HideCheckBox(this TreeView treeView, Func<TreeNode, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            treeView.CheckBoxes = true;
            treeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            treeView.DrawNode += (sender, e) =>
            {
                if (predicate(e.Node))
                {
                    User32Api.TVITEM tvi = new User32Api.TVITEM
                    {
                        hItem = e.Node.Handle,
                        mask = TVIF_STATE,
                        stateMask = TVIS_STATEIMAGEMASK,
                        state = 0
                    };

                    User32Api.SendMessage(treeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
                }

                e.DrawDefault = true;
            };
        }

    }
}
