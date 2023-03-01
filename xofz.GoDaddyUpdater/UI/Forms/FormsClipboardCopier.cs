namespace xofz.GoDaddyUpdater.UI.Forms
{
    using System.Windows.Forms;

    public sealed class FormsClipboardCopier 
        : ClipboardCopier
    {
        void ClipboardCopier.Copy(
            string text)
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
