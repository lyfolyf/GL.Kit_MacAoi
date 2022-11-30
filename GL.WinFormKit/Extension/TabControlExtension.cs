namespace System.Windows.Forms
{
    public static class TabControlExtension
    {
        public static TabPage FindPage(this TabControl tabControl, Func<TabPage, bool> predicate)
        {
            return tabControl.TabPages.FirstOrDefault(predicate);
        }

        public static TabPage FirstOrDefault(this TabControl.TabPageCollection pageCollection, Func<TabPage, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (TabPage page in pageCollection)
            {
                if (predicate(page))
                {
                    return page;
                }
            }

            return null;
        }

        public static TabPage RemoveFirst(this TabControl.TabPageCollection pageCollection, Func<TabPage, bool> predicate)
        {
            TabPage page = FirstOrDefault(pageCollection, predicate);

            if (page != null)
                pageCollection.Remove(page);

            return page;
        }
    }
}
