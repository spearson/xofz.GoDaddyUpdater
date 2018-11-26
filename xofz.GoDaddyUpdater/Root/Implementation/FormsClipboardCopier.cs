namespace xofz.GoDaddyUpdater.Root.Implementation
{
    using System.Windows.Forms;
    using xofz.GoDaddyUpdater.Framework;

    public sealed class FormsClipboardCopier : ClipboardCopier
    {
        void ClipboardCopier.Copy(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch
            {
                // swallow
            }            
        }
    }
}
